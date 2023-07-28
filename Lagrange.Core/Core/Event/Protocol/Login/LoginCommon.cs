namespace Lagrange.Core.Core.Event.Protocol.Login;

internal class LoginCommon
{
    public enum Error : uint
    {
        TokenExpired = 140022015,
        UnusualVerify = 140022011,
        CaptchaVerify = 140022008,
        Success = 0,
        Unknown = 1,
    }
}