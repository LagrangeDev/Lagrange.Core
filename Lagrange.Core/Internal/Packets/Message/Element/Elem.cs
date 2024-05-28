using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Message.Element;

[ProtoContract]
internal class Elem
{
	[ProtoMember(1)] public Text? Text { get; set; } // TextEntity
    
    [ProtoMember(2)] public Face? Face { get; set; }
    
    [ProtoMember(3)] public OnlineImage? OnlineImage { get; set; }
    
	[ProtoMember(4)] public NotOnlineImage? NotOnlineImage { get; set; } // Offline Image
	
	[ProtoMember(5)] public TransElem? TransElem { get; set; }
	
	[ProtoMember(6)] public Marketface? Marketface { get; set; }
	
	[ProtoMember(8)] public CustomFace? CustomFace { get; set; }
	
	[ProtoMember(9)] public ElemFlags2? ElemFlags2 { get; set; }
	
	[ProtoMember(12)] public RichMsg? RichMsg { get; set; }

	[ProtoMember(13)] public GroupFile? GroupFile { get; set; }
	
	[ProtoMember(16)] public ExtraInfo? ExtraInfo { get; set; }
	
	[ProtoMember(19)] public VideoFile? VideoFile { get; set; }
	
	[ProtoMember(21)] public AnonymousGroupMessage? AnonGroupMsg { get; set; }
	
	[ProtoMember(24)] public QQWalletMsg? QQWalletMsg { get; set; }
	
	[ProtoMember(31)] public CustomElem? CustomElem { get; set; }
	
	[ProtoMember(37)] public GeneralFlags? GeneralFlags { get; set; }
	
	[ProtoMember(45)] public SrcMsg? SrcMsg { get; set; } // Forward/ReplyEntity
	
	[ProtoMember(51)] public LightAppElem? LightAppElem { get; set; }
	
	[ProtoMember(53)] public CommonElem? CommonElem { get; set; }
}