namespace Lagrange.Core.Utility.Binary.JceStruct;

internal enum JceType : byte
{
    /// <summary>Byte or Boolean</summary>
    Byte = 0,
    Short = 1,
    Int = 2,
    Long = 3,
    Float = 4,
    Double = 5,
    /// <summary>Indicates that the length of the string is a single byte</summary>
    StringByte = 6,
    /// <summary>Indicates that the length of the string is a int</summary>
    StringInt = 7,
    Map = 8,
    List = 9,
    StructBegin = 10,
    StructEnd = 11,
    ZeroTag = 12,
    SimpleList = 13,
}