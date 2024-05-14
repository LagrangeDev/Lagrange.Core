using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lagrange.Core.Utility.Binary;

/// <summary>
/// Binary Writer, Inspired by <see cref="System.IO.BinaryWriter"/>
/// <para>Provide only Sync Apis</para>
/// </summary>
internal unsafe partial class BinaryPacket : IDisposable  // TODO: Reimplement im raw byte[]
{
    private readonly MemoryStream _stream;

    private readonly BinaryReader _reader;
    
    public long Length => _stream.Length;
    
    public long Remaining => _stream.Length - _stream.Position;
    
    /// <summary>
    /// Create a Packet for write
    /// </summary>
    public BinaryPacket()
    {
        _stream = new MemoryStream();
        _reader = new BinaryReader(_stream);
    }
    
    /// <summary>
    /// Create a Packet for read
    /// </summary>
    public BinaryPacket(byte[] data)
    {
        _stream = new MemoryStream(data);
        _reader = new BinaryReader(_stream);
    }
    
    /// <summary>
    /// Create a Packet for read
    /// </summary>
    public BinaryPacket(Span<byte> data)
    {
        _stream = new MemoryStream(data.ToArray());
        _reader = new BinaryReader(_stream);
    }
    
    /// <summary>
    /// Create a Packet Reader for read
    /// </summary>
    public BinaryPacket(MemoryStream stream)
    {
        _stream = stream;
        _reader = new BinaryReader(_stream);
    }
    
    public BinaryPacket WriteString(string value, Prefix flag, int addition = 0)
    {
        WriteLength(value.Length, flag, addition);
        return WriteBytes(Encoding.UTF8.GetBytes(value));
    }

    public BinaryPacket WriteBytes(ReadOnlySpan<byte> value, Prefix flag, int addition = 0)
    {
        WriteLength(value.Length, flag, addition);
        return WriteBytes(value);
    }

    private BinaryPacket WriteLength(int origin, Prefix flag, int addition)
    {
        int prefixLength = (byte)flag & 0b0111;
        if (prefixLength == 0) return this;
        
        bool lengthCounted = (flag & Prefix.WithPrefix) > 0;
        int length = (lengthCounted ? prefixLength + origin : origin) + addition;
        _ = prefixLength switch
        {
            sizeof(byte) => WriteByte((byte)length),
            sizeof(ushort) => WriteUshort((ushort)length),
            sizeof(uint) => WriteUint((uint)length),
            _ => throw new InvalidDataException("Invalid Prefix is given")
        };

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryPacket WriteBytes(ReadOnlySpan<byte> value)
    {
        _stream.Write(value);
        return this;
    }

    public BinaryPacket WritePacket(BinaryPacket value)
    {
        value._stream.Seek(0, SeekOrigin.Begin);
        value._stream.CopyTo(_stream);
        return this;
    }
    
    public BinaryPacket WriteBool(bool value)
    {
        _stream.WriteByte(value ? (byte)1 : (byte)0);
        return this;
    }
    
    public BinaryPacket WriteByte(byte value)
    {
        _stream.WriteByte(value);
        return this;
    }
    
    public BinaryPacket WriteUshort(ushort value)
    {
        value = BinaryPrimitives.ReverseEndianness(value);
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(ushort));
        return WriteBytes(buffer);
    }

