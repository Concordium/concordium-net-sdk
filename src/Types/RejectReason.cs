using Google.Protobuf;

namespace Concordium.Sdk.Types;

public sealed partial class RejectReason
{
  public ReasonOneofCase ReasonCase { get; init; }
  
  public ModuleReference? ModuleHashAlreadyExists { get; init; }
  public AccountAddress? InvalidAccountReference { get; init; }
  public (ModuleReference ModuleReference, OwnedContractName OwnedContractName)? InvalidInitMethod { get; init; }
  public (ModuleReference ModuleReference, OwnedReceiveName OwnedReceivedName)? InvalidReceiveMethod { get; init; }
  public ModuleReference? InvalidModuleReference { get; init; }
  public ContractAddress? InvalidContractAddress { get; init; }
  public (Address, CcdAmount)? AmountTooLarge { get; init; }
  public int RejectedInit { get; init; }
  public (
    int RejectReason,
    ContractAddress ContractAddress,                
    OwnedReceiveName OwnedReceiveName,
    OwnedParameter OwnedParameter
    )? RejectedReceive { get; init; }
  public BakerId? AlreadyABaker { get; init; }
  public AccountAddress? NotABaker { get; init; }
  public byte[]? DuplicateAggregationKey { get; init; }
  public AccountAddress? EncryptedAmountSelfTransfer { get; init; }
  public AccountAddress? ScheduledSelfTransfer { get; init; }
  public IList<byte[]>? DuplicateCredIds { get; init; }
  public IList<byte[]>? NonExistentCredIds { get; init; }
  public AccountAddress? NotADelegator { get; init; }
  public BakerId? DelegationTargetNotABaker { get; init; }
  

