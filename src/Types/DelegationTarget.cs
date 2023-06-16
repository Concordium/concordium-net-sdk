﻿using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

public abstract record DelegationTarget
{
    public static DelegationTarget From(Grpc.V2.DelegationTarget stakeDelegatorTarget) =>
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
public record PassiveDelegationTarget : DelegationTarget;

/// <summary>
/// Delegate to a specific baker.
/// </summary>
public record BakerDelegationTarget(BakerId BakerId) : DelegationTarget;
