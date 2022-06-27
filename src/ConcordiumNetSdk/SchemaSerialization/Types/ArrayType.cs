namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a array type using in smart contract parameter schema.
/// </summary>
/// <param name="Size">the array size.</param>
/// <param name="ValueType">the array value type.</param>
public record ArrayType(uint Size, Type ValueType) : Type(ParameterType.Array);
