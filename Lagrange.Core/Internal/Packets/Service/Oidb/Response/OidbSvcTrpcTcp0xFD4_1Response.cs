using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xFD4_1Response
{
    [ProtoMember(2)] public OidbSvcTrpcTcp0xFD4_1ResponseUin? Next { get; set; }
    
    [ProtoMember(3)] public uint DisplayFriendCount { get; set; }
    
    [ProtoMember(6)] public uint Timestamp { get; set; }
    
    [ProtoMember(7)] public uint SelfUin { get; set; }

    [ProtoMember(101)] public List<OidbFriend> Friends { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFD4_1ResponseUin
{
    [ProtoMember(1)] public uint Uin { get; set; }
}