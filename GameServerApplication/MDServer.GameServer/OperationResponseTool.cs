
namespace MDServer.GameServer
{
    public class OperationResponseTool
    {
        public static ArrByte64K GetPacket(OperationResponse response)
        {
            PacketSend packetSend = PacketSend.Create(response.OperationCode, PacketSend.CodeType.OperationCode);
            packetSend.SetReturnCode(response.ReturnCode);
            if (response.Parameters != null)
            {
                foreach (var parameter in response.Parameters)
                {
                    packetSend.Write((byte)parameter.Key);
                    //添加value的值类型，如int，byte，string，等
                    if (parameter.Value.GetType() == typeof(byte))
                    {
                        packetSend.Write((byte)ValueType.Byte);
                        packetSend.Write((byte)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(int))
                    {
                        packetSend.Write((byte)ValueType.Int32);
                        packetSend.Write((int)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(string))
                    {
                        packetSend.Write((byte)ValueType.String);
                        packetSend.Write((string)parameter.Value);
                    }
                    else if (parameter.Value.GetType().IsEnum)
                    {
                        packetSend.Write((byte)ValueType.EnumInt);
                        packetSend.Write((int)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(bool))
                    {
                        packetSend.Write((byte)ValueType.Boolean);
                        packetSend.Write((bool)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(sbyte))
                    {
                        packetSend.Write((byte)ValueType.SByte);
                        packetSend.Write((sbyte)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(short))
                    {
                        packetSend.Write((byte)ValueType.Int16);
                        packetSend.Write((short)parameter.Value);
                    }

                    else if (parameter.Value.GetType() == typeof(long))
                    {
                        packetSend.Write((byte)ValueType.Int64);
                        packetSend.Write((long)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(ushort))
                    {
                        packetSend.Write((byte)ValueType.UInt16);
                        packetSend.Write((ushort)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(uint))
                    {
                        packetSend.Write((byte)ValueType.UInt32);
                        packetSend.Write((uint)parameter.Value);
                    }
                    else if (parameter.Value.GetType() == typeof(ulong))
                    {
                        packetSend.Write((byte)ValueType.UInt64);
                        packetSend.Write((ulong)parameter.Value);
                    }
                    else {
                        
                    }
                    
                   
                }
            }

            return packetSend.CreateArrByte64K();
        }
    }
}
