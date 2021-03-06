using System.Formats.Cbor;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// A Memo is stored on chain as just an array of bytes.
/// Convention is to encode a text message as CBOR, but this is not enforced by nodes.  
/// </summary>
public class Memo
{
    private const int MaxBytesLength = 255;

    private readonly byte[] _bytes;

    private Memo(byte[] bytes)
    {
        _bytes = bytes;
    }

    public static Memo FromHex(string hexString)
    {
        var bytes = Convert.FromHexString(hexString);
        return From(bytes);
    }

    public static Memo FromText(string text)
    {
        var encoder = new CborWriter();
        encoder.WriteTextString(text);
        var encodedBytes = encoder.Encode();
        return From(encodedBytes);
    }

    public static Memo From(byte[] memoAsBytes)
    {
        if (memoAsBytes.Length > 256)
            throw new ArgumentException($"Size of a memo is not allowed to exceed {MaxBytesLength} bytes.");
        return new Memo(memoAsBytes);
    }

    public bool TryCborDecodeToText(out string? decodedText)
    {
        var encoder = new CborReader(_bytes);
        try
        {
            var textRead = encoder.ReadTextString();
            if (encoder.BytesRemaining == 0)
            {
                decodedText = textRead;
                return true;
            }
        }
        catch (CborContentException)
        {
        }
        catch (InvalidOperationException)
        {
        }

        decodedText = null;
        return false;
    }

    public byte[] AsBytes => _bytes;

    public string AsHex => Convert.ToHexString(_bytes).ToLowerInvariant();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (Memo) obj;
        return _bytes.SequenceEqual(other._bytes);
    }

    public override int GetHashCode()
    {
        return _bytes.GetHashCode();
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
