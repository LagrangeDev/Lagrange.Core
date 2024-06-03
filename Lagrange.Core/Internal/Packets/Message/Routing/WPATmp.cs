using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Routing;

[ProtoContract]
internal class WPATmp
{
	[ProtoMember(1)] public ulong ToUin { get; set; }
	
	[ProtoMember(2)] public byte[] Sig { get; set; }
}