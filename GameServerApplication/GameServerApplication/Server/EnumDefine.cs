
namespace GameServerApplication.Server
{
    public enum DisconnectReason
    {
        ClientDisconnect = 0,
        ClientTimeoutDisconnect = 1,
        ManagedDisconnect = 2,
        ServerDisconnect = 3,
        TimeoutDisconnect = 4,
        ConnectTimeout = 5,
    }
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
