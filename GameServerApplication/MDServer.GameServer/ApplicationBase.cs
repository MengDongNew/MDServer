using System.Net;

namespace MDServer.GameServer
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
