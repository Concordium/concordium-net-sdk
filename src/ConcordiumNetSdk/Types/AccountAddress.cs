using NBitcoin.DataEncoders;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base58 check with version byte 1 encoded account address (with Bitcoin mapping table).
/// </summary>
public class AccountAddress
{
    private static readonly Base58CheckEncoder EncoderInstance = new();
    private const int BytesLength = 32;

    private readonly string _formatted;
    private readonly byte[] _value;

    private AccountAddress(string addressAsBase58String)
    {
        _formatted = addressAsBase58String;
        var decodedBytes = EncoderInstance.DecodeData(addressAsBase58String);
        _value = decodedBytes.Skip(1).ToArray(); // Removes version byte
    }

    private AccountAddress(byte[] addressAsBytes)
    {
        _value = addressAsBytes;
        var bytesToEncode = new byte[33];
        bytesToEncode[0] = 1;
        addressAsBytes.CopyTo(bytesToEncode, 1);
        _formatted = EncoderInstance.EncodeData(bytesToEncode);
    }

    /// <summary>
    /// Gets the address as a base58 check encoded string.
    /// </summary>
    public string AsString => _formatted;

    /// <summary>
    /// Gets the address as a 32 byte array (without leading version byte).
    /// </summary>
    public byte[] AsBytes => _value;

    /// <summary>
    /// Creates an instance from a base58 check encoded string representing account address.
    /// </summary>
    /// <param name="addressAsBase58String">the account address as base58 check encoded string.</param>
    public static AccountAddress From(string addressAsBase58String)
    {
        return new AccountAddress(addressAsBase58String);
    }

    /// <summary>
    /// Creates an instance from a 32 bytes representing address (ie. excluding the version byte).
    /// </summary>
    /// <param name="addressAsBytes">the account address as 32 bytes.</param>
    public static AccountAddress From(byte[] addressAsBytes)
    {
        if (addressAsBytes.Length != BytesLength) throw new ArgumentException($"The account address bytes length must be {BytesLength}.");
        return new AccountAddress(addressAsBytes);
    }

    /// <summary>
    /// Checks if passed string is base58 check encoded address with Bitcoin mapping table.
    /// </summary>
    /// <param name="addressAsBase58String">the account address as base58 check encoded string.</param>
    /// <returns><see cref="bool"/> - true or false base on validation result.</returns>
    public static bool IsValid(string? addressAsBase58String)
    {
        if (addressAsBase58String == null) return false;

        try
        {
            EncoderInstance.DecodeData(addressAsBase58String);
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
        return _formatted;
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
