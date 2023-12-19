using Lagrange.Core.Event.EventArg;

namespace Lagrange.Core.Test.Utility;

public static class Console
{
    public static void ChangeColorByTitle(this LogLevel level) => System.Console.ForegroundColor = level switch
    {
        LogLevel.Debug => ConsoleColor.White,
        LogLevel.Verbose => ConsoleColor.DarkGray,
        LogLevel.Information => ConsoleColor.Blue,
        LogLevel.Warning => ConsoleColor.Yellow,
        LogLevel.Fatal => ConsoleColor.Red,
        _ => System.Console.ForegroundColor
    };
}