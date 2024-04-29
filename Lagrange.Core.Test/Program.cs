using Lagrange.Core.Test.Tests;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Test;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var data = new byte[] { 0x07, 0x21 };
        Console.WriteLine(data.Hex(true, true) + ' ');

        // BenchmarkRunner.Run<ProtoBufTest>(new DebugBuildConfig());
        await new NTLoginTest().LoginByPassword();
        // await new WtLoginTest().FetchQrCode();
    }
}