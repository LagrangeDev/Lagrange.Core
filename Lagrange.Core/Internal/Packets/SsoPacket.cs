using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Packets;

internal class SsoPacket : IDisposable
{
    public uint PacketType { get; set; }
    
    public byte EncodeType { get; set; }

    public string Command { get; }

    public byte[] MsgCookie { get; }
    
    public uint Sequence { get; }
    
    public BinaryPacket Payload { get; }
    
    public SsoPacket(uint packetType, byte encodeType, string command, byte[] msgCookie, uint sequence, BinaryPacket payload)
    {
        PacketType = packetType;
        EncodeType = encodeType;
        Command = command;
        MsgCookie = msgCookie;
        Sequence = sequence;
        Payload = payload;
    }

    public void Dispose() => Payload.Dispose();
}