using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0xEB7_1Response
{
    [ProtoMember(2)] public BodyClass Body { get; set; }

    [ProtoContract]
    internal class BodyClass
    {
        [ProtoMember(2)] public ResultClass Result { get; set; }

        [ProtoContract]
        internal class ResultClass
        {
            [ProtoMember(1)] public string Title { get; set; }  // 今日已成功打卡
            [ProtoMember(2)] public string KeepDayText { get; set; }  // 已打卡N天
            [ProtoMember(3)] public string[] ClockInInfo1 { get; set; }  // ["群内排名第N位", "[clock in timestamp (second)]"]
            [ProtoMember(4)] public string DetailUrl { get; set; }  // https://qun.qq.com/v2/signin/detail?...
        }
    }
}
