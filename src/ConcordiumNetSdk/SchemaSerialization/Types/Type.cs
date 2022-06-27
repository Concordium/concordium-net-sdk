namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a base type using in smart contract parameter schema.
/// </summary>
/// <param name="TypeTag">the parameter type tag.</param>
public record Type(ParameterType TypeTag);
