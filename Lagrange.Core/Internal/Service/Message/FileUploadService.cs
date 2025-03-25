using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(FileUploadEvent))]
[Service("OidbSvcTrpcTcp.0xe37_1700")]
internal class FileUploadService : BaseService<FileUploadEvent>
{
    protected override bool Build(FileUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        if (input.Entity.FileStream is null) throw new Exception();

        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xE37_1700>(new OidbSvcTrpcTcp0xE37_1700
        {
            Command = 1700,
            Seq = 0,
            Upload = new ApplyUploadReqV3
            {
                SenderUid = keystore.Uid ?? "",
                ReceiverUid = input.TargetUid,
                FileSize = (uint)input.Entity.FileStream.Length,
                FileName = input.Entity.FileName,
                Md510MCheckSum = input.Entity.FileStream.Md5(0, 10 * 1024 * 1024).UnHex(),
                Sha1CheckSum = input.Entity.FileSha1,
                LocalPath = "/",
                Md5CheckSum = input.Entity.FileMd5,
                Sha3CheckSum = Array.Empty<byte>()
            },
            BusinessId = 3,
            ClientType = 1,
            FlagSupportMediaPlatform = 1
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FileUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xE37Response>>(input);
        var upload = payload.Body.Upload;

        output = FileUploadEvent.Result((int)payload.ErrorCode, payload.ErrorMsg, upload.BoolFileExist, upload.Uuid, upload.MediaPlatformUploadKey, upload.UploadIp, upload.UploadPort, upload.FileAddon);
        extraEvents = null;
        return true;
    }
}