using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Login;

internal class LoginEventReq(LoginEventReq.Command cmd, string password = "") : ProtocolEvent
{
    public enum Command
    {
        Tgtgt = 0x09,
        Captcha = 0x02,
        FetchSMSCode = 0x08,
        SubmitSMSCode = 0x07
    }
    
    public Command Cmd { get; } = cmd;
    
    public string Password { get; } = password;

    public string Ticket { get; init; } = string.Empty;
    
    public string Code { get; init; } = string.Empty;
}

internal class LoginEventResp : ProtocolEvent
{
    public byte RetCode { get; }
    
    public States State => (States)RetCode;
    
    public (string, string)? Error { get; }
    
    public Dictionary<ushort, byte[]> Tlvs { get; set; }

    public LoginEventResp(byte retCode, (string, string) error)
    {
        RetCode = retCode;
        Error = error;
        Tlvs = new Dictionary<ushort, byte[]>();
    }
    
    public LoginEventResp(byte retCode, Dictionary<ushort, byte[]> tlvs)
    {
        RetCode = retCode;
        Tlvs = tlvs;
    }

    public enum States
    {
        Success = 0,
        CaptchaVerify = 2,
        SmsRequired = 160,
        DeviceLock = 204,
        DeviceLockViaSmsNewArea = 239,

        PreventByIncorrectPassword = 1,
        PreventByReceiveIssue = 3,
        PreventByTokenExpired = 15,
        PreventByAccountBanned = 40,
        PreventByOperationTimeout = 155,
        PreventBySmsSentFailed = 162,
        PreventByIncorrectSmsCode = 163,
        PreventByLoginDenied = 167,
        PreventByOutdatedVersion = 235,
        PreventByHighRiskOfEnvironment = 237,
        Unknown = 240,
    }
}