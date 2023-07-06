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

    /// <summary>
    /// Try parse input name against expected format.
    /// </summary>
    /// <param name="name">Input receive name.</param>
    /// <param name="receiveName">Parsed receive name when succeeded.</param>
    /// <returns>True if name satisfied expected format.</returns>
    public static bool TryParse(string name, out ReceiveName? receiveName)
    {
        var validate = IsValid(name);
        receiveName = validate ? new ReceiveName(name) : null;
        return validate;
    }

    private static bool IsValid(string name)
    {
        if (!name.Contains('.'))
        {
            return false;
        }
        if (name.Length > MaxByteLength)
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
