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
    /// <param name="initName">Parsed init name when succeeded.</param>
    /// <returns>True if name satisfied expected format.</returns>
    public static bool TryParse(string name, out ContractName? initName)
    {
        var validate = IsValid(name);
        initName = validate ? new ContractName(name) : null;
        return validate;
    }

    private static bool IsValid(string name)
    {
        if (!name.StartsWith("init_", StringComparison.Ordinal))
        {
            return false;
        }
        if (name.Length > MaxByteLength)
        {
            return false;
        }
        if (name.Contains('.'))
        {
            return false;
        }
        if (!name.All(c => AsciiHelpers.IsAsciiAlphaNumeric(c) || AsciiHelpers.IsAsciiPunctuation(c)))
        {
            return false;
        }

        return true;
    }
}
