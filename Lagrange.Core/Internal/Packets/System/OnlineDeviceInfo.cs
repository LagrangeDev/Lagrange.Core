using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class OnlineDeviceInfo
{
	[ProtoMember(1)] public string User { get; set; }
	
	[ProtoMember(2)] public string Os { get; set; }
	
	[ProtoMember(3)] public string OsVer { get; set; }
	
	[ProtoMember(4)] public string? VendorName { get; set; }
	
	[ProtoMember(5)] public string OsLower { get; set; }
}