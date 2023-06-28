using System.Text.Json;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Context.Attributes;
using Lagrange.Core.Core.Event.EventArg;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Login;
using Lagrange.Core.Core.Event.Protocol.Login.Ecdh;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets.Login.NTLogin;
using Lagrange.Core.Core.Packets.Login.WtLogin.Entity;
using Lagrange.Core.Core.Service;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Network;

// ReSharper disable AsyncVoidLambda

namespace Lagrange.Core.Core.Context.Logic.Implementation;

[EventSubscribe(typeof(TransEmpEvent))]
[EventSubscribe(typeof(LoginEvent))]
[EventSubscribe(typeof(KickNTEvent))]
[BusinessLogic("WtExchangeLogic", "Manage the online task of the Bot")]
internal class WtExchangeLogic : LogicBase
{
    private const string Tag = nameof(WtExchangeLogic);

    private const string Interface = "https://ntlogin.qq.com/qr/getFace";

    private const string QueryEvent = "wtlogin.trans_emp CMD0x12";

    internal WtExchangeLogic(ContextCollection collection) : base(collection) { }

    public override async Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case KickNTEvent kick:
                Collection.Log.LogFatal(Tag, $"KickNTEvent: {kick.Tag}: {kick.Message}");
                Collection.Log.LogFatal(Tag, "Bot will be offline in 5 seconds...");
                await Task.Delay(5000);
                
