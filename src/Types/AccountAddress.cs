using Concordium.Sdk.Client;
using Concordium.Sdk.Helpers;
using NBitcoin.DataEncoders;

namespace Concordium.Sdk.Types;

/// <summary>
/// Represents an account address.
///
/// The address of an account is a 32-byte value which uniquely identifies the account.
/// </summary>
public readonly struct AccountAddress : IEquatable<AccountAddress>
{
    public const uint BytesLength = 32;

    /// <summary>
    /// A version byte that is prepended to addresses represented as base58 strings.
    /// </summary>
    private const byte VersionByte = 1;

    private static readonly Base58CheckEncoder _encoderInstance = new();

    /// <summary>
    /// Representation of the account address as a byte array (without the version byte prepended).
    /// </summary>
    private readonly byte[] _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountAddress"/> class.
    /// </summary>
    /// <param name="addressAsBytes">The address as a sequence of bytes without a version byte prepended.</param>
    private AccountAddress(byte[] addressAsBytes) => this._value = addressAsBytes;

    /// <summary>
    /// Get the address represented as a base58 encoded string.
    /// </summary>
    public override string ToString() =>
        // Prepend version byte.
        _encoderInstance.EncodeData((new byte[] { VersionByte }).Concat(this._value).ToArray());

    /// <summary>
    /// Get the address as a length-32 byte array without the version byte prepended.
    /// </summary>
    public byte[] GetBytes() => (byte[])this._value.Clone();

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountAddress"/> class.
    /// </summary>
    /// <param name="addressAsBase58String">The address represented as a length-50 base58 encoded string.</param>
    /// <exception cref="ArgumentException">The input is not a length-50 base58 encoded string.</exception>
    public static AccountAddress From(string addressAsBase58String)
    {
        try
        {
            var decodedData = _encoderInstance.DecodeData(addressAsBase58String).AsSpan();
            var decodedBytes = decodedData[1..].ToArray(); // Remove version byte.
            return From(decodedBytes);
        }
        catch (Exception e) when (e is FormatException or ArgumentException or ArgumentNullException)
        {
            throw new ArgumentException($"'{addressAsBase58String}' is not a length-50 base58 encoded string", e);
        }
    }

    /// <summary>
    /// Creates an instance from a length-32 byte array representing the address (without the version byte prepended).
    /// </summary>
    /// <param name="addressAsBytes">The address as a length-32 byte array, i.e. without the version byte prepended.</param>
    /// <exception cref="ArgumentException">The input is not a length-32 byte array.</exception>
    public static AccountAddress From(byte[] addressAsBytes)
    {
        if (addressAsBytes.Length != BytesLength)
        {
            throw new ArgumentException($"The account address bytes length must be {BytesLength}.");
        }

        return new AccountAddress((byte[])addressAsBytes.Clone());
    }

    /// <summary>
    /// Checks if passed string is a base58 encoded address.
    /// </summary>
    /// <param name="addressAsBase58String">The account address as base58 encoded string.</param>
    /// <returns><c>true</c> if the input could be decoded as a base58 encoded address and <c>false</c> otherwise.</returns>
    public static bool IsValid(string? addressAsBase58String)
    {
        if (addressAsBase58String == null)
        {
            return false;
        }
        try
        {
            _ = _encoderInstance.DecodeData(addressAsBase58String);
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
    public bool IsAliasOf(AccountAddress other) =>
        this._value.Take(29).SequenceEqual(other._value.Take(29));

    /// <summary>
    /// Gets the <c>n</c>th alias of this account address.
    ///
    /// The first 29 bytes of an address identifies a unique account with the
    /// remaining 3 bytes representing aliases of that account. An alias can thus
    /// be thought of as an unsigned integer whose value is between <c>0</c> and
    /// <c>2^24-1</c>. We refer to the address where the last three bytes corresponds
    /// to the value <c>n</c> in big endian format as the <c>n</c>th alias of the account.
    /// </summary>
    /// <param name="n">An unsigned integer representing the <c>n</c>th alias.</param>
    /// <returns name="n">A new account address which is the <c>n</c>th alias of the account.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><c>n</c> is larger than <c>2^24-1</c>.</exception>
    public AccountAddress GetNthAlias(uint n)
    {
        if (n > ((1 << 24) - 1))
        {
            throw new ArgumentOutOfRangeException($"Alias can at most be 16777215, got {n}.");
        }
        // Serialize n to its corresponding alias bytes. Since we output it
        // in big endian format, we truncate the first and most significant byte.
        var aliasBytes = Serialization.GetBytes(n).Skip(1).ToArray();
        var address = this._value.Take(29).ToArray().Concat(aliasBytes).ToArray();
        return From(address);
    }

    /// <summary>
    /// Converts the account address to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="RawClient"/>.
    /// </summary>
    public Grpc.V2.AccountAddress ToProto() =>
        new() { Value = Google.Protobuf.ByteString.CopyFrom(this._value) };

    /// <summary>
    /// Converts the block hash to a corresponding <see cref="Grpc.V2.AccountIdentifierInput"/>
    ///
    /// This can be used as the input for class methods of <see cref="RawClient"/>.
    /// </summary>
    public Grpc.V2.AccountIdentifierInput ToAccountIdentifierInput() =>
        new() { Address = this.ToProto() };

    public bool Equals(AccountAddress other) => this._value.SequenceEqual(other._value);

    public override bool Equals(object? obj) =>
        obj is not null && obj is AccountAddress other && this.Equals(other);

    public override int GetHashCode() => Helpers.HashCode.GetHashCodeByteArray(this._value);

    public static bool operator ==(AccountAddress left, AccountAddress right) => left.Equals(right);

    public static bool operator !=(AccountAddress left, AccountAddress right) => !(left == right);
}
