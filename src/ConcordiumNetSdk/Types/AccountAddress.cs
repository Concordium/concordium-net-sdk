using ConcordiumNetSdk.Helpers;
using NBitcoin.DataEncoders;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models an account address.
///
/// The address of an account is a 256-bit value which uniquely identifies the account.
/// </summary>
public readonly struct AccountAddress : IEquatable<AccountAddress>
{
    public const UInt32 BytesLength = 32;
    private static readonly Base58CheckEncoder EncoderInstance = new();

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
    }

    /// <summary>
    /// Get the address represented as a base58 encoded string.
    /// </summary>
    public override string ToString()
    {
        return EncoderInstance.EncodeData(_value);
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
        var decodedBytes = EncoderInstance.DecodeData(addressAsBase58String).Skip(1).ToArray(); // Remove version byte.
        return new AccountAddress(decodedBytes);
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
    /// <param name="addressAsBase58String">The account address as base58 encoded string.</param>
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
            // Decode throws <c>FormatException</c> if decode is not successful
            return false;
        }
    }

    /// <summary>
    /// Check whether the account address is an alias of another.
    ///
    /// Two addresses are aliases if they identify the same account. This is
    /// defined to be when the addresses agree on the first 29 bytes.
    /// </summary>
    /// <param name="other">the account address as base58 encoded string.</param>
    public bool IsAliasOf(AccountAddress other)
    {
        return _value.Take(29).SequenceEqual(other._value.Take(29));
    }

    /// <summary>
    /// Gets the <c>n</c>th alias.
    ///
    /// The first 29 bytes of an address are unique to the account. The integer
    /// 3 bytes represent an unsigned integer in big endian format whose value
    /// lies between <c>0</c> and <c>2^24-1</c>. The address
    /// </summary>
    /// <param name="n">the account address as base58 encoded string.</param>
    /// <exception cref="ArgumentOutOfRangeException">If <>n</c> is larger than <c>2^24-1</c>.</exception>
    public AccountAddress GetNthAlias(UInt32 n)
    {
        if (n > 16777215)
        {
            throw new ArgumentOutOfRangeException($"Alias can at most be 16777215, got {n}.");
        }
        // Serialize n to its corresponding alias bytes. Since we output it
        // in big endian format, we truncate the first and most significant byte.
        byte[] aliasBytes = Serialization.GetBytes((UInt32)n).Skip(1).ToArray();
        byte[] address = _value.Take(29).ToArray().Concat(aliasBytes).ToArray();
        return AccountAddress.From(address);
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

    public bool Equals(AccountAddress other)
    {
        return _value.SequenceEqual(other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && obj is AccountAddress other && Equals(other);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}
