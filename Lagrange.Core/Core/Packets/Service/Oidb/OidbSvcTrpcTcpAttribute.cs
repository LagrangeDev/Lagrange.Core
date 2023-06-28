namespace Lagrange.Core.Core.Packets.Service.Oidb;

[AttributeUsage(AttributeTargets.Class)]
internal class OidbSvcTrpcTcpAttribute : Attribute
{
    public uint Command { get; set; }
    
    public uint SubCommand { get; set; }
    
    public bool IsLafter { get; set; }
    
    public OidbSvcTrpcTcpAttribute(uint command, uint subCommand, bool isLafter = false)
    {
        Command = command;
        SubCommand = subCommand;
        IsLafter = isLafter;
    }
}