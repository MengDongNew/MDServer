using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.Server
{
    //消息包
    public class Packet
    {
        public int connId { get; set; }
        public ArrByte64K arrByte64K { get; set; }
        public Packet() { }
    }

    public class PacketPool
    {
        private static PacketPool _instance;

        public static PacketPool Instance{
            get { if(_instance==null)_instance = new PacketPool();
                return _instance;
            }
        }

        public Queue<Packet> qPacketSend { get; set; }

        private PacketPool()
        {
            qPacketSend = new Queue<Packet>(1000);
        }

        
    }
}
