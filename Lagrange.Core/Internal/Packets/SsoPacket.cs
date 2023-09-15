using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Packets;

internal class SsoPacket
{
    public byte PacketType { get; set; }
    
    public string Command { get; }
    
    public uint Sequence { get; }
    
    public BinaryPacket Payload { get; }
    
    public SsoPacket(byte packetType, string command, uint sequence, BinaryPacket payload)
    {
        PacketType = packetType;
        Command = command;
        Sequence = sequence;
        Payload = payload;
    }
}