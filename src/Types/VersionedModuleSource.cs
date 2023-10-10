using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Contains source code of a versioned module where inherited classes are concrete versions.
/// </summary>
public abstract record VersionedModuleSource(byte[] Source);

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
}

/// <summary>
/// Version 1 module source.
/// </summary>
/// <param name="Source">Source code of module</param>
public sealed record ModuleV1(byte[] Source) : VersionedModuleSource(Source)
{
    internal static ModuleV1 From(Grpc.V2.VersionedModuleSource.Types.ModuleSourceV1 moduleSourceV1) =>
        new(moduleSourceV1.Value.ToByteArray());
}
