using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Contains source code of a versioned module where inherited classes are concrete versions.
/// </summary>
public interface IVersionedModuleSource
{
    /// <summary>
    /// Returns underlying bytes as span.
    /// </summary>
    /// <returns></returns>
    Span<byte> AsSpan();
}

internal static class VersionedModuleSourceFactory
{
    internal static IVersionedModuleSource From(VersionedModuleSource versionedModuleSource) =>
        versionedModuleSource.ModuleCase switch
        {
            VersionedModuleSource.ModuleOneofCase.V0 => ModuleV0.From(versionedModuleSource.V0),
            VersionedModuleSource.ModuleOneofCase.V1 => ModuleV1.From(versionedModuleSource.V1),
            VersionedModuleSource.ModuleOneofCase.None => throw new
                MissingEnumException<VersionedModuleSource.ModuleOneofCase>(versionedModuleSource.ModuleCase),
            _ => throw new MissingEnumException<VersionedModuleSource.ModuleOneofCase>(versionedModuleSource
                .ModuleCase)
        };
}

/// <summary>
/// Version 0 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV0(byte[] Source) : IVersionedModuleSource
{
    internal static ModuleV0 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV0 moduleSourceV0) =>
        new(moduleSourceV0.Value.ToByteArray());

    /// <inheritdoc />
    public Span<byte> AsSpan() => this.Source.AsSpan();
}

/// <summary>
/// Version 1 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV1(byte[] Source) : IVersionedModuleSource
{
    internal static ModuleV1 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV1 moduleSourceV1) =>
        new(moduleSourceV1.Value.ToByteArray());

    /// <inheritdoc />
    public Span<byte> AsSpan() => this.Source.AsSpan();
}
