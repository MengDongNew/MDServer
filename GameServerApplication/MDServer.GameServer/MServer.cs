using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MDServer.GameServer
{
    public class MServer
    {

        private ApplicationBase application { get; set; }
        //private Dictionary<int, PeerBase> dicPeers { get; set; }
        
        private Dictionary<int, UserToken> dicUserToken { get; set; }// = new Dictionary<int, UserToken>();
        private UserTokenPool m_asyncSocketUserTokenPool { get; set; }
        private int connId { get; set; }
        private int m_numConnections { get; set; }
        private int m_numConnectedSockets;

        private Socket listenSocket { get; set; }

        private AutoResetEvent seqSend { get; set; }
        private AutoResetEvent seqReceive { get; set; }
        //存储接收到的数据
        private Queue<Packet> queuePacketsReceive { get; set; }
        //存储需要发送的数据
        private Queue<Packet> queuePacketsSend { get; set; }

        private ArrByte64KPool arrByte64KPool { get; set; }

        public MServer(int numConnections,ApplicationBase application)
        {
            this.application = application;
            m_numConnectedSockets = 0;
            m_numConnections = numConnections;
            dicUserToken = new Dictionary<int, UserToken>();
           
            m_asyncSocketUserTokenPool = new UserTokenPool(numConnections);
            seqSend = new AutoResetEvent(false);
            seqReceive = new AutoResetEvent(false);
            queuePacketsReceive = new Queue<Packet>();
            queuePacketsSend = new Queue<Packet>();

            arrByte64KPool = ArrByte64KPool.Instance;// new ArrByte64KPool();
            for (int i = 0; i < m_numConnections; i++)
            {
               m_asyncSocketUserTokenPool.Push(CreateUserToken());
            }
        }

        private UserToken CreateUserToken()
        {
            UserToken userToken = new UserToken(this);
            userToken.receiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            userToken.sendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            return userToken;
        }
        public void Start(IPEndPoint localEndPoint)
        {
            Log("Listent " + localEndPoint.ToString());
            listenSocket = new Socket(localEndPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(100);
            StartAccept(null);
            new Thread(Thread_Send).Start();
            new Thread(Thread_Receive).Start();
            //启动完成
            application.Setup();
        }


        #region Accept

        private void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArgs.AcceptSocket = null;
            }
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {

            ProcessAccept(e);

        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Interlocked.Increment(ref m_numConnectedSockets);
            try
            {
                if (e.AcceptSocket.RemoteEndPoint != null)
                {
                    
                    Log(string.Format("Connection {1} accepted. {0} connected to the server",
                        m_numConnectedSockets, e.AcceptSocket.RemoteEndPoint.ToString()));
                }
                else
                {
                    Log(string.Format("Connection accepted. {0} connected to the server",
                        m_numConnectedSockets));
                }
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
            var userToken = m_asyncSocketUserTokenPool.Pop();
            userToken.connId = ++connId;
            userToken.Socket = e.AcceptSocket;
            lock (dicUserToken)
            {
                dicUserToken.Add(userToken.connId, userToken);
            }
            try
            {
                InitRequest initRequest = new InitRequest()
                {
                    ConnectionId = userToken.connId,
                };

                userToken.Peer = application.CreatePeer(initRequest);
                userToken.Peer.OnConnected(userToken);
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
            try
            {
                bool  willRaiseEvent = e.AcceptSocket.ReceiveAsync(userToken.receiveEventArgs);
                if (!willRaiseEvent)
                {
                    lock (userToken)
                    {
                        ProcessReceive(userToken.receiveEventArgs);
                    }
                }

                StartAccept(e);
            }
            catch (Exception ex)
            {
                lock (userToken)
                {
                    CloseClientSocket(userToken.receiveEventArgs);
                }
                Log(ex.ToString());
                StartAccept(e);
                return;
            }

        }
        #endregion

        #region Close Socket

         private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (token.Socket == null)
                return;
            try
            {
                token.Socket.Shutdown(SocketShutdown.Both);
                token.Socket.Close();
                token.Socket = null;
            }
            catch (Exception ex) { Log(ex.Message);}
            Interlocked.Decrement(ref m_numConnectedSockets);
            Log("Disconnected "+ m_numConnectedSockets+ " connected");
            if (token.arrBuffRead != null)
            {
                //do something
                arrByte64KPool.Put(token.arrBuffRead);
                token.arrBuffRead = null;
            }
            if (token.arrBuffSend != null)
            {
                arrByte64KPool.Put(token.arrBuffSend);
                token.arrBuffSend = null;
                token.sendComplete = true;
            }
            try
            {
                token.Peer.OnDisconnect(DisconnectReason.ClientDisconnect, "");
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
            
            lock (dicUserToken)
            {
                dicUserToken.Remove(token.connId);
            }
            m_asyncSocketUserTokenPool.Push(token);
            
        }

        #endregion
       

        #region Receive && Send

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {

            UserToken userToken = e.UserToken as UserToken;
            lock (userToken)
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(e);
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend(e);
                        break;
                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if(token.Socket == null)return;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                int offset = token.receiveEventArgs.Offset;
                int count = token.receiveEventArgs.BytesTransferred;
                var readBuff = token.receiveEventArgs.Buffer;
                if (token.arrBuffRead == null)
                    token.arrBuffRead = arrByte64KPool.Get();
                while (count>0)
                {
                    if (count + token.arrBuffRead.len < token.arrBuffRead.arrByte64K.Length)
                    {
                        Array.Copy(readBuff, offset,token.arrBuffRead.arrByte64K,token.arrBuffRead.len, count);
                        token.arrBuffRead.len += count;
                        offset += count;
                        count = 0;
                    }
                    else
                    {
                        Array.Copy(readBuff, offset,token.arrBuffRead.arrByte64K,token.arrBuffRead.len, token.arrBuffRead.arrByte64K.Length - token.arrBuffRead.len);
                        token.arrBuffRead.len = token.arrBuffRead.arrByte64K.Length;
                        offset += token.arrBuffRead.arrByte64K.Length - token.arrBuffRead.len;
                        count -= token.arrBuffRead.arrByte64K.Length - token.arrBuffRead.len;
                    }
                    while (token.arrBuffRead!=null && token.arrBuffRead.len >=2)
                    {
                        int len = ArrByteReader.GetuShort(token.arrBuffRead.arrByte64K, 0);
                        if (len > token.arrBuffRead.arrByte64K.Length)
                        {
                            CloseClientSocket(e);
                            return;
                        }
                        if (token.arrBuffRead.len >= len)
                        {
                            var pk = arrByte64KPool.Get();
                            Array.Copy(token.arrBuffRead.arrByte64K,2,pk.arrByte64K, 0,len-2);
                            pk.len = len - 2;
                            {
                                //dosomething 分发数据
                                Packet packet = new Packet();
                                packet.arrByte64K = pk;
                                packet.connId = token.connId;
                                PutReceivePacket(packet);
                            }


                            if (token.arrBuffRead.len > len)
                            {
                                var arrBuff = arrByte64KPool.Get();
                                arrBuff.len = token.arrBuffRead.len - len;
                                Array.Copy(token.arrBuffRead.arrByte64K, len, arrBuff.arrByte64K, 0, arrBuff.len);
                                arrByte64KPool.Put(token.arrBuffRead);
                                token.arrBuffRead = arrBuff;
                            }
                            else
                            {
                                arrByte64KPool.Put(token.arrBuffRead);
                                token.arrBuffRead = null;
                            }

                        }
                        else
                        {
                            break;
                        }
                    }
                }
                try
                {
                    bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                    if (!willRaiseEvent)
                    {
                        ProcessReceive(e);
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    CloseClientSocket(e);

                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (e.SocketError == SocketError.Success)
            {
                if (token.arrBuffSend != null)
                {
                    arrByte64KPool.Put(token.arrBuffSend);
                }
                token.arrBuffSend = null;
                token.sendComplete = true;
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        #endregion

        #region Receive Data

        public void PutReceivePacket(Packet packet)
        {
            lock (queuePacketsReceive)
            {
                queuePacketsReceive.Enqueue(packet);
            }
            seqReceive.Set();
        }
        private void Thread_Receive()
        {
            Queue<Packet> queuePackets = new Queue<Packet>(100);
            while (true)
            {
                if (queuePacketsReceive.Count == 0)
                {
                    seqReceive.WaitOne();
                }
                while (queuePacketsReceive.Count > 0)
                {
                    queuePackets.Enqueue(queuePacketsReceive.Dequeue());
                }
                while (queuePackets.Count>0)
                {
                    try
                    {
                        lock (queuePackets)
                        {
                            var p = queuePackets.Dequeue();
                            UserToken userToken;
                            if (dicUserToken.TryGetValue(p.connId, out userToken))
                            {
                                userToken.Peer.OnOperationRequest(OperationRequestTool.CreateOperationRequest(p.arrByte64K), new SendParameters());
                            }
                            else
                            {
                                arrByte64KPool.Put(p.arrByte64K);
                            }
                        }
                     
                    }
                    catch (Exception e)
                    {
                        LogError(e.Message);
                    }
                   
                }
                
            }
        }

        #endregion

        #region Send Data

        public void PutSendPacket(Packet packet)
        {
            lock (queuePacketsSend)
            {
                queuePacketsSend.Enqueue(packet);
            }
            seqSend.Set();
        }
        private void Thread_Send()
        {
            Dictionary<int, Queue<Packet>> connq = new Dictionary<int, Queue<Packet>>();
            List<int> lstDel = new List<int>();
            Queue<Queue<Packet>> qBuff = new Queue<Queue<Packet>>(1000);
            bool sleep = false;

            for (int i = 0; i < 1000; i++)
            {
                qBuff.Enqueue(new Queue<Packet>());
            }
            while (true)
            {
                if (connq.Count == 0)
                {
                    seqSend.WaitOne();
                }
                else if(sleep)
                {
                    Thread.Sleep(1);
                }
                sleep = true;
                lock (queuePacketsSend)
                {
                    while (queuePacketsSend.Count>0)
                    {
                        var p = queuePacketsSend.Dequeue();
                        Queue<Packet> q;
                        if (!connq.TryGetValue(p.connId, out q))
                        {
                            if (qBuff.Count > 0)
                            {
                                q = qBuff.Dequeue();
                            }
                            else
                            {
                                q = new Queue<Packet>();
                            }
                            connq.Add(p.connId,q);
                        }
                        q.Enqueue(p);
                    }
                }
                lstDel.Clear();
                foreach (var q in connq.Values)
                {
                    var p = q.Peek();
                    UserToken userToken;
                    lock (dicUserToken)
                    {
                        dicUserToken.TryGetValue(p.connId, out userToken);
                    }
                    if (userToken!=null)
                    {
                        if (userToken.sendComplete)
                        {
                            q.Dequeue();
                            if (q.Count <= 0)
                            {
                                lstDel.Add(p.connId);
                                qBuff.Enqueue(q);
                            }
                            SendAsyncEvent(userToken, p);
                            sleep = false;
                        }
                    }
                    else
                    {
                        while (q.Count>0)
                        {
                            arrByte64KPool.Put(q.Dequeue().arrByte64K);
                        }
                        lstDel.Add(p.connId);
                        qBuff.Enqueue(q);
                    }
                }
                foreach (var connId in lstDel)
                {
                    connq.Remove(connId);
                }

            }
        }

        private void SendAsyncEvent(UserToken token, Packet pk)
        {
            lock (token)
            {
                if (token.Socket == null)
                {
                    arrByte64KPool.Put(pk.arrByte64K);
                    token.arrBuffSend = null;
                    token.sendComplete = true;
                    return;
                }

                token.sendComplete = false;
                token.arrBuffSend = pk.arrByte64K;
                token.sendEventArgs.SetBuffer(pk.arrByte64K.arrByte64K,0, pk.arrByte64K.len);
                bool willRaiseEvent = token.Socket.SendAsync(token.sendEventArgs);//如果 I/O 操作同步完成，将返回 false
                if (!willRaiseEvent)
                {
                    ProcessSend(token.sendEventArgs);
                }
            }
        }
        #endregion
        private  void Log(string s)
        {
            Console.WriteLine(s);
        }

        private void LogError(string s)
        {
            Console.WriteLine("Error! MServer:"+s);
        }
    }
}
