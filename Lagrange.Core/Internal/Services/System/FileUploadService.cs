using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Cryptography;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FileUploadEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xe37_1700")]
internal class FileUploadService : OidbService<FileUploadEventReq, FileUploadEventResp, OfflineFileUploadRequest, OfflineFileUploadResponse>
{
    protected override uint Command => 0xe37;

    protected override uint Service => 1700;
    
    protected override Task<OfflineFileUploadRequest> ProcessRequest(FileUploadEventReq request, BotContext context)
    {
        return Task.FromResult(new OfflineFileUploadRequest
        {
            Command = 1700,
            Seq = 0,
            Upload = new ApplyUploadReqV3
            {
                SenderUid = context.Keystore.Uid,
                ReceiverUid = request.TargetUid,
                FileSize = (uint)request.FileStream.Length,
                FileName = request.FileName,
                Md510MCheckSum = request.File10MMd5,
                Sha1CheckSum = request.FileSha1,
                LocalPath = "/",
                Md5CheckSum = request.FileMd5,
                Sha3CheckSum = TriSha1Provider.CalculateTriSha1(request.FileStream)
            },
            BusinessId = 3,
            ClientType = 1,
            FlagSupportMediaPlatform = 1
        });
    }

    protected override Task<FileUploadEventResp> ProcessResponse(OfflineFileUploadResponse response, BotContext context)
    {
        var upload = response.Upload;
        var addrs = upload.RtpMediaPlatformUploadAddress.Select(addr => (ProtocolHelper.UInt32ToIPV4Addr(addr.InnerIp), addr.InnerPort)).ToList();

        return Task.FromResult(new FileUploadEventResp(upload.BoolFileExist, upload.Uuid, upload.MediaPlatformUploadKey, addrs, upload.FileIdCrc));
    }
}