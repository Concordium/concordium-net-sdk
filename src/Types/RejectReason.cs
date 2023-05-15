using Concordium.Sdk.Exceptions;
using Google.Protobuf;

namespace Concordium.Sdk.Types;
/// A reason for why a transaction was rejected. Rejected means included in a
/// block, but the desired action was not achieved. The only effect of a
/// rejected transaction is payment.
///
/// OBS: Always check property ReasonCase first to validate which property is not null and hence
/// which type of reject reason the object is.
public sealed class RejectReason
{
  /// <summary>
  /// Type of reason. Should be used to determine which property is set.
  /// </summary>
  public ReasonOneOfCase ReasonCase { get; init; }

  /// <summary>
  /// As the name says.
  /// </summary>
  public ModuleReference? ModuleHashAlreadyExists { get; init; }
  /// <summary>
  /// Account does not exist.
  /// </summary>
  public AccountAddress? InvalidAccountReference { get; init; }
  /// <summary>
  /// Reference to a non-existing contract init method.
  /// </summary>
  public (ModuleReference ModuleReference, OwnedContractName OwnedContractName)? InvalidInitMethod { get; init; }
  /// <summary>
  /// Reference to a non-existing contract receive method.
  /// </summary>
  public (ModuleReference ModuleReference, OwnedReceiveName OwnedReceivedName)? InvalidReceiveMethod { get; init; }
  /// <summary>
  /// Reference to a non-existing module.
  /// </summary>
  public ModuleReference? InvalidModuleReference { get; init; }
  /// <summary>
  /// Contract instance does not exist.
  /// </summary>
  public ContractAddress? InvalidContractAddress { get; init; }
  /// <summary>
  /// When one wishes to transfer an amount from A to B but there
  /// are not enough funds on account/contract A to make this
  /// possible. The data are the from address and the amount to transfer.
  /// </summary>
  public (Address, CcdAmount)? AmountTooLarge { get; init; }
  /// <summary>
  /// Rejected due to contract logic in init function of a contract.
  /// </summary>
  public int RejectedInit { get; init; }
  /// <summary>
  /// Rejected due to contract logic in receive function of a contract.
  /// </summary>
  public (
    int RejectReason,
    ContractAddress ContractAddress,
    OwnedReceiveName OwnedReceiveName,
    OwnedParameter OwnedParameter
    )? RejectedReceive { get; init; }
  /// <summary>
  /// Tried to add baker for an account that already has a baker
  /// </summary>
  public BakerId? AlreadyABaker { get; init; }
  /// <summary>
  /// Tried to remove a baker for an account that has no baker
  /// </summary>
  public AccountAddress? NotABaker { get; init; }
  /// <summary>
  /// A baker with the given aggregation key already exists
  /// </summary>
  public byte[]? DuplicateAggregationKey { get; init; }
  /// <summary>
  /// Account tried to transfer an encrypted amount to itself, that's not
  /// allowed.
  /// </summary>
  public AccountAddress? EncryptedAmountSelfTransfer { get; init; }
  /// <summary>
  /// Account tried to transfer with schedule to itself, that's not allowed.
  /// </summary>
  public AccountAddress? ScheduledSelfTransfer { get; init; }
  /// <summary>
  /// Some of the credential IDs already exist or are duplicated in the
  /// transaction.
  /// </summary>
  public IList<byte[]>? DuplicateCredIds { get; init; }
  /// <summary>
  /// A credential id that was to be removed is not part of the account.
  /// </summary>
  public IList<byte[]>? NonExistentCredIds { get; init; }
  /// <summary>
  /// Account is not a delegation account.
  /// </summary>
  public AccountAddress? NotADelegator { get; init; }
  /// <summary>
  /// Delegation target is not a baker.
  /// </summary>
  public BakerId? DelegationTargetNotABaker { get; init; }


