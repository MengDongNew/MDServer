using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MDServer.GameServer;

namespace GameServerApplication.Handlers
{
    public abstract class HandlerBase
    {
        protected HandlerBase()
        {
          
        }

        public abstract OperationCode OpCode { get; }

        public abstract void OnHandleMessage(OperationRequest request, OperationResponse response, MasterClientPeer peer, SendParameters sendParameters);

    }
}
