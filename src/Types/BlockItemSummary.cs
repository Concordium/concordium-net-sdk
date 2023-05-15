using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Summary of the outcome of a block item in structured form.
/// The summary determines which transaction type it was.
/// </summary>
public sealed class BlockItemSummary {
    private readonly Concordium.Grpc.V2.BlockItemSummary _blockItemSummary;

    internal BlockItemSummary(Concordium.Grpc.V2.BlockItemSummary blockItemSummary) => this._blockItemSummary = blockItemSummary;

    /// <summary>
    /// Return whether the transaction was successful, i.e., the intended effect
    /// happened.
    /// </summary>
    public bool IsSuccess() =>
        this._blockItemSummary.DetailsCase switch
        {
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction =>
                !AccountTransactionDetailsIsRejected(this._blockItemSummary.AccountTransaction, out _),
            Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation => true,
            Grpc.V2.BlockItemSummary.DetailsOneofCase.Update => true,
            Grpc.V2.BlockItemSummary.DetailsOneofCase.None =>
                throw new MissingEnumException<Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase>(Grpc.V2.BlockItemSummary.DetailsOneofCase.None),
            _ =>
                throw new MissingEnumException<Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase>(this._blockItemSummary.DetailsCase)
        };

    /// <summary>
    /// Return whether the transaction has failed to achieve the intended
    /// effects.
    /// </summary>
    public bool IsReject() => this.IsRejectedAccountTransaction(out _);

    /// <summary>
    /// Return true and sets `rejectReason` if the result corresponds to a rejected account
    /// transaction.
    /// </summary>
    public bool IsRejectedAccountTransaction(out RejectReason? rejectReason)
    {
        rejectReason = null;
        return this._blockItemSummary.DetailsCase switch
        {
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction =>
                AccountTransactionDetailsIsRejected(this._blockItemSummary.AccountTransaction, out rejectReason),
            Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation => false,
            Grpc.V2.BlockItemSummary.DetailsOneofCase.Update => false,
            Grpc.V2.BlockItemSummary.DetailsOneofCase.None =>
                throw new MissingEnumException<Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase>(Grpc.V2.BlockItemSummary.DetailsOneofCase.None),
            _ =>
                throw new MissingEnumException<Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase>(this._blockItemSummary.DetailsCase)
        };
    }

    /// <summary>
    /// Returns true and set sender account if of account transaction type.
    /// </summary>
    public bool SenderAccount(out AccountAddress? sender)
    {
        sender = null;
        switch (this._blockItemSummary.DetailsCase)
        {
            case Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction:
                sender = AccountAddress.From(this._blockItemSummary.AccountTransaction.Sender.Value.ToByteArray());
                return true;
            case Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation:
            case Grpc.V2.BlockItemSummary.DetailsOneofCase.Update:
                return false;
            case Grpc.V2.BlockItemSummary.DetailsOneofCase.None:
            default:
                throw new MissingEnumException<Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase>(this._blockItemSummary
                    .DetailsCase);
        }
    }

    /// <summary>
    /// Returns affected contracts. Only relevant if transaction is a account transaction and effect of the
    /// transaction was contract initialization or contract update.
    /// </summary>
    public IList<ContractAddress> AffectedContracts()
    {
        var affectedContracts = new List<ContractAddress>();

        if (this._blockItemSummary.DetailsCase != Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return affectedContracts;
        }

        var effects = this._blockItemSummary.AccountTransaction.Effects;

        switch (effects.EffectCase)
        {
            case AccountTransactionEffects.EffectOneofCase.ContractInitialized:
                affectedContracts.Add(ContractAddress.From(effects.ContractInitialized.Address));
                break;
            case AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued:
                var seen = new HashSet<(ulong, ulong)>();
                foreach (var contractTraceElement in effects.ContractUpdateIssued.Effects)
                {
                    switch (contractTraceElement.ElementCase)
                    {
                        case ContractTraceElement.ElementOneofCase.Updated:
                            if (seen.Add((contractTraceElement.Updated.Address.Index,
                                    contractTraceElement.Updated.Address.Subindex)))
                            {
                                affectedContracts.Add(ContractAddress.From(contractTraceElement.Updated.Address));
                            }
                            break;
                        case ContractTraceElement.ElementOneofCase.None:
                        case ContractTraceElement.ElementOneofCase.Transferred:
                        case ContractTraceElement.ElementOneofCase.Interrupted:
                        case ContractTraceElement.ElementOneofCase.Resumed:
                        case ContractTraceElement.ElementOneofCase.Upgraded:
                        default:
                            continue;
                    }
                }
                break;
            case AccountTransactionEffects.EffectOneofCase.None_:
            case AccountTransactionEffects.EffectOneofCase.ModuleDeployed:
            case AccountTransactionEffects.EffectOneofCase.AccountTransfer:
            case AccountTransactionEffects.EffectOneofCase.BakerAdded:
            case AccountTransactionEffects.EffectOneofCase.BakerRemoved:
            case AccountTransactionEffects.EffectOneofCase.BakerStakeUpdated:
            case AccountTransactionEffects.EffectOneofCase.BakerRestakeEarningsUpdated:
            case AccountTransactionEffects.EffectOneofCase.BakerKeysUpdated:
            case AccountTransactionEffects.EffectOneofCase.EncryptedAmountTransferred:
            case AccountTransactionEffects.EffectOneofCase.TransferredToEncrypted:
            case AccountTransactionEffects.EffectOneofCase.TransferredToPublic:
            case AccountTransactionEffects.EffectOneofCase.TransferredWithSchedule:
            case AccountTransactionEffects.EffectOneofCase.CredentialKeysUpdated:
            case AccountTransactionEffects.EffectOneofCase.CredentialsUpdated:
            case AccountTransactionEffects.EffectOneofCase.DataRegistered:
            case AccountTransactionEffects.EffectOneofCase.BakerConfigured:
            case AccountTransactionEffects.EffectOneofCase.DelegationConfigured:
                break;
            case AccountTransactionEffects.EffectOneofCase.None:
            default:
                throw new MissingEnumException<AccountTransactionEffects.EffectOneofCase>(effects.EffectCase);
        }

        return affectedContracts;
    }

