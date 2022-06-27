namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a list type using in smart contract parameter schema.
/// </summary>
/// <param name="SizeLength">the list size length.</param>
/// <param name="ValueType">the list value type.</param>
/// <param name="TypeTag">the parameter type tag.</param>
public record ListType(SizeLength SizeLength, Type ValueType, ParameterType TypeTag) : Type(TypeTag);
