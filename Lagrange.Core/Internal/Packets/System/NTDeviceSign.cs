using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class NTDeviceSign
{
    [ProtoMember(15)] public string Trace { get; set; }
    
    [ProtoMember(16)] public string? Uid { get; set; }
    
    [ProtoMember(24)] public SecInfo? Sign { get; set; }
}