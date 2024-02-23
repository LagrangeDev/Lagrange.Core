using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using FileInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.FileInfo;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(RecordDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x126d_200")]
internal class RecordDownloadService : BaseService<RecordDownloadEvent>
{
    protected override bool Build(RecordDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 1,
                    Command = 200
                },
                Scene = new SceneInfo
                {
                    RequestType = 1,
                    BusinessType = 3,
                    SceneType = 1,
                    C2C = new C2CUserInfo
                    {
                        AccountType = 2,
                        SelfUid = input.SelfUid
                    }
                },
                Client = new ClientMeta
                {
                    AgentType = 2
                }
            },
            Download = new DownloadReq
            {
                Node = new IndexNode
                {
                    Info = new FileInfo
                    {
                        FileSize = 0,
                        FileHash = input.FileName.Replace(".amr", ""),
                        FileSha1 = input.FileSha1 ?? "",
                        FileName = input.FileName,
                        Type = new FileType
                        {
                            Type = 2,
                            PicFormat = 0,
                            VideoFormat = 0,
                            VoiceFormat = 1
                        },
                        Width = 0,
                        Height = 0,
                        Time = 2,
                        Original = 0
                    },
                    FileUuid = input.Uuid,
                    StoreId = Convert.ToUInt32(input.FileSha1 != null),
                    UploadTime = 0,
                    Ttl = 0,
                    SubType = 0
                },
                Download = new DownloadExt
                {
                    Video = new VideoDownloadExt
                    {
                        BusiType = 0,
                        SceneType = 0
                    }
                }
            }
        }, 0x126d, 200, false, true);
        output = packet.Serialize();
        extraPackets = null;
        
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out RecordDownloadEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x1026_200Response>>(input.AsSpan());
        var body = payload.Body.Body;
        string url = $"https://{body.Field3.Domain}{body.Field3.Suffix}{body.DownloadParams}";
        
        output = RecordDownloadEvent.Result((int)payload.ErrorCode, url);
        extraEvents = null;
        return true;
    }
}