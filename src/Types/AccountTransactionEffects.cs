using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Effects of an account transactions. All variants apart from
/// <see cref="None"/> correspond to a unique transaction that
/// was successful.
/// </summary>
public interface IAccountTransactionEffects{}

internal static class AccountTransactionEffectsFactory
{
    internal static IAccountTransactionEffects From(Grpc.V2.AccountTransactionEffects effects) =>
        effects.EffectCase switch
        {
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.None_ =>
                new None(effects.None.HasTransactionType ? effects.None.TransactionType.Into() : null, RejectReasonFactory.From(effects.None.RejectReason)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.ModuleDeployed =>
                new ModuleDeployed(ModuleReference: new ModuleReference(effects.ModuleDeployed.Value)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.ContractInitialized =>
                new ContractInitialized(ContractInitializedEvent.From(effects.ContractInitialized)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued =>
                new ContractUpdateIssued(effects.ContractUpdateIssued.Effects.Select(ContractTraceElementFactory.From).ToList()),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.AccountTransfer =>
                new AccountTransfer(
                    CcdAmount.From(effects.AccountTransfer.Amount),
                 AccountAddress.From(effects.AccountTransfer.Receiver),
                    OnChainData.From(effects.AccountTransfer.Memo)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.BakerAdded =>
                new BakerAdded(BakerKeysEvent.From(
                    effects.BakerAdded.KeysEvent),
                    effects.BakerAdded.Stake.ToCcd(),
                    effects.BakerAdded.RestakeEarnings),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.BakerRemoved =>
                new BakerRemoved(BakerId.From(effects.BakerRemoved)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.BakerStakeUpdated =>
                new BakerStakeUpdated(BakerStakeUpdatedData.From(effects.BakerStakeUpdated.Update)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.BakerRestakeEarningsUpdated =>
                new BakerRestakeEarningsUpdated(BakerId.From(effects.BakerRestakeEarningsUpdated.BakerId), effects.BakerRestakeEarningsUpdated.RestakeEarnings),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.BakerKeysUpdated =>
                new BakerKeysUpdated(BakerKeysEvent.From(effects.BakerKeysUpdated)),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.EncryptedAmountTransferred =>
                new EncryptedAmountTransferred(
                    EncryptedAmountRemovedEvent.From(effects.EncryptedAmountTransferred.Removed),
                    NewEncryptedAmountEvent.From(effects.EncryptedAmountTransferred.Added),
                    OnChainData.From(effects.EncryptedAmountTransferred.Memo)
                ),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.TransferredToEncrypted =>
                new TransferredToEncrypted(
                    EncryptedSelfAmountAddedEvent.From(effects.TransferredToEncrypted)
                    ),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.TransferredToPublic =>
                new TransferredToPublic(EncryptedAmountRemovedEvent.From(effects.TransferredToPublic.Removed), effects.TransferredToPublic.Amount.ToCcd()),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.TransferredWithSchedule =>
                new TransferredWithSchedule(
                    AccountAddress.From(effects.TransferredWithSchedule.Receiver),
                    effects.TransferredWithSchedule.Amount.Select(a => (a.Timestamp.ToDateTimeOffset(), a.Amount.ToCcd())).ToList(),
                    OnChainData.From(effects.TransferredWithSchedule.Memo)
                    ),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.CredentialKeysUpdated =>
                CredentialKeysUpdated.From(effects.CredentialKeysUpdated),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.CredentialsUpdated =>
                CredentialsUpdated.From(effects.CredentialsUpdated),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.DataRegistered =>
                new DataRegistered(effects.DataRegistered.Value.ToByteArray()),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.BakerConfigured =>
                new BakerConfigured(effects.BakerConfigured.Events.Select(BakerEventFactory.From).ToList()),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.DelegationConfigured =>
                new DelegationConfigured(effects.DelegationConfigured.Events.Select(DelegationEventFactory.From).ToList()),
            Grpc.V2.AccountTransactionEffects.EffectOneofCase.None =>
                throw new MissingEnumException<Grpc.V2.AccountTransactionEffects.EffectOneofCase>(effects.EffectCase),
            _ => throw new MissingEnumException<Grpc.V2.AccountTransactionEffects.EffectOneofCase>(effects.EffectCase)
        };
}

/// <summary>
/// No effects other than payment from this transaction.
/// The rejection reason indicates why the transaction failed.
/// </summary>
/// <param name="TransactionType">
/// Transaction type of a failed transaction, if known.
/// In case of serialization failure this will be null.
/// </param>
/// <param name="RejectReason">Reason for rejection of the transaction</param>
public sealed record None(TransactionType? TransactionType, IRejectReason RejectReason) : IAccountTransactionEffects;

/// <summary>
/// A module was deployed.
/// </summary>
/// <param name="ModuleReference">Reference to contract deployed module.</param>
public sealed record ModuleDeployed(ModuleReference ModuleReference) : IAccountTransactionEffects;

/// <summary>
/// A contract was initialized was deployed.
/// </summary>
/// <param name="Data">Contract initialization data.</param>
public sealed record ContractInitialized(ContractInitializedEvent Data) : IAccountTransactionEffects;

/// <summary>
/// A contract update transaction was issued and produced the given traces.
/// </summary>
public sealed record ContractUpdateIssued(IList<IContractTraceElement> Effects) : IAccountTransactionEffects
{
    internal IList<(ContractAddress, IList<ContractEvent>)> GetAffectedAddressesWithLogs()
    {
        var items = new List<(ContractAddress, IList<ContractEvent>)>();
        foreach (var contractTraceElement in this.Effects)
        {
            switch (contractTraceElement)
            {
                case Updated updated:
                    items.Add((updated.Address, updated.Events));
                    break;
                case Interrupted interrupted:
                    items.Add((interrupted.Address, interrupted.Events));
                    break;
                case Transferred:
                case Resumed:
                case Upgraded:
                    continue;
            }
        }
        return items;
    }

    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        var seen = new HashSet<AccountAddress>();
        foreach (var contractTraceElement in this.Effects)
        {
            switch (contractTraceElement)
            {
                case Transferred transferred:
                    if (seen.Add(transferred.To))
                    {
                        yield return transferred.To;
                    }
                    break;
                case Updated:
                case Interrupted:
                case Resumed:
                case Upgraded:
                    continue;
            }
        }
    }

    internal IEnumerable<ContractAddress> GetAffectedContractAddresses()
    {
        var seen = new HashSet<(ulong, ulong)>();
        foreach (var contractTraceElement in this.Effects)
        {
            switch (contractTraceElement)
            {
                case Updated updated:
                    if (seen.Add((updated.Address.Index,
                            updated.Address.SubIndex)))
                    {
                        yield return updated.Address;
                    }
                    break;
                case Transferred:
                case Interrupted:
                case Resumed:
                case Upgraded:
                    continue;
            }
        }
    }
}

/// <summary>
/// A simple account to account transfer occurred possible with a memo.
/// </summary>
/// <param name="Amount">Amount that was transferred.</param>
/// <param name="To">Receiver account.</param>
/// <param name="Memo">Included memo.</param>
public sealed record AccountTransfer(CcdAmount Amount, AccountAddress To, OnChainData? Memo) : IAccountTransactionEffects
{
    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        yield return this.To;
    }
}

/// <summary>
/// An account was registered as a baker.
/// </summary>
/// <param name="KeysEvent">The keys with which the baker registered.</param>
/// <param name="Stake">
/// The amount the account staked to become a baker. This amount is
/// locked.
/// </param>
/// <param name="RestakeEarnings">
/// Whether the baker will automatically add earnings to their stake or
/// not.
/// </param>
public sealed record BakerAdded(BakerKeysEvent KeysEvent, CcdAmount Stake, bool RestakeEarnings) : IAccountTransactionEffects;

/// <summary>
/// An account was deregistered as a baker.
/// </summary>
public sealed record BakerRemoved(BakerId BakerId) : IAccountTransactionEffects;

/// <summary>
/// If the stake was updated (that is, it changed and did not stay the
/// same) then this is present, otherwise null.
/// </summary>
public sealed record BakerStakeUpdated(BakerStakeUpdatedData? Data) : IAccountTransactionEffects;

/// <summary>
/// An account changed its preference for restaking earnings.
/// </summary>
/// <param name="BakerId">Baker Id</param>
/// <param name="RestakeEarnings">The new value of the flag.</param>
public sealed record BakerRestakeEarningsUpdated(BakerId BakerId, bool RestakeEarnings) : IAccountTransactionEffects;

/// <summary>
/// The baker's keys were updated.
/// </summary>
/// <param name="KeysEvent">Information regarding updated keys.</param>
public sealed record BakerKeysUpdated(BakerKeysEvent KeysEvent) : IAccountTransactionEffects;

/// <summary>
/// An encrypted amount was transferred possible with memo.
/// </summary>
public sealed record EncryptedAmountTransferred(EncryptedAmountRemovedEvent Removed, NewEncryptedAmountEvent Added,
    OnChainData? Memo) : IAccountTransactionEffects
{
    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        yield return this.Removed.Account;
        yield return this.Added.Receiver;
    }
}

/// <summary>
/// An account transferred part of its public balance to its encrypted
/// balance.
/// </summary>
public sealed record TransferredToEncrypted(EncryptedSelfAmountAddedEvent Data) : IAccountTransactionEffects
{
    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        yield return this.Data.Account;
    }
}

/// <summary>
/// An account transferred part of its encrypted balance to its public
/// balance.
/// </summary>
public sealed record TransferredToPublic(EncryptedAmountRemovedEvent Removed, CcdAmount Amount) : IAccountTransactionEffects
{
    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        yield return this.Removed.Account;
    }
}

/// <summary>
/// A transfer with schedule was performed possible with a memo.
/// </summary>
/// <param name="To">Receiver account.</param>
/// <param name="Amount">The list of releases. Ordered by increasing timestamp.</param>
public sealed record TransferredWithSchedule
    (AccountAddress To, IList<(DateTimeOffset, CcdAmount)> Amount, OnChainData? Memo) : IAccountTransactionEffects
{
    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        yield return this.To;
    }
}

