using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameServer
{
    public enum DisconnectReason
    {
        ClientDisconnect = 0,
        ClientTimeoutDisconnect = 1,
        ManagedDisconnect = 2,
        ServerDisconnect = 3,
        TimeoutDisconnect = 4,
        ConnectTimeout = 5,
    }
}
