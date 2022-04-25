using NBitcoin.DataEncoders;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents base-58 check with version byte 1 encoded account address (with Bitcoin mapping table)
/// </summary>
public class AccountAddress : Address
{
    private static readonly Base58CheckEncoder EncoderInstance = new();
    private readonly byte[] _bytes;

    /// <summary>
    /// Creates an instance from a 32 byte address (ie. excluding the version byte).
    /// </summary>
    private AccountAddress(byte[] bytes)
    {
        _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

        if (_bytes.Length != 32) throw new ArgumentException("Expected length to be exactly 32 bytes");

        var bytesToEncode = new byte[33];
        bytesToEncode[0] = 1;
        bytes.CopyTo(bytesToEncode, 1);
        AsString = EncoderInstance.EncodeData(bytesToEncode);
    }

    /// <summary>
    /// Creates an instance from a base58-check encoded string
    /// </summary>
    private AccountAddress(string base58CheckEncodedAddress)
    {
        AsString = base58CheckEncodedAddress ?? throw new ArgumentNullException(nameof(base58CheckEncodedAddress));

        var decodedBytes = EncoderInstance.DecodeData(base58CheckEncodedAddress);
        _bytes = decodedBytes.Skip(1).ToArray(); // Remove version byte
    }

    /// <summary>
    /// Gets the address as a byte array (without leading version byte).
    /// Will always be 32 bytes. 
    /// </summary>
    public byte[] AsBytes => _bytes;

    /// <summary>
    /// Gets the address as a base58-check encoded string.
    /// </summary>
    public string AsString { get; }

    /// <summary>
    /// Creates an instance from a 32 byte address (ie. excluding the version byte).
    /// </summary>
    public static AccountAddress From(byte[] bytes)
    {
        return new AccountAddress(bytes);
    }
    
    /// <summary>
    /// Creates an instance from a base58-check encoded string
    /// </summary>
    public static AccountAddress From(string base58CheckEncodedAddress)
    {
        return new AccountAddress(base58CheckEncodedAddress);
    }

    /// <summary>
    /// Checks if passed string is base-58 check encoded address with Bitcoin mapping table
    /// </summary>
    /// <param name="base58CheckEncodedAddress">Base-58 check encoded address</param>
    /// <returns><see cref="bool"/> true or false base on validation result.</returns>
    public static bool IsValid(string? base58CheckEncodedAddress)
    {
        if (base58CheckEncodedAddress == null) return false;

        try
        {
            EncoderInstance.DecodeData(base58CheckEncodedAddress);
            return true;
        }
        catch (FormatException)
        {
            // Decode throws FormatException if decode is not successful
            return false;
        }
    }

    public override string ToString()
    {
        return AsString;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return AsString == ((AccountAddress) obj).AsString;
    }

    public override int GetHashCode()
    {
        return AsString.GetHashCode();
    }

    public static bool operator ==(AccountAddress? left, AccountAddress? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AccountAddress? left, AccountAddress? right)
    {
        return !Equals(left, right);
    }
}