using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Events that may happen as a result of the
/// <see cref="TransactionType.ConfigureDelegation"/> transaction.
/// </summary>
public interface IDelegationEvent {}

internal static class DelegationEventFactory
{
    internal static IDelegationEvent From(DelegationEvent delegationEvent) =>
        delegationEvent.EventCase switch
        {
            DelegationEvent.EventOneofCase.DelegationStakeIncreased =>
                DelegationStakeIncreased.From(delegationEvent.DelegationStakeIncreased),
            DelegationEvent.EventOneofCase.DelegationStakeDecreased =>
                DelegationStakeDecreased.From(delegationEvent.DelegationStakeDecreased),
            DelegationEvent.EventOneofCase.DelegationSetRestakeEarnings =>
                DelegationSetRestakeEarnings.From(delegationEvent.DelegationSetRestakeEarnings),
            DelegationEvent.EventOneofCase.DelegationSetDelegationTarget =>
                DelegationSetDelegationTarget.From(delegationEvent.DelegationSetDelegationTarget),
            DelegationEvent.EventOneofCase.DelegationAdded =>
                DelegationAdded.From(delegationEvent.DelegationAdded),
            DelegationEvent.EventOneofCase.DelegationRemoved =>
                DelegationRemoved.From(delegationEvent.DelegationRemoved),
            DelegationEvent.EventOneofCase.None =>
                throw new MissingEnumException<DelegationEvent.EventOneofCase>(delegationEvent.EventCase),
            _ => throw new MissingEnumException<DelegationEvent.EventOneofCase>(delegationEvent.EventCase)
        };
}

/// <param name="DelegatorId">Delegator's id</param>
/// <param name="NewStake">New stake</param>
public sealed record DelegationStakeIncreased(DelegatorId DelegatorId, CcdAmount NewStake) : IDelegationEvent
{
    internal static DelegationStakeIncreased From(Grpc.V2.DelegationEvent.Types.DelegationStakeIncreased delegation) =>
        new(
            DelegatorId.From(delegation.DelegatorId),
            delegation.NewStake.ToCcd()
        );
}

/// <param name="DelegatorId">Delegator's id</param>
/// <param name="NewStake">New stake</param>
public sealed record DelegationStakeDecreased(DelegatorId DelegatorId, CcdAmount NewStake) : IDelegationEvent
{
    internal static DelegationStakeDecreased From(Grpc.V2.DelegationEvent.Types.DelegationStakeDecreased delegation) =>
        new(
            DelegatorId.From(delegation.DelegatorId),
            delegation.NewStake.ToCcd()
        );
}

/// <param name="DelegatorId">Delegator's id</param>
/// <param name="RestakeEarnings">Whether earnings will be restaked</param>
public sealed record DelegationSetRestakeEarnings(DelegatorId DelegatorId, bool RestakeEarnings) : IDelegationEvent
{
    internal static DelegationSetRestakeEarnings From(Grpc.V2.DelegationEvent.Types.DelegationSetRestakeEarnings delegation) =>
        new(
            DelegatorId.From(delegation.DelegatorId),
            delegation.RestakeEarnings
        );
}

/// <param name="DelegatorId">Delegator's id</param>
/// <param name="DelegationTarget">New delegation target</param>
public sealed record DelegationSetDelegationTarget
    (DelegatorId DelegatorId, DelegationTarget DelegationTarget) : IDelegationEvent
{
    internal static DelegationSetDelegationTarget From(Grpc.V2.DelegationEvent.Types.DelegationSetDelegationTarget delegation) =>
        new(
            DelegatorId.From(delegation.DelegatorId),
            DelegationTarget.From(delegation.DelegationTarget)
        );
}

/// <param name="DelegatorId">Delegator's id</param>
public sealed record DelegationAdded(DelegatorId DelegatorId) : IDelegationEvent
{
    internal static DelegationAdded From(Grpc.V2.DelegatorId id) =>
        new(
            DelegatorId.From(id)
        );
}

/// <param name="DelegatorId">Delegator's id</param>
public sealed record DelegationRemoved(DelegatorId DelegatorId) : IDelegationEvent
{
    internal static DelegationRemoved From(Grpc.V2.DelegatorId id) =>
        new(
            DelegatorId.From(id)
        );
}
