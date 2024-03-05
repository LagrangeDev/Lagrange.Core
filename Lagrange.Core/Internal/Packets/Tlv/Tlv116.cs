using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x116)]
internal class Tlv116 : TlvBody
{
    public Tlv116(BotAppInfo appInfo)
    {
        MiscBitmap = appInfo.WtLoginSdk.MiscBitmap;
        SubSigMap = appInfo.WtLoginSdk.SubSigBitmap;
        SubAppIdCount = (byte)appInfo.WtLoginSdk.SubAppIdList.Length;
        SubAppIdBody = new byte[SubAppIdCount * 4];
        for (var i = 0; i < SubAppIdCount; i++)
            BitConverter.GetBytes(appInfo.WtLoginSdk.SubAppIdList[i], false).CopyTo(SubAppIdBody, i * 4);
    }

    [BinaryProperty] public byte Version { get; set; } = 0;

    [BinaryProperty] public uint MiscBitmap { get; set; } // 0x0AF7FF7C when login0X9, 0x08F7FF7C when login0x2,0X8 exchange0xF

    [BinaryProperty] public uint SubSigMap { get; set; }

    [BinaryProperty] public byte SubAppIdCount { get; set; }

    [BinaryProperty(Prefix.None)] public byte[] SubAppIdBody { get; set; }
}