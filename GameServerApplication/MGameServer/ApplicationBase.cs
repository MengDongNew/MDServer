using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameServer
{
    public abstract class ApplicationBase
    {
        protected static ApplicationBase _instance;

        protected string _applicationName;

        public static ApplicationBase Instance {
            get { return _instance; }
        }
        public string ApplicationName {
            get { return _applicationName; }
        }
        protected ApplicationBase(string applicationName)
        {
            _instance = this;
            _applicationName = applicationName;
        }

        //
        // 摘要: 
        //     This method is called by the PhotonHostRuntimeInterfaces.IPhotonApplication.OnInit(PhotonHostRuntimeInterfaces.IPhotonPeer,System.Byte[],System.Byte)
        //     implementation of this class.  The inheritor should return a Photon.SocketServer.PeerBase
        //     implementation.
        //
        // 参数: 
        //   initRequest:
        //     The initialization request.
        //
        // 返回结果: 
        //     A new instance of Photon.SocketServer.PeerBase or null.
        protected abstract PeerBase CreatePeer(InitRequest initRequest);
        //
        // 摘要: 
        //     This method is called when the current application has been started.  The
        //     inheritor can setup log4net here and execute other initialization routines
        //     here.
        protected abstract void Setup();
        //
        // 摘要: 
        //     This method is called when the current application is being stopped.  The
        //     inheritor can execute cleanup routines here.
        protected abstract void TearDown();

    }
}
