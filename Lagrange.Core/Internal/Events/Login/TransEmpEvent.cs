using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class TransEmp31EventReq(byte[]? unusualSig) : ProtocolEvent
{
    public byte[]? UnusualSig { get; } = unusualSig;
}

internal class TransEmp31EventResp(string url, byte[] image, byte[] qrSig) : ProtocolEvent
{
    public string Url { get; } = url;

    public byte[] Image { get; } = image;
    
    public byte[] QrSig { get; } = qrSig;
}

internal class TransEmp12EventReq : ProtocolEvent;

internal class TransEmp12EventResp(byte state, long uin, (byte[], byte[], byte[])? data) : ProtocolEvent
{
    public TransEmpState State { get; } = (TransEmpState)state;
    
    public long Uin { get; } = uin;

    public (byte[] TgtgtKey, byte[] NoPicSig, byte[] TempPassword)? Data { get; } = data;

    internal enum TransEmpState : byte
    {
        Confirmed = 0,
        CodeExpired = 17,
        WaitingForScan = 48,
        WaitingForConfirm = 53,
        Canceled = 54,
        Invalid = 144
    }
}
