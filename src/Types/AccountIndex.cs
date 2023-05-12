namespace Concordium.Sdk.Types;

/// Index of the account in the account table. These are assigned sequentially
/// in the order of creation of accounts. The first account has index 0.
public record struct AccountIndex(ulong Index);