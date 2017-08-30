
using System.Collections.Generic;

namespace MDServer.GameServer
{
    public class OperationResponse
    {
        private object _dataContract;
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationResponse class.
        public OperationResponse()
        {
        }

        //
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationResponse class.
        //      This constructor sets the Photon.SocketServer.OperationResponse.OperationCode.
        //
        // 参数: 
        //   operationCode:
        //     Determines the Photon.SocketServer.OperationResponse.OperationCode.
        public OperationResponse(byte operationCode)
        {
            OperationCode = operationCode;
        }
        //
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationResponse class.
        //      This constructor sets the Photon.SocketServer.OperationResponse.Parameters
        //     and the Photon.SocketServer.OperationResponse.OperationCode.
        //
        // 参数: 
        //   operationCode:
        //     Determines the Photon.SocketServer.OperationResponse.OperationCode.
        //
        //   parameters:
        //     Determines the Photon.SocketServer.OperationResponse.Parameters.
        public OperationResponse(byte operationCode, Dictionary<byte, object> parameters)
        {
            OperationCode = operationCode;
            Parameters = parameters;
        }
        //
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationResponse class.
        //      This constructor calls Photon.SocketServer.OperationResponse.SetParameters(System.Object)
        //     and sets the Photon.SocketServer.OperationResponse.OperationCode.
        //
        // 参数: 
        //   operationCode:
        //     Determines the Photon.SocketServer.OperationResponse.OperationCode.
        //
        //   dataContract:
        //     All properties of the dataContract with the Photon.SocketServer.Rpc.DataMemberAttribute
        //     are copied to the Photon.SocketServer.OperationResponse.Parameters dictionary.
        public OperationResponse(byte operationCode, object dataContract)
        {
            OperationCode = operationCode;
            _dataContract = dataContract;
        }

        // 摘要: 
        //     Gets or sets the debug message. Error code 0 returns typically debug message
        //     "Ok".
        public string DebugMessage { get; set; }
        //
        // 摘要: 
        //     Gets or sets the operation code. It allows the client to idenitfy which operation
        //     was answered.
        public byte OperationCode { get; set; }
        //
        // 摘要: 
        //     Gets or sets the response parameters.
        public Dictionary<byte, object> Parameters { get; set; }
        //
        // 摘要: 
        //     Gets or sets the error code. Code 0 means OK.
        public short ReturnCode { get; set; }

        // 摘要: 
        //     Gets or sets the paramter associated with the specified key.
        //
        // 参数: 
        //   parameterKey:
        //     The key of the parameter to get or set.
        //
        // 返回结果: 
        //     The parameter associated with the specified key. If the specified key is
        //     not found, a get operation throws a KeyNotFoundException, and a set operation
        //     creates a new paramter with the specified key.
        //
        // 异常: 
        //   System.NullReferenceException:
        //     The Photon.SocketServer.OperationResponse.Parameters property has not been
        //     initialized.
        public object this[byte parameterKey]
        {
            get { return Parameters[parameterKey]; }
            set { Parameters[parameterKey] = value; }
        }

        // 摘要: 
        //     Replaces the Photon.SocketServer.OperationResponse.Parameters with parameters.
        //
        // 参数: 
        //   parameters:
        //     The parameters to set.
        public void SetParameters(Dictionary<byte, object> parameters)
        {
            Parameters = parameters;
        }
        //
        // 摘要: 
        //     Converts properties of an object to response Photon.SocketServer.OperationResponse.Parameters.
        //      Included properties require the Photon.SocketServer.Rpc.DataMemberAttribute.
        //
        // 参数: 
        //   dataContract:
        //     Properties of this object with the the Photon.SocketServer.Rpc.DataMemberAttribute
        //     converted to Photon.SocketServer.OperationResponse.Parameters.
        public void SetParameters(object dataContract)
        {
            _dataContract = dataContract;
        }
    }
}
