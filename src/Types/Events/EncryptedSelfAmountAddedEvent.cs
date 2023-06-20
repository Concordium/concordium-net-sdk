using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types.Events;

/// <summary>
/// An account transferred part of its public balance to its encrypted
/// balance.
/// </summary>
/// <param name="Account">The affected account.</param>
/// <param name="NewAmount">The new self encrypted amount of the account.</param>
/// <param name="Amount">The amount that was transferred from public to encrypted balance.</param>
public sealed record EncryptedSelfAmountAddedEvent(AccountAddress Account, byte[] NewAmount, CcdAmount Amount)
{
    internal static EncryptedSelfAmountAddedEvent From(Grpc.V2.EncryptedSelfAmountAddedEvent addedEvent) =>
        new(
            AccountAddress.From(addedEvent.Account),
            addedEvent.NewAmount.Value.ToByteArray(),
            addedEvent.Amount.ToCcd()
        );
}
