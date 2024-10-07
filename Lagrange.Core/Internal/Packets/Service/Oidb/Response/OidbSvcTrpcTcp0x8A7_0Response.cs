using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

[ProtoContract]
internal class OidbSvcTrpcTcp0x8A7_0Response
{
    [ProtoMember(1)] public bool CanAtAll { get; set; }
    
    [ProtoMember(2)] public uint RemainAtAllCountForUin { get; set; }
    
    [ProtoMember(3)] public uint RemainAtAllCountForGroup { get; set; }
    
    [ProtoMember(4)] public string? PromptMsg1 { get; set; }
    
    [ProtoMember(5)] public string? PromptMsg2 { get; set; }
}