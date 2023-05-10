using System.Formats.Cbor;

using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents data to be stored or that was stored on-chain as part of a transaction.
///
/// This can be any data which is at most <see cref="MaxLength"/> bytes, but the convention is
/// to use CBOR encoded data.
/// </summary>
public record OnChainData : IEquatable<OnChainData>
{
    public const int MaxLength = 256;

    /// <summary>
    /// Byte array representing the data.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="OnChainData"/> class.
    /// </summary>
    /// <param name="bytes">Data represented by at most <see cref="MaxLength"/> bytes.</param>
    private OnChainData(byte[] bytes) => this._value = bytes;

    /// <summary>
    /// Creates an instance from a hex encoded string.
    /// </summary>
    /// <param name="hexString">Data represented as a hex encoded string representing at most <see cref="MaxLength"/> bytes.</param>
    /// <exception cref="ArgumentException">The supplied string is not a hex encoded string representing at most <see cref="MaxLength"/> bytes.</exception>
    public static OnChainData FromHex(string hexString)
    {
        try
        {
            var value = Convert.FromHexString(hexString);
            return From(value);
        }
        catch (Exception e)
        {
            throw new ArgumentException("The provided string is not hex encoded: ", e);
        }
    }

    /// <summary>
    /// Creates an instance from a <see cref="string"/> whose CBOR encoding will be used for the data to be registered on-chain.
    /// </summary>
    /// <param name="data">
    /// Text to store on-chain represented as a <see cref="string"/> whose CBOR encoding will be used for the data to registered on-chain.
    /// </param>
    /// <param name="conformanceMode">
    /// The conformance mode to use for encoding and decoding CBOR data.
    /// </param>
    /// <exception cref="ArgumentException">The resulting CBOR encoded string exceeds <see cref="MaxLength"/> bytes.</exception>
    /// <exception cref="ArgumentException">The supplied string is not a valid UTF-8 encoding and this is not permitted under the current conformance mode.</exception>
    /// <exception cref="InvalidOperationException">The written CBOR data is not accepted under the current conformance mode.</exception>
    public static OnChainData FromTextEncodeAsCBOR(
        string data,
        CborConformanceMode conformanceMode = CborConformanceMode.Strict
    )
    {
        var encoder = new CborWriter(conformanceMode);
        encoder.WriteTextString(data);
        var encodedBytes = encoder.Encode();
        return From(encodedBytes);
    }

    /// <summary>
    /// Creates an instance from byte array.
    /// </summary>
    /// <param name="data">The data to be registered on-chain represented as a byte array.</param>
    /// <exception cref="ArgumentException">The length of the supplied data exceeds <see cref="MaxLength"/>.</exception>
    public static OnChainData From(byte[] dataAsBytes)
    {
        if (dataAsBytes.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Size of a data is not allowed to exceed {MaxLength} bytes."
            );
        }

        return new OnChainData((byte[])dataAsBytes.Clone());
    }

    /// <summary>
    /// Try to decode the data to be registered on-chain as a single CBOR encoded string.
    /// </summary>
    /// <returns>
    /// A <c>string</c> corresponding to the decoded data if it contained
    /// a single CBOR encoded string, and <c>null</c> otherwise.
    /// </returns>
    public string? TryCborDecodeToString()
    {
        var encoder = new CborReader(this._value);
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
    /// Get the on-chain data in the binary format expected by the node.
    ///
    /// That is, represented as a byte array with the length of the array
    /// prepended as a 16-bit unsigned integer in big-endian format.
    /// </summary>
    public byte[] GetBytes()
    {
        using var memoryStream = new MemoryStream();
        memoryStream.Write(Serialization.GetBytes((ushort)this._value.Length));
        memoryStream.Write(this._value);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Get the data to register on-chain as a hex-encoded string.
    /// </summary>
    public override string ToString() => Convert.ToHexString(this._value).ToLowerInvariant();

    public virtual bool Equals(OnChainData? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != this.GetType())
        {
            return false;
        }

        return this._value.SequenceEqual(other._value);
    }

    public override int GetHashCode() => this._value.GetHashCode();
}
