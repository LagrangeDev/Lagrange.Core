using System.Text.Json;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Login.NTLogin;
using Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;
using Lagrange.Core.Internal.Service;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Network;

// ReSharper disable AsyncVoidLambda

namespace Lagrange.Core.Internal.Context.Logic.Implementation;

[EventSubscribe(typeof(TransEmpEvent))]
[EventSubscribe(typeof(WtLoginEvent))]
[EventSubscribe(typeof(KickNTEvent))]
[BusinessLogic("WtExchangeLogic", "Manage the online task of the Bot")]
internal class WtExchangeLogic : LogicBase
{
    private const string Tag = nameof(WtExchangeLogic);

    private readonly TaskCompletionSource<bool> _transEmpTask;
    private TaskCompletionSource<(string, string)>? _captchaTask;
    private TaskCompletionSource<string>? _newDeviceTask;

    private const string Interface = "https://ntlogin.qq.com/qr/getFace";

    private const string QueryEvent = "wtlogin.trans_emp CMD0x12";

    internal WtExchangeLogic(ContextCollection collection) : base(collection)
    {
        _transEmpTask = new TaskCompletionSource<bool>();
    }

    public override async Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case KickNTEvent kick:
                Collection.Log.LogFatal(Tag, $"KickNTEvent: {kick.Tag}: {kick.Message}");
                Collection.Log.LogFatal(Tag, "Bot will be offline in 5 seconds...");
                await Task.Delay(5000);

