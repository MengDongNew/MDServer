using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MDServer.GameServer;

namespace GameServerApplication.Handlers
{
    class ServerHandler:HandlerBase
    {


        #region MyMethords

        void SendEvent(PeerBase peer, EventData eventData)
        {
            peer.SendEvent(eventData);
        }

        #endregion

        #region Interface


        public override OperationCode OpCode {
            get { return OperationCode.Server;}
        }

        public override void OnHandleMessage(OperationRequest request, OperationResponse response, PeerBase peer,
            SendParameters sendParameters)
        {
            string serverlist = "我是服务器列表";
            response.Parameters.Add((byte)ParameterCode.ServerList, serverlist);
            response.ReturnCode = (short) ReturnCode.Succeed;

            EventData eventData = new EventData((byte) OperationCode.Server);
            SendEvent(peer,eventData);
        }
        #endregion

    }
}
