using Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;

#pragma warning disable CS8618
namespace Lagrange.Core.Internal.Event.Login;

internal class TransEmpEvent : ProtocolEvent
{
    public State EventState { get; set; }

    #region TransEmp CMD0x31

    public byte[] QrCode { get; }
    
    public uint Expiration { get; }
    
    public string Url { get; }

    public string QrSig { get; }
    
    public byte[] Signature { get; }

    #endregion

    #region TransEmp CMD0x12

    public byte[]? TgtgtKey { get; }
    
    public byte[]? TempPassword { get; }
    
    public byte[]? NoPicSig { get; }

    #endregion
    
    private TransEmpEvent(State eventState) : base(true) => EventState = eventState;

    private TransEmpEvent(int result, byte[] qrCode, uint expiration, string url, string qrSig, byte[] signature) 
        : base(result)
    {
        EventState = State.FetchQrCode;
        
        QrCode = qrCode;
        Expiration = expiration;
        Url = url;
        QrSig = qrSig;
        Signature = signature;
    }

    private TransEmpEvent(int result, byte[]? tgtgtKey, byte[]? tempPassword, byte[]? noPicSig) : base(result)
    {
        TgtgtKey = tgtgtKey;
        TempPassword = tempPassword;
        NoPicSig = noPicSig;
        EventState = State.QueryResult;
    }
    
    public static TransEmpEvent Create(State eventState) => new(eventState);

    public static TransEmpEvent Result(byte[] qrCode, uint expiration, string url, string qrSig, byte[] signature) => 
        new(0, qrCode, expiration, url, qrSig, signature);
    
    public static TransEmpEvent Result(TransEmp12.State state, byte[]? tgtgtKey, byte[]? tempPassword, byte[]? noPicSig) => 
        new((int)state, tgtgtKey, tempPassword, noPicSig);

    public enum State : byte
    {
        FetchQrCode = 0x31,
        QueryResult = 0x12
    }
}