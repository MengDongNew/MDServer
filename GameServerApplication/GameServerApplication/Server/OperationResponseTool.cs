﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.Server
{
    public class OperationResponseTool
    {
        public static ArrByte64K GetPacket(OperationResponse response)
        {
            PacketSend packetSend = PacketSend.Create(response.OperationCode);
            foreach (var parameter in response.Parameters)
            {
                packetSend.Write((byte)parameter.Key);
                packetSend.Write((string)parameter.Value);
            }
            return packetSend.CreateArrByte64K();
        }
    }
}