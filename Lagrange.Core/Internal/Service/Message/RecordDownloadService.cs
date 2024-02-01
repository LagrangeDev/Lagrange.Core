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

[EventSubscribe(typeof(RecordDownloadEvent))]
[Service("OidbSvcTrpcTcp.0x126d_200")]
internal class RecordDownloadService : BaseService<RecordDownloadEvent>
{
    protected override bool Build(RecordDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x126D_200>(new OidbSvcTrpcTcp0x126D_200
        {
            Field1 = new OidbSvcTrpcTcp0x126D_200Field1
            {
                Field1 = new OidbSvcTrpcTcp0x126D_200Field1Field1
                {
                    Field1 = 1,
                    Field2 = 200
                },
                Field2 = new OidbSvcTrpcTcp0x126D_200Field1Field2
                {
                    Field1 = 1,
                    Field2 = 3,
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
                        FileHash = input.FileName.Replace(".amr", ""),
                        FileSha1 = input.FileSha1 ?? "",
                        FileName = input.FileName,
                        Field5 = new OidbSvcTrpcTcp0x126D_200Field3Field1Field1Field5
                        {
                            Field1 = 2,
                            Field2 = 0,
                            Field3 = 0,
                            Field4 = 1
                        },
                        Field6 = 0,
                        Field7 = 0,
                        Field8 = 2,
                        Field9 = 0
                    },
                    FileUuid = input.Uuid,
                    Field3 = Convert.ToUInt32(input.IsGroup),
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