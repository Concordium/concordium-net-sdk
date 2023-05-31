namespace Concordium.Sdk.Types.New;

public record AccountBalanceUpdate(
    AccountAddress AccountAddress,
    long AmountAdjustment,
    BalanceUpdateType BalanceUpdateType,
    TransactionHash? TransactionHash = null);
