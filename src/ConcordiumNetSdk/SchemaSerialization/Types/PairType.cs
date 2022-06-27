namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a pair type using in smart contract parameter schema.
/// </summary>
/// <param name="LeftType">the pair left type.</param>
/// <param name="RightType">the pair right type.</param>
public record PairType(Type LeftType, Type RightType) : Type(ParameterType.Pair);
