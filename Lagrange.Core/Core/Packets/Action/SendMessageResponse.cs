using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action;

[ProtoContract]
internal class SendMessageResponse
{
	[ProtoMember(1)] public int Result { get; set; }
	
	[ProtoMember(2)] public string ErrMsg { get; set; }
}