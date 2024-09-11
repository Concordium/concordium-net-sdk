using System.Buffers.Binary;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;
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

    internal const uint MaxLength = 8 * 65536;

    internal abstract uint GetVersion();

    /// <summary>
    /// Gets the length (number of bytes) of the serialized Module.
    /// </summary>
    internal uint SerializedLength() => (uint)((2 * sizeof(int)) + this.Source.Length);

    internal const uint MinSerializedLength = 2 * sizeof(int);

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

    internal byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.SerializedLength());
        memoryStream.Write(Serialization.ToBytes(this.GetVersion()));
        memoryStream.Write(Serialization.ToBytes((uint)this.Source.Length));
        memoryStream.Write(this.Source);
        return memoryStream.ToArray();
    }

    /// <summary>Check for equality.</summary>
    public virtual bool Equals(VersionedModuleSource? other) => other != null &&
               other.GetType().Equals(this.GetType()) &&
               this.Source.SequenceEqual(other.Source);

    /// <summary>Gets hash code.</summary>
    public override int GetHashCode()
    {
        var sourceHash = Helpers.HashCode.GetHashCodeByteArray(this.Source);
        return sourceHash + (int)this.GetVersion();
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
    /// From custom sections in <paramref name="module"/> get entry with name <paramref name="entryKey"/>.
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
            .SingleOrDefault(section => section.Name.Equals(entryKey, StringComparison.Ordinal));

        if (customSection == null)
        {
            return false;
        }

        schema = customSection.Content.ToArray();
        return true;
    }
}

/// <summary>
/// Crates a VersiondModuleSource.
/// </summary>
public static class VersionedModuleSourceFactory
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

    /// <summary>
    /// Create a cref="VersionedModuleSource" from a versioned WASM file.
    /// </summary>
    /// <param name="modulePath">The path to the versioned WASM file.</param>
    /// <exception cref="DeserialException">The provided WASM module was unable to be parsed.</exception>
    public static VersionedModuleSource FromFile(string modulePath)
    {
        var bytes = File.ReadAllBytes(modulePath);
        if (TryDeserial(bytes, out var versionedModule))
        {
            return versionedModule.VersionedModuleSource!;
        }
        else
        {
            throw new DeserialException(versionedModule.Error!);
        }
    }

    /// <summary>
    /// Create a <see cref="VersionedModuleSource" /> from a byte array.
    /// </summary>
    /// <param name="bytes">Span of bytes with the serialized smart contract module</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (VersionedModuleSource? VersionedModuleSource, string? Error) output)
    {
        if (bytes.Length < VersionedModuleSource.MinSerializedLength)
        {
            output = (null, $"The given byte array in `VersionModuleSourceFactory.TryDeserial`, is too short. Must be longer than {VersionedModuleSource.MinSerializedLength}.");
            return false;
        }

        // The functions below would throw if it were not for the above check.
        var version = BinaryPrimitives.ReadUInt32BigEndian(bytes);
        var length = BinaryPrimitives.ReadUInt32BigEndian(bytes[sizeof(int)..]);

        var rest = bytes.Slice(2 * sizeof(int), (int)length).ToArray();

        if (version == 0)
        {
            output = (ModuleV0.From(rest), null);
            return true;
        }
        else if (version == 1)
        {
            output = (ModuleV1.From(rest), null);
            return true;
        }
        else
        {
            output = (null, $"Invalid module version byte, expected 0 or 1 but found {version}");
            return false;
        };
    }
}

/// <summary>
/// Version 0 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV0(byte[] Source) : VersionedModuleSource(Source)
{
    internal override uint GetVersion() => 0;

    internal static ModuleV0 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV0 moduleSourceV0) =>
        new(moduleSourceV0.Value.ToByteArray());

    /// <summary>
    /// Creates a WASM-module from byte array.
    /// Note: Does not copy the given byte array, so it assumes that the underlying
    /// byte array is not mutated
    /// </summary>
    /// <param name="source">WASM-module as a byte array.</param>
    /// <exception cref="ArgumentException">The length of the supplied module exceeds "MaxLength".</exception>
    public static ModuleV0 From(byte[] source)
    {
        if (source.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Size of a data is not allowed to exceed {MaxLength} bytes."
            );
        }

        return new ModuleV0(source);
    }

    /// <summary>
    /// Creates an instance from a hex encoded string.
    /// </summary>
    /// <param name="hexString">The WASM-module represented as a hex encoded string representing at most "MaxLength" bytes.</param>
    /// <exception cref="ArgumentException">The supplied string is not a hex encoded WASM-module representing at most "MaxLength" bytes.</exception>
    public static ModuleV0 FromHex(string hexString)
    {
        var value = Convert.FromHexString(hexString);
        return From(value);
    }

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
    internal override uint GetVersion() => 1;

    internal static ModuleV1 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV1 moduleSourceV1) =>
        new(moduleSourceV1.Value.ToByteArray());

    /// <summary>
    /// Creates a WASM-module from byte array.
    /// Note: Does not copy the given byte array, so it assumes that the underlying
    /// byte array is not mutated
    /// </summary>
    /// <param name="source">WASM-module as a byte array.</param>
    /// <exception cref="ArgumentException">The length of the supplied module exceeds "MaxLength".</exception>
    public static ModuleV1 From(byte[] source)
    {
        if (source.Length > MaxLength)
        {
            throw new ArgumentException(
                $"Size of a data is not allowed to exceed {MaxLength} bytes."
            );
        }

        return new ModuleV1(source);
    }

    /// <summary>
    /// Creates an instance from a hex encoded string.
    /// </summary>
    /// <param name="hexString">The WASM-module represented as a hex encoded string representing at most "MaxLength" bytes.</param>
    /// <exception cref="ArgumentException">The supplied string is not a hex encoded WASM-module representing at most "MaxLength" bytes.</exception>
    public static ModuleV1 FromHex(string hexString)
    {
        try
        {
            var value = Convert.FromHexString(hexString);
            return From(value);
        }
        catch (Exception e)
        {
            throw new ArgumentException("The provided string is not hex encoded: ", e);
        }
    }

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
