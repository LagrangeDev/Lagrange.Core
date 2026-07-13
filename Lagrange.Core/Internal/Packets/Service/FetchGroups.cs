using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class FetchGroupsRequest
{
    [ProtoMember(1)] public FetchGroupsRequestConfig Config { get; set; }
}

[ProtoPackable]
internal partial class FetchGroupsRequestConfig
{
    [ProtoMember(1)] public FetchGroupsRequestConfig1 Config1 { get; set; }

    [ProtoMember(2)] public FetchGroupsRequestConfig2 Config2 { get; set; }

    [ProtoMember(3)] public FetchGroupsRequestConfig3 Config3 { get; set; }
}

[ProtoPackable]
internal partial class FetchGroupsRequestConfig1
{
    [ProtoMember(1)] public bool GroupOwner { get; set; } = true;

    [ProtoMember(2)] public bool Field2 { get; set; } = true;

    [ProtoMember(3)] public bool MemberMax { get; set; } = true;

    [ProtoMember(4)] public bool MemberCount { get; set; } = true;

    [ProtoMember(5)] public bool GroupName { get; set; } = true;

    [ProtoMember(8)] public bool Field8 { get; set; } = true;

    [ProtoMember(9)] public bool Field9 { get; set; } = true;

    [ProtoMember(10)] public bool Field10 { get; set; } = true;

    [ProtoMember(11)] public bool Field11 { get; set; } = true;

    [ProtoMember(12)] public bool Field12 { get; set; } = true;

    [ProtoMember(13)] public bool Field13 { get; set; } = true;

    [ProtoMember(14)] public bool Field14 { get; set; } = true;

    [ProtoMember(15)] public bool Field15 { get; set; } = true;

    [ProtoMember(16)] public bool Field16 { get; set; } = true;

    [ProtoMember(17)] public bool Field17 { get; set; } = true;

    [ProtoMember(18)] public bool Field18 { get; set; } = true;

    [ProtoMember(19)] public bool Question { get; set; } = true;

    [ProtoMember(20)] public bool Field20 { get; set; } = true;

    [ProtoMember(22)] public bool Field22 { get; set; } = true;

    [ProtoMember(23)] public bool Field23 { get; set; } = true;

    [ProtoMember(24)] public bool Field24 { get; set; } = true;

    [ProtoMember(25)] public bool Field25 { get; set; } = true;

    [ProtoMember(26)] public bool Field26 { get; set; } = true;

    [ProtoMember(27)] public bool Field27 { get; set; } = true;

    [ProtoMember(28)] public bool Field28 { get; set; } = true;

    [ProtoMember(29)] public bool Field29 { get; set; } = true;

    [ProtoMember(30)] public bool Field30 { get; set; } = true;

    [ProtoMember(31)] public bool Field31 { get; set; } = true;

    [ProtoMember(32)] public bool Field32 { get; set; } = true;

    [ProtoMember(5001)] public bool Field5001 { get; set; } = true;

    [ProtoMember(5002)] public bool Field5002 { get; set; } = true;

    [ProtoMember(5003)] public bool Field5003 { get; set; } = true;
}

[ProtoPackable]
internal partial class FetchGroupsRequestConfig2
{
    [ProtoMember(1)] public bool Field1 { get; set; } = true;

    [ProtoMember(2)] public bool Field2 { get; set; } = true;

    [ProtoMember(3)] public bool Field3 { get; set; } = true;

    [ProtoMember(4)] public bool Field4 { get; set; } = true;

    [ProtoMember(5)] public bool Field5 { get; set; } = true;

    [ProtoMember(6)] public bool Field6 { get; set; } = true;

    [ProtoMember(7)] public bool Field7 { get; set; } = true;

    [ProtoMember(8)] public bool Field8 { get; set; } = true;
}

[ProtoPackable]
internal partial class FetchGroupsRequestConfig3
{
    [ProtoMember(5)] public bool Field5 { get; set; } = true;

    [ProtoMember(6)] public bool Field6 { get; set; } = true;
}

[ProtoPackable]
internal partial class FetchGroupsResponse
{
    [ProtoMember(2)] public List<FetchGroupsResponseGroup> Groups { get; set; }
}

[ProtoPackable]
internal partial class FetchGroupsResponseGroup
{
    [ProtoMember(3)] public long GroupUin { get; set; }

    [ProtoMember(4)] public FetchGroupsResponseGroupInfo Info { get; set; }

    [ProtoMember(5)] public FetchGroupsResponseCustomInfo CustomInfo { get; set; }
}

[ProtoPackable]
internal partial class FetchGroupsResponseGroupInfo
{
    [ProtoMember(1)] public FetchGroupsResponseMember GroupOwner { get; set; }

    [ProtoMember(2)] public uint CreatedTime { get; set; }

    [ProtoMember(3)] public uint MemberMax { get; set; }

    [ProtoMember(4)] public uint MemberCount { get; set; }

    [ProtoMember(5)] public string GroupName { get; set; }

    [ProtoMember(18)] public string? Description { get; set; }

    [ProtoMember(19)] public string? Question { get; set; }

    [ProtoMember(30)] public string? Announcement { get; set; }
}

[ProtoPackable]
internal partial class FetchGroupsResponseCustomInfo
{
    [ProtoMember(1)] public long LastSpeakTime { get; set; }

    [ProtoMember(3)] public string? Remark { get; set; }
    
    [ProtoMember(5)] public uint LastestSeq { get; set; }
}

[ProtoPackable]
internal partial class FetchGroupsResponseMember
{
    [ProtoMember(2)] public string Uid { get; set; }
}
