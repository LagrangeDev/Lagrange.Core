using Lagrange.Core.Test.Tests;

namespace Lagrange.Core.Test;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        // BenchmarkRunner.Run<ProtoBufTest>(new DebugBuildConfig());
        await new NTLoginTest().LoginByPassword();
        // await new WtLoginTest().FetchQrCode();
    }
}