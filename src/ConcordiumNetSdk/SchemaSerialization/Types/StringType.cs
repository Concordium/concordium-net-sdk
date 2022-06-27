namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a string type using in smart contract parameter schema.
/// </summary>
/// <param name="SizeLength">the string size length.</param>
/// <param name="TypeTag">the parameter type tag.</param>
public record StringType(SizeLength SizeLength, ParameterType TypeTag = ParameterType.String) : Type(TypeTag);
