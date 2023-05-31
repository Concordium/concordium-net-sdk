using System.Formats.Cbor;

namespace Concordium.Sdk.Types.New;

/// <summary>
/// A Memo is stored on chain as just an array of bytes.
/// Convention is to encode a text message as CBOR, but this is not enforced by nodes.
/// </summary>
public class Memo
{
    private readonly byte[] _bytes;

    public Memo(byte[] bytes)
    {
        if (bytes == null) throw new ArgumentNullException(nameof(bytes));
        if (bytes.Length > 256) throw new ArgumentException("Size of a memo is not allowed to exceed 256 bytes.");
        this._bytes = bytes;
    }

    public static Memo CreateFromHex(string hexString)
    {
        var bytes = Convert.FromHexString(hexString);
        return new Memo(bytes);
    }

    public static Memo CreateCborEncodedFromText(string text)
    {
        var encoder = new CborWriter();
        encoder.WriteTextString(text);
        var encodedBytes = encoder.Encode();
        return new Memo(encodedBytes);
    }

    public byte[] AsBytes => this._bytes;

    public string AsHex => Convert.ToHexString(this._bytes).ToLowerInvariant();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        var other = (Memo)obj;
        return this._bytes.SequenceEqual(other._bytes);
    }

    public override int GetHashCode()
    {
        return this._bytes.GetHashCode();
    }

    public static bool operator ==(Memo? left, Memo? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Memo? left, Memo? right)
    {
        return !Equals(left, right);
    }
}
