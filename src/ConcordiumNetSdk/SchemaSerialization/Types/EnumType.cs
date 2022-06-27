namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a enum type using in smart contract parameter schema.
/// </summary>
/// <param name="Variants">the enum variants consisting of variant name and variant fields.</param>
public record EnumType((string VariantName, Fields VariantFields)[] Variants) : Type(ParameterType.Enum);
