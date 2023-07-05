namespace Concordium.Sdk.Types;

/// <summary>
/// A receive name of the contract function called. Expected format: "&lt;contract_name&gt;.&lt;func_name&gt;".
/// The name must not exceed 100 bytes and all characters are ascii alphanumeric or punctuation.
/// </summary>
public sealed record ReceiveName(string Receive);
