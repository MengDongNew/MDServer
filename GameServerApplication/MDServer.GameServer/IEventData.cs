using System;
using System.Collections.Generic;
namespace MDServer.GameServer
{
    //数据接口
    public interface IEventData
    {

        object this[byte parameterKey] { get; set; }

        //
        // 摘要:
        //     Gets Code.
        byte Code { get; }
        //
        // 摘要:
        //     Gets the event parameters that will be sent to the client.
        Dictionary<byte, object> Parameters { get; }

    }
}
