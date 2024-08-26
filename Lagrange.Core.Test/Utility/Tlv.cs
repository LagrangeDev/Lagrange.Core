using System.Text;
using Lagrange.Core.Utility.Extension;
using static Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Test.Utility;

public static class Tlv
{
    public static Dictionary<string, string> GetTlvDictionary(byte[] tlvs, bool isCommand = true)
    {
        var result = new Dictionary<string, string>();

        using var reader = new BinaryReader(new MemoryStream(tlvs));

        ushort command;
        if (isCommand)
        {
            command = ToUInt16(reader.ReadBytes(2), false);
        }

        ushort tlvCount = ToUInt16(reader.ReadBytes(2), false);

        for (int i = 0; i < tlvCount; i++)
        {
            ushort tlvTag = ToUInt16(reader.ReadBytes(2), false);
            ushort tlvLength = ToUInt16(reader.ReadBytes(2), false);
            byte[] tlvValue = reader.ReadBytes(tlvLength);

            result.Add($"0x{tlvTag:X} {tlvLength}", tlvValue.Hex());
            result.Add($"0x{tlvTag:X} UTF8 {tlvLength}", Encoding.UTF8.GetString(tlvValue));
        }

        return result;
    }
}