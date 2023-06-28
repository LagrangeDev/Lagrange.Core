using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Core.Packets.Tlv;

[Tlv(0x116)]
internal class Tlv116 : TlvBody
{
    public Tlv116(BotAppInfo appInfo) => SubSigMap = appInfo.SubSigMap;
    
    private static readonly uint[] AppIds = { };

    [BinaryProperty] public byte Version { get; set; } = 0;

    [BinaryProperty] public uint MiscBitmap { get; set; } = 12058620;
    
    [BinaryProperty] public uint SubSigMap { get; set; }
    
    [BinaryProperty] public byte AppIdCount { get; set; } = (byte)AppIds.Length;

    [BinaryProperty(Prefix.None)] public byte[] AppIdBytes => 
        AppIds.Select(x => BitConverter.GetBytes(x, false)).SelectMany(x => x).ToArray();
}