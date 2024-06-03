using Lagrange.Core.Internal.Packets.Message.Component;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message;

[ProtoContract]
internal class MessageBody
{
	[ProtoMember(1)] public RichText? RichText { get; set; }
    
    [ProtoMember(2)] public byte[]? MsgContent { get; set; } // Offline file is now put here(?
    
	[ProtoMember(3)] public byte[]? MsgEncryptContent { get; set; }
}