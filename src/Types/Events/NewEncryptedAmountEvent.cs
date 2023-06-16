namespace Concordium.Sdk.Types.Events;

/// <summary>
/// Event generated when an account receives a new encrypted amount.
/// </summary>
/// <param name="Receiver">The account onto which the amount was added.</param>
/// <param name="NewIndex">The index the amount was assigned.</param>
/// <param name="EncryptedAmount">The encrypted amount that was added.</param>
public record NewEncryptedAmountEvent(AccountAddress Receiver, ulong NewIndex, byte[] EncryptedAmount)
{
    internal static NewEncryptedAmountEvent From(Grpc.V2.NewEncryptedAmountEvent amountEvent) =>
        new(
            AccountAddress.From(amountEvent.Receiver),
            amountEvent.NewIndex,
            amountEvent.EncryptedAmount.Value.ToArray()
        );
}
