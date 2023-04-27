using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Data to be registered on-chain with a <see cref="RegisterData"/> account transaction.
/// </summary>
public readonly struct Data
{
    /// <summary>
    /// Maximum length of the data in bytes.
    /// </summary>
    public const int MaxLength = 256;

    /// <summary>
    /// Data to be registered on-chain.
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Data"/> class.
    /// </summary>
    /// <param name="data">Data to be registered on-chain.</param>
    private Data(byte[] data)
    {
        _value = data;
    }

    /// <summary>
    /// Creates an instance from data represented by a hex encoded string.
    /// </summary>
    /// <param name="hexString">The data to be registered on-chain represented by a hex encoded string.</param>
    public static Data From(string hexString)
    {
        try
        {
            return Data.From(Convert.FromHexString(hexString));
        }
        catch (Exception e)
        {
            throw new ArgumentException("The provided string is not hex encoded: ", e);
        }
    }

    /// <summary>
    /// Creates an instance from byte array.
    /// </summary>
    /// <param name="data">The data to be registered on-chain represented as a byte array.</param>
    /// <exception cref="ArgumentException">When the data is <c>null</c> or the length exceeds <see cref="MaxLength"/>.</exception>
    public static Data From(byte[] data)
    {
        if (data == null)
        {
            throw new ArgumentException("Data cannot be null");
        }

        if (data.Length > MaxLength)
        {
            throw new ArgumentException($"Size of Data cannot exceed {MaxLength} bytes");
        }

        return new Data(data);
    }

    /// <summary>
    /// Get the data in the binary format expected by the node.
    ///
    /// Specifically this is as a byte array with the length of the array
    /// prepended as a 16-bit unsigned integer in big-endian format.
    /// </summary>
    public byte[] GetBytes()
    {
        using MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(Serialization.GetBytes((UInt16)_value.Length));
        memoryStream.Write(_value);
        return memoryStream.ToArray();
    }
}
