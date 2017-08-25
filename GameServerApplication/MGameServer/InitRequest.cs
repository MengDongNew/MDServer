using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameServer
{
    public sealed class InitRequest
    {
        public string ApplicationId { get; }
        public int ConnectionId { get; }
        public string LocalIP { get; }
        public int LocalPort { get; }
        public Uri Uri { get; }
        public object UserData { get; set; }
    }
}
