using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDServer.GameServer
{
    //
    // 摘要:
    //
    public sealed class EventData : IEventData
    {
        private byte eventCode;
        private Dictionary<byte, object> parameters;
        public EventData()
        {
        }

        public EventData(byte eventCode)
        {
            this.eventCode = eventCode;
        }

        public object this[byte parameterKey]
        {
            get { return parameters[parameterKey]; }
            set { parameters[parameterKey] = value; }
        }

        public byte Code {
            get { return eventCode; }
            set { eventCode = value; }
        }
        public Dictionary<byte, object> Parameters {
            get { return parameters; }
            set { parameters = value; }
        }


    }
}
