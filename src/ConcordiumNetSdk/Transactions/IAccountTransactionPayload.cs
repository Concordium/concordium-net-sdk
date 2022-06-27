namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents a union type that contains contents of the specific transaction.
/// </summary>
public interface IAccountTransactionPayload
{
    /// <summary>
    /// Serializes account transaction payload to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized payload in byte format.</returns>
    byte[] SerializeToBytes();

    /// <summary>
    /// Gets the base amount of specific transaction energy.
    /// Base energy cost is not final amount allocated for the execution of this transaction.
    /// The amount of energy allocated for the execution of this transaction is calculated during sending transaction see <see cref="IAccountTransactionService"/>.
    /// </summary>
    /// <returns><see cref="ulong"/> - base energy cost value.</returns>
    ulong GetBaseEnergyCost();
}
