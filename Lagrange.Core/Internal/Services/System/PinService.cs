using System.Buffers.Binary;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<SetPinGroupEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x5d6_1")]
internal class SetPinGroupService : OidbService<SetPinGroupEventReq, SetPinGroupEventResp, D5D6ReqBody, D5D6RspBody>
{
    private protected override uint Command => 0x5d6;

    private protected override uint Service => 1;

    private protected override Task<D5D6ReqBody> ProcessRequest(SetPinGroupEventReq request, BotContext context)
    {
        return Task.FromResult(new D5D6ReqBody
        {
            Uint32Seq = 0,
            RptMsgUpdateBuffer =
            [
                new SnsUpdateBuffer
                {
                    GroupUin = (ulong)request.GroupUin,
                    RptMsgSnsUpdateItem =
                    [
                        new SnsUpdateItem
                        {
                            Uint32UpdateSnsType = 13569,
                            BytesValue = request.IsPin ? GetTimestamp() : []
                        }
                    ]
                }
            ],
            Uint32Domain = 11
        });
    }

    private protected override Task<SetPinGroupEventResp> ProcessResponse(D5D6RspBody response, BotContext context)
    {
        return Task.FromResult(SetPinGroupEventResp.Default);
    }

    private static byte[] GetTimestamp()
    {
        byte[] timestamp = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(timestamp, (int)DateTimeOffset.Now.ToUnixTimeSeconds());
        return timestamp;
    }
}

[EventSubscribe<SetPinFriendEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x5d6_18")]
internal class SetPinFriendService : OidbService<SetPinFriendEventReq, SetPinFriendEventResp, D5D6ReqBody, D5D6RspBody>
{
    private protected override uint Command => 0x5d6;

    private protected override uint Service => 18;

    private protected override Task<D5D6ReqBody> ProcessRequest(SetPinFriendEventReq request, BotContext context)
    {
        return Task.FromResult(new D5D6ReqBody
        {
            Uint32Seq = 0,
            RptMsgUpdateBuffer =
            [
                new SnsUpdateBuffer
                {
                    Uid = request.Uid,
                    RptMsgSnsUpdateItem =
                    [
                        new SnsUpdateItem
                        {
                            Uint32UpdateSnsType = 13578,
                            BytesValue = request.IsPin ? GetTimestamp() : []
                        }
                    ]
                }
            ],
            Uint32Domain = 1
        });
    }

    private protected override Task<SetPinFriendEventResp> ProcessResponse(D5D6RspBody response, BotContext context)
    {
        return Task.FromResult(SetPinFriendEventResp.Default);
    }

    private static byte[] GetTimestamp()
    {
        byte[] timestamp = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(timestamp, (int)DateTimeOffset.Now.ToUnixTimeSeconds());
        return timestamp;
    }
}
