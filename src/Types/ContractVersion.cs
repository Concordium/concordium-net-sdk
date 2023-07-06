using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Contract version.
/// </summary>
public enum ContractVersion
{
    V0 = 0,
    V1 = 1
}

internal static class ContractVersionFactory
{
    internal static ContractVersion Into(this Grpc.V2.ContractVersion contractVersion) =>
        contractVersion switch
        {
            Grpc.V2.ContractVersion.V0 => ContractVersion.V0,
            Grpc.V2.ContractVersion.V1 => ContractVersion.V1,
            _ => throw new MissingEnumException<Grpc.V2.ContractVersion>(contractVersion)
        };
}
