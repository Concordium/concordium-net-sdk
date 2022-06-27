namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a unnamed fields type using in smart contract parameter schema.
/// </summary>
/// <param name="Contents">the contents consisting of the field value types.</param>
public record UnnamedFields(Type[] Contents) : Fields(FieldsTag.Unnamed);
