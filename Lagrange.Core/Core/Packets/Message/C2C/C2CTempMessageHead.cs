using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.C2C;

[ProtoContract]
internal class C2CTempMessageHead
{
	[ProtoMember(1)] public ulong GroupUin { get; set; }
	
	[ProtoMember(2)] public int C2CType { get; set; }
	
	[ProtoMember(3)] public int ServiceType { get; set; }
	
	[ProtoMember(4)] public string Card { get; set; }
	
	[ProtoMember(5)] public byte[] Sig { get; set; }
	
	[ProtoMember(6)] public int SigType { get; set; }
	
	[ProtoMember(7)] public string FromPhone { get; set; }
	
	[ProtoMember(8)] public string ToPhone { get; set; }
	
	[ProtoMember(9)] public int LockDisplay { get; set; }
	
	[ProtoMember(10)] public int DirectionFlag { get; set; }
	
	[ProtoMember(11)] public byte[] Reserved { get; set; }
}