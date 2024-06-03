using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

//Resharper Disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class NTV2RichMediaHighwayExt
{
    [ProtoMember(1)] public string FileUuid { get; set; }
    
    [ProtoMember(2)] public string UKey { get; set; }
    
    [ProtoMember(5)] public NTHighwayNetwork Network { get; set; }
    
    [ProtoMember(6)] public List<MsgInfoBody> MsgInfoBody { get; set; }
    
    [ProtoMember(10)] public uint BlockSize { get; set; }
    
    [ProtoMember(11)] public NTHighwayHash Hash { get; set; }
}

[ProtoContract]
internal class NTHighwayHash
{
    [ProtoMember(1)] public List<byte[]> FileSha1 { get; set; }
}

[ProtoContract]
internal class NTHighwayNetwork
{
    [ProtoMember(1)] public List<NTHighwayIPv4> IPv4s { get; set; }
}


[ProtoContract]
internal class NTHighwayIPv4
{
    [ProtoMember(1)] public NTHighwayDomain Domain { get; set; }
    
    [ProtoMember(2)] public uint Port { get; set; }
}

[ProtoContract]
internal class NTHighwayDomain
{
    [ProtoMember(1)] public bool IsEnable { get; set; }  // true
    
    [ProtoMember(2)] public string IP { get; set; }
}