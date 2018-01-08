using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tool;
using GameServerApplication.DB.Manager;
using MDServer.GameServer;

namespace GameServerApplication.Handlers
{
    class ServerHandler:HandlerBase
    {
        readonly ServerPropertyManager serverManager = new ServerPropertyManager();

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

        public override void OnHandleMessage(OperationRequest request, OperationResponse response, MasterClientPeer peer,  SendParameters sendParameters)
        {
            var list = serverManager.GetServerList();
            ParameterTool.AddParameter(response.Parameters, ParameterCode.ServerList,list);
            //string serverlist = "我是服务器列表";
            //response.Parameters.Add((byte)ParameterCode.ServerList, serverlist);
            response.ReturnCode = (short) ReturnCode.Success;

           // EventData eventData = new EventData((byte) OperationCode.Server);
           // SendEvent(peer,eventData);
        }
        #endregion

    }
}
