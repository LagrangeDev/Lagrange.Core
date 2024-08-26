using System.Numerics;

namespace Lagrange.Core.Utility.Crypto.Provider.Ecdh;

/// <summary>
/// The Elliptic Point used in EC Diffie-Hellman key exchange.
/// </summary>
internal struct EllipticPoint : IEquatable<EllipticPoint>
{
    public BigInteger X { get; set; }

    public BigInteger Y { get; set; }

    public bool IsDefault => X.IsZero && Y.IsZero;

    public EllipticPoint(BigInteger x, BigInteger y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(EllipticPoint other) => other.X.Equals(X) && other.Y.Equals(Y);

    public override bool Equals(object? obj) => obj is EllipticPoint other && Equals(other);

    public override int GetHashCode() => Y.GetHashCode() + X.GetHashCode();

    public override string ToString() => $"({X:X}, {Y:X})";

    public static bool operator ==(EllipticPoint left, EllipticPoint right) => left.Equals(right);

    public static bool operator !=(EllipticPoint left, EllipticPoint right) => !(left == right);

    public static EllipticPoint operator -(EllipticPoint p) => new(-p.X, -p.Y);
}