using Lagrange.Core.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x066)]
internal class Tlv066 : TlvBody
{
    public Tlv066(BotAppInfo appInfo) => PtOsVersion = appInfo.PtOsVersion;

    [BinaryProperty] public int PtOsVersion { get; }
}