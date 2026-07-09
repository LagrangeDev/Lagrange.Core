using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class SetStatusEventReq(
    int status,
    int extStatus,
    int batteryStatus,
    SetStatusCustomExtEvent? customExt = null) : ProtocolEvent
{
    public int Status { get; } = status;

    public int ExtStatus { get; } = extStatus;

    public int BatteryStatus { get; } = batteryStatus;

    public SetStatusCustomExtEvent? CustomExt { get; } = customExt;
}

internal class SetStatusCustomExtEvent(ulong faceId, string wording, ulong faceType)
{
    public ulong FaceId { get; } = faceId;

    public string Wording { get; } = wording;

    public ulong FaceType { get; } = faceType;
}

internal class SetStatusEventResp(int errCode, string errMsg) : ProtocolEvent
{
    public int ErrCode { get; } = errCode;

    public string ErrMsg { get; } = errMsg;

    public bool Success => ErrCode == 0;
}
