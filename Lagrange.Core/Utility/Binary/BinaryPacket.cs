using System.Text;
using Lagrange.Core.Utility.Binary.Tlv;

namespace Lagrange.Core.Utility.Binary;

/// <summary>
/// Binary Writer, Inspired by <see cref="System.IO.BinaryWriter"/>
/// <para>Provide only Sync Apis</para>
/// </summary>
internal partial class BinaryPacket
{
    private readonly MemoryStream _stream;
    
    private readonly BinaryReader _reader;
    
    public long Length => _stream.Length;
    
    public long Remaining => _stream.Length - _stream.Position;
    
    public BinaryPacket()
    {
        _stream = new MemoryStream();
        _reader = new BinaryReader(_stream);
    }
    
    public BinaryPacket(byte[] data)
    {
        _stream = new MemoryStream(data);
        _reader = new BinaryReader(_stream);
    }

    public BinaryPacket WriteByte(byte value)
    {
        _stream.WriteByte(value);
        return this;
    }

    public BinaryPacket WriteBytes(ReadOnlySpan<byte> value)
    {
        _stream.Write(value);
        return this;
    }
    
    public BinaryPacket WriteBytes(byte[] value, Prefix prefixFlag = Prefix.None, byte limitedLength = 0)
    {
        int prefixLength = (int)prefixFlag & 0b0111;
        byte[] array; // 处理后的数据
        
        if (limitedLength > 0) // 限制长度时，写入数据长度=前缀+限制
        {
            limitedLength = (byte)value.Length;
            array = new byte[prefixLength + limitedLength];
            int len = value.Length > limitedLength ? limitedLength : value.Length;
            if (len > 0) Buffer.BlockCopy(value, 0, array, prefixLength, len);
        }
        else if (prefixLength > 0) // 不限制长度且有前缀时，写入数据长度 = 前缀 + value长度
        {
            array = new byte[prefixLength + value.Length];
            if (value.Length > 0) Buffer.BlockCopy(value, 0, array, prefixLength, value.Length);
        }
        else // 不限制又没有前缀，写入的就是value本身，不用处理，直接写入
        {
            WriteBytes(value.AsSpan());
            return this;
        }
        
        if (prefixLength > 0) // 添加前缀，使用大端序
        {
            int len = value.Length;
            if ((prefixFlag & Prefix.WithPrefix) > 0) len += prefixLength;
            if (!InsertPrefix(array, 0, (uint)len, (Prefix)prefixLength, false))
            {
                throw new IOException("Given prefix length is too small for value bytes."); // 给定的prefix不够填充value.Length，终止写入
            }
        }
        WriteBytes(array.AsSpan());
        
        return this;
    }
    
    public BinaryPacket WriteString(string value, Prefix prefixFlag = Prefix.None, Encoding? encoding = null, byte limitedLength = 0)
    {
        encoding ??= Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(value);
        WriteBytes(bytes, prefixFlag, limitedLength);
        return this;
    }
    
    public BinaryPacket WriteBool(bool value)
    {
        _stream.WriteByte(value ? (byte)1 : (byte)0);
        return this;
    }

    public BinaryPacket WriteShort(short value, bool isLittleEndian = true)
    {
        _stream.Write(BitConverter.GetBytes(value, isLittleEndian).AsSpan());
        return this;
    }
    
    public BinaryPacket WriteUshort(ushort value, bool isLittleEndian = true)
    {
        _stream.Write(BitConverter.GetBytes(value, isLittleEndian).AsSpan());
        return this;
    }

    public BinaryPacket WriteInt(int value, bool isLittleEndian = true)
    {
        _stream.Write(BitConverter.GetBytes(value, isLittleEndian).AsSpan());
        return this;
    }
    
    public BinaryPacket WriteUint(uint value, bool isLittleEndian = true)
    {
        _stream.Write(BitConverter.GetBytes(value, isLittleEndian).AsSpan());
        return this;
    }

    public BinaryPacket WriteLong(long value, bool isLittleEndian = true)
    {
        _stream.Write(BitConverter.GetBytes(value, isLittleEndian).AsSpan());
        return this;
    }

    public BinaryPacket WriteUlong(ulong value, bool isLittleEndian = true)
    {
        _stream.Write(BitConverter.GetBytes(value, isLittleEndian).AsSpan());
        return this;
    }
    
    public BinaryPacket WritePacket(BinaryPacket packet)
    {
        _stream.Write(packet.ToArray());
        return this;
    }

    public byte ReadByte() => _reader.ReadByte();
    
    public byte[] ReadBytes(int length) => _reader.ReadBytes(length);

