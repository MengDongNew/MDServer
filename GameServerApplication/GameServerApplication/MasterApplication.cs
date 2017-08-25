using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MDServer.GameServer;


namespace GameServerApplication
{
    class MasterApplication:ApplicationBase
    {
        public MasterApplication(IPEndPoint ipEndPoint) : base(ipEndPoint)
        {
        }

        public override PeerBase CreatePeer(InitRequest initRequest)
        {
            Log("CreatePeer:"+initRequest.LocalIP);
            return new MasterClientPeer(initRequest);
        }

        public override void Setup()
        {
            Log("SetUp()");
        }

        void Log(string s)
        {
            Console.WriteLine("MasterApplication:"+s);
        }
    }
}
