using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Interop;

namespace Concordium.Sdk.Types;

/// <summary>
/// Module schema embedded within <see cref="VersionedModuleSource"/>.
/// </summary>
/// <param name="Schema">Module schema</param>
/// <param name="Version">Module schema version</param>
public sealed record VersionedModuleSchema(byte[] Schema, ModuleSchemaVersion Version)
{
    /// <summary>
    /// Constructor which converts <paramref name="schema"/> into hexadecimal string.
    /// </summary>
    /// <param name="schema">Module schema given as an hexadecimal string.</param>
    /// <param name="version">Module schema version.</param>
    public static VersionedModuleSchema Create(string schema, ModuleSchemaVersion version) => new(Convert.FromHexString(schema), version);

    /// <summary>
    /// Deserialize schema.
    /// </summary>
    /// <returns>Schema as json uft8 encoded.</returns>
    /// <exception cref="SchemaJsonException">Thrown when schema wasn't able to be deserialized.</exception>
    public Utf8Json GetDeserializedSchema() => InteropBinding.SchemaDisplay(this);
};