    public BinaryPacket WriteUint(uint value)
    {
        value = BinaryPrimitives.ReverseEndianness(value);
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(uint));
        return WriteBytes(buffer);
    }

    public BinaryPacket WriteUlong(ulong value)
    {
        value = BinaryPrimitives.ReverseEndianness(value);
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(ulong));
        return WriteBytes(buffer);
    }

    public BinaryPacket WriteSbyte(sbyte value)
    {
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(sbyte));
        return WriteBytes(buffer);
    }

    public BinaryPacket WriteShort(short value)
    {
        value = BinaryPrimitives.ReverseEndianness(value);
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(short));
        return WriteBytes(buffer);
    }

    public BinaryPacket WriteInt(int value)
    {
        value = BinaryPrimitives.ReverseEndianness(value);
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(int));
        return WriteBytes(buffer);
    }

    public BinaryPacket WriteLong(long value)
    {
        value = BinaryPrimitives.ReverseEndianness(value);
        var buffer = new ReadOnlySpan<byte>((byte*)&value, sizeof(long));
        return WriteBytes(buffer);
    }

    public BinaryPacket Barrier(Action<BinaryPacket> writer, Prefix flag, int addition = 0)
    {
        var packet = new BinaryPacket();
        writer(packet);
        return WriteLength((int)packet.Length, flag, addition).WritePacket(packet);
    }
    
    private int ReadLength(Prefix flag)
    {
        bool lengthCounted = (flag & Prefix.WithPrefix) > 0;
        int prefixLength = (byte)flag & 0b0111;

        int length = prefixLength switch
        {
            0 => (int)Remaining,
            1 => _stream.ReadByte(),
            2 => ReadUshort(),
            4 => (int)ReadUint(),
            _ => throw new InvalidDataException("Invalid Prefix is given")
        };

        if (lengthCounted) length -= prefixLength;
    
        return length;
    }
    
    public bool ReadBool() => Convert.ToBoolean(_stream.ReadByte());

    public byte ReadByte() => (byte)_stream.ReadByte();
    
    public ushort ReadUshort()
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        _ = _stream.Read(buffer);
        return BinaryPrimitives.ReadUInt16BigEndian(buffer);
    }

    public uint ReadUint()
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        _ = _stream.Read(buffer);
        return BinaryPrimitives.ReadUInt32BigEndian(buffer);
    }

    public ulong ReadUlong()
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        _ = _stream.Read(buffer);
        return BinaryPrimitives.ReadUInt64BigEndian(buffer);
    }

    public sbyte ReadSbyte()
    {
        Span<byte> buffer = stackalloc byte[sizeof(sbyte)];
        _ = _stream.Read(buffer);
        return (sbyte)buffer[0];
    }

    public short ReadShort()
    {
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        _ = _stream.Read(buffer);
        return BinaryPrimitives.ReadInt16BigEndian(buffer);
    }

    public int ReadInt()
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        _ = _stream.Read(buffer);
        return BinaryPrimitives.ReadInt32BigEndian(buffer);
    }

    public long ReadLong()
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        _ = _stream.Read(buffer);
        return BinaryPrimitives.ReadInt64BigEndian(buffer);
    }

    public Span<byte> ReadBytes(int count)
    {
        if (_stream.Length - _stream.Position < count) throw new InvalidDataException("Not enough data to read, remaining: " + (_stream.Length - _stream.Position) + " required: " + count);
        if (count < 0) throw new InvalidDataException("Invalid count to read, count: " + count);
        
        Span<byte> buffer =  new byte[count];
        _ = _stream.Read(buffer);
        return buffer;
    }

    public Span<byte> ReadBytes(Prefix flag)
    {
        int length = ReadLength(flag);
        if (_stream.Length - _stream.Position < length) throw new InvalidDataException("Not enough data to read, remaining: " + (_stream.Length - _stream.Position) + " required: " + length);
        if (length < 0) throw new InvalidDataException("Invalid count to read, count: " + length);

        return ReadBytes(length);
    }

    public string ReadString(Prefix flag)
    {
        int length = ReadLength(flag);
        return Encoding.UTF8.GetString(ReadBytes(length));
    }
    
    public string ReadString(int count) => Encoding.UTF8.GetString(ReadBytes(count));

    public BinaryPacket ReadPacket(int count) => new(ReadBytes(count).ToArray());
    
    public void Skip(int length) => _reader.BaseStream.Seek(length, SeekOrigin.Current);
    
    public bool IsAvailable(int length) => _stream.Length - _stream.Position >= length;

    public byte[] ToArray() => _stream.ToArray();


    public void Dispose()
    {
        _stream.Dispose();
        _reader.Dispose();
    }
}

[Flags]
public enum Prefix
{
    None = 0,
    Uint8 = 0b0001,
    Uint16 = 0b0010,
    Uint32 = 0b0100,
    LengthOnly = 0,
    WithPrefix = 0b1000,
}