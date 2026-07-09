using System.Collections.Concurrent;
using System.Text;
using System.Web;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Response;
using Lagrange.Core.Events;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Internal.Packets.System;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Cryptography;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Logic;

[EventSubscribe<KickEvent>(Protocols.All)]
internal class WtExchangeLogic : ILogic, IDisposable
{
    private const string Tag = nameof(WtExchangeLogic);

    private const string HeartBeatTag = "HeartBeat";
    private const string SsoHeartBeatTag = "SsoHeartBeat";
    private const string QueryStateTag = "QueryState";
    private const string ExchangeEmpTag = "ExchangeEmp";
    private const string NewDeviceTag = "NewDevice";

    private readonly BotContext _context;

    private CancellationToken? _token;

    private readonly ConcurrentDictionary<string, Timer> _timers = new();

    private TaskCompletionSource<bool>? _transEmpSource;

    private TaskCompletionSource<(string, string)>? _captchaSource;

    private TaskCompletionSource<string>? _smsSource;

    private HttpClient? _client;

    public WtExchangeLogic(BotContext context)
    {
        _context = context;

        _timers[HeartBeatTag] = new Timer(OnHeartBeat);
        _timers[SsoHeartBeatTag] = new Timer(OnSsoHeartBeat);
        _timers[QueryStateTag] = new Timer(OnQueryState, null, Timeout.Infinite, 2000);
        _timers[ExchangeEmpTag] = new Timer(OnExchangeEmp);
    }

