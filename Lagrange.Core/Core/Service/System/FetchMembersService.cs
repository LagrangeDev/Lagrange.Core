using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Packets.Service.Oidb.Resopnse;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(FetchMembersEvent))]
[Service("OidbSvcTrpcTcp.0xfe7_2")]
internal class FetchMembersService : BaseService<FetchMembersEvent>
{
    protected override bool Build(FetchMembersEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE7_2>(new OidbSvcTrpcTcp0xFE7_2
        {
            GroupUin = input.GroupUin,
            Field2 = 5,
            Field3 = 2,
            Body = new OidbSvcTrpcScp0xFE7_2Body
            {
                MemberName = true,
                MemberCard = true,
                Permission = true,
                JoinTimestamp = true,
                LastMsgTimestamp = true
            }
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FetchMembersEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var response = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0xFE7_2Response>>(payload.AsSpan());

        var members = response.Body.Members.Select(member =>
        {
            var permission = GroupMemberPermission.Member;
            if (member.Permission?.Infos != null)
            {
                permission = member.Permission.Infos[0] switch
                {
                    2 => GroupMemberPermission.Owner,
                    3 => GroupMemberPermission.Admin,
                    _ => permission
                };
            }

            return new BotGroupMember(member.Uin.Uin,
                                      member.Uin.Uid,
                                      permission,
                                      member.Permission?.Level ?? 0,
                                      member.MemberCard.MemberCard,
                                      member.MemberName,
                                      DateTimeOffset.FromUnixTimeSeconds(member.JoinTimestamp).DateTime,
                                      DateTimeOffset.FromUnixTimeSeconds(member.LastMsgTimestamp).DateTime);
        }).ToList();
        
        output = FetchMembersEvent.Result(members);
        extraEvents = null;
        return true;
    }
}