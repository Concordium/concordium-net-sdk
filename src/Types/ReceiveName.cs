using Concordium.Sdk.Helpers;
using System.Buffers.Binary;
using System.Text;

namespace Concordium.Sdk.Types;

/// <summary>
/// A receive name of the contract function called. Expected format: "&lt;contract_name&gt;.&lt;func_name&gt;".
/// The name must not exceed 100 bytes and all characters are ascii alphanumeric or punctuation.
/// </summary>
public sealed record ReceiveName
{
    private const uint MaxByteLength = 100;

    /// <summary>
    /// Name with format "&lt;contract_name&gt;.&lt;func_name&gt;".
    /// </summary>
    public string Receive { get; init; }

    internal ReceiveName(string receive) => this.Receive = receive;

    internal static ReceiveName From(Grpc.V2.ReceiveName receiveName) => new(receiveName.Value);

    /// <summary>
    /// Try parse input name against expected format.
    /// </summary>
    /// <param name="name">Input receive name.</param>
    /// <param name="output">
    /// If parsing succeeded then ReceiveName will be not null.
    /// If parsing failed Error will be not null with first error seen.</param>
    /// <returns>True if name satisfied expected format.</returns>
    public static bool TryParse(string name, out (ReceiveName? ReceiveName, ValidationError? Error) output)
    {
        var validate = IsValid(name, out var error);
        output = validate ? (new ReceiveName(name), null) : (null, error!);
        return validate;
    }

    /// <summary>
    /// Parse input name against expected format.
    /// </summary>
    /// <param name="name">Input receive name.</param>
    /// <returns>The parsed receive name</returns>
    public static ReceiveName Parse(string name) {
        if (!TryParse(name, out var result)) {
            throw new ArgumentException(ValidationErrorToString(result.Error!.Value));
        }
        return result.ReceiveName!;
    }

    /// <summary>
    /// Get the contract name part of <see cref="Receive"/>.
    /// </summary>
    /// <returns>Contract identification name</returns>
    public ContractIdentifier GetContractName() => new(this.Receive[..this.Receive.IndexOf('.')]);

    /// <summary>
    /// Get entrypoint part of <see cref="Receive"/> which is the entrypoint called on the contract.
    /// </summary>
    /// <returns>Entrypoint</returns>
    public EntryPoint GetEntrypoint() => new(this.Receive[(this.Receive.IndexOf('.') + 1)..]);

    /// <summary>
    /// Validation error of receive name.
    /// </summary>
    public enum ValidationError
    {
        MissingDotSeparator,
        TooLong,
        InvalidCharacters,
    }

    private static string ValidationErrorToString(ValidationError error) {
        return error switch
        {
            ValidationError.MissingDotSeparator => $"Receive name did not include the mandatory '.' character.",
                ValidationError.TooLong => $"The receive name is more than 100 characters.",
                ValidationError.InvalidCharacters => $"The receive name contained invalid characters.",
                };
    }

    private static bool IsValid(string name, out ValidationError? error)
    {
        if (!name.Contains('.'))
        {
            error = ValidationError.MissingDotSeparator;
            return false;
        }
        if (name.Length > MaxByteLength)
        {
            error = ValidationError.TooLong;
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

    /// <summary>
    /// Attempt to deserialize a span of bytes into a smart contract receive name.
    /// </summary>
    /// <param name="bytes">The span of bytes potentially containing a receive name.</param>
    /// <param name="output">Where to write the result of the operation</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (ReceiveName? receiveName, string? Error) output)
    {
        if (bytes.Length < MinSerializedLength)
        {
            var msg = $"Invalid length of input in `ReceiveName.TryDeserial`. Expected at least {MinSerializedLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };
        var sizeRead = BinaryPrimitives.ReadUInt16BigEndian(bytes); // This should never throw, since we already checked the length.
        var size = sizeRead + MinSerializedLength;

        if (size > bytes.Length)
        {
            var msg = $"Invalid length of input in `ReceiveName.TryDeserial`. Expected array of size at least {size}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        try
        {
            var ascii = Encoding.ASCII.GetString(bytes[sizeof(ushort)..sizeRead]);

            if (!TryParse(ascii, out var parseOut)) {
                var error = ValidationErrorToString(parseOut.Error!.Value);
                output = (null, error);
                return false;

            }
            output = (parseOut.ReceiveName, null);
            return true;
        }
        catch (ArgumentException e)
        {
            var msg = $"Invalid ReceiveName in `ReceiveName.TryDeserial`: {e.Message}";
            output = (null, msg);
            return false;
        };
    }

    /// <summary>
    /// Gets the serialized length (number of bytes) of the receive name.
    /// </summary>
    internal uint SerializedLength() => sizeof(ushort) + (uint)this.Receive.Length; // Safe to cast the length since a valid receivename is at most 100.

    /// <summary>
    /// Gets the minimum serialized length (number of bytes) of the receive name.
    /// </summary>
    internal const uint MinSerializedLength = sizeof(ushort);

    /// <summary>
    /// Serialize the smart contract receive name into a byte array which has the length preprended.
    /// </summary>
	public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.SerializedLength());
        memoryStream.Write(Serialization.ToBytes((ushort)this.Receive.Length)); // Safe since a valid receive name must be within 100 ASCII characters.
        var bytes = new byte[this.Receive.Length];
        Encoding.ASCII.GetBytes(this.Receive, bytes);
        memoryStream.Write(bytes);
        return memoryStream.ToArray();
    }

}
