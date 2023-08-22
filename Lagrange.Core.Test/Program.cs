using Lagrange.Core.Test.Tests;

// BenchmarkRunner.Run<ProtoBufTest>(new DebugBuildConfig());
await new NTLoginTest().LoginByPassword();
// await new WtLoginTest().FetchQrCode();

public class Test
{
    public string Name { get; set; } = "";
}