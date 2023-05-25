using Concordium.Sdk.Exceptions;
using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// <summary>
/// Factory used to map Reject Reasons.
/// </summary>
internal static class RejectReasonFactory
{
    /// <summary>
    /// Creates a Reject Reason record.
    /// </summary>
    /// <param name="other">Reject reason from response</param>
    /// <exception cref="MissingEnumException{ReasonOneofCase}">If reject reason from response not known</exception>
    internal static IRejectReason From(Grpc.V2.RejectReason other) =>
        other.ReasonCase switch
        {
            Grpc.V2.RejectReason.ReasonOneofCase.ModuleNotWf => new ModuleNotWf(),
            Grpc.V2.RejectReason.ReasonOneofCase.ModuleHashAlreadyExists => new ModuleHashAlreadyExists(
                new ModuleReference(other.ModuleHashAlreadyExists.Value)),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidAccountReference => new InvalidAccountReference(
                AccountAddress.From(other.InvalidAccountReference.Value.ToByteArray())),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidInitMethod => new InvalidInitMethod(
                new ModuleReference(other.InvalidInitMethod.ModuleRef.Value),
                new ContractName(other.InvalidInitMethod.InitName.Value)),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidReceiveMethod => new InvalidReceiveMethod(
                new ModuleReference(other.InvalidReceiveMethod.ModuleRef.Value),
                new ReceiveName(other.InvalidReceiveMethod.ReceiveName.Value)),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidModuleReference => new InvalidModuleReference(
                new ModuleReference(other.InvalidModuleReference.Value)),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidContractAddress => new InvalidContractAddress(
                ContractAddress.From(other.InvalidContractAddress.Index, other.InvalidContractAddress.Subindex)),
            Grpc.V2.RejectReason.ReasonOneofCase.RuntimeFailure => new RuntimeFailure(),
            Grpc.V2.RejectReason.ReasonOneofCase.AmountTooLarge => new AmountTooLarge(
                AddressFactory.From(other.AmountTooLarge.Address),
                CcdAmount.FromMicroCcd(other.AmountTooLarge.Amount.Value)),
            Grpc.V2.RejectReason.ReasonOneofCase.SerializationFailure => new SerializationFailure(),
            Grpc.V2.RejectReason.ReasonOneofCase.OutOfEnergy => new OutOfEnergy(),
            Grpc.V2.RejectReason.ReasonOneofCase.RejectedInit => new RejectedInit(other.RejectedInit.RejectReason),
            Grpc.V2.RejectReason.ReasonOneofCase.RejectedReceive => new RejectedReceive(
                other.RejectedReceive.RejectReason, ContractAddress.From(other.RejectedReceive.ContractAddress),
                new ReceiveName(other.RejectedReceive.ReceiveName.Value),
                new Parameter(other.RejectedReceive.Parameter.Value.ToByteArray())),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidProof => new InvalidProof(),
            Grpc.V2.RejectReason.ReasonOneofCase.AlreadyABaker => new AlreadyABaker(
                new BakerId(new AccountIndex(other.AlreadyABaker.Value))),
            Grpc.V2.RejectReason.ReasonOneofCase.NotABaker => new NotABaker(
                AccountAddress.From(other.NotABaker.ToByteArray())),
            Grpc.V2.RejectReason.ReasonOneofCase.InsufficientBalanceForBakerStake =>
                new InsufficientBalanceForBakerStake(),
            Grpc.V2.RejectReason.ReasonOneofCase.StakeUnderMinimumThresholdForBaking =>
                new StakeUnderMinimumThresholdForBaking(),
            Grpc.V2.RejectReason.ReasonOneofCase.BakerInCooldown => new BakerInCooldown(),
            Grpc.V2.RejectReason.ReasonOneofCase.DuplicateAggregationKey => new DuplicateAggregationKey(
                other.DuplicateAggregationKey.Value.ToByteArray()),
            Grpc.V2.RejectReason.ReasonOneofCase.NonExistentCredentialId => new NonExistentCredentialId(),
            Grpc.V2.RejectReason.ReasonOneofCase.KeyIndexAlreadyInUse => new KeyIndexAlreadyInUse(),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidAccountThreshold => new InvalidAccountThreshold(),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidCredentialKeySignThreshold =>
                new InvalidCredentialKeySignThreshold(),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidEncryptedAmountTransferProof =>
                new InvalidEncryptedAmountTransferProof(),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidTransferToPublicProof => new InvalidTransferToPublicProof(),
            Grpc.V2.RejectReason.ReasonOneofCase.EncryptedAmountSelfTransfer => new EncryptedAmountSelfTransfer(
                AccountAddress.From(other.EncryptedAmountSelfTransfer.ToByteArray())),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidIndexOnEncryptedTransfer =>
                new InvalidIndexOnEncryptedTransfer(),
            Grpc.V2.RejectReason.ReasonOneofCase.ZeroScheduledAmount => new ZeroScheduledAmount(),
            Grpc.V2.RejectReason.ReasonOneofCase.NonIncreasingSchedule => new NonIncreasingSchedule(),
            Grpc.V2.RejectReason.ReasonOneofCase.FirstScheduledReleaseExpired => new FirstScheduledReleaseExpired(),
            Grpc.V2.RejectReason.ReasonOneofCase.ScheduledSelfTransfer => new ScheduledSelfTransfer(
                AccountAddress.From(other.ScheduledSelfTransfer.ToByteArray())),
            Grpc.V2.RejectReason.ReasonOneofCase.InvalidCredentials => new InvalidCredentials(),
            Grpc.V2.RejectReason.ReasonOneofCase.DuplicateCredIds => new DuplicateCredIds(other.DuplicateCredIds.Ids
                .Select(id => id.ToByteArray())
                .ToList()),
            Grpc.V2.RejectReason.ReasonOneofCase.NonExistentCredIds => new NonExistentCredIds(other.NonExistentCredIds
                .Ids.Select(id => id.ToByteArray())
                .ToList()),
            Grpc.V2.RejectReason.ReasonOneofCase.RemoveFirstCredential => new RemoveFirstCredential(),
            Grpc.V2.RejectReason.ReasonOneofCase.CredentialHolderDidNotSign => new CredentialHolderDidNotSign(),
            Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedMultipleCredentials => new NotAllowedMultipleCredentials(),
            Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedToReceiveEncrypted => new NotAllowedToReceiveEncrypted(),
            Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedToHandleEncrypted => new NotAllowedToHandleEncrypted(),
            Grpc.V2.RejectReason.ReasonOneofCase.MissingBakerAddParameters => new MissingBakerAddParameters(),
            Grpc.V2.RejectReason.ReasonOneofCase.FinalizationRewardCommissionNotInRange =>
                new FinalizationRewardCommissionNotInRange(),
            Grpc.V2.RejectReason.ReasonOneofCase.BakingRewardCommissionNotInRange =>
                new BakingRewardCommissionNotInRange(),
            Grpc.V2.RejectReason.ReasonOneofCase.TransactionFeeCommissionNotInRange =>
                new TransactionFeeCommissionNotInRange(),
            Grpc.V2.RejectReason.ReasonOneofCase.AlreadyADelegator => new AlreadyADelegator(),
            Grpc.V2.RejectReason.ReasonOneofCase.InsufficientBalanceForDelegationStake =>
                new InsufficientBalanceForDelegationStake(),
            Grpc.V2.RejectReason.ReasonOneofCase.MissingDelegationAddParameters => new MissingDelegationAddParameter(),
            Grpc.V2.RejectReason.ReasonOneofCase.InsufficientDelegationStake => new InsufficientDelegationStake(),
            Grpc.V2.RejectReason.ReasonOneofCase.DelegatorInCooldown => new DelegatorInCooldown(),
            Grpc.V2.RejectReason.ReasonOneofCase.NotADelegator => new NotADelegator(
                AccountAddress.From(other.NotADelegator.Value.ToByteArray())),
            Grpc.V2.RejectReason.ReasonOneofCase.DelegationTargetNotABaker => new DelegationTargetNotABaker(
                new BakerId(new AccountIndex(other.DelegationTargetNotABaker.Value))),
            Grpc.V2.RejectReason.ReasonOneofCase.StakeOverMaximumThresholdForPool =>
                new StakeOverMaximumThresholdForPool(),
            Grpc.V2.RejectReason.ReasonOneofCase.PoolWouldBecomeOverDelegated => new PoolWouldBecomeOverDelegated(),
            Grpc.V2.RejectReason.ReasonOneofCase.PoolClosed => new PoolClosed(),
            Grpc.V2.RejectReason.ReasonOneofCase.None =>
                throw new MissingEnumException<Grpc.V2.RejectReason.ReasonOneofCase>(other.ReasonCase),
            _ => throw new MissingEnumException<Grpc.V2.RejectReason.ReasonOneofCase>(other.ReasonCase)
        };
}

