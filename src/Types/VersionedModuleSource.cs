using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Contains source code of a versioned module where inherited classes are concrete versions.
/// </summary>
public abstract record VersionedModuleSource(byte[] Source) {
    internal const uint MaxLength = 8 * 65536;
    internal uint BytesLength = 2 * sizeof(int) + (uint) Source.Length;

    internal abstract uint GetVersion();

    internal byte[] ToBytes() {
        using var memoryStream = new MemoryStream((int)(BytesLength));
        memoryStream.Write(Serialization.ToBytes(GetVersion()));
        memoryStream.Write(Serialization.ToBytes((uint) Source.Length));
        memoryStream.Write(Source);
        return memoryStream.ToArray();
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

    internal static bool TryDeserialize(byte[] bytes, out (VersionedModuleSource? ContractName, DeserialErr? Error) output) {
        (uint?, DeserialErr?) version = (null, null);
        var versionSuccess = Deserial.TryDeserialU32(bytes, 0, out version);

        if (!versionSuccess) {
            output = (null, version.Item2);
            return false;
        }
        if (bytes.Length < 8) {
            output = (null, DeserialErr.TooShort);
            return false;
        }

        var rest = bytes.Skip(8).ToArray();

        if (version.Item1 == 0) {
            output = (ModuleV0.From(rest), null);
            return true;
        } else if (version.Item1== 1) {
            output = (ModuleV0.From(rest), null);
            return true;
        } else {
            output = (null, DeserialErr.InvalidModuleVersion);
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
    override internal uint GetVersion() => 0;

    internal static ModuleV0 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV0 moduleSourceV0) =>
        new(moduleSourceV0.Value.ToByteArray());

    /// <summary>
    /// Creates a WASM-module from byte array.
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

        return new ModuleV0(source.ToArray());
    }

    /// <summary>
    /// Creates an instance from a hex encoded string.
    /// </summary>
    /// <param name="hexString">The WASM-module represented as a hex encoded string representing at most "MaxLength" bytes.</param>
    /// <exception cref="ArgumentException">The supplied string is not a hex encoded WASM-module representing at most "MaxLength" bytes.</exception>
    public static ModuleV0 FromHex(string hexString)
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
}

/// <summary>
/// Version 1 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV1(byte[] Source) : VersionedModuleSource(Source)
{
    override internal uint GetVersion() => 1;

    internal static ModuleV1 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV1 moduleSourceV1) =>
        new(moduleSourceV1.Value.ToByteArray());

    /// <summary>
    /// Creates a WASM-module from byte array.
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

        return new ModuleV1(source.ToArray());
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
}

/// <summary>
/// Thrown when a matched enum value could not be handled in a switch statement.
/// </summary>
public sealed class InvalidModuleVersion : Exception
{
    internal InvalidModuleVersion(uint versionByte) :
        base($"Unknown version byte: {versionByte}")
    { }
}
