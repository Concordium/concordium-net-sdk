using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Types of account transactions.
/// </summary>
public enum TransactionType : byte
{
    /// <summary>
    /// Deploy a Wasm module.
    /// </summary>
    DeployModule = 0,
    /// <summary>
    /// Initialize a smart contract instance.
    /// </summary>
    InitContract = 1,
    /// <summary>
    /// Update a smart contract instance.
    /// </summary>
    Update = 2,
    /// <summary>
    /// Transfer CCD from an account to another.
    /// </summary>
    Transfer = 3,
    /// <summary>
    /// Register an account as a baker.
    /// </summary>
    AddBaker = 4,
    /// <summary>
    /// Remove an account as a baker.
    /// </summary>
    RemoveBaker = 5,
    /// <summary>
    /// Update the staked amount.
    /// </summary>
    UpdateBakerStake = 6,
    /// <summary>
    /// Update whether the baker automatically restakes earnings.
    /// </summary>
    UpdateBakerRestakeEarnings = 7,
    /// <summary>
    /// Update baker keys
    /// </summary>
    UpdateBakerKeys = 8,
    /// <summary>
    /// Update given credential keys
    /// </summary>
    UpdateCredentialKeys = 13,
    /// <summary>
    /// Transfer encrypted amount.
    /// </summary>
    EncryptedAmountTransfer = 16,
    /// <summary>
    /// Transfer from public to encrypted balance of the same account.
    /// </summary>
    TransferToEncrypted = 17,
    /// <summary>
    /// Transfer from encrypted to public balance of the same account.
    /// </summary>
    TransferToPublic = 18,
    /// <summary>
    /// Transfer a CCD with a release schedule.
    /// </summary>
    TransferWithSchedule = 19,
    /// <summary>
    /// Update the account's credentials.
    /// </summary>
    UpdateCredentials = 20,
    /// <summary>
    /// Register some data on the chain.
    /// </summary>
    RegisterData = 21,
    /// <summary>
    /// Same as transfer but with a memo field.
    /// </summary>
    TransferWithMemo = 22,
    /// <summary>
    /// Same as encrypted transfer, but with a memo.
    /// </summary>
    EncryptedAmountTransferWithMemo = 23,
    /// <summary>
    /// Same as transfer with schedule, but with an added memo.
    /// </summary>
    TransferWithScheduleAndMemo = 24,
    /// <summary>
    /// Configure an account's baker.
    /// </summary>
    ConfigureBaker = 25,
    /// <summary>
    /// Configure an account's stake delegation.
    /// </summary>
    ConfigureDelegation = 26,
}

/// <summary>
/// Helper for constructing transaction type structures.
/// </summary>
public static class TransactionTypeFactory
{
    /// <summary>
    /// Get transaction enum from transaction type.
    /// </summary>
    /// <exception cref="MissingTypeException{IAccountTransactionEffects}"></exception>
    public static bool TryFrom(IAccountTransactionEffects effects, out TransactionType? transactionType)
    {
        transactionType = effects switch
        {
            AccountTransfer accountTransactionEffects => accountTransactionEffects.Memo == null
                ? TransactionType.Transfer
                : TransactionType.TransferWithMemo,
            BakerAdded => TransactionType.AddBaker,
            BakerConfigured => TransactionType.ConfigureBaker,
            BakerKeysUpdated => TransactionType.UpdateBakerKeys,
            BakerRemoved => TransactionType.RemoveBaker,
            BakerRestakeEarningsUpdated => TransactionType.UpdateBakerRestakeEarnings,
            BakerStakeUpdated => TransactionType.UpdateBakerStake,
            ContractInitialized => TransactionType.InitContract,
            ContractUpdateIssued => TransactionType.Update,
            CredentialKeysUpdated => TransactionType.UpdateCredentialKeys,
            CredentialsUpdated => TransactionType.UpdateCredentials,
            DataRegistered => TransactionType.RegisterData,
            DelegationConfigured => TransactionType.ConfigureDelegation,
            EncryptedAmountTransferred encryptedAmountTransferred =>
                encryptedAmountTransferred.Memo == null
                    ? TransactionType.EncryptedAmountTransfer
                    : TransactionType.EncryptedAmountTransferWithMemo,
            ModuleDeployed => TransactionType.DeployModule,
            TransferredToEncrypted => TransactionType.TransferToEncrypted,
            TransferredToPublic => TransactionType.TransferToPublic,
            TransferredWithSchedule transferredWithSchedule =>
                transferredWithSchedule.Memo == null
                    ? TransactionType.TransferWithSchedule
                    : TransactionType.TransferWithScheduleAndMemo,
            None none => none.TransactionType,
            _ => throw new MissingTypeException<IAccountTransactionEffects>(effects)
        };
        return transactionType != null;
    }

    internal static TransactionType Into(this Grpc.V2.TransactionType transactionType) =>
        transactionType switch
        {
            Grpc.V2.TransactionType.DeployModule => TransactionType.DeployModule,
            Grpc.V2.TransactionType.InitContract => TransactionType.InitContract,
            Grpc.V2.TransactionType.Update => TransactionType.Update,
            Grpc.V2.TransactionType.Transfer => TransactionType.Transfer,
            Grpc.V2.TransactionType.AddBaker => TransactionType.AddBaker,
            Grpc.V2.TransactionType.RemoveBaker => TransactionType.RemoveBaker,
            Grpc.V2.TransactionType.UpdateBakerStake => TransactionType.UpdateBakerStake,
            Grpc.V2.TransactionType.UpdateBakerRestakeEarnings => TransactionType.UpdateBakerRestakeEarnings,
            Grpc.V2.TransactionType.UpdateBakerKeys => TransactionType.UpdateBakerKeys,
            Grpc.V2.TransactionType.UpdateCredentialKeys => TransactionType.UpdateCredentialKeys,
            Grpc.V2.TransactionType.EncryptedAmountTransfer => TransactionType.EncryptedAmountTransfer,
            Grpc.V2.TransactionType.TransferToEncrypted => TransactionType.TransferToEncrypted,
            Grpc.V2.TransactionType.TransferToPublic => TransactionType.TransferToPublic,
            Grpc.V2.TransactionType.TransferWithSchedule => TransactionType.TransferWithSchedule,
            Grpc.V2.TransactionType.UpdateCredentials => TransactionType.UpdateCredentials,
            Grpc.V2.TransactionType.RegisterData => TransactionType.RegisterData,
            Grpc.V2.TransactionType.TransferWithMemo => TransactionType.TransferWithMemo,
            Grpc.V2.TransactionType.EncryptedAmountTransferWithMemo => TransactionType.EncryptedAmountTransferWithMemo,
            Grpc.V2.TransactionType.TransferWithScheduleAndMemo => TransactionType.TransferWithScheduleAndMemo,
            Grpc.V2.TransactionType.ConfigureBaker => TransactionType.ConfigureBaker,
            Grpc.V2.TransactionType.ConfigureDelegation => TransactionType.ConfigureDelegation,
            _ => throw new MissingEnumException<Grpc.V2.TransactionType>(transactionType)
        };
}
