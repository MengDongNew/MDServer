using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.Server
{
    public class UserToken : IDisposable
    {
        public MServer Server { get; }
        public int connId;
        public Socket Socket { get; set; }
        public SocketAsyncEventArgs receiveEventArgs;
        public SocketAsyncEventArgs sendEventArgs;
        public ArrByte64K arrBuffRead;
        public ArrByte64K arrBuffSend;
        public bool sendComplete { get; set; }

        private PeerBase peer;
        public PeerBase Peer {
            get { return peer; }
            set { peer = value;peer.OnConnected(this); }
        }

        public UserToken(MServer server)
        {
            sendComplete = true;
            this.Server = server;
            receiveEventArgs = new SocketAsyncEventArgs();
            receiveEventArgs.UserToken = this;
            var receiveBuffer = new byte[1024];
            receiveEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
            sendEventArgs = new SocketAsyncEventArgs();
            sendEventArgs.UserToken = this;
        }
        public void Dispose()
        {
            receiveEventArgs.Dispose();
            sendEventArgs.Dispose();
        }
    }

    public class UserTokenPool
    {
        private Queue<UserToken> m_pool { get; set; }

        //private int count = 0;
        public UserTokenPool(int capacity)
        {
            m_pool = new Queue<UserToken>(capacity);
        }
        public void Push(UserToken item)
        {
            if (item == null)
            {
                throw new ArgumentException("Items added to a AsyncSocketUserToken cannot be null");
            }
            lock (m_pool)
            {
                m_pool.Enqueue(item);
            }
        }
        public UserToken Pop()
        {
            lock (m_pool)
            {
                if (m_pool.Count <= 0)
                {
                    return null;
                    //var userToken = new UserToken();
                    //userToken.receiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Server._ServerInstance_.IO_Completed);
                    //userToken.sendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Server._ServerInstance_.IO_Completed);
                    //return userToken;
                }
                else
                    return m_pool.Dequeue();
            }
        }
        public int Count
        {
            get { return m_pool.Count; }
        }

        private static void Log(string s)
        {
            Console.WriteLine(s);
        }
        private static void LogFile(string s)
        {
            Console.WriteLine(s);
        }

    }
}
