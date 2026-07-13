using System;

namespace Lagrange.Milky.Extensions;

public static class DateTimeExtension
{
    public static long ToUnixTimeSeconds(this DateTime time) => new DateTimeOffset(time).ToUnixTimeSeconds();
}