namespace Concordium.Sdk.Types.Views;

public record AccountBalanceUpdate(
    AccountAddress AccountAddress,
    long AmountAdjustment,
    BalanceUpdateType BalanceUpdateType,
    TransactionHash? TransactionHash = null);
