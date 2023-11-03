namespace Concordium.Sdk.Types;

/// <summary>
/// Module schema embedded within <see cref="VersionedModuleSource"/>.
/// </summary>
public sealed record VersionedModuleSchema
{
    /// <summary>
    /// Module schema in hexadecimal.
    /// </summary>
    public string Schema { get; }
    /// <summary>
    /// Module schema version
    /// </summary>
    public ModuleSchemaVersion Version { get; }

    /// <summary>
    /// Constructor which converts <see cref="schema"/> into hexadecimal string.
    /// </summary>
    /// <param name="schema">Module schema in bytes.</param>
    /// <param name="version">Module schema version.</param>
    public VersionedModuleSchema(byte[] schema, ModuleSchemaVersion version)
    {
        this.Schema = Convert.ToHexString(schema).ToLowerInvariant();
        this.Version = version;
    }

    /// <summary>
    /// Constructor which converts <see cref="schema"/> into hexadecimal string.
    /// </summary>
    /// <param name="schema">Module schema given as an hexadecimal string.</param>
    /// <param name="version">Module schema version.</param>
    public VersionedModuleSchema(string schema, ModuleSchemaVersion version)
    {
        this.Schema = schema;
        this.Version = version;
    }
};