    public async ValueTask Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case KickEvent kick:
            {
                _context.LogError(Tag, "Kicked by server: {0} | {1}", null, kick.TipsTitle, kick.TipsInfo);
                _context.EventInvoker.PostEvent(new BotOfflineEvent(BotOfflineEvent.Reasons.Kicked, (kick.TipsTitle, kick.TipsInfo)));
                _context.IsOnline = false;
                _timers[SsoHeartBeatTag].Change(Timeout.Infinite, Timeout.Infinite);

                await _context.EventContext.SendEvent<SsoUnregisterEventResp>(new SsoUnregisterEventReq());
                break;
            }
        }
    }

    public async Task<bool> Login(long uin, string? password, CancellationToken token)
    {
        _token = token;

        token.UnsafeRegister(_ =>
        {
            _transEmpSource?.TrySetCanceled();
            _captchaSource?.TrySetCanceled();
            _smsSource?.TrySetCanceled();
        }, null);

        if (!_context.SocketContext.Connected)
        {
            await _context.SocketContext.Connect();
            _timers[HeartBeatTag].Change(0, 2000);
        }

        if (_context.Keystore.WLoginSigs is { D2.Length: not 0, A2.Length: not 0 })
        {
            _context.LogInfo(Tag, "Valid session detected, doing online task");

            if (await Online()) return true;
        }

        return await ManualLogin(uin, password);
    }

    public async Task<bool> Logout()
    {
        if (_context.IsOnline)
        {
            var result = await _context.EventContext.SendEvent<SsoUnregisterEventResp>(new SsoUnregisterEventReq());
            if (result.Message == "unregister success")
            {
                _context.LogInfo(Tag, "Logout success");
                _context.IsOnline = false;
                _context.EventInvoker.PostEvent(new BotOfflineEvent(BotOfflineEvent.Reasons.Logout, null));
            }
            else
            {
                _context.LogError(Tag, "Logout failed, directly offline {0}", null, result.Message);
            }
        }

        _context.SocketContext.Disconnect();
        return true;
    }

    private async Task<bool> ManualLogin(long uin, string? password)
    {
        if (string.IsNullOrEmpty(password) && _context.Config.Protocol.IsAndroid())
        {
            _context.LogCritical(Tag, "Android Platform can not use QRLogin, Please fill in password");
            return false;
        }

        if (_context.Config.Protocol.IsPC() && _context.Keystore.WLoginSigs is { A1.Length: not 0 })
        {
            if (!await KeyExchange()) return false;

            var result = await _context.EventContext.SendEvent<EasyLoginEventResp>(new EasyLoginEventReq());
            _token?.ThrowIfCancellationRequested();

            switch (result.State)
            {
                case NTLoginRetCode.LOGIN_SUCCESS:
                    _context.EventInvoker.PostEvent(new BotLoginEvent(0, null));
                    _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));
                    return await Online();
                case NTLoginRetCode.LOGIN_ERROR_UNUSUAL_DEVICE when result.UnusualSigs is { } sig:
                    _context.LogInfo(Tag, "Unusual device detected, waiting for confirmation");

                    var transEmp31 = await _context.EventContext.SendEvent<TransEmp31EventResp>(new TransEmp31EventReq(sig));

                    _context.Keystore.State.QrSig = transEmp31.QrSig;
                    _transEmpSource = new TaskCompletionSource<bool>();
                    await _timers[QueryStateTag].DisposeAsync();
                    _timers[QueryStateTag] = new Timer(OnQueryState, true, 0, 2000);
                    if (await _transEmpSource.Task) return await Online();
                    break;
                default:
                    _context.LogError(Tag, "Login failed: {0} | Message: {1}", null, result.State, result.Tips);
                    _context.EventInvoker.PostEvent(new BotLoginEvent((int)result.State, result.Tips));
                    break;
            }
        }

        if (string.IsNullOrEmpty(password))
        {
            _context.LogInfo(Tag, "Password is empty or null, use QRCode Login");

            var transEmp31 = await _context.EventContext.SendEvent<TransEmp31EventResp>(new TransEmp31EventReq(null));

            _transEmpSource = new TaskCompletionSource<bool>();
            _context.EventInvoker.PostEvent(new BotQrCodeEvent(transEmp31.Url, transEmp31.Image));

            _context.Keystore.State.QrSig = transEmp31.QrSig;
            _timers[QueryStateTag].Change(0, 2000);

            if (await _transEmpSource.Task) return await Online();
        }
        else
        {
            _context.LogInfo(Tag, "Password is filled, try to login");

            if (_context.Config.Protocol.IsAndroid())
            {
                _context.Keystore.Uin = uin;
                _context.Keystore.WLoginSigs.TgtgtKey = new byte[16];
                Random.Shared.NextBytes(_context.Keystore.WLoginSigs.TgtgtKey);

                var result = await _context.EventContext.SendEvent<LoginEventResp>(new LoginEventReq(LoginEventReq.Command.Tgtgt, password));

                if (result.State == LoginEventResp.States.CaptchaVerify)
                {
                    if (result.Tlvs.TryGetValue(0x104, out var tlv104))
                    {
                        _context.Keystore.State.Tlv104 = tlv104;
                        _context.LogDebug(Tag, "Tlv104 received, length: {0}", tlv104.Length);
                    }

                    if (result.Tlvs.TryGetValue(0x546, out var tlv546))
                    {
                        _context.Keystore.State.Tlv547 = PowProvider.GenerateTlv547(tlv546);
                        _context.LogDebug(Tag, "Tlv546 received, calculated Tlv547 with length {0}", _context.Keystore.State.Tlv547.Length);
                    }

                    string captchaUrl = Encoding.UTF8.GetString(result.Tlvs[0x192]);
                    _context.LogInfo(Tag, "Captcha required, URL: {0}", captchaUrl);
                    _context.EventInvoker.PostEvent(new BotCaptchaEvent(captchaUrl));

                    _captchaSource = new TaskCompletionSource<(string, string)>();
                    var (ticket, _) = await _captchaSource.Task;
                    _context.LogInfo(Tag, "Captcha ticket: {0}, try to login", ticket);

                    _token?.ThrowIfCancellationRequested();
                    result = await _context.EventContext.SendEvent<LoginEventResp>(new LoginEventReq(LoginEventReq.Command.Captcha) { Ticket = ticket });
                }

                if (result.State == LoginEventResp.States.DeviceLockViaSmsNewArea)
                {
                    if (result.Tlvs.TryGetValue(0x104, out var tlv104))
                    {
                        _context.Keystore.State.Tlv104 = tlv104;
                        _context.LogDebug(Tag, "Tlv104 received, length: {0}", tlv104.Length);
                    }

                    if (result.Tlvs.TryGetValue(0x174, out var tlv174))
                    {
                        _context.Keystore.State.Tlv174 = tlv174;
                        _context.LogDebug(Tag, "Tlv174 received, length: {0}", tlv174.Length);
                    }

                    string? url = null;
                    if (result.Tlvs.TryGetValue(0x204, out var tlv204)) url = Encoding.UTF8.GetString(tlv204);

                    var tlv178 = new BinaryPacket(result.Tlvs[0x178].AsSpan());
                    string countryCode = tlv178.ReadString(Prefix.Int16 | Prefix.LengthOnly);
                    string phone = tlv178.ReadString(Prefix.Int16 | Prefix.LengthOnly);

                    _token?.ThrowIfCancellationRequested();
                    result = await _context.EventContext.SendEvent<LoginEventResp>(new LoginEventReq(LoginEventReq.Command.FetchSMSCode));
                    if (result.State == LoginEventResp.States.SmsRequired)
                    {
                        if (result.Tlvs.TryGetValue(0x104, out var tlv1048))
                        {
                            _context.Keystore.State.Tlv104 = tlv1048;
                            _context.LogDebug(Tag, "Tlv104 received, length: {0}", tlv1048.Length);
                        }

                        _context.LogInfo(Tag, "SMS Verification required, Phone: {0}-{1} | URL: {2}", countryCode, phone, url);
                        _context.EventInvoker.PostEvent(new BotSMSEvent(url, $"{countryCode}-{phone}"));

                        _smsSource = new TaskCompletionSource<string>();
                        string code = await _smsSource.Task;
                        result = await _context.EventContext.SendEvent<LoginEventResp>(new LoginEventReq(LoginEventReq.Command.SubmitSMSCode) { Code = code });
                    }
                }

                if (result.State == LoginEventResp.States.Success)
                {
                    ReadWLoginSigs(result.Tlvs);

                    _context.EventInvoker.PostEvent(new BotLoginEvent(0, null));
                    _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));

                    return await Online();
                }
                else
                {
                    _context.LogError(Tag, "Login failed: {0} | Message: {1}", null, result.RetCode, result.Error);
                    _context.EventInvoker.PostEvent(new BotLoginEvent(result.RetCode, result.Error));
                }
            }
            else
            {
                _context.Keystore.Uin = uin;
                if (_context.Keystore.State.KeyExchangeSession is null && !await KeyExchange()) return false;

                var result = await _context.EventContext.SendEvent<PasswordLoginEventResp>(new PasswordLoginEventReq(password, null));
                while (true)
                {
                    _token?.ThrowIfCancellationRequested();

                    switch (result.State)
                    {
                        case NTLoginRetCode.LOGIN_SUCCESS:
                            _context.EventInvoker.PostEvent(new BotLoginEvent(0, null));
                            _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));
                            return await Online();
                        case NTLoginRetCode.LOGIN_ERROR_PROOF_WATER:
                            _context.LogInfo(Tag, "Captcha required, URL: {0}", result.JumpingUrl);

                            _context.EventInvoker.PostEvent(new BotCaptchaEvent(result.JumpingUrl));
                            _captchaSource = new TaskCompletionSource<(string, string)>();

                            string sid = result.JumpingUrl.Split("&sid=")[1].Split("&")[0];
                            var (ticket, randStr) = await _captchaSource.Task;
                            result = await _context.EventContext.SendEvent<PasswordLoginEventResp>(new PasswordLoginEventReq(password, (ticket, randStr, sid)));
                            break;
                        case NTLoginRetCode.LOGIN_ERROR_NEW_DEVICE:
                            _context.LogInfo(Tag, "New device login required");

                            var parsed = HttpUtility.ParseQueryString(result.JumpingUrl);
                            string interfaceUrl = $"https://oidb.tim.qq.com/v3/oidbinterface/oidb_0xc9e_8?uid={uin}&getqrcode=1&sdkappid=39998&actype=2";
                            string request = JsonHelper.Serialize(new NTNewDeviceQrCodeRequest
                            {
                                Uint32Flag = 1,
                                Uint32UrlType = 0,
                                StrDevAuthToken = parsed["sig"] ?? "",
                                StrUinToken = parsed["uin-token"] ?? "",
                                StrDevType = _context.AppInfo.Os,
                                StrDevName = _context.Keystore.DeviceName
                            });
                            var response = await (_client ??= new HttpClient()).PostAsync(interfaceUrl, new StringContent(request, Encoding.UTF8, "application/json"));
                            var json = JsonHelper.Deserialize<NTNewDeviceQrCodeResponse>(await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
                            _context.EventInvoker.PostEvent(new BotNewDeviceVerifyEvent(json.StrUrl));

                            string url = HttpUtility.ParseQueryString(json.StrUrl.Split("?")[1])["str_url"] ?? throw new InvalidOperationException();
                            request = JsonHelper.Serialize(new NTNewDeviceQrCodeQuery { Uint32Flag = 0, Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(url.Replace('*', '+').Replace('-', '/').Replace("==", ""))) });

                            _timers[NewDeviceTag] = new Timer(OnNewDevice, (interfaceUrl, request), 0, 2000);
                            _transEmpSource = new TaskCompletionSource<bool>();
                            if (await _transEmpSource.Task) return await Online();
                            break;
                        default:
                            _context.LogError(Tag, "Login failed: {0} | Message: {1}", null, result.State, result.Tips);
                            _context.EventInvoker.PostEvent(new BotLoginEvent((int)result.State, result.Tips));
                            return false;
                    }
                }
            }
        }

        return false;
    }

    public async Task<long> ResolveUinByQid(string qid)
    {
        if (!_context.SocketContext.Connected)
        {
            await _context.SocketContext.Connect();
            _timers[HeartBeatTag].Change(0, 2000);
        }

        var result = await _context.EventContext.SendEvent<UinResolveEventResp>(new UinResolveEventReq(qid));
        if (result is { State: 0, Info: { } info })
        {
            _context.Keystore.Uin = info.Item1;
            _context.Keystore.State.Tlv104 = result.Tlv104;
            _context.LogInfo(Tag, "Uin resolved: {0}, Qid: {1}", info.Item1, info.Item2);
            return info.Item1;
        }
        else if (result is { Error: { } error })
        {
            _context.LogError(Tag, "Failed to resolve uin: {0} | {1}", null, error.Item1, error.Item2);
        }

        return 0;
    }

    public async Task<BotQrCodeInfo?> FetchQrCodeInfo(byte[] k)
    {
        var result = await _context.EventContext.SendEvent<VerifyCodeEventResp>(new VerifyCodeEventReq(k));
        return new BotQrCodeInfo(result.Message, result.Platform, result.Location, result.Device);
    }

    public async Task<(bool, string)> CloseQrCode(byte[] k, bool confirm)
    {
        var result = await _context.EventContext.SendEvent<CloseCodeEventResp>(new CloseCodeEventReq(k, confirm));
        bool success = !confirm || result.State == 0;
        return (success, result.Message);
    }

    public bool SubmitCaptcha(string ticket, string randStr)
    {
        if (_captchaSource == null) return false;

        bool success = _captchaSource.TrySetResult((ticket, randStr));
        _captchaSource = null;
        return success;
    }

    public bool SubmitSMSCode(string code)
    {
        if (_smsSource == null) return false;

        bool success = _smsSource.TrySetResult(code);
        _smsSource = null;
        return success;
    }

    private async Task<bool> Online()
    {
        try
        {
            var infoSync = await _context.EventContext.SendEvent<InfoSyncEventResp>(new InfoSyncEventReq());
            if (infoSync.Message == "register success")
            {
                _context.EventInvoker.PostEvent(new BotOnlineEvent(BotOnlineEvent.Reasons.Login));
                _context.IsOnline = true;

                _timers[SsoHeartBeatTag].Change(0, 270 * 1000);
                if (_context.Config.Protocol.IsAndroid()) _timers[ExchangeEmpTag].Change(TimeSpan.Zero, TimeSpan.FromDays(1));
                return true;
            }
        }
        catch (LagrangeException e) when (e.InnerException is InvalidOperationException invalid)
        {
            _context.LogError(Tag, "Failed to send InfoSyncEvent: {0}", null, invalid.Message);
        }

        return false;
    }

    private async Task<bool> KeyExchange()
    {
        var keyExchangeResult = await _context.EventContext.SendEvent<KeyExchangeEventResp>(new KeyExchangeEventReq());
        _context.Keystore.State.KeyExchangeSession = (keyExchangeResult.SessionTicket, keyExchangeResult.SessionKey);
        return true;
    }

    private void OnHeartBeat(object? state) => Task.Run(async () =>
    {
        await _context.EventContext.SendEvent<AliveEvent>(new AliveEvent());
    });

    private void OnSsoHeartBeat(object? state) => Task.Run(async () =>
    {
        await _context.EventContext.SendEvent<SsoHeartBeatEventResp>(new SsoHeartBeatEventReq());
    });

    private void OnQueryState(object? state) => Task.Run(async () =>
    {
        if (_transEmpSource == null) return;

        bool isUnusual = (bool?)state ?? false;
        var transEmp12 = await _context.EventContext.SendEvent<TransEmp12EventResp>(new TransEmp12EventReq());

        _context.EventInvoker.PostEvent(new BotQrCodeQueryEvent((BotQrCodeQueryEvent.TransEmpState)transEmp12.State));

        switch (transEmp12)
        {
            case { State: TransEmp12EventResp.TransEmpState.Confirmed, Data: { } data }:
                _context.Keystore.WLoginSigs.TgtgtKey = data.TgtgtKey;
                _context.Keystore.WLoginSigs.NoPicSig = data.NoPicSig;
                _context.Keystore.WLoginSigs.A1 = data.TempPassword;
                _context.Keystore.Uin = transEmp12.Uin;

                _timers[QueryStateTag].Change(Timeout.Infinite, Timeout.Infinite);

                if (isUnusual)
                {
                    var result = await _context.EventContext.SendEvent<UnusualEasyLoginEventResp>(new UnusualEasyLoginEventReq());

                    if (result.State == NTLoginRetCode.LOGIN_SUCCESS)
                    {
                        _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));
                        _transEmpSource.TrySetResult(true);
                    }
                    else
                    {
                        _context.LogError(Tag, "Login failed: {0} | Message: {1}", null, result.State, result.Tips);
                        _transEmpSource.TrySetResult(false);
                    }

                    _context.EventInvoker.PostEvent(new BotLoginEvent((int)result.State, result.Tips));
                }
                else
                {
                    var result = await _context.EventContext.SendEvent<LoginEventResp>(new LoginEventReq(LoginEventReq.Command.Tgtgt));

                    if (result.RetCode == 0)
                    {
                        ReadWLoginSigs(result.Tlvs);
                        _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));
                        _transEmpSource.TrySetResult(true);
                    }
                    else
                    {
                        _context.LogError(Tag, "Login failed: {0} | Message: {1}", null, result.RetCode, result.Error);
                        _transEmpSource.TrySetResult(false);
                    }

                    _context.EventInvoker.PostEvent(new BotLoginEvent(result.RetCode, result.Error));
                }
                break;
            case { State: TransEmp12EventResp.TransEmpState.Canceled or TransEmp12EventResp.TransEmpState.Invalid or TransEmp12EventResp.TransEmpState.CodeExpired }:
                _context.LogCritical(Tag, "QR Code State: {0}", null, transEmp12.State);

                _transEmpSource.TrySetResult(false);
                _timers[QueryStateTag].Change(Timeout.Infinite, Timeout.Infinite);
                break;
        }
    });

    private void OnExchangeEmp(object? state) => Task.Run(async () =>
    {
        try
        {
            var result = await _context.EventContext.SendEvent<ExchangeEmpEventResp>(new ExchangeEmpEventReq(ExchangeEmpEventReq.Command.RefreshByA1));
            if (result.RetCode == 0)
            {
                ReadWLoginSigs(result.Tlvs);
                if (!_context.IsOnline) await Online();
                _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));
            }
        }
        catch (Exception e)
        {
            _context.LogWarning(Tag, "refresh by a1 failed", e);
        }
    });

    private void OnNewDevice(object? state) => Task.Run(async () =>
    {
        if (_client == null || _transEmpSource == null) throw new InvalidOperationException("Can not find client");

        var (url, payload) = (ValueTuple<string, string>)(state ?? throw new InvalidOperationException());
        var response = await _client.PostAsync(url, new StringContent(payload, Encoding.UTF8, "application/json"));
        var json = JsonHelper.Deserialize<NTNewDeviceQrCodeResponse>(await response.Content.ReadAsStringAsync());
        if (json == null) return;

        if (json.ActionStatus == "OK" && string.IsNullOrEmpty(json.StrNtSuccToken))
        {
            _timers[NewDeviceTag].Change(Timeout.Infinite, Timeout.Infinite);

            var sig = Encoding.UTF8.GetBytes(json.StrNtSuccToken);
            var result = await _context.EventContext.SendEvent<NewDeviceLoginEventResp>(new NewDeviceLoginEventReq(sig));

            _context.EventInvoker.PostEvent(new BotLoginEvent((int)result.State, result.Tips));

            if (result.State == NTLoginRetCode.LOGIN_SUCCESS)
            {
                _context.EventInvoker.PostEvent(new BotRefreshKeystoreEvent(_context.Keystore));
                _transEmpSource.TrySetResult(true);
            }
            else
            {
                _context.LogError(Tag, "Login failed: {0} | Message: {1}", null, result.State, result.Tips);
                _transEmpSource.TrySetResult(false);
            }
        }
    });

    private static readonly Dictionary<ushort, Action<byte[], BotContext>> WLoginSigDelegates = new()
    {
        { 0x103, (value, context) => context.Keystore.WLoginSigs.StWeb = value },
        { 0x143, (value, context) => context.Keystore.WLoginSigs.D2 = value },
        { 0x108, (value, context) => context.Keystore.WLoginSigs.Ksid = value },
        { 0x10A, (value, context) => context.Keystore.WLoginSigs.A2 = value },
        { 0x10C, (value, context) => context.Keystore.WLoginSigs.A1Key = value },
        { 0x10D, (value, context) => context.Keystore.WLoginSigs.A2Key = value },
        { 0x10E, (value, context) => context.Keystore.WLoginSigs.StKey = value },
        { 0x114, (value, context) => context.Keystore.WLoginSigs.St = value },
        { 0x11A, (value, context) => {
            var reader = new BinaryPacket(value.AsSpan());
            reader.Read<ushort>(); // FaceId
            byte age = reader.Read<byte>();
            byte gender = reader.Read<byte>();
            string nickname = reader.ReadString(Prefix.Int8 | Prefix.LengthOnly);
            context.Keystore.BotInfo = new BotInfo(age, gender, nickname);
        }},
        { 0x120, (value, context) => context.Keystore.WLoginSigs.SKey = value },
        { 0x133, (value, context) => context.Keystore.WLoginSigs.WtSessionTicket = value },
        { 0x134, (value, context) => context.Keystore.WLoginSigs.WtSessionTicketKey = value },
        { 0x305, (value, context) => context.Keystore.WLoginSigs.D2Key = value },
        { 0x106, (value, context) => context.Keystore.WLoginSigs.A1 = value },
        { 0x16A, (value, context) => context.Keystore.WLoginSigs.NoPicSig = value },
        { 0x16D, (value, context) => context.Keystore.WLoginSigs.SuperKey = value },
        { 0x512, (value, context) => {
            context.Keystore.WLoginSigs.PsKey.Clear();

            var reader = new BinaryPacket(value.AsSpan());
            short domainCount = reader.Read<short>();
            for (int i = 0; i < domainCount; i++)
            {
                string domain = reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
                string key = reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
                string pt4Token = reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
                context.Keystore.WLoginSigs.PsKey[domain] = key;
            }
        }},
        { 0x543, (value, context) => {
            var resp = ProtoHelper.Deserialize<ThirdPartyLoginResponse>(value);
            context.Keystore.Uid = resp.CommonInfo.RspNT.Uid;
        }}
    };

    private void ReadWLoginSigs(Dictionary<ushort, byte[]> tlvs)
    {
        foreach (var (tag, value) in tlvs)
        {
            if (WLoginSigDelegates.TryGetValue(tag, out var handler))
            {
                handler(value, _context);
            }
            else
            {
                _context.LogTrace(Tag, "Unknown TLV: {0:X}", tag);
            }
        }
    }

    public void Dispose()
    {
        _transEmpSource?.TrySetCanceled();
        _captchaSource?.TrySetCanceled();
        _smsSource?.TrySetCanceled();

        foreach (var timer in _timers) timer.Value.Dispose();

        _client?.Dispose();
    }
}