    /// <summary>
    /// If the block item is a smart contract init transaction then
    /// return the initialization data.
    /// </summary>
    public bool ContractInit(out ContractInitializedEvent? contractInitializedEventEvent)
    {
        contractInitializedEventEvent = null;
        if (this._blockItemSummary.DetailsCase != Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return false;
        }

        if (this._blockItemSummary.AccountTransaction.Effects.EffectCase != AccountTransactionEffects.EffectOneofCase.ContractInitialized)
        {
            return false;
        }

        contractInitializedEventEvent =
            new ContractInitializedEvent(this._blockItemSummary.AccountTransaction.Effects.ContractInitialized);

        return true;
    }

    /// <summary>
    /// If the block item is a smart contract update transaction then return
    /// an iterator over pairs of a contract address that was affected, and the
    /// logs that were produced.
    /// </summary>
    public bool ContractUpdateLogs(out IList<(ContractAddress, IList<ContractEvent>)> items)
    {
        items = new List<(ContractAddress, IList<ContractEvent>)>();
        if (this._blockItemSummary.DetailsCase != Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return false;
        }
        if (this._blockItemSummary.AccountTransaction.Effects.EffectCase != AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued)
        {
            return false;
        }

        foreach (var contractTraceElement in this._blockItemSummary.AccountTransaction.Effects.ContractUpdateIssued.Effects)
        {
            switch (contractTraceElement.ElementCase)
            {
                case ContractTraceElement.ElementOneofCase.Updated:
                    var itemUpdated = (
                        ContractAddress.From(contractTraceElement.Updated.Address),
                        contractTraceElement.Updated.Events
                            .Select(e => new ContractEvent(e.Value.ToByteArray()))
                            .ToList())
                        ;
                    items.Add(itemUpdated);
                    break;
                case ContractTraceElement.ElementOneofCase.Interrupted:
                    var itemInterrupted = (
                            ContractAddress.From(contractTraceElement.Interrupted.Address),
                            contractTraceElement.Interrupted.Events
                                .Select(e => new ContractEvent(e.Value.ToByteArray()))
                                .ToList())
                        ;
                    items.Add(itemInterrupted);
                    break;
                case ContractTraceElement.ElementOneofCase.Transferred:
                case ContractTraceElement.ElementOneofCase.Resumed:
                case ContractTraceElement.ElementOneofCase.Upgraded:
                    break;
                case ContractTraceElement.ElementOneofCase.None:
                default:
                    throw new MissingEnumException<ContractTraceElement.ElementOneofCase>(contractTraceElement.ElementCase);
            }
        }

        return true;
    }

