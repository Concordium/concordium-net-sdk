namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a module that consists of different contracts and their function parameter schemas.
/// </summary>
/// <param name="ContractSchemas">the contract schemas where key is contract name and value is contract.</param>
public record Module(Dictionary<string, Contract> ContractSchemas);
