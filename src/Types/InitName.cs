using System.Buffers.Binary;
using System.Text;
using System.Text.RegularExpressions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;
/// <summary>
/// A contract init name.
/// </summary>
public sealed record InitName : IEquatable<InitName>
{
    /// A contract init name.
    public string Name { get; }

    private readonly byte[] _bytes;

    /// <summary>
    /// Gets the serialized length (number of bytes) of the init name.
    /// </summary>
    internal uint SerializedLength() => sizeof(ushort) + (uint)this._bytes.Length;

    /// <summary>
    /// Gets the minimum serialized length (number of bytes) of the init name.
    /// </summary>
    internal const uint MinSerializedLength = sizeof(ushort);

    /// <summary>
    /// Copies the init name to a byte array which has the length preprended.
    /// </summary>
	public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.SerializedLength());
        memoryStream.Write(Serialization.ToBytes((ushort)this._bytes.Length));
        memoryStream.Write(this._bytes);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Deserialize an init name from a serialized byte array.
    /// </summary>
    /// <param name="bytes">The serialized init name.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (InitName? Name, string? Error) output)
    {
        if (bytes.Length < MinSerializedLength)
        {
            var msg = $"Invalid length of input in `InitName.TryDeserial`. Expected at least {MinSerializedLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        var sizeRead = BinaryPrimitives.ReadUInt16BigEndian(bytes);
        var size = sizeRead + MinSerializedLength;
        if (size > bytes.Length)
        {
            var msg = $"Invalid length of input in `InitName.TryDeserial`. Expected array of size at least {size}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        try
        {
            var initNameBytes = bytes.Slice(sizeof(ushort), sizeRead).ToArray();
            var ascii = Encoding.ASCII.GetString(initNameBytes);
            var initName = new InitName(ascii);
            output = (initName, null);
            return true;
        }
        catch (ArgumentException e)
        {
            var msg = $"Invalid InitName in `InitName.TryDeserial`: {e.Message}";
            output = (null, msg);
            return false;
        };
    }

    /// <summary>
    /// Creates an instance from a string.
    /// </summary>
    /// <param name="name">
    /// The init name of a smart contract function. Expected format:
    /// `init_&lt;contract_name&gt;`. It must only consist of atmost 100 ASCII alphanumeric
    /// or punctuation characters, must not contain a '.' and must start with `init_`.
    /// </param>
    public InitName(string name)
    {
        var containsDot = name.Contains('.');
        var longerThan100 = name.Length > 100;
        var startsWithInit = new Regex(@"^init_").IsMatch(name);
        var alphanumericOrPunctuation = name.All(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c));
        var isAscii = name.All(char.IsAscii);

        if (containsDot)
        {
            throw new ArgumentException($"InitName must not contain a '.'");
        }
        else if (longerThan100)
        {
            throw new ArgumentException($"InitName must not exceed 100 characters.");
        }
        else if (!startsWithInit)
        {
            throw new ArgumentException($"InitName must start with \"init_\"");
        }
        else if (!alphanumericOrPunctuation)
        {
            throw new ArgumentException($"InitName must only contain alphanumeric characters or punctuation characters");
        }
        else if (!isAscii)
        {
            throw new ArgumentException($"InitName must only contain alphanumeric characters or punctuation characters");
        }
        else
        {
            this._bytes = Encoding.ASCII.GetBytes(name);
            this.Name = name;
        }
    }

    /// <summary>Check for equality.</summary>
    public bool Equals(InitName? other) => other != null && this._bytes.SequenceEqual(other._bytes);

    /// <summary>Gets hash code.</summary>
    public override int GetHashCode()
    {
        var paramHash = Helpers.HashCode.GetHashCodeByteArray(this._bytes);
        return paramHash;
    }
}