/// <summary>
/// Keys of a specific credential were updated.
/// </summary>
/// <param name="CredId">ID of the credential whose keys were updated.</param>
public sealed record CredentialKeysUpdated(CredentialRegistrationId CredId) : IAccountTransactionEffects
{
    internal static CredentialKeysUpdated From(Grpc.V2.CredentialRegistrationId id) => new(CredentialRegistrationId.From(id));
}

/// <summary>
/// Account's credentials were updated.
/// </summary>
/// <param name="NewCredIds">The credential ids that were added.</param>
/// <param name="RemovedCredIds">The credentials that were removed.</param>
/// <param name="NewThreshold">The (possibly) updated account threshold.</param>
public sealed record CredentialsUpdated
(IList<CredentialRegistrationId> NewCredIds, IList<CredentialRegistrationId> RemovedCredIds,
    AccountThreshold NewThreshold) : IAccountTransactionEffects
{
    internal static CredentialsUpdated From(Grpc.V2.AccountTransactionEffects.Types.CredentialsUpdated updated) =>
        new(
            updated.NewCredIds.Select(CredentialRegistrationId.From).ToList(),
            updated.RemovedCredIds.Select(CredentialRegistrationId.From).ToList(),
            new AccountThreshold(updated.NewThreshold.Value)
        );
}

