namespace Lagrange.Core.Internal.Service;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class ServiceAttribute : Attribute
{
    public string Command { get; }
    
    public byte PacketType { get; set; }
    
    public ServiceAttribute(string command, byte packetType = 12)
    {
        Command = command;
        PacketType = packetType;
    }
}