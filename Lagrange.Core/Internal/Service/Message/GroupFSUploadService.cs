using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[Service("OidbSvcTrpcTcp.0x6d6_0")]
internal class GroupFSUploadService : BaseService<GroupFSUploadEvent>
{
    protected override bool Build(GroupFSUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6_0>(new OidbSvcTrpcTcp0x6D6_0
        {
            File = new OidbSvcTrpcTcp0x6D6_0Upload
            {
                GroupUin = input.GroupUin,
                AppId = 7,
                BusId = 102,
                Entrance = 6,
                TargetDirectory = "/",
                FileName = input.Entity.FileName,
                LocalDirectory = $"/{input.Entity.FileName}",
                FileSize = input.Entity.FileSize,
                FileSha1 = input.Entity.FileStream!.Sha1().UnHex(),
                FileSha3 = Array.Empty<byte>(),
                FileMd5 = input.Entity.FileMd5,
                Field15 = true
            }
        }, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupFSUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        Console.WriteLine(input.Hex());
        
        return base.Parse(input, keystore, appInfo, device, out output, out extraEvents);
    }
}