    /// <summary>
    /// Return the list of addresses affected by the block summary.
    /// </summary>
    public IList<AccountAddress> AffectedAddresses()
    {
        var affectedAddresses = new List<AccountAddress>();

        if (this._blockItemSummary.DetailsCase != Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return affectedAddresses;
        }

        var accountTransaction = this._blockItemSummary.AccountTransaction;
        var effects = accountTransaction.Effects;
        var sender = AccountAddress.From(accountTransaction.Sender.Value.ToByteArray());

        switch (effects.EffectCase)
        {
            case AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued:
                var seen = new HashSet<AccountAddress>();
                affectedAddresses.Add(sender);
                seen.Add(sender);

                foreach (var effect in effects.ContractUpdateIssued.Effects)
                {
                    switch (effect.ElementCase)
                    {
                        case ContractTraceElement.ElementOneofCase.Transferred:
                            var to = AccountAddress.From(effect.Transferred.Receiver.Value.ToByteArray());
                            if (seen.Add(to))
                            {
                                affectedAddresses.Add(to);
                            }
                            break;
                        case ContractTraceElement.ElementOneofCase.Updated:
                        case ContractTraceElement.ElementOneofCase.Interrupted:
                        case ContractTraceElement.ElementOneofCase.Resumed:
                        case ContractTraceElement.ElementOneofCase.Upgraded:
                            continue;
                        case ContractTraceElement.ElementOneofCase.None:
                        default:
                            throw new MissingEnumException<ContractTraceElement.ElementOneofCase>(effect.ElementCase);
                    }
                }
                break;
            case AccountTransactionEffects.EffectOneofCase.AccountTransfer:
                affectedAddresses.Add(sender);
                var toTransfer = AccountAddress.From(effects.AccountTransfer.Receiver.Value.ToByteArray());
                if (!sender.Equals(toTransfer))
                {
                    affectedAddresses.Add(toTransfer);
                }
                break;
            case AccountTransactionEffects.EffectOneofCase.EncryptedAmountTransferred:
                affectedAddresses.Add(AccountAddress.From(effects.EncryptedAmountTransferred.Removed.Account.Value.ToByteArray()));
                affectedAddresses.Add(AccountAddress.From(effects.EncryptedAmountTransferred.Added.Receiver.Value.ToByteArray()));
                break;
            case AccountTransactionEffects.EffectOneofCase.TransferredToEncrypted:
                affectedAddresses.Add(AccountAddress.From(effects.TransferredToEncrypted.Account.Value.ToByteArray()));
                break;
            case AccountTransactionEffects.EffectOneofCase.TransferredToPublic:
                affectedAddresses.Add(AccountAddress.From(effects.TransferredToPublic.Removed.Account.Value.ToByteArray()));
                break;
            case AccountTransactionEffects.EffectOneofCase.TransferredWithSchedule:
                affectedAddresses.Add(sender);
                affectedAddresses.Add(AccountAddress.From(effects.TransferredWithSchedule.Receiver.Value.ToByteArray()));
                break;
            case AccountTransactionEffects.EffectOneofCase.None_:
            case AccountTransactionEffects.EffectOneofCase.ModuleDeployed:
            case AccountTransactionEffects.EffectOneofCase.ContractInitialized:
            case AccountTransactionEffects.EffectOneofCase.BakerAdded:
            case AccountTransactionEffects.EffectOneofCase.BakerRemoved:
            case AccountTransactionEffects.EffectOneofCase.BakerStakeUpdated:
            case AccountTransactionEffects.EffectOneofCase.BakerRestakeEarningsUpdated:
            case AccountTransactionEffects.EffectOneofCase.BakerKeysUpdated:
            case AccountTransactionEffects.EffectOneofCase.CredentialKeysUpdated:
            case AccountTransactionEffects.EffectOneofCase.CredentialsUpdated:
            case AccountTransactionEffects.EffectOneofCase.DataRegistered:
            case AccountTransactionEffects.EffectOneofCase.BakerConfigured:
            case AccountTransactionEffects.EffectOneofCase.DelegationConfigured:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.None:
            default:
                throw new MissingEnumException<AccountTransactionEffects.EffectOneofCase>(effects.EffectCase);
        }

        return affectedAddresses;
    }

    private static bool AccountTransactionDetailsIsRejected(AccountTransactionDetails details,
        out RejectReason? rejectReason)
    {
        switch (details.Effects.EffectCase)
        {
            case AccountTransactionEffects.EffectOneofCase.None_:
                rejectReason = new RejectReason(details.Effects.None.RejectReason);
                return true;
            case AccountTransactionEffects.EffectOneofCase.ModuleDeployed:
            case AccountTransactionEffects.EffectOneofCase.ContractInitialized:
            case AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued:
            case AccountTransactionEffects.EffectOneofCase.AccountTransfer:
            case AccountTransactionEffects.EffectOneofCase.BakerAdded:
            case AccountTransactionEffects.EffectOneofCase.BakerRemoved:
            case AccountTransactionEffects.EffectOneofCase.BakerStakeUpdated:
            case AccountTransactionEffects.EffectOneofCase.BakerRestakeEarningsUpdated:
            case AccountTransactionEffects.EffectOneofCase.BakerKeysUpdated:
            case AccountTransactionEffects.EffectOneofCase.EncryptedAmountTransferred:
            case AccountTransactionEffects.EffectOneofCase.TransferredToEncrypted:
            case AccountTransactionEffects.EffectOneofCase.TransferredToPublic:
            case AccountTransactionEffects.EffectOneofCase.TransferredWithSchedule:
            case AccountTransactionEffects.EffectOneofCase.CredentialKeysUpdated:
            case AccountTransactionEffects.EffectOneofCase.CredentialsUpdated:
            case AccountTransactionEffects.EffectOneofCase.DataRegistered:
            case AccountTransactionEffects.EffectOneofCase.BakerConfigured:
            case AccountTransactionEffects.EffectOneofCase.DelegationConfigured:
                rejectReason = null;
                return false;
            case AccountTransactionEffects.EffectOneofCase.None:
            default:
                throw new MissingEnumException<AccountTransactionEffects.EffectOneofCase>(details.Effects.EffectCase);
        }
    }
}