/// <summary>
/// Common interface for reject reasons. The user can with a switch statement determine the type
/// and act on it. See example.
/// </summary>
/// <example>
/// <code>
/// internal static void Example(IRejectReason rejectReason)
/// {
///     switch (rejectReason)
///     {a
///         case AlreadyABaker alreadyABaker:
///             Console.WriteLine($"Do this when already a baker with id: {alreadyABaker.BakerId}");
///             break;
///         case AlreadyADelegator alreadyADelegator:
///             Console.WriteLine($"Or do this when already a delegator");
///             break;
///             ...
///     }
/// }
/// </code>
/// </example>
public interface IRejectReason{}
/// <summary>
/// Error raised when validating the Wasm module.
/// </summary>
public record ModuleNotWf : IRejectReason;
/// <summary>
/// Module Hash Already Exists
/// </summary>
public record ModuleHashAlreadyExists(ModuleReference ModuleReference) : IRejectReason;
/// <summary>
/// Account does not exist.
/// </summary>
public record InvalidAccountReference(AccountAddress AccountAddress) : IRejectReason;
/// <summary>
/// Reference to a non-existing contract init method.
/// </summary>
public record InvalidInitMethod(ModuleReference ModuleReference, ContractName ContractName) : IRejectReason;
/// <summary>
/// Reference to a non-existing contract receive method.
/// </summary>
public record InvalidReceiveMethod(ModuleReference ModuleReference, ReceiveName ReceiveName) : IRejectReason;
/// <summary>
/// Reference to a non-existing module.
/// </summary>
public record InvalidModuleReference(ModuleReference ModuleReference) : IRejectReason;
/// <summary>
/// Contract instance does not exist.
/// </summary>
public record InvalidContractAddress(ContractAddress ContractAddress) : IRejectReason;
/// <summary>
/// Runtime exception occurred when running either the init or receive
/// method.
/// </summary>
public record RuntimeFailure : IRejectReason;
/// <summary>
/// When one wishes to transfer an amount from A to B but there
/// are not enough funds on account/contract A to make this
/// possible. The data are the from address and the amount to transfer.
/// </summary>
public record AmountTooLarge(IAddress Address, CcdAmount Amount) : IRejectReason;
/// <summary>
/// Serialization of the body failed.
/// </summary>
public record SerializationFailure : IRejectReason;
/// <summary>
/// We ran of out energy to process this transaction.
/// </summary>
public record OutOfEnergy : IRejectReason;
/// <summary>
/// Rejected due to contract logic in init function of a contract.
/// </summary>
public record RejectedInit(int RejectReason) : IRejectReason;
/// <summary>
/// Rejected due to contract logic in receive function of a contract.
/// </summary>
public record RejectedReceive(int RejectReason, ContractAddress ContractAddress, ReceiveName ReceiveName,
    Parameter Parameter) : IRejectReason;
