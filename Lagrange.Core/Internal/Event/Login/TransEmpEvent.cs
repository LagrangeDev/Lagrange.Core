namespace Lagrange.Core.Internal.Event.Login;
#pragma warning disable CS8618

internal class TransEmpEvent : ProtocolEvent
{
    public State EventState { get; set; }

    #region TransEmp CMD0x31
    public byte[] QrCode { get; }
    public uint Expiration { get; }
    public string Url { get; }
    #endregion

    private TransEmpEvent(State eventState) : base(true) => EventState = eventState;

    private TransEmpEvent(int result) : base(result) { }

    private TransEmpEvent(int result, byte[] qrCode, uint expiration, string url) : base(result)
    {
        QrCode = qrCode;
        Expiration = expiration;
        Url = url;
    }

    public static TransEmpEvent Create(State eventState) => new(eventState);

    public static TransEmpEvent Result(int result) => new(result);

    public static TransEmpEvent Result(byte[] qrCode, uint expiration, string url) =>
        new(0, qrCode, expiration, url);

    public enum State : ushort
    {
        FetchQrCode = 0x31,
        QueryResult = 0x12
    }
}