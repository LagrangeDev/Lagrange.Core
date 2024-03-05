namespace Lagrange.Core.Internal.Service;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class ServiceAttribute : Attribute
{
    public string Command { get; }
    
    public uint PacketType { get; set; }

    public byte EncodeType { get; set; }

    public ServiceAttribute(string command, uint packetType = 12, byte encodeType = 1)
    {
        Command = command;
        EncodeType = encodeType;
        PacketType = packetType;
    }
}