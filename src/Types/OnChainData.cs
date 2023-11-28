using System.Formats.Cbor;

using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents data to be stored or that was stored on-chain as part of a transaction.
///
/// This can be any data which is at most <see cref="MaxLength"/> bytes, but the convention is
/// to use CBOR encoded data.
/// </summary>
public sealed record OnChainData : IEquatable<OnChainData>
{
    /// <summary>
    /// The maximum length of a bytearray passed to the constructor.
    /// </summary>
    public const uint MaxLength = 256;

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
    /// <param name="dataAsBytes">The data to be registered on-chain represented as a byte array.</param>
    /// <exception cref="ArgumentException">The length of the supplied data exceeds <see cref="MaxLength"/>.</exception>
    public static OnChainData From(byte[] dataAsBytes)
    {
        if (dataAsBytes.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Size of a data is not allowed to exceed {MaxLength} bytes."
            );
        }

        return new OnChainData(dataAsBytes.ToArray());
    }

    /// <summary>
    /// Returns span of underlying data.
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<byte> AsSpan() => this._value.AsSpan();

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
        catch (Exception e) when (e is CborContentException or InvalidOperationException)
        { }
        return null;
    }

    /// <summary>
    /// Copies the on-chain data in the binary format expected by the node
    /// to a byte array.
    ///
    /// That is, represented as a byte array with the length of the array
    /// prepended as a 16-bit unsigned integer in big-endian format.
    /// </summary>
    public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream(sizeof(ushort) + this._value.Length);
        memoryStream.Write(Serialization.ToBytes((ushort)this._value.Length));
        memoryStream.Write(this._value);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Get the data to register on-chain as a hex-encoded string.
    /// </summary>
    public override string ToString() => Convert.ToHexString(this._value).ToLowerInvariant();

    /// <summary>
    /// Create an "OnChainData" from a byte array.
    /// </summary>
    /// <param name="bytes">The serialized "OnChainData".</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(byte[] bytes, out (OnChainData? accountAddress, string? Error) output)
    {
        if (bytes.Length == 0)
        {
            var msg = "Invalid length of input in `OnChainData.TryDeserial`. Length must be more than 0";
            output = (null, msg);
            return false;
        };

        if (bytes.Length > sizeof(ushort) + MaxLength)
        {
            var msg = $"Invalid length of input in `OnChainData.TryDeserial`. Length must not be more than {sizeof(ushort) + MaxLength}";
            output = (null, msg);
            return false;
        };

        var deserialSuccess = Deserial.TryDeserialU16(bytes, 0, out var sizeRead);
        if (!deserialSuccess)
        {
            output = (null, sizeRead.Error);
            return false;
        };

        var size = sizeRead.Uint + sizeof(ushort);

        if (bytes.Length != size)
        {
            var msg = $"Invalid length of input in `OnChainData.TryDeserial`. Expected array of size {size}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        output = (new OnChainData(bytes.Skip(sizeof(ushort)).ToArray()), null);
        return true;
    }

    public bool Equals(OnChainData? other) => other is not null && this._value.SequenceEqual(other._value);

    public override int GetHashCode() => Helpers.HashCode.GetHashCodeByteArray(this._value);

    internal static OnChainData? From(Grpc.V2.Memo? memo)
    {
        if (memo == null || memo.Value.Length == 0)
        {
            return null;
        }

        return From(memo.Value.ToByteArray());
    }

    internal static OnChainData From(Grpc.V2.RegisteredData registeredData) => From(registeredData.Value.ToByteArray());
}
