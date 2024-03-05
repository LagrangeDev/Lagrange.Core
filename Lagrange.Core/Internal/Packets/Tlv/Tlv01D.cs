using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

/// <summary>
/// Different from oicq
/// </summary>
[Tlv(0x01D)]
internal class Tlv01D : TlvBody
{
    public Tlv01D(BotAppInfo appInfo) => MiscBitmap = (uint)appInfo.WtLoginSdk.MiscBitmap;

    [BinaryProperty] public byte FieldByte { get; set; } = 1;
    
    [BinaryProperty] public uint MiscBitmap { get; set; }
    
    [BinaryProperty] public uint Field0 { get; set; } = 0;
    
    [BinaryProperty] public byte FieldByte0 { get; set; } = 0;
}