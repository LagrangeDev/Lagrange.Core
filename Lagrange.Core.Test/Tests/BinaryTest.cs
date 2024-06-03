using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Test.Tests;

public class BinaryTest
{
    public class Binary
    {
        [BinaryProperty] public uint Value { get; set; } = 114514;
        
        [BinaryProperty] public ulong Value2 { get; set; } = 1919810;
    }
    
    public void Test()
    {
        var binary = new Binary();
        var bytes = BinarySerializer.Serialize(binary);
        Console.WriteLine(bytes.ToArray().Hex());
        Console.WriteLine(bytes.Length);

        var binary2 = new BinaryPacket();
        binary2.WriteUint(114514);
        binary2.WriteUlong(1919810);
        Console.WriteLine(binary2.ToArray().Hex());
        
        var newPacket = new BinaryPacket(bytes.ToArray());
        var binary3 = newPacket.Deserialize<Binary>();
        Console.WriteLine(binary3.Value);
        Console.WriteLine(binary3.Value2);
    }
}