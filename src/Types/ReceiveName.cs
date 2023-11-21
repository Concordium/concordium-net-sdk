using Concordium.Sdk.Helpers;

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
}
