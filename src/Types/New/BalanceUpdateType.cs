namespace Concordium.Sdk.Types.New;

public enum BalanceUpdateType
{
    FoundationReward,
    BakerReward,
    TransactionFeeReward,
    FinalizationReward,
    TransactionFee,
    AmountDecrypted,
    AmountEncrypted,
    TransferOut,
    TransferIn
}
