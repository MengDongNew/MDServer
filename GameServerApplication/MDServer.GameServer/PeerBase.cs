

namespace MDServer.GameServer
{
    public abstract class PeerBase
    {
        private InitRequest initRequest { get;  }

        //private MServer server { get; set; }
        private UserToken userToken { get; set; }

        protected PeerBase(InitRequest initRequest)
        {
            this.initRequest = initRequest;
        }

        public void OnConnected(UserToken userToken)
        {
            this.userToken = userToken;
        }
        
        //Send Operation handle data send back
        public SendResult SendOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            Packet packet = new Packet()
            {
                connId = userToken.connId,
                arrByte64K = OperationResponseTool.GetPacket(operationResponse)
            };
            
            userToken.Server.PutSendPacket(packet);

            return SendResult.Ok;
        }

        //Send Event Data
        public SendResult SendEvent(IEventData eventData, SendParameters sendParameters)
        {
            Packet packet = new Packet()
            {
                connId = userToken.connId,
                arrByte64K = EventDataTool.GetPacket(eventData)
            };
            userToken.Server.PutSendPacket(packet);
            return SendResult.Ok;
        }


        public abstract void OnDisconnect(DisconnectReason reasonCode, string reasonDetail);

        protected internal abstract void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters);

    }
}
