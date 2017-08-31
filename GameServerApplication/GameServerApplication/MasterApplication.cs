using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GameServerApplication.Handlers;
using MDServer.GameServer;
using System.Reflection;

namespace GameServerApplication
{
    class MasterApplication:ApplicationBase
    {

        public static MasterApplication Instance { get; set; }

        private Dictionary<byte, HandlerBase> handlers = new Dictionary<byte, HandlerBase>();

        public MasterApplication(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
            Instance = this;
            RegistHandlers();
        }

        #region Methords

        void RegistHandlers()
        {
            Type[] types = Assembly.GetAssembly(typeof(HandlerBase)).GetTypes();
            foreach (var type in types)
            {
                if (type.FullName.EndsWith("Handler"))
                {
                   HandlerBase handlerBase = (HandlerBase)Activator.CreateInstance(type);
                    handlers.Add((byte) handlerBase.OpCode,handlerBase);
                }
            }
        }

        public bool TryGetHandler(byte code, out HandlerBase handler)
        {
            return handlers.TryGetValue(code, out handler);
        }
        #endregion

        #region Interface

        public override PeerBase CreatePeer(InitRequest initRequest)
        {
            Log("CreatePeer:"+initRequest.LocalIP);
            return new MasterClientPeer(initRequest);
        }

        public override void Setup()
        {
            Log("SetUp()");
        }


        #endregion
       
        void Log(string s)
        {
            Console.WriteLine("MasterApplication:"+s);
        }
    }
}
