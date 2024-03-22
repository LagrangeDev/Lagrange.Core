using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;
using FileInfo = Lagrange.Core.Internal.Packets.Service.Oidb.Common.FileInfo;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(RecordUploadEvent))]
[Service("OidbSvcTrpcTcp.0x126d_100")]
internal class RecordUploadService : BaseService<RecordUploadEvent>
{
    protected override bool Build(RecordUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.Entity.AudioStream is null) throw new Exception();
        
        string md5 = input.Entity.AudioStream.Md5(true);
        string sha1 = input.Entity.AudioStream.Sha1(true);

        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 4u,
                    Command = 100
                },
                Scene = new SceneInfo
                {
                    RequestType = 2,
                    BusinessType = 3,
                    SceneType = 1u,
                    C2C = new C2CUserInfo
                    {
                        AccountType = 2,
                        TargetUid = input.TargetUid
                    }
                },
                Client = new ClientMeta { AgentType = 2 },
            },
            Upload = new UploadReq
            {
                UploadInfo = new List<UploadInfo>
                {
                    new()
                    {
                        FileInfo = new FileInfo
                        {
                            FileSize = (uint)input.Entity.AudioStream.Length,
                            FileHash = md5,
                            FileSha1 = sha1,
                            FileName = md5 + ".amr",
                            Type = new FileType
                            {
                                Type = 3,
                                PicFormat = 0,
                                VideoFormat = 0,
                                VoiceFormat = 1
                            },
                            Width = 0,
                            Height = 0,
                            Time = (uint)input.Entity.AudioLength,
                            Original = 0
                        },
                        SubFileType = 0
                    }
                },
                TryFastUploadCompleted = true,
                SrvSendMsg = false,
                ClientRandomId = (ulong)Random.Shared.Next(),
                CompatQMsgSceneType = 1u,
                ExtBizInfo = new ExtBizInfo
                {
                    Pic = new PicExtBizInfo { TextSummary = "" },
                    Video = new VideoExtBizInfo { BytesPbReserve = Array.Empty<byte>() },
                    Ptt = new PttExtBizInfo
                    {
                        BytesReserve = new byte[] { 0x08, 0x00, 0x38, 0x00 },
                        BytesPbReserve = Array.Empty<byte>(),
                        BytesGeneralFlags = new byte[] { 0x9a, 0x01, 0x0b, 0xaa, 0x03, 0x08, 0x08, 0x04, 0x12, 0x04, 0x00, 0x00, 0x00, 0x00 }
                    }
                },
                ClientSeq = 0,
                NoNeedCompatMsg = false
            }
        }, 0x126d, 100, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out RecordUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<NTV2RichMediaResp>>(input);
        var upload = packet.Body.Upload;
        var compat = Serializer.Deserialize<RichText>(upload.CompatQMsg.AsSpan());
        
        output = RecordUploadEvent.Result((int)packet.ErrorCode, upload.UKey, upload.MsgInfo, upload.IPv4s, compat);
        extraEvents = null;
        return true;
    }
}