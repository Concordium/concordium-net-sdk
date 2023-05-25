namespace Concordium.Sdk.Types;

/// <summary>
/// A contract name. Expected format: "init_<contract_name>".
/// </summary>
public record struct ContractName(string Contract);
