using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Modal;
using GameServerApplication.Handlers;
using MDServer.GameServer;

namespace GameServerApplication
{
    public class MasterClientPeer : PeerBase
    {
        public User User { get; set; }

        public MasterClientPeer(InitRequest initRequest) : base(initRequest)
        {
        }


        #region Interface

            public override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Log("DisconnectReason=" + reasonCode);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            Log("OperationRequest.operationCode=" + operationRequest.OperationCode);
            foreach (var parameter in operationRequest.Parameters)
            {
                Log(parameter.Key + ":" + parameter.Value.ToString());
            }

            HandlerBase handler;
            if (MasterApplication.Instance.TryGetHandler(operationRequest.OperationCode, out handler))
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                response.Parameters = new Dictionary<byte, object>();
                handler.OnHandleMessage(operationRequest,response,this,sendParameters);
                SendOperationResponse(response, sendParameters);
            }
            else
            {
                Log("Can't find handler from OperationCode ："+operationRequest.OperationCode);
            }

           /*
           
            EventData eventData = new EventData()
            {
                Code = operationRequest.OperationCode,
            };
            eventData.Parameters = new Dictionary<byte, object>();
            eventData.Parameters.Add(100,"我是服务器EventData");
            SendEvent(eventData, sendParameters);
            */
        }

        #endregion
    

        private void Log(string s)
        {
            Console.WriteLine("MasterClientPeer:" + s);
        }
    }
}
