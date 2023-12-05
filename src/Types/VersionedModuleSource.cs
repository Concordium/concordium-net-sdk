using System.Buffers.Binary;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Contains source code of a versioned module where inherited classes are concrete versions.
/// </summary>
public abstract record VersionedModuleSource(byte[] Source) : IEquatable<VersionedModuleSource>
{
    internal const uint MaxLength = 8 * 65536;

    internal abstract uint GetVersion();

    internal uint GetBytesLength() => (uint)((2 * sizeof(int)) + this.Source.Length);

    internal byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)this.GetBytesLength());
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

    /// <summary>
    /// Create a versioned module schema from a byte array.
    /// </summary>
    /// <param name="bytes">The serialized schema.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (VersionedModuleSource? VersionedModuleSource, string? Error) output)
    {
        if (bytes.Length < 2 * sizeof(int))
        {
            output = (null, $"The given byte array in `VersionModuleSourceFactory.TryDeserial`, is too short. Must be longer than {2 * sizeof(int)}.");
            return false;
        }

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
    internal override uint GetVersion() => 1;

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
