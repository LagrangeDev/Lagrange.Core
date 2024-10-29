using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;
#pragma warning disable CS8618
[ProtoContract]
internal class OidbSvcTrpcTcp0x929C_0Response
{
    [ProtoMember(1)] List<OidbSvcTrpcTcp0x929C_0ResponseProperty> Property{get;set;}
    
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x929C_0ResponseProperty
{
    [ProtoMember(1)] string tts { get; set; }
    
    [ProtoMember(2)] string ttsName { get; set; }
    
    [ProtoMember(3)] string ttsVoiceUrl { get; set; }
}