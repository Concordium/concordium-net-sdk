using System.Buffers.Binary;
using System.Formats.Cbor;

using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Memo to be registered on-chain with the <see cref="TransferWithMemo"/> account transaction.
/// Convention is to encode a text message as CBOR, but this is not enforced.
/// </summary>
public class Memo
{
    private const int MaxLength = 255;

    /// <summary>
    /// Byte array representing the memo.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Memo"/> class.
    /// </summary>
    /// <param name="bytes">A hash represented as a length-64 hex encoded string.</param>
    private Memo(byte[] bytes)
    {
        _value = bytes;
    }

    public static Memo FromHex(string hexString)
    {
        var value = Convert.FromHexString(hexString);
        return From(value);
    }

    public static Memo FromText(string text)
    {
        var encoder = new CborWriter();
        encoder.WriteTextString(text);
        var encodedBytes = encoder.Encode();
        return From(encodedBytes);
    }

    /// <summary>
    /// Creates an instance from byte array.
    /// </summary>
    /// <param name="data">The memo to be registered on-chain represented as a byte array.</param>
    /// <exception cref="ArgumentException">When the data is <c>null</c> or the length exceeds <see cref="MaxLength"/>.</exception>
    public static Memo From(byte[] memoAsBytes)
    {
        if (memoAsBytes.Length > MaxLength)
            throw new ArgumentException(
                $"Size of a memo is not allowed to exceed {MaxLength} bytes."
            );
        return new Memo(memoAsBytes);
    }

    /// <summary>
    /// Try to decode the memo as a single CBOR encoded string.
    /// </summary>
    /// <returns>A <see cref="string"> corresponding to the decoded memo if it contained a single CBOR encoded string, and <c>null</c>  otherwise.</returns>
    public string? TryCborDecodeToText()
    {
        var encoder = new CborReader(_value);
        try
        {
            var textRead = encoder.ReadTextString();
            if (encoder.BytesRemaining == 0)
            {
                return textRead;
            }
        }
        catch (CborContentException) { }
        catch (InvalidOperationException) { }
        return null;
    }

    /// <summary>
    /// Gets the memo as a byte array with the length of the array prepended as a 16-bit unsigned integer in big-endian format.
    /// </summary>
    public byte[] GetBytes()
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(Serialization.GetBytes((UInt16)_value.Length));
        memoryStream.Write(_value);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Gets the memo as a hex-encoded string.
    /// </summary>
    public string GetHexString()
    {
        return Convert.ToHexString(_value).ToLowerInvariant();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        var other = (Memo)obj;
        return _value.SequenceEqual(other._value);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
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
