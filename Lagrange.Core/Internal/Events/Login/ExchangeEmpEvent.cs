using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class ExchangeEmpEventReq(ExchangeEmpEventReq.Command cmd) : ProtocolEvent
{
    public enum Command
    {
        RefreshByA1 = 0xF
    }
    
    public Command Cmd { get; } = cmd;
}

internal class ExchangeEmpEventResp(byte retCode, Dictionary<ushort, byte[]> tlvs) : ProtocolEvent
{
    public byte RetCode { get; } = retCode;

    public Dictionary<ushort, byte[]> Tlvs { get; set; } = tlvs;
}