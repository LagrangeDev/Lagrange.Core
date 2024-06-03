using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Packets;

internal class SsoPacket : IDisposable
{
    public byte PacketType { get; set; }
    
    public string Command { get; }
    
    public uint Sequence { get; }
    
    public BinaryPacket Payload { get; }
    
    public int RetCode { get; }
    
    public string? Extra { get; }
    
    public SsoPacket(byte packetType, string command, uint sequence, BinaryPacket payload)
    {
        PacketType = packetType;
        Command = command;
        Sequence = sequence;
        Payload = payload;
    }

    public SsoPacket(byte packetType, string command, uint sequence, int retCode, string extra)
    {
        PacketType = packetType;
        Command = command;
        Sequence = sequence;
        RetCode = retCode;
        Payload = new BinaryPacket();
        Extra = extra;
    }

    public void Dispose() => Payload.Dispose();
}