using Concordium.Sdk.Exceptions;
using WebAssembly;

namespace Concordium.Sdk.Types;

/// <summary>
/// Contains source code of a versioned module where inherited classes are concrete versions.
/// </summary>
public abstract record VersionedModuleSource
{
    /// <summary>
    /// Module source code.
    /// </summary>
    public byte[] Source { get; }
    private readonly Lazy<Module> _module;

    /// <summary>
    /// Base constructor
    /// </summary>
    /// <param name="source">Module source code.</param>
    protected VersionedModuleSource(byte[] source)
    {
        this.Source = source;
        this._module = new Lazy<Module>(this.GetWasmModule);
    }

    /// <summary>
    /// Get possible module schema embedded in module source code.
    /// </summary>
    /// <returns>Possible module schema and module schema version</returns>
    public VersionedModuleSchema? GetModuleSchema()
    {
        var result = this.ExtractSchemaFromWebAssemblyModule(this._module.Value);
        return result == null ? null : new VersionedModuleSchema(result.Value.Schema!, result.Value.SchemaVersion);
    }

    private Module GetWasmModule()
    {
        using var stream = new MemoryStream(this.Source);
        var moduleWasm = Module.ReadFromBinary(stream);
        return moduleWasm;
    }

    /// <summary>
    /// The module can contain a schema in one of two different custom sections.
    /// The supported sections depend on the module version.
    /// The schema version can be either defined by the section name or embedded into the actual schema:
    /// - Both v0 and v1 modules support the section 'concordium-schema' where the schema includes the version.
    ///   - For v0 modules this is always a v0 schema.
    ///   - For v1 modules this can be a v1, v2, or v3 schema.
    ///- V0 modules additionally support section 'concordium-schema-v1' which always contain a v0 schema (not a typo).
    /// - V1 modules additionally support section 'concordium-schema-v2' which always contain a v1 schema (not a typo).
    /// The section 'concordium-schema' is the most common and is what the current tooling produces.
    /// </summary>
    private protected abstract (byte[]? Schema, ModuleSchemaVersion SchemaVersion)?
        ExtractSchemaFromWebAssemblyModule(Module module);

    /// <summary>
    /// From custom sections in <see cref="module"/> get entry with name <see cref="entryKey"/>.
    ///
    /// Fails if multiple entries exist with the same name.
    /// </summary>
    /// <param name="module">Web assembly module</param>
    /// <param name="entryKey">Name which is search for in custom sections.</param>
    /// <param name="schema">Possible schema if exist in custom sections.</param>
    /// <returns>True if schema was embedded in the custom section.</returns>
    protected static bool GetSchemaFromWasmCustomSection(Module module, string entryKey, out byte[]? schema)
    {
        schema = null;
        var customSection = module.CustomSections
            .SingleOrDefault(section => section.Name.Equals(entryKey, StringComparison.InvariantCulture));

        if (customSection == null)
        {
            return false;
        }

        schema = customSection.Content.ToArray();
        return true;
    }
}

internal static class VersionedModuleSourceFactory
{
    internal static VersionedModuleSource From(Grpc.V2.VersionedModuleSource versionedModuleSource) =>
        versionedModuleSource.ModuleCase switch
        {
            Grpc.V2.VersionedModuleSource.ModuleOneofCase.V0 => ModuleV0.From(versionedModuleSource.V0),
            Grpc.V2.VersionedModuleSource.ModuleOneofCase.V1 => ModuleV1.From(versionedModuleSource.V1),
            Grpc.V2.VersionedModuleSource.ModuleOneofCase.None => throw new
                MissingEnumException<Grpc.V2.VersionedModuleSource.ModuleOneofCase>(versionedModuleSource.ModuleCase),
            _ => throw new MissingEnumException<Grpc.V2.VersionedModuleSource.ModuleOneofCase>(versionedModuleSource
                .ModuleCase)
        };
}

/// <summary>
/// Version 0 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV0(byte[] Source) : VersionedModuleSource(Source)
{
    internal static ModuleV0 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV0 moduleSourceV0) =>
        new(moduleSourceV0.Value.ToByteArray());

    private protected override (byte[]? Schema, ModuleSchemaVersion SchemaVersion)? ExtractSchemaFromWebAssemblyModule(Module module)
    {
        if (GetSchemaFromWasmCustomSection(module, "concordium-schema", out var moduleV0SchemaUndefined))
        {
            return (moduleV0SchemaUndefined!, ModuleSchemaVersion.Undefined); // always v0
        }
        if (GetSchemaFromWasmCustomSection(module, "concordium-schema-v1", out var moduleV0SchemaV0))
        {
            return (moduleV0SchemaV0!, ModuleSchemaVersion.V0); // v0 (not a typo)
        }
        return null;
    }
}

/// <summary>
/// Version 1 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV1(byte[] Source) : VersionedModuleSource(Source)
{
    internal static ModuleV1 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV1 moduleSourceV1) =>
        new(moduleSourceV1.Value.ToByteArray());

    private protected override (byte[]? Schema, ModuleSchemaVersion SchemaVersion)? ExtractSchemaFromWebAssemblyModule(Module module)
    {
        if (GetSchemaFromWasmCustomSection(module, "concordium-schema", out var moduleV1SchemaUndefined))
        {
            return (moduleV1SchemaUndefined!, ModuleSchemaVersion.Undefined); // v1, v2, or v3
        }
        if (GetSchemaFromWasmCustomSection(module, "concordium-schema-v2", out var moduleV1SchemaV1))
        {
            return (moduleV1SchemaV1!, ModuleSchemaVersion.V1); // v1 (not a typo)
        }
        return null;
    }
}
