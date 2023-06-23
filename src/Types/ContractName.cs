namespace Concordium.Sdk.Types;

/// <summary>
/// A contract name. Expected format: "init_&lt;contract_name&gt;".
/// </summary>
public sealed record ContractName(string Contract);
