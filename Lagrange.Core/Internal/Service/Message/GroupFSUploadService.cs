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

[EventSubscribe(typeof(GroupFSUploadEvent))]
[Service("OidbSvcTrpcTcp.0x6d6_0")]
internal class GroupFSUploadService : BaseService<GroupFSUploadEvent>
{
    protected override bool Build(GroupFSUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.Entity.FileStream is null) throw new Exception();

        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6>(new OidbSvcTrpcTcp0x6D6
        {
            File = new OidbSvcTrpcTcp0x6D6Upload
            {
                GroupUin = input.GroupUin,
                AppId = 7,
                BusId = 102,
                Entrance = 6,
                TargetDirectory = "/",
                FileName = input.Entity.FileName,
                LocalDirectory = $"/{input.Entity.FileName}",
                FileSize = input.Entity.FileSize,
                FileSha1 = input.Entity.FileStream.Sha1().UnHex(),
                FileSha3 = Array.Empty<byte>(),
                FileMd5 = input.Entity.FileMd5,
                Field15 = true
            }
        }, 0x6D6, 0, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupFSUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x6D6Response>>(input.AsSpan());
        var upload = payload.Body.Upload;
        
        output = GroupFSUploadEvent.Result(upload.RetCode, upload.BoolFileExist, upload.FileId, upload.FileKey, upload.CheckKey, upload.UploadIp, upload.UploadPort);
        extraEvents = null;
        return true;
    }
}