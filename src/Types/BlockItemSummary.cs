using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Summary of the outcome of a block item in structured form.
/// The summary determines which transaction type it was.
/// </summary>
/// <param name="Index">Index of the transaction in the block where it is included.</param>
/// <param name="EnergyCost">The amount of NRG the transaction cost.</param>
/// <param name="TransactionHash">Hash of the transaction.</param>
/// <param name="Details">
/// Details that are specific to different transaction types.
/// For successful transactions there is a one to one mapping of transaction
/// types and variants (together with subvariants) of this type.
/// </param>
public sealed record BlockItemSummary(ulong Index, EnergyAmount EnergyCost, TransactionHash TransactionHash, IBlockItemSummaryDetails Details)
{
    internal static BlockItemSummary From(Grpc.V2.BlockItemSummary blockItemSummary) =>
        new(
            Index: blockItemSummary.Index.Value,
            EnergyCost: new EnergyAmount(blockItemSummary.EnergyCost.Value),
            TransactionHash: TransactionHash.From(blockItemSummary.Hash.Value.ToByteArray()),
            Details: BlockItemSummaryDetailsFactory.From(blockItemSummary)
        );

    /// <summary>
    /// Return whether the transaction was successful, i.e., the intended effect
    /// happened.
    /// </summary>
    /// <returns>bool representing if transaction succeeded.</returns>
    /// <exception cref="MissingEnumException{DetailsOneofCase}">Throws exception when returned type not known</exception>
    public bool IsSuccess() =>
        this.Details switch
        {
            AccountTransactionDetails accountTransaction =>
                !accountTransaction.TryGetRejectedReason(out _),
            AccountCreationDetails => true,
            UpdateDetails => true,
            _ => throw new MissingTypeException<IBlockItemSummaryDetails>(this.Details)
        };

    /// <summary>
    /// Return whether the transaction has failed to achieve the intended
    /// effects.
    /// </summary>
    /// <returns>bool representing if transaction is rejected</returns>
    public bool IsReject() => this.TryGetRejectedAccountTransaction(out _);

    /// <summary>
    /// Returns transaction cost.
    /// </summary>
    public CcdAmount GetCost() =>
        this.Details switch
        {
            AccountTransactionDetails accountTransactionDetails => accountTransactionDetails.Cost,
            AccountCreationDetails => CcdAmount.Zero,
            UpdateDetails => CcdAmount.Zero,
            _ => CcdAmount.Zero
        };

    /// <summary>
    /// When transaction is rejected this method will return true and rejected reason
    /// will be return in out param.
    /// </summary>
    /// <param name="rejectReason">If rejected then reject reason not null.</param>
    /// <returns>bool representing if transaction is rejected.</returns>
    /// <exception cref="MissingEnumException{DetailsOneofCase}">Throws exception when returned type not known</exception>
    public bool TryGetRejectedAccountTransaction(out IRejectReason? rejectReason)
    {
        rejectReason = null;
        return this.Details switch
        {
            AccountTransactionDetails accountTransaction =>
                accountTransaction.TryGetRejectedReason(out rejectReason),
            AccountCreationDetails => false,
            UpdateDetails => false,
            _ => throw new MissingTypeException<IBlockItemSummaryDetails>(this.Details)
        };
    }

    /// <summary>
    /// Returns true and set sender account if of account transaction type.
    /// </summary>
    /// <param name="sender">Output given when sender present.</param>
    /// <returns>If sender account present.</returns>
    /// <exception cref="MissingEnumException{DetailsOneofCase}">Throws exception when returned type not known</exception>
    public bool TryGetSenderAccount(out AccountAddress? sender)
    {
        sender = null;
        switch (this.Details)
        {
            case AccountTransactionDetails accountTransaction:
                sender = accountTransaction.Sender;
                return true;
            case AccountCreationDetails:
            case UpdateDetails:
                return false;
            default:
                throw new MissingTypeException<IBlockItemSummaryDetails>(this.Details);
        }
    }

