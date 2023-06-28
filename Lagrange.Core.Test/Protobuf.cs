using Lagrange.Core.Core.Packets.Login.Ecdh;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Test;

public class Protobuf
{
    public void Test()
    {
        
        var test = new SsoKeyExchange()
        {
            PubKey = new byte[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 },
            GcmCalc2 = new byte[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 },
            GcmCalc1 = new byte[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 },
            Timestamp = 23456789,
            Type = 1
        };
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, test);
        Console.WriteLine(stream.ToArray().Hex(false, true));
    }
}