                Collection.Invoker.PostEvent(new BotOfflineEvent(kick.Tag, kick.Message)); // TODO: Fill in the reason of offline
                Collection.Scheduler.Dispose();
                break;
        }
    }

    /// <summary>
    /// <para>1. resolve wtlogin.trans_emp CMD0x31 packet</para>
    /// <para>2. Schedule wtlogin.trans_emp CMD0x12 Task</para>
    /// </summary>
    public async Task<(string, byte[])?> FetchQrCode()
    {
        Collection.Log.LogInfo(Tag, "Connecting Servers...");
        if (!await Collection.Socket.Connect()) return null;
        Collection.Scheduler.Interval("Heartbeat.Alive", 10 * 1000, async () => await Collection.Business.PushEvent(AliveEvent.Create()));

        var transEmp = TransEmpEvent.Create(TransEmpEvent.State.FetchQrCode);
        var result = await Collection.Business.SendEvent(transEmp);

        if (result.Count != 0)
        {
            var @event = (TransEmpEvent)result[0];

            Collection.Log.LogInfo(Tag, $"QrCode Fetched, Expiration: {@event.Expiration} seconds");
            return (@event.Url, @event.QrCode);
        }
        return null;
    }

    public Task LoginByQrCode()
    {
        Collection.Scheduler.Interval(QueryEvent, 2 * 1000, async () => await QueryTransEmpState(async @event =>
        {
            return await DoWtLogin();
        }));

        return _transEmpTask.Task;
    }

    public async Task<bool> LoginByPassword()
    {
        if (!Collection.Socket.Connected) // if socket not connected, try to connect
        {
            if (!await Collection.Socket.Connect()) return false;
            Collection.Scheduler.Interval("Heartbeat.Alive", 10 * 1000, async () => await Collection.Business.PushEvent(AliveEvent.Create()));
        }

        if (Collection.AppInfo.Os == "Android")
        {
            if (Collection.Keystore.Session.TempPassword != null)
            {
                Collection.Log.LogInfo(Tag, "Trying to do Exchange...");

                var exchangeEmpEvent = ExchangeEmpEvent.Create(ExchangeEmpEvent.State.RefreshToken);
                var exchangeEmpResult = await Collection.Business.SendEvent(exchangeEmpEvent);

                if (exchangeEmpResult.Count != 0)
                {
                    var @event = (ExchangeEmpEvent)exchangeEmpResult[0];
                    if ((ExchangeEmp.State)@event.ResultCode != ExchangeEmp.State.Success)
                    {
                        Collection.Log.LogWarning(Tag, @event is { Message: not null, Tag: not null }
                            ? $"Login Failed: {(ExchangeEmp.State)@event.ResultCode} ({@event.ResultCode}) | {@event.Tag}: {@event.Message}"
                            : $"Login Failed: {(ExchangeEmp.State)@event.ResultCode} ({@event.ResultCode})");
                        return false;
                    }

                    Collection.Log.LogInfo(Tag, "Exchange Success");
                    await BotOnline();
                    return true;
                }
            }
            else
            {
                var wtLoginEvent = WtLoginEvent.Create(WtLoginEvent.State.Login);

            login:
                var wtLoginResult = await Collection.Business.SendEvent(wtLoginEvent);

                if (wtLoginResult.Count != 0)
                {
                    var @event = (WtLoginEvent)wtLoginResult[0];
                    switch ((LoginCommon.State)@event.ResultCode)
                    {
                        case LoginCommon.State.Success:
                            {
                                Collection.Log.LogInfo(Tag, "Login Success");

                                await BotOnline();
                                return true;
                            }
                        case LoginCommon.State.CaptchaVerify:
                            {
                                Collection.Log.LogInfo(Tag, "Login Success, but captcha is required, please follow the link from event");

                                if (Collection.Keystore.Session.CaptchaUrl != null)
                                {
                                    var captchaEvent = new BotCaptchaEvent(Collection.Keystore.Session.CaptchaUrl);
                                    Collection.Invoker.PostEvent(captchaEvent);

                                    string aid;
                                    try
                                    {
                                        aid = Collection.Keystore.Session.CaptchaUrl.Split("&sid=")[1].Split("&")[0];
                                    }
                                    catch
                                    {
                                        aid = "";
                                    }
                                    _captchaTask = new TaskCompletionSource<(string, string)>();
                                    var (ticket, randStr) = await _captchaTask.Task;
                                    Collection.Keystore.Session.Captcha = new ValueTuple<string, string, string>(ticket, randStr, aid);

                                    wtLoginEvent = WtLoginEvent.Create(WtLoginEvent.State.SubmitCaptcha);
                                    goto login;
                                }

                                Collection.Log.LogInfo(Tag, "Captcha Url is null, please try again later");
                                return false;
                            }
                        case LoginCommon.State.DeviceLock2:
                            {
                                Collection.Log.LogInfo(Tag, "Login Success, but sms code is required, are you sure send sms?");

                                if (Collection.Keystore.Session.PhoneNumber != null)
                                {
                                    var newDeviceEvent = new BotNewDeviceVerifyEvent(Collection.Keystore.Session.PhoneNumber);
                                    Collection.Invoker.PostEvent(newDeviceEvent);

                                    _newDeviceTask = new TaskCompletionSource<string>();
                                    var smsCode = await _newDeviceTask.Task; // need to add timeout
                                    Collection.Keystore.Session.SmsCode = smsCode;

                                    wtLoginEvent = WtLoginEvent.Create(WtLoginEvent.State.SubmitSmsCode);
                                    goto login;
                                }

                                Collection.Log.LogInfo(Tag, "Phone Number is null, please try again later");
                                return false;
                            }
                        default:
                            {
                                Collection.Log.LogWarning(Tag, @event is { Message: not null, Tag: not null }
                                    ? $"Login Failed: {(LoginCommon.State)@event.ResultCode} ({@event.ResultCode}) | {@event.Tag}: {@event.Message}"
                                    : $"Login Failed: {(LoginCommon.State)@event.ResultCode} ({@event.ResultCode})");

                                Collection.Invoker.Dispose();
                                return false;
                            }
                    }
                }
            }
        }
        else
        {
            if (Collection.Keystore.Session.ExchangeKey == null)
            {
                if (!await KeyExchange())
                {
                    Collection.Log.LogInfo(Tag, "Key Exchange Failed, please try again later");
                    return false;
                }
            }

            if (Collection.Keystore.Session.TempPassword != null) // try EasyLogin
            {
                Collection.Log.LogInfo(Tag, "Trying to Login by EasyLogin...");
                var easyLoginEvent = EasyLoginEvent.Create();
                var easyLoginResult = await Collection.Business.SendEvent(easyLoginEvent);

                if (easyLoginResult.Count != 0)
                {
                    switch ((LoginCommon.Error)easyLoginResult[0].ResultCode)
                    {
                        case LoginCommon.Error.Success:
                            {
                                Collection.Log.LogInfo(Tag, "Login Success");

                                await BotOnline();
                                return true;
                            }
                        case LoginCommon.Error.UnusualVerify:
                            {
                                Collection.Log.LogInfo(Tag, "Verification needed");

                                if (!await FetchUnusual())
                                {
                                    Collection.Log.LogInfo(Tag, "Fetch unusual state failed");
                                    return false;
                                }

                                Collection.Scheduler.Interval(QueryEvent, 2 * 1000, async () => await QueryTransEmpState(async e =>
                                {
                                    if (e.ResultCode == 0)
                                    {
                                        if (await DoWtLogin())
                                            return await DoUnusualEasyLogin();
                                    }

                                    return false;
                                }));
                                bool result = await _transEmpTask.Task;
                                if (result) await BotOnline();
                                return result;
                            }
                        default:
                            {
                                if (Collection.Keystore.PasswordMd5 != null && Collection.Keystore.PasswordMd5.Length == 16)
                                {
                                    Collection.Log.LogWarning(Tag, $"Fast Login Failed with code {easyLoginResult[0].ResultCode}, trying to Login by Password...");

                                    Collection.Keystore.Session.TempPassword = null; // clear temp password
                                    return await LoginByPassword(); // try password login
                                }
                                else
                                {
                                    Collection.Log.LogWarning(Tag, $"Fast Login Failed with code {easyLoginResult[0].ResultCode}");
                                    return false;
                                }
                            }
                    }
                }
            }
            else
            {
                Collection.Log.LogInfo(Tag, "Trying to Login by Password...");
                var passwordLoginEvent = PasswordLoginEvent.Create();
                var passwordLoginResult = await Collection.Business.SendEvent(passwordLoginEvent);

                if (passwordLoginResult.Count != 0)
                {
                    var @event = (PasswordLoginEvent)passwordLoginResult[0];
                    switch ((LoginCommon.Error)@event.ResultCode)
                    {
                        case LoginCommon.Error.Success:
                            {
                                Collection.Log.LogInfo(Tag, "Login Success");

                                await BotOnline();
                                return true;
                            }
                        case LoginCommon.Error.UnusualVerify:
                            {
                                Collection.Log.LogInfo(Tag, "Unusual Verify is not currently supported for PasswordLogin");
                                return true;
                            }
                        case LoginCommon.Error.CaptchaVerify:
                            {
                                Collection.Log.LogInfo(Tag, "Login Success, but captcha is required, please follow the link from event");

                                if (Collection.Keystore.Session.CaptchaUrl != null)
                                {
                                    var captchaEvent = new BotCaptchaEvent(Collection.Keystore.Session.CaptchaUrl);
                                    Collection.Invoker.PostEvent(captchaEvent);

                                    string aid = Collection.Keystore.Session.CaptchaUrl.Split("&sid=")[1].Split("&")[0];
                                    _captchaTask = new TaskCompletionSource<(string, string)>();
                                    var (ticket, randStr) = await _captchaTask.Task;
                                    Collection.Keystore.Session.Captcha = new ValueTuple<string, string, string>(ticket, randStr, aid);

                                    return await LoginByPassword();
                                }

                                Collection.Log.LogInfo(Tag, "Captcha Url is null, please try again later");
                                return false;
                            }
                        case LoginCommon.Error.NewDeviceVerify:
                            {
                                Collection.Log.LogInfo(Tag, $"NewDeviceVerify Url: {Collection.Keystore.Session.NewDeviceVerifyUrl}");

                                // Todo: Implement NewDeviceVerify
                                var newDeviceEvent = new BotNewDeviceVerifyEvent("");
                                Collection.Invoker.PostEvent(newDeviceEvent);

                                bool result = await _transEmpTask.Task;
                                if (result) await BotOnline();
                                return result;
                            }
                        default:
                            {
                                Collection.Log.LogWarning(Tag, @event is { Message: not null, Tag: not null }
                                    ? $"Login Failed: {(LoginCommon.Error)@event.ResultCode} ({@event.ResultCode}) | {@event.Tag}: {@event.Message}"
                                    : $"Login Failed: {(LoginCommon.Error)@event.ResultCode} ({@event.ResultCode})");

                                Collection.Invoker.Dispose();
                                return false;
                            }
                    }
                }
            }
        }
        return false;
    }

    public async Task<bool> SendSmsCode()
    {
        var wtLoginEvent = WtLoginEvent.Create(WtLoginEvent.State.RequestSendSms);
        var wtLoginResult = await Collection.Business.SendEvent(wtLoginEvent);

        if (wtLoginResult.Count != 0)
        {
            var @event = (WtLoginEvent)wtLoginResult[0];
            switch ((LoginCommon.State)@event.ResultCode)
            {
                case LoginCommon.State.DeviceLock:
                    {
                        Collection.Log.LogInfo(Tag, "Send SmsCode Success");
                        return true;
                    }
                default:
                    {
                        Collection.Log.LogWarning(Tag, @event is { Message: not null, Tag: not null }
                            ? $"Send SmsCode Failed: {(LoginCommon.State)@event.ResultCode} ({@event.ResultCode}) | {@event.Tag}: {@event.Message}"
                            : $"Send SmsCode Failed: {(LoginCommon.State)@event.ResultCode} ({@event.ResultCode})");

                        Collection.Invoker.Dispose();
                        return false;
                    }
            }
        }

        return false;
    }

    private async Task<bool> KeyExchange()
    {
        var keyExchangeEvent = KeyExchangeEvent.Create();
        var exchangeResult = await Collection.Business.SendEvent(keyExchangeEvent);
        if (exchangeResult.Count != 0)
        {
            Collection.Log.LogInfo(Tag, "Key Exchange successfully!");
            return true;
        }

        return false;
    }

    private async Task<bool> DoWtLogin()
    {
        Collection.Log.LogInfo(Tag, "Doing Login...");
        Collection.Keystore.Session.Sequence = 0;

        var loginEvent = WtLoginEvent.Create(WtLoginEvent.State.LoginWithA2);
        var result = await Collection.Business.SendEvent(loginEvent);

        if (result.Count != 0)
        {
            var @event = (WtLoginEvent)result[0];
            if (@event.ResultCode == 0)
            {
                Collection.Log.LogInfo(Tag, "Login Success");
                Collection.Log.LogInfo(Tag, Collection.Keystore.Info.ToString());
                await BotOnline();

                return true;
            }

            Collection.Log.LogFatal(Tag, $"Login failed: {@event.ResultCode}");
            Collection.Log.LogFatal(Tag, $"Tag: {@event.Tag}\nState: {@event.Message}");
        }

        return false;
    }

    private async Task QueryTransEmpState(Func<TransEmpEvent, Task<bool>> callback)
    {
        if (Collection.Keystore.Session.QrSig != null)
        {
            var request = new NTLoginHttpRequest
            {
                Appid = Collection.AppInfo.AppId,
                Qrsig = Collection.Keystore.Session.QrSig,
                FaceUpdateTime = 0
            };

            var payload = JsonSerializer.SerializeToUtf8Bytes(request);
            var response = await Http.PostAsync(Interface, payload, "application/json");
            var info = JsonSerializer.Deserialize<NTLoginHttpResponse>(response);
            if (info != null) Collection.Keystore.Uin = info.Uin;
        }

        var transEmp = TransEmpEvent.Create(TransEmpEvent.State.QueryResult);
        var result = await Collection.Business.SendEvent(transEmp);

        if (result.Count != 0)
        {
            var @event = (TransEmpEvent)result[0];
            var state = (TransEmp.State)@event.ResultCode;
            Collection.Log.LogInfo(Tag, $"QrCode State Queried: {state} Uin: {Collection.Keystore.Uin}");

            switch (state)
            {
                case TransEmp.State.Confirmed:
                    {
                        Collection.Log.LogInfo(Tag, "QrCode Confirmed, Logging in with A1 sig...");
                        Collection.Scheduler.Cancel(QueryEvent); // cancel query task
                        _transEmpTask.SetResult(await callback.Invoke(@event));
                        break;
                    }
                case TransEmp.State.CodeExpired:
                    {
                        Collection.Log.LogWarning(Tag, "QrCode Expired, Please Fetch QrCode Again");
                        Collection.Scheduler.Cancel(QueryEvent);
                        Collection.Scheduler.Dispose();

                        _transEmpTask.SetResult(false);
                        return;
                    }
                case TransEmp.State.Canceled:
                    {
                        Collection.Log.LogWarning(Tag, "QrCode Canceled, Please Fetch QrCode Again");
                        Collection.Scheduler.Cancel(QueryEvent);
                        Collection.Scheduler.Dispose();

                        _transEmpTask.SetResult(false);
                        return;
                    }
                case TransEmp.State.WaitingForConfirm:
                case TransEmp.State.WaitingForScan:
                default:
                    break;
            }
        }

    }

    private async Task BotOnline()
    {
        var onlineEvent = new BotOnlineEvent();
        Collection.Invoker.PostEvent(onlineEvent);

        var registerEvent = StatusRegisterEvent.Create();
        var registerResponse = await Collection.Business.SendEvent(registerEvent);
        var heartbeatDelegate = new Action(async () => await Collection.Business.PushEvent(SsoAliveEvent.Create()));
        Collection.Log.LogInfo(Tag, $"Register Status: {((StatusRegisterEvent)registerResponse[0]).Message}");
        Collection.Scheduler.Interval("SsoHeartBeat", (int)(4.5 * 60 * 1000), heartbeatDelegate);

        await Collection.Business.PushEvent(InfoSyncEvent.Create());
    }

    private async Task<bool> FetchUnusual()
    {
        var transEmp = TransEmpEvent.Create(TransEmpEvent.State.FetchQrCode);
        var result = await Collection.Business.SendEvent(transEmp);

        if (result.Count != 0)
        {
            Collection.Log.LogInfo(Tag, "Confirmation Request Send");
            return true;
        }

        return false;
    }

    private async Task<bool> DoUnusualEasyLogin()
    {
        Collection.Log.LogInfo(Tag, "Trying to Login by EasyLogin...");
        var unusualEvent = UnusualEasyLoginEvent.Create();
        var result = await Collection.Business.SendEvent(unusualEvent);
        return result.Count != 0 && ((UnusualEasyLoginEvent)result[0]).Success;
    }

    private async Task<bool> DoNewDeviceLogin()
    {
        Collection.Log.LogInfo(Tag, "Trying to Login by EasyLogin...");
        var unusualEvent = NewDeviceLoginEvent.Create();
        var result = await Collection.Business.SendEvent(unusualEvent);
        return result.Count != 0 && ((NewDeviceLoginEvent)result[0]).Success;
    }

    public bool SubmitCaptcha(string ticket, string randStr) => _captchaTask?.TrySetResult((ticket, randStr)) ?? false;
    public bool SubmitSmsCode(string smsCode) => _newDeviceTask?.TrySetResult(smsCode) ?? false;
}