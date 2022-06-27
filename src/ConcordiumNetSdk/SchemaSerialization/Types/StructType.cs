namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a struct type using in smart contract parameter schema.
/// </summary>
/// <param name="Fields">the struct fields.</param>
public record StructType(Fields Fields) : Type(ParameterType.Struct);
