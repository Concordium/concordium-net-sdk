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
}