/// <summary>
/// Some data was registered on the chain.
/// </summary>
/// <param name="Data">Data that was registered on the chain.</param>
public sealed record DataRegistered(byte[] Data) : IAccountTransactionEffects
{
    /// <summary>
    /// Returns hex representation of data.
    /// </summary>
    public string ToHex() => Convert.ToHexString(this.Data);
}

/// <summary>
/// A baker was configured.
/// </summary>
/// <param name="Data">The details of what happened</param>
public sealed record BakerConfigured(IList<IBakerEvent> Data) : IAccountTransactionEffects
{

    /// <summary>
    /// Get baker id's affected.
    /// </summary>
    public IEnumerable<BakerId> GetBakerIds()
    {
        foreach (var bakerEvent in this.Data)
        {
            switch (bakerEvent)
            {
                case BakerAddedEvent bakerAddedEvent:
                    yield return bakerAddedEvent.KeysEvent.BakerId;
                    break;
                case BakerKeysUpdatedEvent bakerKeysUpdatedEvent:
                    yield return bakerKeysUpdatedEvent.Data.BakerId;
                    break;
                case BakerRemovedEvent bakerRemovedEvent:
                    yield return bakerRemovedEvent.BakerId;
                    break;
                case BakerRestakeEarningsUpdatedEvent bakerRestakeEarningsUpdatedEvent:
                    yield return bakerRestakeEarningsUpdatedEvent.BakerId;
                    break;
                case BakerSetBakingRewardCommissionEvent bakerSetBakingRewardCommissionEvent:
                    yield return bakerSetBakingRewardCommissionEvent.BakerId;
                    break;
                case BakerSetFinalizationRewardCommissionEvent bakerSetFinalizationRewardCommissionEvent:
                    yield return bakerSetFinalizationRewardCommissionEvent.BakerId;
                    break;
                case BakerSetMetadataUrlEvent bakerSetMetadataUrlEvent:
                    yield return bakerSetMetadataUrlEvent.BakerId;
                    break;
                case BakerSetOpenStatusEvent bakerSetOpenStatusEvent:
                    yield return bakerSetOpenStatusEvent.BakerId;
                    break;
                case BakerSetTransactionFeeCommissionEvent bakerSetTransactionFeeCommissionEvent:
                    yield return bakerSetTransactionFeeCommissionEvent.BakerId;
                    break;
                case BakerStakeDecreasedEvent bakerStakeDecreasedEvent:
                    yield return bakerStakeDecreasedEvent.BakerId;
                    break;
                case BakerStakeIncreasedEvent bakerStakeIncreasedEvent:
                    yield return bakerStakeIncreasedEvent.BakerId;
                    break;
            }
        }
    }
}

/// <summary>
/// An account configured delegation.
/// </summary>
/// <param name="Data">The details of what happened</param>
public sealed record DelegationConfigured(IList<IDelegationEvent> Data) : IAccountTransactionEffects;
