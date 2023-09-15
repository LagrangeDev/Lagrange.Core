using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Generics;

[ProtoContract]
internal class OidbLafter
{
	[ProtoMember(1)] public int Type { get; set; }
	
	[ProtoMember(2)] public byte[] D2 { get; set; }
	
	[ProtoMember(3)] public uint SubAppid { get; set; }
}