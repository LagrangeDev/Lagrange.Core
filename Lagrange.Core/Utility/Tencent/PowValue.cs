using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Utility.Tencent;

internal class PowValue
{
    public byte Version { get; set; }
    
    public byte CheckType { get; set; }
    
    public byte AlgorithmType { get; set; }
    
    public byte HasHashResult { get; set; }
    
    public ushort BaseCount { get; set; }
    
    public byte[] Filling { get; set; }
    
    
    public ushort OriginSize { get; set; }

    public byte[] Origin { get; set; } = Array.Empty<byte>();
    
    
    public ushort CpSize { get; set; }

    public byte[] Cp { get; set; } = Array.Empty<byte>();
    
    
    public ushort Sm3Size { get; set; }
    
    public byte[] Sm3 { get; set; } = Array.Empty<byte>();

    
    public ushort HashResultSize { get; set; }
    
    public byte[] HashResult { get; set; } = Array.Empty<byte>();
    
    public int Cost { get; set; }
    
    public int Count { get; set; }
    public PowValue()
    {
        Version = 1;
        CheckType = 2;
        AlgorithmType = 1;
        HasHashResult = 1;
        BaseCount = 10;

        Filling = new byte[] { 0, 0 };

        OriginSize = 128;
        Origin = new byte[128];

        Random.Shared.NextBytes(Origin);

        CpSize = 32;
        Cp = Origin.Sha256().UnHex();

        var writer = new BinaryPacket();

        writer.WriteByte(Version);
        writer.WriteByte(CheckType);
        writer.WriteByte(AlgorithmType);
        writer.WriteByte(HasHashResult);
        writer.WriteUshort(BaseCount, false);

        writer.WriteBytes(Filling, BinaryPacket.Prefix.None);

        writer.WriteBytes(Origin, BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
        writer.WriteBytes(Cp, BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);

        Sm3Size = 172; // 1+1+1+1+2+2+2+128+2+32
        Sm3 = writer.ToArray();
    }

    public PowValue(byte[] data)
    {
        var reader = new BinaryPacket(data);

        Version = reader.ReadByte();
        CheckType = reader.ReadByte();
        AlgorithmType = reader.ReadByte();
        HasHashResult = reader.ReadByte();
        BaseCount = reader.ReadUshort(false);

        Filling = reader.ReadBytes(2);

        OriginSize = reader.ReadUshort(false);
        if (OriginSize > 0) Origin = reader.ReadBytes(OriginSize);
        
        CpSize = reader.ReadUshort(false);
        if (CpSize > 0) Cp = reader.ReadBytes(CpSize);
        
        Sm3Size = reader.ReadUshort(false);
        if (Sm3Size > 0) Sm3 = reader.ReadBytes(Sm3Size);

        if (HasHashResult == 1)
        {
            HashResultSize = reader.ReadUshort(false);
            if (HashResultSize > 0) HashResult = reader.ReadBytes(Sm3Size);
            Cost = reader.ReadInt(false);
            Count = reader.ReadInt(false);
        }
    }

    public byte[] ToArray()
    {
        var writer = new BinaryPacket();

        writer.WriteByte(Version);
        writer.WriteByte(CheckType);
        writer.WriteByte(AlgorithmType);
        writer.WriteByte(HasHashResult);
        writer.WriteUshort(BaseCount, false);
        
        writer.WriteBytes(Filling, BinaryPacket.Prefix.None);

        writer.WriteBytes(Origin, BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
        writer.WriteBytes(Cp, BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
        writer.WriteBytes(Sm3, BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);

        if (HasHashResult == 1)
        {
            writer.WriteUshort(HashResultSize, false);
            writer.WriteBytes(HashResult);
            writer.WriteInt(Cost, false);
            writer.WriteInt(Count, false);
        }

        return writer.ToArray();
    }
}