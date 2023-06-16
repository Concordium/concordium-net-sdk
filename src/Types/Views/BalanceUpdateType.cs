namespace Concordium.Sdk.Types.Views;

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
