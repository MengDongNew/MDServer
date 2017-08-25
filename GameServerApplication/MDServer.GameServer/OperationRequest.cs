using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDServer.GameServer
{
    public class OperationRequest
    {
        private byte _operationCode;
        private Dictionary<byte, object> _parameters;

        private object _dataContract;
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationRequest class.
        public OperationRequest()
        {
        }

        //
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationRequest class.
        //
        // 参数: 
        //   operationCode:
        //     The operation Code.
        public OperationRequest(byte operationCode)
        {
            _operationCode = operationCode;
        }

        //
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationRequest class.
        //      This constructor sets the Photon.SocketServer.OperationRequest.Parameters
        //     and the Photon.SocketServer.OperationRequest.OperationCode.
        //
        // 参数: 
        //   operationCode:
        //     Determines the Photon.SocketServer.OperationRequest.OperationCode.
        //
        //   parameters:
        //     Determines the Photon.SocketServer.OperationRequest.Parameters.
        public OperationRequest(byte operationCode, Dictionary<byte, object> parameters)
        {
            _operationCode = operationCode;
            _parameters = parameters;
        }

        //
        // 摘要: 
        //     Initializes a new instance of the Photon.SocketServer.OperationRequest class.
        //
        // 参数: 
        //   operationCode:
        //     The operation Code.
        //
        //   dataContract:
        //     All properties of dataContract with the Photon.SocketServer.Rpc.DataMemberAttribute
        //     are mapped to the Photon.SocketServer.OperationRequest.Parameters dictionary.
        public OperationRequest(byte operationCode, object dataContract)
        {
            _operationCode = operationCode;
            _dataContract = dataContract;
        }

        // 摘要: 
        //     Gets or sets the operation code. It determines how the server responds.
        public byte OperationCode
        {
            get { return _operationCode; }
            set { _operationCode = value; }
        }
        //
        // 摘要: 
        //     Gets or sets the request parameters.
        public Dictionary<byte, object> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

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
        //     The Photon.SocketServer.OperationRequest.Parameters property has not been
        //     initialized.
        public object this[byte parameterKey]
        {
            get { return _parameters[parameterKey]; }
            set { _parameters[parameterKey] = value; }
        }

        // 摘要: 
        //     Replaces the Photon.SocketServer.OperationRequest.Parameters with parameters.
        //
        // 参数: 
        //   parameters:
        //     The parameters to set.
        public void SetParameters(Dictionary<byte, object> parameters)
        {
            _parameters = parameters;
        }

        //
        // 摘要: 
        //     Converts properties of an object to Photon.SocketServer.OperationRequest.Parameters.
        //      Included properties require the Photon.SocketServer.Rpc.DataMemberAttribute.
        //
        // 参数: 
        //   dataContract:
        //     The properties of this object are mapped to Photon.SocketServer.OperationRequest.Parameters.
        public void SetParameters(object dataContract)
        {
            _dataContract = dataContract;
        }
    }
}
