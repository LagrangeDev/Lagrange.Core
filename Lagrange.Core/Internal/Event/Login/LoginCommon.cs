namespace Lagrange.Core.Internal.Event.Login;

internal class LoginCommon
{
    public enum Error : uint
    {
        TokenExpired = 140022015,
        UnusualVerify = 140022011,
        NewDeviceVerify = 140022010,
        CaptchaVerify = 140022008,
        Success = 0,
        Unknown = 1,
    }

    /*
     * 0 登录成功
     * 1 密码错误
     * 2 验证码
     * 32 被回收
     * 40 被冻结
     * 160 设备锁
     * 162 短信发送失败
     * 163 短信验证码错误
     * 180 回滚 (ecdh错误, ...)
     * 204 设备锁 验证
     * 235 版本过低
     * 237 上网环境异常
     * 239 设备锁
     * 243 非法来源禁止登录
     */
    public enum State : uint
    {
        Success = 0,
        PasswordError = 1,
        CaptchaVerify = 2,
        Recycle = 32,
        Freeze = 40,
        DeviceLock = 160,
        SmsSendFail = 162,
        SmsVerifyError = 163,
        Rollback = 180,
        DeviceLockVerify = 204,
        VersionLow = 235,
        NetworkAbnormal = 237,
        DeviceLock2 = 239,
        Unknown = 240,
        IllegalSource = 243,
    }
}