namespace Concordium.Sdk.Types;

/// <summary>
/// Represents the different module schema versions.
/// </summary>
public enum ModuleSchemaVersion
{
    /// <summary>
    /// Version not known and possibly embedded in schema.
    /// </summary>
    Undefined = -1,
    /// <summary>
    /// Version 0 schema, only supported by V0 smart contracts.
    /// </summary>
    V0 = 0,
    /// <summary>
    /// Version 1 schema, only supported by V1 smart contracts.
    /// </summary>
    V1 = 1,
    /// <summary>
    /// Version 2 schema, only supported by V1 smart contracts.
    /// </summary>
    V2 = 2,
    /// <summary>
    /// Version 3 schema, only supported by V1 smart contracts.
    /// </summary>
    V3 = 3
}