    /// <summary>
    /// Returns affected contracts. Only relevant if transaction is a account transaction and effect of the
    /// transaction was contract initialization or contract update.
    /// </summary>
    /// <returns>List of affected contract addresses.</returns>
    /// <exception cref="MissingEnumException{DetailsOneofCase}">Throws exception when returned type not known</exception>
    public IList<ContractAddress> AffectedContracts()
    {
        var affectedContracts = new List<ContractAddress>();

        if (this.Details is not AccountTransactionDetails accountTransactionDetails)
        {
            return affectedContracts;
        }

        switch (accountTransactionDetails.Effects)
        {
            case ContractInitialized contractInitialized:
                affectedContracts.Add(contractInitialized.Data.ContractAddress);
                break;
            case ContractUpdateIssued contractUpdateIssued:
                affectedContracts.AddRange(contractUpdateIssued.GetAffectedContractAddresses());
                break;
            case None:
            case ModuleDeployed:
            case AccountTransfer:
            case BakerAdded:
            case BakerRemoved:
            case BakerStakeUpdated:
            case BakerRestakeEarningsUpdated:
            case BakerKeysUpdated:
            case EncryptedAmountTransferred:
            case TransferredToEncrypted:
            case TransferredToPublic:
            case TransferredWithSchedule:
            case CredentialKeysUpdated:
            case CredentialsUpdated:
            case DataRegistered:
            case BakerConfigured:
            case DelegationConfigured:
                break;
            default:
                throw new MissingTypeException<IAccountTransactionEffects>(accountTransactionDetails.Effects);
        }

        return affectedContracts;
    }

    /// <summary>
    /// If the block item is a smart contract init transaction then
    /// return the initialization data.
    /// </summary>
    /// <param name="contractInitializedEventEvent">Output as a contract initialization event when transaction
    /// was of type contract initialization.</param>
    /// <returns>bool representing if transaction of type contract initialization.</returns>
    public bool TryGetContractInit(out ContractInitializedEvent? contractInitializedEventEvent)
    {
        contractInitializedEventEvent = null;
        if (this.Details is not AccountTransactionDetails accountTransactionDetails)
        {
            return false;
        }

        if (accountTransactionDetails.Effects is not ContractInitialized contractInitialized)
        {
            return false;
        }

        contractInitializedEventEvent = contractInitialized.Data;

        return true;
    }

    /// <summary>
    /// If the block item is a smart contract update transaction then return
    /// an iterator over pairs of a contract address that was affected, and the
    /// logs that were produced.
    /// </summary>
    /// <param name="items">Output as list of contract event for all updated contract when
    /// transaction of type contract update.</param>
    /// <returns>bool representing if transaction of type contract update.</returns>
    /// <exception cref="MissingEnumException{ElementOneofCase}">Throws exception when returned type not known</exception>
    public bool TryGetContractUpdateLogs(out IList<(ContractAddress, IList<ContractEvent>)> items)
    {
        items = new List<(ContractAddress, IList<ContractEvent>)>();
        if (this.Details is not AccountTransactionDetails accountTransactionDetails)
        {
            return false;
        }

        if (accountTransactionDetails.Effects is not ContractUpdateIssued contractUpdateIssued)
        {
            return false;
        }

        items = contractUpdateIssued.GetAffectedAddressesWithLogs();

        return true;
    }

    /// <summary>
    /// Return the list of addresses affected by a account transaction. OBS! this doesn't include account creation
    /// transaction. Use <see cref="AllAffectedAddressesIncludedCreated"/> if these should be included.
    /// </summary>
    /// <returns>list of affected addresses in block</returns>
    /// <exception cref="MissingEnumException{ElementOneofCase}">Throws exception when returned type not known</exception>
    public IList<AccountAddress> AffectedAddresses()
    {
        var affectedAddresses = new List<AccountAddress>();
        if (this.Details is not AccountTransactionDetails accountTransactionDetails)
        {
            return affectedAddresses;
        }

        return accountTransactionDetails.GetAffectedAccountAddresses().ToList();
    }

    /// <summary>
    /// Return the list of affected addresses included account creation.
    /// </summary>
    /// <returns>List of affected addresses included created accounts.</returns>
    /// <exception cref="MissingEnumException{ElementOneofCase}">Throws exception when returned type not known</exception>
    public IList<AccountAddress> AllAffectedAddressesIncludedCreated() =>
        this.Details switch
        {
            AccountCreationDetails accountCreationDetails =>
                new List<AccountAddress> { accountCreationDetails.Address },
            AccountTransactionDetails accountTransactionDetails => accountTransactionDetails
                .GetAffectedAccountAddresses().ToList(),
            _ => new List<AccountAddress>()
        };
}
