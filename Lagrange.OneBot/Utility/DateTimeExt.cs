using System.Runtime.CompilerServices;

namespace Lagrange.OneBot.Utility;

public static class DateTimeExt
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ToTimestamp(this DateTime time) => 
        (uint)(time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
}