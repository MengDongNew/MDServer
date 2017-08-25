using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameServer
{
    // 摘要: 
    //     Specifies the available network protocol types.
    public enum NetworkProtocolType
    {
        // 摘要: 
        //     Unknown protocol type
        Unknown = 0,
        //
        // 摘要: 
        //     The udp protocol
        Udp = 1,
        //
        // 摘要: 
        //     The tcp protocol
        Tcp = 2,
        //
        // 摘要: 
        //     The websocket protocol
        WebSocket = 3,
        //
        // 摘要: 
        //     The HTTP protocol
        Http = 4,
        //
        // 摘要: 
        //     The secure websocket protocol
        SecureWebSocket = 5,
    }
}
