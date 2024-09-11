using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// The target for delegation of stake.
/// </summary>
public abstract record DelegationTarget
{
    internal static DelegationTarget From(Grpc.V2.DelegationTarget stakeDelegatorTarget) =>
        stakeDelegatorTarget.TargetCase switch
        {
            Grpc.V2.DelegationTarget.TargetOneofCase.Passive =>
                new PassiveDelegationTarget(),
            Grpc.V2.DelegationTarget.TargetOneofCase.Baker =>
                new BakerDelegationTarget(new BakerId(new AccountIndex(stakeDelegatorTarget.Baker.Value))),
            Grpc.V2.DelegationTarget.TargetOneofCase.None =>
                throw new MissingEnumException<Grpc.V2.DelegationTarget.TargetOneofCase>(
                    stakeDelegatorTarget.TargetCase),
            _ => throw new MissingEnumException<Grpc.V2.DelegationTarget.TargetOneofCase>(stakeDelegatorTarget
                .TargetCase)
        };
}

/// <summary>
/// Delegate passively, i.e., to no specific baker.
/// </summary>
public sealed record PassiveDelegationTarget : DelegationTarget;

/// <summary>
/// Delegate to a specific baker.
/// </summary>
public sealed record BakerDelegationTarget(BakerId BakerId) : DelegationTarget;
