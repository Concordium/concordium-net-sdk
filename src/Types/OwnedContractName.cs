namespace Concordium.Sdk.Types;

/// <summary>
/// A contract name (owned version). Expected format: "init_<contract_name>".
/// </summary>
public record struct OwnedContractName(string OwnedContract);