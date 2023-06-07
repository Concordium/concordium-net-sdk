using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types.Mapped;

public abstract record DelegationTarget
{
    public static DelegationTarget From(Grpc.V2.DelegationTarget stakeDelegatorTarget) =>
        stakeDelegatorTarget.TargetCase switch
        {
            Grpc.V2.DelegationTarget.TargetOneofCase.Passive =>
                new PassiveDelegationTarget(),
            Grpc.V2.DelegationTarget.TargetOneofCase.Baker =>
                new BakerDelegationTarget(stakeDelegatorTarget.Baker.Value),
            Grpc.V2.DelegationTarget.TargetOneofCase.None =>
                throw new MissingEnumException<Grpc.V2.DelegationTarget.TargetOneofCase>(
                    stakeDelegatorTarget.TargetCase),
            _ => throw new MissingEnumException<Grpc.V2.DelegationTarget.TargetOneofCase>(stakeDelegatorTarget
                .TargetCase)
        };
}

public record PassiveDelegationTarget : DelegationTarget;

public record BakerDelegationTarget(ulong BakerId) : DelegationTarget;
