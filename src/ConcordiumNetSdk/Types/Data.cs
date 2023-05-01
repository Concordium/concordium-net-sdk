using ConcordiumNetSdk.Helpers;
using ConcordiumNetSdk.Transactions;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents data to be registered on-chain with a <see cref="RegisterData"/> account transaction.
/// </summary>
public readonly struct DataToRegister
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
    /// Initializes a new instance of the <see cref="DataToRegister"/> class.
    /// </summary>
    /// <param name="data">Data to be registered on-chain.</param>
    private DataToRegister(byte[] data)
    {
        _value = data;
    }

    /// <summary>
    /// Creates an instance from data represented by a hex encoded string.
    /// </summary>
    /// <param name="hexString">The data to be registered on-chain represented by a hex encoded string.</param>
    public static DataToRegister From(string hexString)
    {
        try
        {
            byte[] dataAsBytes = Convert.FromHexString(hexString);
            return DataToRegister.From(dataAsBytes);
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
    /// <exception cref="ArgumentException">The length of the input exceeds <see cref="MaxLength"/> bytes.</exception>
    public static DataToRegister From(byte[] data)
    {
        if (data.Length > MaxLength)
        {
            throw new ArgumentException($"Size of data cannot exceed {MaxLength} bytes");
        }
        return new DataToRegister((byte[])data.Clone());
    }

    /// <summary>
    /// Get the data as a hex encoded string.
    /// </summary>
    public override string ToString()
    {
        return Convert.ToHexString(_value).ToLowerInvariant();
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

    public bool Equals(DataToRegister other)
    {
        return _value.SequenceEqual(other._value);
    }

    public override bool Equals(Object? obj)
    {
        return obj is DataToRegister other && Equals(other);
    }

    public static bool operator ==(DataToRegister? left, DataToRegister? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DataToRegister? left, DataToRegister? right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}
