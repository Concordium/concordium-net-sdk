namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a option tag enum using in smart contract parameter schema.
/// </summary>
public enum OptionTag : byte
{
    /// <summary>
    /// Means that there's no value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Means that there's a value.
    /// </summary>
    Some,
}
