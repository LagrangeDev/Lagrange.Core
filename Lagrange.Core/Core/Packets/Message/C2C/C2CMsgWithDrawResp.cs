using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.C2C;

[ProtoContract]
internal class C2CMsgWithDrawResp
{
	[ProtoMember(1)] public int Result { get; set; }
	
	[ProtoMember(2)] public string ErrMsg { get; set; }
}