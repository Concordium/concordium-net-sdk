namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a map type using in smart contract parameter schema.
/// </summary>
/// <param name="SizeLength">the map size length.</param>
/// <param name="KeyType">the map key type.</param>
/// <param name="ValueType">the map value type.</param>
public record MapType(SizeLength SizeLength, Type KeyType, Type ValueType) : Type(ParameterType.Map);