                Collection.Invoker.PostEvent(new BotOfflineEvent()); // TODO: Fill in the reason of offline
                Collection.Scheduler.Dispose();
                break;
        }
    }

    /// <summary>
    /// <para>1. resolve wtlogin.trans_emp CMD0x31 packet</para>
    /// <para>2. Schedule wtlogin.trans_emp CMD0x12 Task</para>
    /// </summary>
    public async Task<byte[]?> FetchQrCode()
    {
        Collection.Log.LogInfo(Tag, "Connecting Servers...");
        if (!await Collection.Socket.Connect()) return null;
        Collection.Scheduler.Interval("Heartbeat.Alive", 10 * 1000, async () => await Collection.Business.PushEvent(AliveEvent.Create()));
        
        var transEmp = TransEmpEvent.Create(TransEmpEvent.State.FetchQrCode);
        var result = await Collection.Business.SendEvent(transEmp);

        if (result.Count != 0)
        {
            var @event = (TransEmpEvent)result[0];
            Collection.Keystore.Session.QrString = @event.QrSig;
            Collection.Keystore.Session.QrSign = @event.Signature;
            
            Collection.Log.LogInfo(Tag, $"QrCode Fetched, Expiration: {@event.Expiration} seconds");
            return @event.QrCode;
        }
        return null;
    }

    public async Task LoginByQrCode() => 
        await Task.Run(() => Collection.Scheduler.Interval(QueryEvent, 2 * 1000, async () => await QueryQrCodeState()));
    
    public async Task<bool> LoginByPassword()
    {
        Collection.Log.LogInfo(Tag, "Trying to Login by Keystore and Password...");
        
        if (!Collection.Socket.Connected) // if socket not connected, try to connect
        {        
            if (!await Collection.Socket.Connect()) return false;
            Collection.Scheduler.Interval("Heartbeat.Alive", 10 * 1000, async () => await Collection.Business.PushEvent(AliveEvent.Create()));
        }
        
        var keyExchangeEvent = KeyExchangeEvent.Create();
        var exchangeResult = await Collection.Business.SendEvent(keyExchangeEvent);
        if (exchangeResult.Count != 0)
        {
            Collection.Log.LogInfo(Tag, "Key Exchange successfully!");

            if (Collection.Keystore.Session.TempPassword != null) // try EasyLogin
            {
                Collection.Log.LogInfo(Tag, "Trying to Login by EasyLogin...");
                var easyLoginEvent = EasyLoginEvent.Create();
                var easyLoginResult = await Collection.Business.SendEvent(easyLoginEvent);

                if (easyLoginResult.Count != 0)
                {
                    var @event = (EasyLoginEvent)easyLoginResult[0];
                    if (@event is { Success: true, UnusualVerify: false })
                    {
                        Collection.Log.LogInfo(Tag, "Login Success");

                        var onlineEvent = new BotOnlineEvent();
                        Collection.Invoker.PostEvent(onlineEvent);
                        
                        var registerEvent = StatusRegisterEvent.Create();
                        var registerResponse = await Collection.Business.SendEvent(registerEvent);
                        Collection.Log.LogInfo(Tag, $"Register Status: {((StatusRegisterEvent)registerResponse[0]).Message}");
                        Collection.Scheduler.Interval("trpc.qq_new_tech.status_svc.StatusService.SsoHeartBeat", 
                            5 * 60 * 1000, async () => await Collection.Business.PushEvent(SsoAliveEvent.Create()));

                        return true;
                    }

                    if (@event is { Success: true, UnusualVerify: true })
                    {
                        throw new NotImplementedException(); // TODO: UnusualVerify
                    }
                }
            }
            else
            {
                throw new NotImplementedException(); // TODO: Login by Password
            }
        }

        return false;
    }

    private async Task<bool> DoWtLogin()
    {
        Collection.Log.LogInfo(Tag, "Doing Login...");
        Collection.Keystore.Session.Sequence = 0;

        Collection.Keystore.SecpImpl = new EcdhImpl(EcdhImpl.CryptMethod.Secp192K1);
        var loginEvent = LoginEvent.Create();
        var result = await Collection.Business.SendEvent(loginEvent);
        
        if (result.Count != 0)
        {
            var @event = (LoginEvent)result[0];
            if (@event.ResultCode == 0)
            {
                Collection.Log.LogInfo(Tag, "Login Success");
                Collection.Keystore.Info = new BotKeystore.BotInfo(@event.Age, @event.Sex, @event.Name);
                Collection.Log.LogInfo(Tag, Collection.Keystore.Info.ToString());

                var registerEvent = StatusRegisterEvent.Create();
                var registerResponse = await Collection.Business.SendEvent(registerEvent);
                Collection.Log.LogInfo(Tag, $"Register Status: {((StatusRegisterEvent)registerResponse[0]).Message}");
                Collection.Scheduler.Interval("trpc.qq_new_tech.status_svc.StatusService.SsoHeartBeat", 5 * 60 * 1000, 
                    async () => await Collection.Business.PushEvent(SsoAliveEvent.Create()));

                var onlineEvent = new BotOnlineEvent();
                Collection.Invoker.PostEvent(onlineEvent);

                return true;
            }

            Collection.Log.LogFatal(Tag, $"Login failed: {@event.ResultCode}");
            Collection.Log.LogFatal(Tag, $"Tag: {@event.Tag}\nState: {@event.Message}");
        }
        
        return false;
    }

    private async Task<bool> QueryQrCodeState()
    {
        if (Collection.Keystore.Session.QrString == null)
        {
            Collection.Log.LogFatal(Tag, "QrString is null, Please Fetch QrCode First");
            return false;
        }

        var request = new NTLoginHttpRequest
        {
            Appid = Collection.AppInfo.AppId,
            Qrsig = Collection.Keystore.Session.QrString,
            FaceUpdateTime = 0
        };
        var payload = JsonSerializer.SerializeToUtf8Bytes(request);
        var response = await Http.PostAsync(Interface, payload, "application/json");
        var info = JsonSerializer.Deserialize<NTLoginHttpResponse>(response);
        if (info != null) Collection.Keystore.Uin = info.Uin;

        var transEmp = TransEmpEvent.Create(TransEmpEvent.State.QueryResult);
        var result = await Collection.Business.SendEvent(transEmp);

        if (result.Count != 0)
        {
            var @event = (TransEmpEvent)result[0];
            var state = (TransEmp12.State)@event.ResultCode;
            Collection.Log.LogInfo(Tag, $"QrCode State Queried: {state} Uin: {Collection.Keystore.Uin}");

            switch (state)
            {
                case TransEmp12.State.Confirmed:
                {
                    Collection.Log.LogInfo(Tag, "QrCode Confirmed, Logging in with A1 sig...");
                    Collection.Scheduler.Cancel(QueryEvent); // cancel query task

                    if (@event.TgtgtKey != null)
                    {
                        Collection.Keystore.Stub.TgtgtKey = @event.TgtgtKey;
                        Collection.Keystore.Session.TempPassword = @event.TempPassword;
                        Collection.Keystore.Session.NoPicSig = @event.NoPicSig;

                        return await DoWtLogin();
                    }
                    break;
                }
                case TransEmp12.State.CodeExpired:
                {
                    Collection.Log.LogWarning(Tag, "QrCode Expired, Please Fetch QrCode Again");
                    Collection.Scheduler.Cancel(QueryEvent);
                    Collection.Scheduler.Dispose();

                    return false;
                }
                case TransEmp12.State.Canceled:
                {
                    Collection.Log.LogWarning(Tag, "QrCode Canceled, Please Fetch QrCode Again");
                    Collection.Scheduler.Cancel(QueryEvent);
                    Collection.Scheduler.Dispose();

                    return false;
                }
                case TransEmp12.State.WaitingForConfirm: 
                case TransEmp12.State.WaitingForScan:
                default:
                    break;
            }
        }

        return false;
    }
}