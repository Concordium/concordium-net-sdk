using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Information about an existing smart contract instance.
/// </summary>
public interface IInstanceInfo { }

internal static class InstanceInfoFactory
{
    internal static IInstanceInfo From(Grpc.V2.InstanceInfo instanceInfo) =>
        instanceInfo.VersionCase switch
        {
            Grpc.V2.InstanceInfo.VersionOneofCase.V0 => InstanceInfoV0.From(instanceInfo.V0),
            Grpc.V2.InstanceInfo.VersionOneofCase.V1 => InstanceInfoV1.From(instanceInfo.V1),
            Grpc.V2.InstanceInfo.VersionOneofCase.None => throw new MissingEnumException<Grpc.V2.InstanceInfo.VersionOneofCase>(instanceInfo.VersionCase),
            _ => throw new MissingEnumException<Grpc.V2.InstanceInfo.VersionOneofCase>(instanceInfo.VersionCase)
        };
}

/// <summary>
/// Version 0 smart contract instance information.
/// </summary>
/// <param name="Model">The state of the instance.</param>
/// <param name="Owner">The account address which deployed the instance.</param>
/// <param name="Amount">The amount of CCD tokens in the balance of the instance.</param>
/// <param name="Methods">A list of endpoints exposed by the instance.</param>
/// <param name="Name">The name of the smart contract of the instance.</param>
/// <param name="SourceModule">The module reference for the smart contract module of the instance.</param>
public sealed record InstanceInfoV0(
    byte[] Model,
    AccountAddress Owner,
    CcdAmount Amount,
    IList<ReceiveName> Methods,
    ContractName Name,
    ModuleReference SourceModule
    ) : IInstanceInfo
{
    internal static InstanceInfoV0 From(Grpc.V2.InstanceInfo.Types.V0 instanceInfo) =>
        new(
            instanceInfo.Model.Value.ToByteArray(),
            AccountAddress.From(instanceInfo.Owner),
            CcdAmount.From(instanceInfo.Amount),
            instanceInfo.Methods.Select(ReceiveName.From).ToList(),
            ContractName.From(instanceInfo.Name),
            ModuleReference.From(instanceInfo.SourceModule)
        );
}

/// <summary>
/// Version 1 smart contract instance information.
/// </summary>
/// <param name="Owner">The account address which deployed the instance.</param>
/// <param name="Amount">The amount of CCD tokens in the balance of the instance.</param>
/// <param name="Methods">A list of endpoints exposed by the instance.</param>
/// <param name="Name">The name of the smart contract of the instance.</param>
/// <param name="SourceModule">The module reference for the smart contract module of the instance.</param>
public sealed record InstanceInfoV1(
    AccountAddress Owner,
    CcdAmount Amount,
    IList<ReceiveName> Methods,
    ContractName Name,
    ModuleReference SourceModule
    ) : IInstanceInfo
{
    internal static InstanceInfoV1 From(Grpc.V2.InstanceInfo.Types.V1 instanceInfo) =>
        new(
            AccountAddress.From(instanceInfo.Owner),
            CcdAmount.From(instanceInfo.Amount),
            instanceInfo.Methods.Select(ReceiveName.From).ToList(),
            ContractName.From(instanceInfo.Name),
            ModuleReference.From(instanceInfo.SourceModule)
        );
}
