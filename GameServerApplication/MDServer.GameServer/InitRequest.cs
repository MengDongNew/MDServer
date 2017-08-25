using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDServer.GameServer
{
    public class InitRequest
    {
        public string ApplicationId { get; set; }
        public int ConnectionId { get; set; }
        public string LocalIP { get; set; }
        public int LocalPort { get; set; }
        public Uri Uri { get; set; }
        public object UserData { get; set; }

        public InitRequest() { }
    }
}
