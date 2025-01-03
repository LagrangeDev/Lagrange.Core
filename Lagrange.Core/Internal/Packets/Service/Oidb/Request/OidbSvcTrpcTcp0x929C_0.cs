﻿using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0x929C, 0)]
internal class OidbSvcTrpcTcp0x929C_0
{
    [ProtoMember(1)] public uint Group { get; set; }

    [ProtoMember(2)] public uint ChatType { get; set; } = 1; //1 voice,2 song
}