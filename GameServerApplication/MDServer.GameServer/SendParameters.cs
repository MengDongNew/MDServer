

namespace MDServer.GameServer
{
    // 摘要: 
    //     The struct contains the parameters for Photon.SocketServer.PeerBase.SendOperationResponse(Photon.SocketServer.OperationResponse,Photon.SocketServer.SendParameters),
    //     Photon.SocketServer.PeerBase.SendEvent(Photon.SocketServer.IEventData,Photon.SocketServer.SendParameters)
    //     and Photon.SocketServer.ServerToServer.S2SPeerBase.SendOperationRequest(Photon.SocketServer.OperationRequest,Photon.SocketServer.SendParameters)
    //     and contains the info about incoming data at Photon.SocketServer.PeerBase.OnOperationRequest(Photon.SocketServer.OperationRequest,Photon.SocketServer.SendParameters),
    //     Photon.SocketServer.ServerToServer.S2SPeerBase.OnEvent(Photon.SocketServer.IEventData,Photon.SocketServer.SendParameters)
    //     and Photon.SocketServer.ServerToServer.S2SPeerBase.OnOperationResponse(Photon.SocketServer.OperationResponse,Photon.SocketServer.SendParameters).
    public struct SendParameters
    {

        // 摘要: 
        //     Gets or sets the channel id for the udp protocol.
        public byte ChannelId { get; set; }
        //
        // 摘要: 
        //     Gets or sets a value indicating whether the data is sent encrypted.
        public bool Encrypted { get; set; }
        //
        // 摘要: 
        //     Gets or sets a value indicating whether to flush all queued data with the
        //     next send.  This overrides the configured send delay.
        public bool Flush { get; set; }
        //
        // 摘要: 
        //     Gets or sets a value indicating whether to send the data unreliable.
        public bool Unreliable { get; set; }
    }
}
