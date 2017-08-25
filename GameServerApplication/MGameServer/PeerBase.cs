using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MGameServer
{
    // 摘要: 
    //     This class is a base class for server connections. 
    //     连接对象
    public abstract class PeerBase : IDisposable
    {
        
        public bool Connected { get; }
        public int ConnectionId { get; }

        public ConnectionState ConnectionState { get; internal set; }

        public string LocalIP { get; }

        public IPAddress LocalIPAddress { get; }

        public int LocalPort { get; }
        public NetworkProtocolType NetworkProtocol { get; }
        private string _debugString { get; set; }

        public void Initialize(InitRequest initRequest)
        {

        }


        public SendResult SendOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            return SendResult.Failed;
        }

        public void SetDebugString(string message)
        {
            _debugString = message;
        }
        #region virtual Methord

        protected internal virtual void OnMessage(object message, SendParameters sendParameters)
        {
            
        }

        protected virtual void OnReceive(byte[] data, SendParameters sendParameters)
        {
        }

        protected internal virtual void OnSend(int bytesSend)
        {
        }

        protected virtual void OnSendBufferEmpty()
        {
        }

        protected internal virtual void OnSendBufferFull()
        {
            
        }

        protected internal virtual void OnSendFailed(SendResult sendResult, SendParameters sendParameters,
            int messageSize)
        {
            
        }

        protected virtual void OnUnexpectedDataReceived(byte[] data, string debugMessage)
        {
            
        }

        protected virtual SendResult SendData(byte[] data, SendParameters sendParameters)
        {
            return SendResult.Failed;
        }
        #endregion

        #region abstract Methord
        protected abstract void OnDisconnect(DisconnectReason reasonCode, string reasonDetail);
        protected internal abstract void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters);


        #endregion

        #region IDisposable Interface
        public void Dispose()
        {
            
        }

        #endregion
    }
}