  internal RejectReason(Concordium.Grpc.V2.RejectReason other)
  {
    switch (other.ReasonCase)
    {
      case Grpc.V2.RejectReason.ReasonOneofCase.ModuleNotWf:
          this.ReasonCase = ReasonOneOfCase.ModuleNotWf;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.ModuleHashAlreadyExists:
          this.ModuleHashAlreadyExists = new ModuleReference(new HashBytes(other.ModuleHashAlreadyExists.Value));
          this.ReasonCase = ReasonOneOfCase.ModuleHashAlreadyExists;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidAccountReference:
          this.InvalidAccountReference = AccountAddress.From(other.InvalidAccountReference.Value.ToByteArray());
          this.ReasonCase = ReasonOneOfCase.InvalidAccountReference;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidInitMethod:
          this.InvalidInitMethod = new(
          new ModuleReference(new HashBytes(other.InvalidInitMethod.ModuleRef.Value)),
          new OwnedContractName(other.InvalidInitMethod.InitName.Value)
        );
          this.ReasonCase = ReasonOneOfCase.InvalidInitMethod;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidReceiveMethod:
          this.InvalidReceiveMethod = new(
          new ModuleReference(new HashBytes(other.InvalidReceiveMethod.ModuleRef.Value)),
          new OwnedReceiveName(other.InvalidReceiveMethod.ReceiveName.Value)
        );
          this.ReasonCase = ReasonOneOfCase.InvalidReceiveMethod;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidModuleReference:
          this.InvalidModuleReference = new ModuleReference(new HashBytes(other.InvalidModuleReference.Value));
          this.ReasonCase = ReasonOneOfCase.InvalidModuleReference;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidContractAddress:
          this.InvalidContractAddress = ContractAddress.Create(
          other.InvalidContractAddress.Index,
          other.InvalidContractAddress.Subindex
        );
          this.ReasonCase = ReasonOneOfCase.InvalidContractAddress;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RuntimeFailure:
          this.ReasonCase = ReasonOneOfCase.RuntimeFailure;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.AmountTooLarge:
        var amount = CcdAmount.FromMicroCcd(other.AmountTooLarge.Amount.Value);
        var address = new Address(other.AmountTooLarge.Address);
        this.AmountTooLarge = (address, amount);
        this.ReasonCase = ReasonOneOfCase.AmountTooLarge;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.SerializationFailure:
          this.ReasonCase = ReasonOneOfCase.SerializationFailure;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.OutOfEnergy:
          this.ReasonCase = ReasonOneOfCase.OutOfEnergy;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RejectedInit:
          this.RejectedInit = other.RejectedInit.RejectReason;
          this.ReasonCase = ReasonOneOfCase.RejectedInit;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RejectedReceive:
          this.RejectedReceive = new(
          other.RejectedReceive.RejectReason,
          ContractAddress.From(other.RejectedReceive.ContractAddress),
          new OwnedReceiveName(other.RejectedReceive.ReceiveName.Value),
          new OwnedParameter(other.RejectedReceive.Parameter.Value.ToByteArray())
        );
          this.ReasonCase = ReasonOneOfCase.RejectedReceive;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidProof:
          this.ReasonCase = ReasonOneOfCase.InvalidProof;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.AlreadyABaker:
        var accountIndex = new AccountIndex(other.AlreadyABaker.Value);
        this.AlreadyABaker = new BakerId(accountIndex);
        this.ReasonCase = ReasonOneOfCase.AlreadyABaker;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotABaker:
          this.NotABaker = AccountAddress.From(other.NotABaker.ToByteArray());
          this.ReasonCase = ReasonOneOfCase.NotABaker;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InsufficientBalanceForBakerStake:
          this.ReasonCase = ReasonOneOfCase.InsufficientBalanceForBakerStake;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.StakeUnderMinimumThresholdForBaking:
          this.ReasonCase = ReasonOneOfCase.StakeUnderMinimumThresholdForBaking;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.BakerInCooldown:
          this.ReasonCase = ReasonOneOfCase.BakerInCooldown;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DuplicateAggregationKey:
          this.DuplicateAggregationKey = other.DuplicateAggregationKey.Value.ToByteArray();
          this.ReasonCase = ReasonOneOfCase.DuplicateAggregationKey;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NonExistentCredentialId:
          this.ReasonCase = ReasonOneOfCase.NonExistentCredentialId;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.KeyIndexAlreadyInUse:
          this.ReasonCase = ReasonOneOfCase.KeyIndexAlreadyInUse;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidAccountThreshold:
          this.ReasonCase = ReasonOneOfCase.InvalidAccountThreshold;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidCredentialKeySignThreshold:
          this.ReasonCase = ReasonOneOfCase.InvalidCredentialKeySignThreshold;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidEncryptedAmountTransferProof:
          this.ReasonCase = ReasonOneOfCase.InvalidEncryptedAmountTransferProof;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidTransferToPublicProof:
          this.ReasonCase = ReasonOneOfCase.InvalidTransferToPublicProof;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.EncryptedAmountSelfTransfer:
          this.EncryptedAmountSelfTransfer = AccountAddress.From(other.EncryptedAmountSelfTransfer.ToByteArray());
          this.ReasonCase = ReasonOneOfCase.EncryptedAmountSelfTransfer;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidIndexOnEncryptedTransfer:
          this.ReasonCase = ReasonOneOfCase.InvalidIndexOnEncryptedTransfer;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.ZeroScheduledAmount:
          this.ReasonCase = ReasonOneOfCase.ZeroScheduledAmount;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NonIncreasingSchedule:
          this.ReasonCase = ReasonOneOfCase.NonIncreasingSchedule;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.FirstScheduledReleaseExpired:
          this.ReasonCase = ReasonOneOfCase.FirstScheduledReleaseExpired;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.ScheduledSelfTransfer:
          this.ScheduledSelfTransfer = AccountAddress.From(other.ScheduledSelfTransfer.ToByteArray());
          this.ReasonCase = ReasonOneOfCase.ScheduledSelfTransfer;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidCredentials:
          this.ReasonCase = ReasonOneOfCase.InvalidCredentials;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DuplicateCredIds:
          this.DuplicateCredIds = other.DuplicateCredIds.Ids.Select(id => id.ToByteArray()).ToList();
          this.ReasonCase = ReasonOneOfCase.DuplicateCredIds;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NonExistentCredIds:
          this.NonExistentCredIds = other.NonExistentCredIds.Ids.Select(id => id.ToByteArray()).ToList();
          this.ReasonCase = ReasonOneOfCase.NonExistentCredIds;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RemoveFirstCredential:
          this.ReasonCase = ReasonOneOfCase.RemoveFirstCredential;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.CredentialHolderDidNotSign:
          this.ReasonCase = ReasonOneOfCase.CredentialHolderDidNotSign;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedMultipleCredentials:
          this.ReasonCase = ReasonOneOfCase.NotAllowedMultipleCredentials;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedToReceiveEncrypted:
          this.ReasonCase = ReasonOneOfCase.NotAllowedToReceiveEncrypted;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedToHandleEncrypted:
          this.ReasonCase = ReasonOneOfCase.NotAllowedToHandleEncrypted;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.MissingBakerAddParameters:
          this.ReasonCase = ReasonOneOfCase.MissingBakerAddParameters;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.FinalizationRewardCommissionNotInRange:
          this.ReasonCase = ReasonOneOfCase.FinalizationRewardCommissionNotInRange;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.BakingRewardCommissionNotInRange:
          this.ReasonCase = ReasonOneOfCase.BakingRewardCommissionNotInRange;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.TransactionFeeCommissionNotInRange:
          this.ReasonCase = ReasonOneOfCase.TransactionFeeCommissionNotInRange;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.AlreadyADelegator:
          this.ReasonCase = ReasonOneOfCase.AlreadyADelegator;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InsufficientBalanceForDelegationStake:
          this.ReasonCase = ReasonOneOfCase.InsufficientBalanceForDelegationStake;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.MissingDelegationAddParameters:
          this.ReasonCase = ReasonOneOfCase.MissingDelegationAddParameters;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InsufficientDelegationStake:
          this.ReasonCase = ReasonOneOfCase.InsufficientDelegationStake;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DelegatorInCooldown:
          this.ReasonCase = ReasonOneOfCase.DelegatorInCooldown;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotADelegator:
          this.NotADelegator = AccountAddress.From(other.NotADelegator.Value.ToByteArray());
          this.ReasonCase = ReasonOneOfCase.NotADelegator;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DelegationTargetNotABaker:
        var accountIndexDelegationTargetNotABaker = new AccountIndex(other.DelegationTargetNotABaker.Value);
        this.DelegationTargetNotABaker = new BakerId(accountIndexDelegationTargetNotABaker);
        this.ReasonCase = ReasonOneOfCase.DelegationTargetNotABaker;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.StakeOverMaximumThresholdForPool:
          this.ReasonCase = ReasonOneOfCase.StakeOverMaximumThresholdForPool;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.PoolWouldBecomeOverDelegated:
          this.ReasonCase = ReasonOneOfCase.PoolWouldBecomeOverDelegated;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.PoolClosed:
          this.ReasonCase = ReasonOneOfCase.PoolClosed;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.None:
      default:
        throw new MissingEnumException<Grpc.V2.RejectReason.ReasonOneofCase>(other.ReasonCase);
    }
  }

