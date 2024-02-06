using System.Buffers.Binary;
using System.Text;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// The init name of a smart contract function. Expected format:
/// "init_&lt;contract_name&gt;". It must only consist of at most 100 ASCII
/// alphanumeric or punctuation characters, must not contain a '.' and must
/// start with 'init_'.
/// </summary>
public sealed record ContractName
{
    private const uint MaxByteLength = 100;

    /// <summary>
    /// A contract name with format: "init_&lt;contract_name&gt;".
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the minimum serialized length (number of bytes) of the init name.
    /// </summary>
    internal const uint MinSerializedLength = sizeof(ushort);

    /// <summary>
    /// Gets the serialized length (number of bytes) of the init name.
    /// </summary>
    internal uint SerializedLength() => sizeof(ushort) + (uint)this.Name.Length;

    private ContractName(string name) => this.Name = name;

    internal static ContractName From(Grpc.V2.InitName initName) => new(initName.Value);

    /// <summary>
    /// Try parse input name against expected format.
    /// </summary>
    /// <param name="name">Input init name.</param>
    /// <param name="output">
    /// If parsing succeeded then ContractName will be not null.
    /// If parsing failed Error will be not null with first error seen.</param>
    /// <returns>True if name satisfied expected format.</returns>
    public static bool TryParse(string name, out (ContractName? ContractName, ValidationError? Error) output)
    {
        var validate = IsValid(name, out var error);
        output = validate ? (new ContractName(name), null) : (null, error!);
        return validate;
    }

    /// <summary>
    /// Copies the init name to a byte array which has the length preprended.
    /// </summary>
	public byte[] ToBytes()
    {
        var bytes = Encoding.ASCII.GetBytes(this.Name);

        using var memoryStream = new MemoryStream((int)this.SerializedLength());
        memoryStream.Write(Serialization.ToBytes((ushort)bytes.Length));
        memoryStream.Write(bytes);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Deserialize an init name from a serialized byte array.
    /// </summary>
    /// <param name="bytes">The serialized init name.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (ContractName? ContractName, string? Error) output)
    {
        if (bytes.Length < MinSerializedLength)
        {
            var msg = $"Invalid length of input in `InitName.TryDeserial`. Expected at least {MinSerializedLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        var sizeRead = BinaryPrimitives.ReadUInt16BigEndian(bytes);
        var size = sizeRead + sizeof(ushort);
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

            var correctlyParsed = TryParse(ascii, out var parseOut);
            output = correctlyParsed ? (parseOut.ContractName, null) : (null, "Error parsing contract name (" + ascii + "): " + parseOut.Error.ToString());
            return correctlyParsed;
        }
        catch (ArgumentException e)
        {
            var msg = $"Invalid InitName in `InitName.TryDeserial`: {e.Message}";
            output = (null, msg);
            return false;
        };
    }

    /// <summary>
    /// Get the contract name part of <see cref="Name"/>.
    /// </summary>
    /// <returns>Contract identification name</returns>
    public ContractIdentifier GetContractName() => new(this.Name[(this.Name.IndexOf('_') + 1)..]);

    /// <summary>
    /// Validation error of contract name.
    /// </summary>
    public enum ValidationError
    {
        MissingInitPrefix,
        TooLong,
        ContainsDot,
        InvalidCharacters,
    }

    private static bool IsValid(string name, out ValidationError? error)
    {
        if (!name.StartsWith("init_", StringComparison.Ordinal))
        {
            error = ValidationError.MissingInitPrefix;
            return false;
        }
        if (name.Length > MaxByteLength)
        {
            error = ValidationError.TooLong;
            return false;
        }
        if (name.Contains('.'))
        {
            error = ValidationError.ContainsDot;
            return false;
        }
        if (!name.All(c => AsciiHelpers.IsAsciiAlphaNumeric(c) || AsciiHelpers.IsAsciiPunctuation(c)))
        {
            error = ValidationError.InvalidCharacters;
            return false;
        }

        error = null;
        return true;
    }

    /// <summary>Check for equality.</summary>
    public bool Equals(ContractName? other) => other != null && this.Name == other.Name;

    /// <summary>Gets hash code.</summary>
    public override int GetHashCode() => this.Name.GetHashCode();
}
