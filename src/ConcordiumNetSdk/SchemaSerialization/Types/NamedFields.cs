namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a named fields type using in smart contract parameter schema.
/// </summary>
/// <param name="Contents">the contents consisting of field name and field type.</param>
public record NamedFields((string FieldName, Type FieldType)[] Contents) : Fields(FieldsTag.Named);
