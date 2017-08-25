using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.Server
{
    public abstract class ApplicationBase
    {
        private MServer server { get; set; }

        protected ApplicationBase(IPEndPoint ipEndPoint)
        {
            server = new MServer(1000, this);
            server.Start(ipEndPoint);
        }

        //有新连接
        public abstract PeerBase CreatePeer(InitRequest initRequest);
        //启动
        public abstract void Setup();
        //关闭：尚未实现
        //public abstract void TearDown();
    }
}
