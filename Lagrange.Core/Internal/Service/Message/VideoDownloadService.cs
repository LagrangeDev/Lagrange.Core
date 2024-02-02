using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(VideoDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x11e9_200")]
internal class VideoDownloadService : BaseService<VideoDownloadEvent>
{
    protected override bool Build(VideoDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x11E9_200>(new OidbSvcTrpcTcp0x11E9_200
        {
            Field1 = new OidbSvcTrpcTcp0x126D_200Field1
            {
                Field1 = new OidbSvcTrpcTcp0x126D_200Field1Field1
                {
                    Field1 = input.IsGroup ? 3u : 34u,
                    Field2 = 200
                },
                Field2 = new OidbSvcTrpcTcp0x126D_200Field1Field2
                {
                    Field1 = 2,
                    Field2 = 2,
                    Field3 = 1,
                    Field201 = new OidbSvcTrpcTcp0x126D_200Field1Field2Field201
                    {
                        Field1 = 2,
                        SelfUid = input.SelfUid
                    }
                },
                Field3 = new OidbSvcTrpcTcp0x126D_200Field1Field3
                {
                    Field1 = 2
                }
            },
            Field3 = new OidbSvcTrpcTcp0x126D_200Field3
            {
                Field1 = new OidbSvcTrpcTcp0x126D_200Field3Field1
                {
                    Field1 = new OidbSvcTrpcTcp0x126D_200Field3Field1Field1
                    {
                        Field1 = 0,
                        FileHash = input.FileMd5,
                        FileSha1 = input.FileSha1 ?? "",
                        FileName = input.FileName,
                        Field5 = new OidbSvcTrpcTcp0x126D_200Field3Field1Field1Field5
                        {
                            Field1 = 2,
                            Field2 = 0,
                            Field3 = 0,
                            Field4 = 0
                        },
                        Field6 = 0,
                        Field7 = 0,
                        Field8 = 0,
                        Field9 = 0
                    },
                    FileUuid = input.Uuid,
                    Field3 = 0,
                    Field4 = 0,
                    Field5 = 0,
                    Field6 = 0
                },
                Field2 = new OidbSvcTrpcTcp0x126D_200Field3Field2
                {
                    Field2 = new OidbSvcTrpcTcp0x126D_200Field3Field2Field2
                    {
                        Field1 = 0,
                        Field2 = 0
                    }
                }
            }
        }, false, true);
        output = packet.Serialize();
        extraPackets = null;
        
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out VideoDownloadEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x1026_200Response>>(input.AsSpan());
        var body = payload.Body.Body;
        string url = $"https://{body.Field3.Domain}{body.Field3.Suffix}{body.DownloadParams}";
        
        output = VideoDownloadEvent.Result((int)payload.ErrorCode, url);
        extraEvents = null;
        return true;
    }
}