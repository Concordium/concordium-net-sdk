namespace Concordium.Sdk.Types.Account;

/// <summary>
/// The minimum number of credentials that need to sign any transaction coming
/// from an associated account.
/// </summary>
public record struct AccountThreshold(uint Threshold);
