namespace Concordium.Sdk.Types;

/// <summary>
/// Event generated when one or more encrypted amounts are consumed from the
/// account.
/// </summary>
/// <param name="Account">The affected account.</param>
/// <param name="NewAmount">The new self encrypted amount on the affected account.</param>
/// <param name="InputAmount">The input encrypted amount that was removed.</param>
/// <param name="UpToIndex">The index indicating which amounts were used.</param>
public sealed record EncryptedAmountRemovedEvent(AccountAddress Account, byte[] NewAmount, byte[] InputAmount,
    ulong UpToIndex)
{
    internal static EncryptedAmountRemovedEvent From(Grpc.V2.EncryptedAmountRemovedEvent removed) =>
        new(
            AccountAddress.From(removed.Account),
            removed.NewAmount.Value.ToArray(),
            removed.InputAmount.Value.ToArray(),
            removed.UpToIndex
        );
}
