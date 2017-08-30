using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MDServer.GameServer;

namespace GameServerApplication
{
    class MasterClientPeer : PeerBase
    {
        public MasterClientPeer(InitRequest initRequest) : base(initRequest)
        {
        }
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
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.Parameters = new Dictionary<byte, object>();
            response.Parameters.Add(2, "我是服务器，hello！");
            response.ReturnCode = (short)ReturnCode.Succeed;
            SendOperationResponse(response, sendParameters);
        }

        private void Log(string s)
        {
            Console.WriteLine("MasterClientPeer:" + s);
        }
    }
}
