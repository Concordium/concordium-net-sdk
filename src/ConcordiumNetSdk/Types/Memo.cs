using System.Formats.Cbor;

using ConcordiumNetSdk.Helpers;
using ConcordiumNetSdk.Transactions;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a memo to be registered on-chain with the <see cref="TransferWithMemo"/> account transaction.
///
/// The memo can be any data which is at most <see cref="MaxLength"/> bytes, but convention is to encode a text message as CBOR.
/// </summary>
public readonly struct Memo : IEquatable<Memo>
{
    private const int MaxLength = 256;

    /// <summary>
    /// Byte array representing the memo.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Memo"/> class.
    /// </summary>
    /// <param name="bytes">A memo represented by at most <see cref="MaxLength"/> bytes.</param>
    private Memo(byte[] bytes)
    {
        _value = bytes;
    }

    /// <summary>
    /// Creates an instance from hex encoded string.
    /// </summary>
    /// <param name="hexString">The memo to be registered on-chain represented as a hex encoded string.</param>
    public static Memo FromHex(string hexString)
    {
        var value = Convert.FromHexString(hexString);
        return From(value);
    }

    /// <summary>
    /// Creates an instance from a <see cref="string"/> whose CBOR encoding will be used for the memo data.
    /// </summary>
    /// <param name="text">The memo represented as a <see cref="string"/>
    /// whose CBOR encoding will be used for the memo data.
    /// </param>
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
    /// <returns>
    /// A <see cref="string"> corresponding to the decoded memo if it contained
    /// a single CBOR encoded string, and <c>null</c> otherwise.
    /// </returns>
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
    /// Get the memo as a byte array with the length of the array
    /// prepended as a 16-bit unsigned integer in big-endian format.
    /// </summary>
    public byte[] GetBytes()
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(Serialization.GetBytes((UInt16)_value.Length));
        memoryStream.Write(_value);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Get the memo as a hex-encoded string.
    /// </summary>
    public string GetHexString()
    {
        return Convert.ToHexString(_value).ToLowerInvariant();
    }

    public bool Equals(Memo memo)
    {
        return _value.SequenceEqual(memo._value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (obj.GetType() != GetType())
            return false;

        var other = (Memo)obj;
        return Equals(other);
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
