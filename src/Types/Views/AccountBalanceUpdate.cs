namespace Concordium.Sdk.Types.Views;

public sealed record AccountBalanceUpdate(
    AccountAddress AccountAddress,
    long AmountAdjustment,
    BalanceUpdateType BalanceUpdateType,
    TransactionHash? TransactionHash = null);
