using Concordium.Grpc.V2;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;
using ContractAddress = Concordium.Sdk.Types.ContractAddress;
using ContractEvent = Concordium.Sdk.Types.ContractEvent;
using ContractInitializedEvent = Concordium.Sdk.Types.ContractInitializedEvent;

namespace BlockItemSummary;

public sealed class BlockItemSummary {
    private readonly Concordium.Grpc.V2.BlockItemSummary _blockItemSummary;

    internal BlockItemSummary(Concordium.Grpc.V2.BlockItemSummary blockItemSummary)
    {
        _blockItemSummary = blockItemSummary;
    }

    public bool IsSuccess()
    {
        return _blockItemSummary.DetailsCase switch
        {
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction => 
                !AccountTransactionDetailsIsRejected(_blockItemSummary.AccountTransaction, out _),
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation => true,
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.Update => true,
            _ => throw new ArgumentOutOfRangeException() // TODO
        };
    }

    public bool IsReject()
    {
        return IsRejectedAccountTransaction(out _);
    }

    public bool IsRejectedAccountTransaction(out Concordium.Sdk.Types.RejectReason? rejectReason)
    {
        rejectReason = null;
        return _blockItemSummary.DetailsCase switch
        {
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction => AccountTransactionDetailsIsRejected(
                _blockItemSummary.AccountTransaction, out rejectReason),
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation => false,
            Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.Update => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool SenderAccount(out AccountAddress? sender)
    {
        sender = null;
        switch (_blockItemSummary.DetailsCase)
        {
            case Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction:
                sender = AccountAddress.From(_blockItemSummary.AccountTransaction.Sender.Value.ToByteArray());
                return true;
            case Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation:
                return false;
            case Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.Update:
                return false;
            default:
                throw new ArgumentOutOfRangeException(); // TODO
        }
    }

    public IList<ContractAddress> AffectedContracts()
    {
        switch (_blockItemSummary.DetailsCase)
        {
            case Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction:
                var effects = _blockItemSummary.AccountTransaction.Effects;
                
                switch (effects.EffectCase)
                {
                    case AccountTransactionEffects.EffectOneofCase.ContractInitialized:
                        return new List<ContractAddress>
                            { ContractAddress.From(effects.ContractInitialized.Address) };
                    case AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued:
                        var addresses = new List<ContractAddress>();
                        var seen = new HashSet<(ulong, ulong)>();
                        foreach (var contractTraceElement in effects.ContractUpdateIssued.Effects)
                        {
                            switch (contractTraceElement.ElementCase)
                            {
                                case ContractTraceElement.ElementOneofCase.Updated:
                                    if (seen.Add((contractTraceElement.Updated.Address.Index,
                                            contractTraceElement.Updated.Address.Subindex)))
                                    {
                                        addresses.Add(ContractAddress.From(contractTraceElement.Updated.Address));
                                    }
                                    break;
                                default:
                                    continue;
                            }
                        }

                        return addresses;
                    default:
                        return new List<ContractAddress>();
                }
            default:
                return new List<ContractAddress>();
        }
    }

    public bool ContractInit(out ContractInitializedEvent? contractInitializedEventEvent)
    {
        contractInitializedEventEvent = null;
        if (_blockItemSummary.DetailsCase != Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return false; 
        }
        
        if (_blockItemSummary.AccountTransaction.Effects.EffectCase != AccountTransactionEffects.EffectOneofCase.ContractInitialized)
        {
            return false;
        }

        contractInitializedEventEvent =
            new ContractInitializedEvent(_blockItemSummary.AccountTransaction.Effects.ContractInitialized);

        return true;
    }

    public bool ContractUpdateLogs(out IList<(ContractAddress, IList<ContractEvent>)>? items)
    {
        items = new List<(ContractAddress, IList<ContractEvent>)>();
        if (_blockItemSummary.DetailsCase != Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return false; 
        }
        if (_blockItemSummary.AccountTransaction.Effects.EffectCase != AccountTransactionEffects.EffectOneofCase.ContractUpdateIssued)
        {
            return false;
        }
        
        foreach (var contractTraceElement in _blockItemSummary.AccountTransaction.Effects.ContractUpdateIssued.Effects)
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
                            ContractAddress.From(contractTraceElement.Updated.Address),
                            contractTraceElement.Updated.Events
                                .Select(e => new ContractEvent(e.Value.ToByteArray()))
                                .ToList())
                        ;
                    items.Add(itemInterrupted);
                    break;
                default:
                    continue;
            }
        }

        return true;
    }

    public IList<AccountAddress> AffectedAddresses()
    {
        var affectedAddresses = new List<AccountAddress>();
        
        if (_blockItemSummary.DetailsCase != Concordium.Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction)
        {
            return affectedAddresses; 
        }

        var accountTransaction = _blockItemSummary.AccountTransaction;
        var effects = accountTransaction.Effects;
        var sender = AccountAddress.From(accountTransaction.Sender.Value.ToByteArray());

        switch (effects.EffectCase)
        {
            case AccountTransactionEffects.EffectOneofCase.None:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.None_:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.ModuleDeployed:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.ContractInitialized:
                affectedAddresses.Add(sender);
                break;
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
                        default:
                            continue;
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
            case AccountTransactionEffects.EffectOneofCase.BakerAdded:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.BakerRemoved:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.BakerStakeUpdated:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.BakerRestakeEarningsUpdated:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.BakerKeysUpdated:
                affectedAddresses.Add(sender);
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
            case AccountTransactionEffects.EffectOneofCase.CredentialKeysUpdated:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.CredentialsUpdated:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.DataRegistered:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.BakerConfigured:
                affectedAddresses.Add(sender);
                break;
            case AccountTransactionEffects.EffectOneofCase.DelegationConfigured:
                affectedAddresses.Add(sender);
                break;
            default:
                affectedAddresses.Add(sender);
                break;
        }
        
        return affectedAddresses;
    }

    private static bool AccountTransactionDetailsIsRejected(AccountTransactionDetails details,
        out Concordium.Sdk.Types.RejectReason? rejectReason)
    {
        switch (details.Effects.EffectCase)
        {
            case AccountTransactionEffects.EffectOneofCase.None:
                rejectReason = new Concordium.Sdk.Types.RejectReason(details.Effects.None.RejectReason);
                return true;
            default:
                rejectReason = null;
                return false;
        }
    }
}