namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a fields enum using in smart contract parameter schema.
/// </summary>
public enum FieldsTag
{
    /// <summary>
    /// Represents named fields.
    /// </summary>
    Named = 0,

    /// <summary>
    /// Represents unnamed (anonymous) struct fields
    /// </summary>
    Unnamed,

    /// <summary>
    /// Represents lack of fields in a struct or an enum.
    /// </summary>
    None
}
