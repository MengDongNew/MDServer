
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
                ValueType type = (ValueType)reader.ReadByte();
                object value = null;//reader.ReadUTF8String();
                switch (type) {
                    case ValueType.EnumInt: { value = reader.ReadInt(); }break;
                    case ValueType.Boolean: { value = reader.ReadBool(); }break;
                    case ValueType.Byte: { value = reader.ReadByte(); } break;
                    case ValueType.Int16: { value = reader.ReadShort(); } break;
                    case ValueType.Int32: { value = reader.ReadInt(); } break;
                    case ValueType.Int64: { value = reader.ReadLong(); } break;
                    case ValueType.SByte: { value = reader.ReadSByte(); } break;
                    case ValueType.String: { value = reader.ReadUTF8String(); } break;
                    case ValueType.UInt16: { value = reader.ReaduShort(); } break;
                    case ValueType.UInt32: { value = reader.ReadUint(); } break;
                    case ValueType.UInt64: { value = reader.ReaduLong(); } break;
                }
                operationRequest.Parameters.Add(key,value);
            }
            return operationRequest;
        }
    }
}
