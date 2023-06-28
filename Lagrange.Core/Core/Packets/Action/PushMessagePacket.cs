using Lagrange.Core.Core.Packets.System;
using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Action;

[ProtoContract]
internal class PushMessagePacket
{
	[ProtoMember(1)] public Message.Message Message { get; set; }
	
	[ProtoMember(3)] public int Status { get; set; }
	
	[ProtoMember(4)] public NTSysEvent NtEvent { get; set; }
	
	[ProtoMember(5)] public int PingFLag { get; set; }
	
	[ProtoMember(9)] public int GeneralFlag { get; set; }
}