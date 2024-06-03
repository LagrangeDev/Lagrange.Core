using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming

[OidbSvcTrpcTcp(0xed3, 1)]
[ProtoContract]
internal class OidbSvcTrpcTcp0xED3_1
{
   [ProtoMember(1)] public uint Uin { get; set; }
   
   [ProtoMember(2)] public uint GroupUin { get; set; }  // same when poke type is friend
   
   [ProtoMember(5)] public uint FriendUin { get; set; }

   [ProtoMember(6)] public uint? Ext { get; set; }
}