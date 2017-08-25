using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGameServer
{
    // 摘要: 
    //     Return value of Photon.SocketServer.PeerBase.SendEvent(Photon.SocketServer.IEventData,Photon.SocketServer.SendParameters),
    //     Photon.SocketServer.PeerBase.SendOperationResponse(Photon.SocketServer.OperationResponse,Photon.SocketServer.SendParameters)
    //     and Photon.SocketServer.ServerToServer.S2SPeerBase.SendOperationRequest(Photon.SocketServer.OperationRequest,Photon.SocketServer.SendParameters).
    public enum SendResult
    {
        // 摘要: 
        //     Encrypted sending failed; peer does not support encryption.
        EncryptionNotSupported = -1,
        //
        // 摘要: 
        //     Successfully enqueued for sending.
        Ok = 0,
        //
        // 摘要: 
        //     The peer's send buffer is full; data sending was refused.
        SendBufferFull = 1,
        //
        // 摘要: 
        //     Peer is disconnected; data sending was refused.
        Disconnected = 2,
        //
        // 摘要: 
        //     Sending failed because the message size exceeded the MaxMessageSize that
        //     was configured for the receiver.
        MessageToBig = 3,
        //
        // 摘要: 
        //     Send Failed due an unexpected error.
        Failed = 4,
        //
        // 摘要: 
        //     Send failed because the specified channel is not supported by the peer.
        InvalidChannel = 5,
        //
        // 摘要: 
        //     Send failed because the specified content type is not supported by the peer.
        InvalidContentType = 6,
    }
}
