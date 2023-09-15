using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Action.Pb;

[ProtoContract]
internal class PbGetOneDayRoamMsgResp
{
	[ProtoMember(1)] public uint Result { get; set; }
	
	[ProtoMember(2)] public string ErrMsg { get; set; }
	
	[ProtoMember(3)] public ulong PeerUin { get; set; }
	
	[ProtoMember(4)] public ulong LastMsgTime { get; set; }
	
	[ProtoMember(5)] public ulong Random { get; set; }
	
	[ProtoMember(6)] public List<Message.Message> Msg { get; set; }
	
	[ProtoMember(7)] public uint IsComplete { get; set; }
}