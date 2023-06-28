namespace Lagrange.Core.Utility.Binary.JceStruct;

/// <summary>
/// <para>JceStruct Entity class, which is derived from <see cref="Dictionary{TKey,TValue}"/></para>
/// <para>JceStruct is the binary format with TTLV(Tag, Type, Length, Value) arrangement</para>
/// <para>TKey is <see cref="byte"/>, which represents the Tag of the field</para>
/// <para>TValue is <see cref="object"/>, you should manually resolve the type</para>
/// </summary>
internal class JceStruct : Dictionary<byte, object>
{
    public T GetValue<T> (byte tag) => (T)this[tag];

    public void SetValue<T>(byte tag, T value) => this[tag] = value!;
}