    public byte[] ReadBytes(Prefix prefixFlag)
    {
        uint length;
        bool reduce = (prefixFlag & Prefix.WithPrefix) > 0;
        uint preLen = (uint)prefixFlag & 0b0111;
        
        switch (preLen)
        {
            case 0: // Read all remaining bytes
                length = (uint)(_stream.Length - _stream.Position);
                break;
            case 1: case 2: case 4: // Read length from prefix
                if (IsAvailable((int)preLen))
                {
                    length = preLen switch
                    {
                        1 => _reader.ReadByte(),
                        2 => ReadUshort(false),
                        4 => ReadUint(false),
                        _ => throw new ArgumentOutOfRangeException($"Invalid prefix length.")
                    };

                    if (reduce)
                    {
                        if (length < preLen) throw new IOException("Data length is less than prefix length.");
                        length -= preLen;
                    }
                    break;
                }
                throw new IOException("Data length is less than prefix length.");
            default:
                throw new ArgumentOutOfRangeException($"Invalid prefix flag.");
        }

        if (IsAvailable((int)length)) return _reader.ReadBytes((int)length);
        throw new IOException("Data length is less than prefix length.");
    }
    
    public string ReadString(int length, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(_reader.ReadBytes(length));
    }
    
    public string ReadString(Prefix prefixFlag, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(ReadBytes(prefixFlag));
    }

    public bool ReadBool() => _reader.ReadByte() == 1;

    public bool ReadBool(int length, bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(length);
        var offset = _reader.BaseStream.Position;
        return bytes[isLittleEndian ? offset : offset + length - 1] > 0;
    }

    public short ReadShort(bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(2);
        return BitConverter.ToInt16(bytes, isLittleEndian);
    }
    
    public ushort ReadUshort(bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(2);
        return BitConverter.ToUInt16(bytes, isLittleEndian);
    }
    
    public int ReadInt(bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(4);
        return BitConverter.ToInt32(bytes, isLittleEndian);
    }
    
    public uint ReadUint(bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(4);
        return BitConverter.ToUInt32(bytes, isLittleEndian);
    }

    public long ReadLong(bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(8);
        return BitConverter.ToInt64(bytes, isLittleEndian);
    }
    
    public ulong ReadUlong(bool isLittleEndian = true)
    {
        var bytes = _reader.ReadBytes(8);
        return BitConverter.ToUInt64(bytes, isLittleEndian);
    }
    
    public BinaryPacket ReadPacket(int length)
    {
        var bytes = _reader.ReadBytes(length);
        return new BinaryPacket(bytes);
    }

    /// <summary>
    /// After the barrier is entered, the Following data will be recorded for length calculation and the written to the packet
    /// </summary>
    public BinaryPacket Barrier(Type barrierType, Func<BinaryPacket> writer, bool isLittleEndian = true, bool withPrefix = false, int addition = 0)
    {
        var barrierWriter = writer();
        var barrierBytes = barrierWriter.ToArray();
        int length = barrierBytes.Length + addition;

        if (barrierType == typeof(byte)) WriteByte((byte)(length + (withPrefix ? 1 : 0)));
        else if (barrierType == typeof(ushort)) WriteShort((short)(length + (withPrefix ? 2 : 0)), isLittleEndian);
        else if (barrierType == typeof(uint)) WriteUint((uint)(length + (withPrefix ? 4 : 0)), isLittleEndian);
        else if (barrierType == typeof(ulong)) WriteUlong((ulong)(length + (withPrefix ? 8 : 0)), isLittleEndian);
        else throw new ArgumentException("Barrier Type must be byte, ushort, uint or ulong");

        WritePacket(barrierWriter);
        
        return this;
    }
    
    public void Skip(int length) => _reader.BaseStream.Seek(length, SeekOrigin.Current);

    public static bool InsertPrefix(byte[] buffer, uint offset, uint value, Prefix prefixFlag, bool isLittleEndian = true)
    {
        switch (prefixFlag)
        {
            case Prefix.Uint8:
                if (value <= byte.MaxValue)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((byte)value), 0, buffer, (int)offset, 1);
                    return true;
                }
                break;
            case Prefix.Uint16:
                if (value <= ushort.MaxValue)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((ushort)value, isLittleEndian), 0, buffer, (int)offset, 2);
                    return true;
                }
                break;
            case Prefix.Uint32:
                Buffer.BlockCopy(BitConverter.GetBytes(value, isLittleEndian), 0, buffer, (int)offset, 4);
                return true;
        }
        return false;
    }
    
    public bool IsAvailable(int length) => _stream.Length - _stream.Position >= length;

    public byte[] ToArray() => _stream.ToArray();
}