/// <summary>
/// Proof that the baker owns relevant private keys is not valid.
/// </summary>
public record InvalidProof : IRejectReason;
/// <summary>
/// Tried to add baker for an account that already has a baker
/// </summary>
public record AlreadyABaker(BakerId BakerId) : IRejectReason;
/// <summary>
/// Tried to remove a baker for an account that has no baker
/// </summary>
public record NotABaker(AccountAddress AccountAddress) : IRejectReason;
/// <summary>
/// The amount on the account was insufficient to cover the proposed stake
/// </summary>
public record InsufficientBalanceForBakerStake : IRejectReason;
/// <summary>
/// The amount provided is under the threshold required for becoming a baker
/// </summary>
public record StakeUnderMinimumThresholdForBaking : IRejectReason;
/// <summary>
/// The change could not be made because the baker is in cooldown for
/// another change
/// </summary>
public record BakerInCooldown : IRejectReason;
/// <summary>
/// A baker with the given aggregation key already exists
/// </summary>
public record DuplicateAggregationKey(byte[] Key) : IRejectReason;
/// <summary>
/// Encountered credential ID that does not exist
/// </summary>
public record NonExistentCredentialId : IRejectReason;
/// <summary>
/// Attempted to add an account key to a key index already in use
/// </summary>
public record KeyIndexAlreadyInUse : IRejectReason;
/// <summary>
/// When the account threshold is updated, it must not exceed the amount of
/// existing keys
/// </summary>
public record InvalidAccountThreshold : IRejectReason;
/// <summary>
/// When the credential key threshold is updated, it must not exceed the
/// amount of existing keys
/// </summary>
public record InvalidCredentialKeySignThreshold : IRejectReason;
/// <summary>
/// Proof for an encrypted amount transfer did not validate.
/// </summary>
public record InvalidEncryptedAmountTransferProof : IRejectReason;
/// <summary>
/// Proof for a secret to public transfer did not validate.
/// </summary>
public record InvalidTransferToPublicProof : IRejectReason;
/// <summary>
/// Account tried to transfer an encrypted amount to itself, that's not
/// allowed.
/// </summary>
public record EncryptedAmountSelfTransfer(AccountAddress AccountAddress) : IRejectReason;
/// <summary>
/// The provided index is below the start index or above `startIndex +
/// length incomingAmounts`
/// </summary>
public record InvalidIndexOnEncryptedTransfer : IRejectReason;
/// <summary>
/// The transfer with schedule is going to send 0 tokens
/// </summary>
public record ZeroScheduledAmount : IRejectReason;
/// <summary>
/// The transfer with schedule has a non strictly increasing schedule
/// </summary>
public record NonIncreasingSchedule : IRejectReason;
/// <summary>
/// The first scheduled release in a transfer with schedule has already
/// expired
/// </summary>
public record FirstScheduledReleaseExpired : IRejectReason;
/// <summary>
/// Account tried to transfer with schedule to itself, that's not allowed.
/// </summary>
public record ScheduledSelfTransfer(AccountAddress AccountAddress) : IRejectReason;
/// <summary>
/// At least one of the credentials was either malformed or its proof was
/// incorrect.
/// </summary>
public record InvalidCredentials : IRejectReason;
/// <summary>
/// Some of the credential IDs already exist or are duplicated in the
/// transaction.
/// </summary>
public record DuplicateCredIds(IList<byte[]> CredIds) : IRejectReason;
/// <summary>
/// A credential id that was to be removed is not part of the account.
/// </summary>
public record NonExistentCredIds(IList<byte[]> CredIds) : IRejectReason;
/// <summary>
/// Attempt to remove the first credential
/// </summary>
public record RemoveFirstCredential : IRejectReason;
/// <summary>
/// The credential holder of the keys to be updated did not sign the
/// transaction
/// </summary>
public record CredentialHolderDidNotSign : IRejectReason;
/// <summary>
/// Account is not allowed to have multiple credentials because it contains
/// a non-zero encrypted transfer.
/// </summary>
public record NotAllowedMultipleCredentials : IRejectReason;
/// <summary>
/// The account is not allowed to receive encrypted transfers because it has
/// multiple credentials.
/// </summary>
public record NotAllowedToReceiveEncrypted : IRejectReason;
/// <summary>
/// The account is not allowed to send encrypted transfers (or transfer
/// from/to public to/from encrypted)
/// </summary>
public record NotAllowedToHandleEncrypted : IRejectReason;
/// <summary>
/// A configure baker transaction is missing one or more arguments in order
/// to add a baker.
/// </summary>
public record MissingBakerAddParameters : IRejectReason;
/// <summary>
/// Finalization reward commission is not in the valid range for a baker
/// </summary>
public record FinalizationRewardCommissionNotInRange : IRejectReason;
/// <summary>
/// Baking reward commission is not in the valid range for a baker
/// </summary>
public record BakingRewardCommissionNotInRange : IRejectReason;
/// <summary>
/// Transaction fee commission is not in the valid range for a baker
/// </summary>
public record TransactionFeeCommissionNotInRange : IRejectReason;
/// <summary>
/// Tried to add baker for an account that already has a delegator.
/// </summary>
public record AlreadyADelegator : IRejectReason;
/// <summary>
/// The amount on the account was insufficient to cover the proposed stake.
/// </summary>
public record InsufficientBalanceForDelegationStake : IRejectReason;
/// <summary>
/// A configure delegation transaction is missing one or more arguments in
/// order to add a delegator.
/// </summary>
public record MissingDelegationAddParameter : IRejectReason;
/// <summary>
/// Delegation stake when adding a delegator was 0.
/// </summary>
public record InsufficientDelegationStake : IRejectReason;
/// <summary>
/// Account is not a delegation account.
/// </summary>
public record DelegatorInCooldown : IRejectReason;
/// <summary>
/// Account is not a delegation account.
/// </summary>
public record NotADelegator(AccountAddress AccountAddress) : IRejectReason;
/// <summary>
/// Delegation target is not a baker.
/// </summary>
public record DelegationTargetNotABaker(BakerId BakerId) : IRejectReason;
/// <summary>
/// The amount would result in pool capital higher than the maximum
/// threshold.
/// </summary>
public record StakeOverMaximumThresholdForPool : IRejectReason;
/// <summary>
/// The amount would result in pool with a too high fraction of delegated
/// capital.
/// </summary>
public record PoolWouldBecomeOverDelegated : IRejectReason;
/// <summary>
/// The pool is not open to delegators.
/// </summary>
public record PoolClosed : IRejectReason;
