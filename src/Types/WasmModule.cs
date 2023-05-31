using System.Collections.Immutable;


namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a versioned Wasm smart contract module.
/// </summary>
public class WasmModule
{
    /// <summary>
    /// Maximum length, in bytes, of a V0 Wasm smart contract module source.
    /// </summary>
    public const long MaxLengthV0 = 65536; // 64 kb

    /// <summary>
    /// Maximum length, in bytes, of a V1 Wasm smart contract module source.
    /// </summary>
    public const long MaxLengthV1 = 8 * 65536; // 512 kb

    /// <summary>
    /// Byte representation of the Wasm smart contract module source.
    /// </summary>
    public ImmutableArray<byte> Bytes { get; init; }

    /// <summary>
    /// Version of the Wasm smart contract module represented by <see cref="Bytes"/>.
    /// </summary>
    public WasmModuleVersion Version { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WasmModule"/> class.
    /// </summary>
    /// <param name="source">Byte representation of the Wasm smart contract module source.</param>
    /// <param name="version">Version of the Wasm smart contract module represented by <paramref name="source"/>.</param>
    /// <exception cref="ArgumentException">Contract module source exceeded the maximum length.</exception>
    private WasmModule(byte[] source, WasmModuleVersion version)
    {
        switch (version)
        {
            case WasmModuleVersion.V0:
                if (source.Length > MaxLengthV0)
                {
                    throw new ArgumentException($"Wasm smart contract V0 modules can be at most {MaxLengthV0} bytes.");
                }
                break;
            case WasmModuleVersion.V1:
                if (source.Length > MaxLengthV1)
                {
                    throw new ArgumentException($"Wasm smart contract V1 modules can be at most {MaxLengthV1} bytes.");
                }
                break;
            default:
                throw new ArgumentException($"Unsupported Wasm smart contract module version '{version}'.");
        }
        this.Bytes = ImmutableArray.Create(source);
        this.Version = version;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ModuleSource"/> class.
    /// </summary>
    /// <param name="source">Byte representation of the Wasm smart contract module source.</param>
    /// <param name="version">Version of the Wasm smart contract module represented by <paramref name="source"/>.</param>
    public static WasmModule From(byte[] source, WasmModuleVersion version) => new(source, version);
}
