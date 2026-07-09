using Lagrange.Core.Events;
using Lagrange.Core.Internal.Packets.Login;

namespace Lagrange.Core.Internal.Events.Login;

internal interface INTLoginEventResp
{
    public NTLoginRetCode State { get; }
    
    public (string, string) Tips { get; }
}

internal class EasyLoginEventReq : ProtocolEvent;

internal class UnusualEasyLoginEventReq : ProtocolEvent;

internal class NewDeviceLoginEventReq(byte[] sig) : ProtocolEvent
{
    public byte[] Sig { get; } = sig;
};

internal class RefreshTicketEventReq : ProtocolEvent;

internal class RefreshA2EventReq : ProtocolEvent;

internal class PasswordLoginEventReq(string password, (string, string, string)? captcha) : ProtocolEvent
{
    public string Password { get; } = password;

    public (string, string, string)? Captcha { get; } = captcha;
}

internal class PasswordLoginEventResp(NTLoginRetCode state, (string, string)? tips, string? jumpingUrl) : ProtocolEvent, INTLoginEventResp
{
    public NTLoginRetCode State { get; } = state;

    public (string, string) Tips { get; } = tips ?? (string.Empty, string.Empty);

    public string JumpingUrl { get; } = jumpingUrl ?? string.Empty;
}

internal class EasyLoginEventResp(NTLoginRetCode state, (string, string)? tips, byte[]? unusualSigs) : ProtocolEvent, INTLoginEventResp
{
    public NTLoginRetCode State { get; } = state;

    public (string, string) Tips { get; } = tips ?? (string.Empty, string.Empty);
    
    public byte[]? UnusualSigs { get; } = unusualSigs;
}

internal class UnusualEasyLoginEventResp(NTLoginRetCode state, (string, string)? tips) : ProtocolEvent
{
    public NTLoginRetCode State { get; } = state;

    public (string, string) Tips { get; } = tips ?? (string.Empty, string.Empty);
}

internal class NewDeviceLoginEventResp(NTLoginRetCode state, (string, string)? tips) : ProtocolEvent
{
    public NTLoginRetCode State { get; } = state;

    public (string, string) Tips { get; } = tips ?? (string.Empty, string.Empty);
}

internal class RefreshTicketEventResp(NTLoginRetCode state, (string, string)? tips) : ProtocolEvent
{
    public NTLoginRetCode State { get; } = state;

    public (string, string) Tips { get; } = tips ?? (string.Empty, string.Empty);
}

internal class RefreshA2EventResp(NTLoginRetCode state, (string, string)? tips) : ProtocolEvent
{
    public NTLoginRetCode State { get; } = state;

    public (string, string) Tips { get; } = tips ?? (string.Empty, string.Empty);
}