  /// A reason for why a transaction was rejected.
  public enum ReasonOneOfCase {
      None = 0,
      ModuleNotWf = 1,
      ModuleHashAlreadyExists = 2,
      InvalidAccountReference = 3,
      InvalidInitMethod = 4,
      InvalidReceiveMethod = 5,
      InvalidModuleReference = 6,
      InvalidContractAddress = 7,
      RuntimeFailure = 8,
      AmountTooLarge = 9,
      SerializationFailure = 10,
      OutOfEnergy = 11,
      RejectedInit = 12,
      RejectedReceive = 13,
      InvalidProof = 14,
      AlreadyABaker = 15,
      NotABaker = 16,
      InsufficientBalanceForBakerStake = 17,
      StakeUnderMinimumThresholdForBaking = 18,
      BakerInCooldown = 19,
      DuplicateAggregationKey = 20,
      NonExistentCredentialId = 21,
      KeyIndexAlreadyInUse = 22,
      InvalidAccountThreshold = 23,
      InvalidCredentialKeySignThreshold = 24,
      InvalidEncryptedAmountTransferProof = 25,
      InvalidTransferToPublicProof = 26,
      EncryptedAmountSelfTransfer = 27,
      InvalidIndexOnEncryptedTransfer = 28,
      ZeroScheduledAmount = 29,
      NonIncreasingSchedule = 30,
      FirstScheduledReleaseExpired = 31,
      ScheduledSelfTransfer = 32,
      InvalidCredentials = 33,
      DuplicateCredIds = 34,
      NonExistentCredIds = 35,
      RemoveFirstCredential = 36,
      CredentialHolderDidNotSign = 37,
      NotAllowedMultipleCredentials = 38,
      NotAllowedToReceiveEncrypted = 39,
      NotAllowedToHandleEncrypted = 40,
      MissingBakerAddParameters = 41,
      FinalizationRewardCommissionNotInRange = 42,
      BakingRewardCommissionNotInRange = 43,
      TransactionFeeCommissionNotInRange = 44,
      AlreadyADelegator = 45,
      InsufficientBalanceForDelegationStake = 46,
      MissingDelegationAddParameters = 47,
      InsufficientDelegationStake = 48,
      DelegatorInCooldown = 49,
      NotADelegator = 50,
      DelegationTargetNotABaker = 51,
      StakeOverMaximumThresholdForPool = 52,
      PoolWouldBecomeOverDelegated = 53,
      PoolClosed = 54,
    }
}