  public RejectReason(Concordium.Grpc.V2.RejectReason other)
  {
    switch (other.ReasonCase)
    {
      case Grpc.V2.RejectReason.ReasonOneofCase.ModuleNotWf:
        ReasonCase = ReasonOneofCase.ModuleNotWf;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.ModuleHashAlreadyExists:
        ModuleHashAlreadyExists = new ModuleReference(new HashBytes(other.ModuleHashAlreadyExists.Value));
        ReasonCase = ReasonOneofCase.ModuleHashAlreadyExists;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidAccountReference:
        InvalidAccountReference = AccountAddress.From(other.InvalidAccountReference.Value.ToByteArray());
        ReasonCase = ReasonOneofCase.InvalidAccountReference;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidInitMethod:
        InvalidInitMethod = new(
          new ModuleReference(new HashBytes(other.InvalidInitMethod.ModuleRef.Value)),
          new OwnedContractName(other.InvalidInitMethod.InitName.Value)
        );
        ReasonCase = ReasonOneofCase.InvalidInitMethod;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidReceiveMethod:
        InvalidReceiveMethod = new(
          new ModuleReference(new HashBytes(other.InvalidReceiveMethod.ModuleRef.Value)),
          new OwnedReceiveName(other.InvalidReceiveMethod.ReceiveName.Value)
        );
        ReasonCase = ReasonOneofCase.InvalidReceiveMethod;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidModuleReference:
        InvalidModuleReference = new ModuleReference(new HashBytes(other.InvalidModuleReference.Value));
        ReasonCase = ReasonOneofCase.InvalidModuleReference;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidContractAddress:
        InvalidContractAddress = ContractAddress.Create(
          other.InvalidContractAddress.Index,
          other.InvalidContractAddress.Subindex
        );
        ReasonCase = ReasonOneofCase.InvalidContractAddress;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RuntimeFailure:
        ReasonCase = ReasonOneofCase.RuntimeFailure;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.AmountTooLarge:
        var amount = CcdAmount.FromMicroCcd(other.AmountTooLarge.Amount.Value);
        var address = new Address(other.AmountTooLarge.Address);
        AmountTooLarge = (address, amount);
        ReasonCase = ReasonOneofCase.AmountTooLarge;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.SerializationFailure:
        ReasonCase = ReasonOneofCase.SerializationFailure;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.OutOfEnergy:
        ReasonCase = ReasonOneofCase.OutOfEnergy;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RejectedInit:
        RejectedInit = other.RejectedInit.RejectReason;
        ReasonCase = ReasonOneofCase.RejectedInit;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RejectedReceive:
        RejectedReceive = new(
          other.RejectedReceive.RejectReason,
          ContractAddress.From(other.RejectedReceive.ContractAddress),
          new OwnedReceiveName(other.RejectedReceive.ReceiveName.Value),
          new OwnedParameter(other.RejectedReceive.Parameter.Value.ToByteArray())
        );
        ReasonCase = ReasonOneofCase.RejectedReceive;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidProof:
        ReasonCase = ReasonOneofCase.InvalidProof;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.AlreadyABaker:
        var accountIndex = new AccountIndex(other.AlreadyABaker.Value);
        AlreadyABaker = new BakerId(accountIndex);
        ;
        ReasonCase = ReasonOneofCase.AlreadyABaker;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotABaker:
        NotABaker = AccountAddress.From(other.NotABaker.ToByteArray());
        ReasonCase = ReasonOneofCase.NotABaker;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InsufficientBalanceForBakerStake:
        ReasonCase = ReasonOneofCase.InsufficientBalanceForBakerStake;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.StakeUnderMinimumThresholdForBaking:
        ReasonCase = ReasonOneofCase.StakeUnderMinimumThresholdForBaking;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.BakerInCooldown:
        ReasonCase = ReasonOneofCase.BakerInCooldown;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DuplicateAggregationKey:
        DuplicateAggregationKey = other.DuplicateAggregationKey.Value.ToByteArray();
        ReasonCase = ReasonOneofCase.DuplicateAggregationKey;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NonExistentCredentialId:
        ReasonCase = ReasonOneofCase.NonExistentCredentialId;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.KeyIndexAlreadyInUse:
        ReasonCase = ReasonOneofCase.KeyIndexAlreadyInUse;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidAccountThreshold:
        ReasonCase = ReasonOneofCase.InvalidAccountThreshold;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidCredentialKeySignThreshold:
        ReasonCase = ReasonOneofCase.InvalidCredentialKeySignThreshold;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidEncryptedAmountTransferProof:
        ReasonCase = ReasonOneofCase.InvalidEncryptedAmountTransferProof;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidTransferToPublicProof:
        ReasonCase = ReasonOneofCase.InvalidTransferToPublicProof;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.EncryptedAmountSelfTransfer:
        EncryptedAmountSelfTransfer = AccountAddress.From(other.EncryptedAmountSelfTransfer.ToByteArray());
        ReasonCase = ReasonOneofCase.EncryptedAmountSelfTransfer;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidIndexOnEncryptedTransfer:
        ReasonCase = ReasonOneofCase.InvalidIndexOnEncryptedTransfer;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.ZeroScheduledAmount:
        ReasonCase = ReasonOneofCase.ZeroScheduledAmount;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NonIncreasingSchedule:
        ReasonCase = ReasonOneofCase.NonIncreasingSchedule;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.FirstScheduledReleaseExpired:
        ReasonCase = ReasonOneofCase.FirstScheduledReleaseExpired;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.ScheduledSelfTransfer:
        ScheduledSelfTransfer = AccountAddress.From(other.ScheduledSelfTransfer.ToByteArray());
        ReasonCase = ReasonOneofCase.ScheduledSelfTransfer;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InvalidCredentials:
        ReasonCase = ReasonOneofCase.InvalidCredentials;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DuplicateCredIds:
        DuplicateCredIds = other.DuplicateCredIds.Ids.Select(id => id.ToByteArray()).ToList();
        ReasonCase = ReasonOneofCase.DuplicateCredIds;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NonExistentCredIds:
        NonExistentCredIds = other.NonExistentCredIds.Ids.Select(id => id.ToByteArray()).ToList();
        ReasonCase = ReasonOneofCase.NonExistentCredIds;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.RemoveFirstCredential:
        ReasonCase = ReasonOneofCase.RemoveFirstCredential;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.CredentialHolderDidNotSign:
        ReasonCase = ReasonOneofCase.CredentialHolderDidNotSign;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedMultipleCredentials:
        ReasonCase = ReasonOneofCase.NotAllowedMultipleCredentials;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedToReceiveEncrypted:
        ReasonCase = ReasonOneofCase.NotAllowedToReceiveEncrypted;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotAllowedToHandleEncrypted:
        ReasonCase = ReasonOneofCase.NotAllowedToHandleEncrypted;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.MissingBakerAddParameters:
        ReasonCase = ReasonOneofCase.MissingBakerAddParameters;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.FinalizationRewardCommissionNotInRange:
        ReasonCase = ReasonOneofCase.FinalizationRewardCommissionNotInRange;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.BakingRewardCommissionNotInRange:
        ReasonCase = ReasonOneofCase.BakingRewardCommissionNotInRange;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.TransactionFeeCommissionNotInRange:
        ReasonCase = ReasonOneofCase.TransactionFeeCommissionNotInRange;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.AlreadyADelegator:
        ReasonCase = ReasonOneofCase.AlreadyADelegator;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InsufficientBalanceForDelegationStake:
        ReasonCase = ReasonOneofCase.InsufficientBalanceForDelegationStake;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.MissingDelegationAddParameters:
        ReasonCase = ReasonOneofCase.MissingDelegationAddParameters;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.InsufficientDelegationStake:
        ReasonCase = ReasonOneofCase.InsufficientDelegationStake;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DelegatorInCooldown:
        ReasonCase = ReasonOneofCase.DelegatorInCooldown;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.NotADelegator:
        NotADelegator = AccountAddress.From(other.NotADelegator.Value.ToByteArray());
        ReasonCase = ReasonOneofCase.NotADelegator;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.DelegationTargetNotABaker:
        var accountIndexDelegationTargetNotABaker = new AccountIndex(other.DelegationTargetNotABaker.Value);
        DelegationTargetNotABaker = new BakerId(accountIndexDelegationTargetNotABaker);
        ;
        ReasonCase = ReasonOneofCase.DelegationTargetNotABaker;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.StakeOverMaximumThresholdForPool:
        ReasonCase = ReasonOneofCase.StakeOverMaximumThresholdForPool;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.PoolWouldBecomeOverDelegated:
        ReasonCase = ReasonOneofCase.PoolWouldBecomeOverDelegated;
        break;

      case Grpc.V2.RejectReason.ReasonOneofCase.PoolClosed:
        ReasonCase = ReasonOneofCase.PoolClosed;
        break;
      
      default:
        throw new MappingException<Grpc.V2.RejectReason>(other);
    }
  }
  
  // TODO: Add documentation
  public enum ReasonOneofCase {
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

public record class InvalidInitMethod(HashBytes ModuleReference,string OwnedContractName);