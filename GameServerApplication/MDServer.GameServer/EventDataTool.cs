using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDServer.GameServer
{
    class EventDataTool
    {
        public static ArrByte64K GetPacket(IEventData eventData)
        {
            PacketSend packetSend = PacketSend.Create(eventData.Code, PacketSend.CodeType.EventCode);
            
            if (eventData.Parameters != null)
            {
                foreach (var parameter in eventData.Parameters)
                {
                    packetSend.Write((byte)parameter.Key);
                    packetSend.Write((string)parameter.Value);
                }
            }

            return packetSend.CreateArrByte64K();
        }
    }
}
