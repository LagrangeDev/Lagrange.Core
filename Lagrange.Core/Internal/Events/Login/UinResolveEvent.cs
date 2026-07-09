using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class UinResolveEventReq(string qid) : ProtocolEvent
{
    public string Qid { get; } = qid;
}

internal class UinResolveEventResp : ProtocolEvent
{
    public byte State { get; }
    
    public (string, string)? Error { get; }
    
    public (long, string)? Info { get; }

    public byte[] Tlv104 { get; } = [];
    
    public UinResolveEventResp(byte retCode, (string, string)? error)
    {
        State = retCode;
        Error = error;
    }
    
    public UinResolveEventResp(byte retCode, (long, string) info, byte[] tlv104)
    {
        State = retCode;
        Info = info;
        Tlv104 = tlv104;
    }
}