using NBitcoin.DataEncoders;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// An account address.
/// </summary>
public class AccountAddress : IEquatable<AccountAddress>
{
    private static readonly Base58CheckEncoder EncoderInstance = new();
    public const UInt32 BytesLength = 32;

    /// <summary>
    /// Representation of the account address as a base58 encoded string.
    /// </summary>
    private readonly string _formatted;

    /// <summary>
    /// Representation of the account address as a byte array (without the version byte prepended).
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountAddress"/> class.
    /// </summary>
    /// <param name="addressAsBytes">The address as a sequence of bytes without a version byte prepended.</param>
    private AccountAddress(byte[] addressAsBytes)
    {
        _value = addressAsBytes;
        var bytesToEncode = new byte[33];
        bytesToEncode[0] = 1;
        addressAsBytes.CopyTo(bytesToEncode, 1);
        _formatted = EncoderInstance.EncodeData(bytesToEncode);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountAddress"/> class.
    /// </summary>
    /// <param name="addressAsBase58String">The address as a base58 encoded string.</param>
    private AccountAddress(string addressAsBase58String)
    {
        _formatted = addressAsBase58String;
        var decodedBytes = EncoderInstance.DecodeData(addressAsBase58String);
        _value = decodedBytes.Skip(1).ToArray(); // Remove version byte.
    }

    /// <summary>
    /// Get the address represented as a base58 encoded string.
    /// </summary>
    public override string ToString()
    {
        return (string)_formatted.Clone();
    }

    /// <summary>
    /// Get the address as a length-32 byte array without the version byte prepended.
    /// </summary>
    public byte[] GetBytes()
    {
        return (byte[])_value.Clone();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountAddress"/> class.
    /// </summary>
    /// <param name="addressAsBase58String">The address represented as a base58 encoded string.</param>
    public static AccountAddress From(string addressAsBase58String)
    {
        return new AccountAddress(addressAsBase58String);
    }

    /// <summary>
    /// Creates an instance from a length-32 byte array representing the address (without the version byte prepended).
    /// </summary>
    /// <param name="addressAsBytes">The address as a length-32 byte array without the version byte prepended.</param>
    public static AccountAddress From(byte[] addressAsBytes)
    {
        if (addressAsBytes.Length != BytesLength)
            throw new ArgumentException($"The account address bytes length must be {BytesLength}.");
        return new AccountAddress(addressAsBytes);
    }

    /// <summary>
    /// Checks if passed string is a base58 encoded address.
    /// </summary>
    /// <param name="addressAsBase58String">the account address as base58 check encoded string.</param>
    /// <returns><c>true<c/> if the input could be decoded as a base58 encoded address and <c>false</c> otherwise.</returns>
    public static bool IsValid(string? addressAsBase58String)
    {
        if (addressAsBase58String == null)
            return false;
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

    /// <summary>
    /// Converts the account address to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.AccountAddress ToProto()
    {
        return new Concordium.V2.AccountAddress()
        {
            Value = Google.Protobuf.ByteString.CopyFrom(this._value)
        };
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return ToString() == ((AccountAddress)obj).ToString();
    }

    public bool Equals(AccountAddress? accountAddress)
    {
        return this.Equals((Object?)accountAddress);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}
