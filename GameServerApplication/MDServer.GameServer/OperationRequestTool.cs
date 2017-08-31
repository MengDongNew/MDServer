
using System.Collections.Generic;
namespace MDServer.GameServer
{
     class OperationRequestTool
    {
        public static OperationRequest CreateOperationRequest(ArrByte64K arrByte64K)
        {
            ArrByteReader reader = new ArrByteReader();
            reader.SetArrByte(arrByte64K);
            OperationRequest operationRequest = new OperationRequest();
            operationRequest.OperationCode = reader.ReadByte();
            operationRequest.Parameters = new Dictionary<byte, object>();
            while (reader.ReadLen< arrByte64K.len)
            {
                byte key = reader.ReadByte();
                string value = reader.ReadUTF8String();
               // object value = "";
                operationRequest.Parameters.Add(key,value);
            }
            return operationRequest;
        }
    }
}
