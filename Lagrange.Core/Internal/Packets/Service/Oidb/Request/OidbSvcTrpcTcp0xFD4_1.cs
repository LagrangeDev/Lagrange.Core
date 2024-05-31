using Lagrange.Core.Internal.Packets.Service.Oidb.Generics;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Fetch Friends List
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xfd4, 1)]
internal class OidbSvcTrpcTcp0xFD4_1
{
    [ProtoMember(2)] public uint FriendCount { get; set; } = 300;  // paging get num
    
    [ProtoMember(4)] public uint Field4 { get; set; } = 0;
    
    [ProtoMember(5)] public OidbSvcTrpcTcp0xFD4_1Uin NextUin { get; set; }
    
    [ProtoMember(6)] public uint Field6 { get; set; } = 1;

    [ProtoMember(7)] public uint Field7 { get; set; } = int.MaxValue;
    
    [ProtoMember(10001)] public List<OidbSvcTrpcTcp0xFD4_1Body> Body { get; set; }

    [ProtoMember(10002)] public List<uint> Field10002 { get; set; } = new() { 13578, 13579, 13573, 13572, 13568 };
    
    [ProtoMember(10003)] public uint Field10003 { get; set; } = 4051;
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFD4_1Body
{
    [ProtoMember(1)] public uint Type { get; set; }
    
    [ProtoMember(2)] public OidbNumber Number { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFD4_1Uin
{
    [ProtoMember(1)] public uint Uin { get; set; }
}