using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchMembersEvent))]
[Service("OidbSvcTrpcTcp.0xfe7_3")]
internal class FetchMembersService : BaseService<FetchMembersEvent>
{
    protected override bool Build(FetchMembersEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE7_3>(new OidbSvcTrpcTcp0xFE7_3
        {
            GroupUin = input.GroupUin,
            Field2 = 5,
            Field3 = 2,
            Body = new OidbSvcTrpcScp0xFE7_3Body
            {
                MemberName = true,
                MemberCard = true,
                Level = true,
                JoinTimestamp = true,
                LastMsgTimestamp = true,
                Permission = true,
            },
            Token = input.Token
        });
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FetchMembersEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var response = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE7_3Response>>(input);

        var members = response.Body.Members.Select(member => 
            new BotGroupMember(member.Uin.Uin,
                               member.Uin.Uid,
                               (GroupMemberPermission)member.Permission,
                               member.Level?.Level ?? 0,
                               member.MemberCard.MemberCard,
                               member.MemberName,
                               member.SpecialTitle,
                               DateTimeOffset.FromUnixTimeSeconds(member.JoinTimestamp).DateTime,
                               DateTimeOffset.FromUnixTimeSeconds(member.LastMsgTimestamp).DateTime)).ToList();
        
        output = FetchMembersEvent.Result(members, response.Body.Token);
        extraEvents = null;
        return true;
    }
}