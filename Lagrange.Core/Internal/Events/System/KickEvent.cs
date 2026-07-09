using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class KickEvent(string tipsTitle, string tipsInfo) : ProtocolEvent
{
    public string TipsTitle { get; } = tipsTitle;

    public string TipsInfo { get; } = tipsInfo;
}