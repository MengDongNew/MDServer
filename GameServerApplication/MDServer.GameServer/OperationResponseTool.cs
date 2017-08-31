
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
                    packetSend.Write((string)parameter.Value);
                }
            }

            return packetSend.CreateArrByte64K();
        }
    }
}
