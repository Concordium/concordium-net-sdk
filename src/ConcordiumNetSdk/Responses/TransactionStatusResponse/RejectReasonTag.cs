namespace ConcordiumNetSdk.Responses.TransactionStatusResponse;

//todo: find out about documentation of each property
/// <summary>
/// Represents the collection of a reject reason tag constants.
/// </summary>
public enum RejectReasonTag
{
    ModuleNotWf,
    ModuleHashAlreadyExists,
    InvalidAccountReference,
    InvalidInitMethod,
    InvalidReceiveMethod,
    InvalidModuleReference,
    InvalidContractAddress,
    RuntimeFailure,
    AmountTooLarge,
    SerializationFailure,
    OutOfEnergy,
    RejectedInit,
    RejectedReceive,
    NonExistentRewardAccount,
    InvalidProof,
    AlreadyABaker,
    NotABaker,
    InsufficientBalanceForBakerStake,
    StakeUnderMinimumThresholdForBaking,
    BakerInCooldown,
    DuplicateAggregationKey,
    NonExistentCredentialId,
    KeyIndexAlreadyInUse,
    InvalidAccountThreshold,
    InvalidCredentialKeySignThreshold,
    InvalidEncryptedAmountTransferProof,
    InvalidTransferToPublicProof,
    EncryptedAmountSelfTransfer,
    InvalidIndexOnEncryptedTransfer,
    ZeroScheduledAmount,
    NonIncreasingSchedule,
    FirstScheduledReleaseExpired,
    ScheduledSelfTransfer,
    InvalidCredentials,
    DuplicateCredIDs,
    NonExistentCredIDs,
    RemoveFirstCredential,
    CredentialHolderDidNotSign,
    NotAllowedMultipleCredentials,
    NotAllowedToReceiveEncrypted,
    NotAllowedToHandleEncrypted,
    MissingBakerAddParameters,
    FinalizationRewardCommissionNotInRange,
    BakingRewardCommissionNotInRange,
    TransactionFeeCommissionNotInRange,
    AlreadyADelegator,
    InsufficientBalanceForDelegationStake,
    MissingDelegationAddParameters,
    InsufficientDelegationStake,
    DelegatorInCooldown,
    NotADelegator,
    DelegationTargetNotABaker,
    StakeOverMaximumThresholdForPool,
    PoolWouldBecomeOverDelegated,
    PoolClosed,
}
