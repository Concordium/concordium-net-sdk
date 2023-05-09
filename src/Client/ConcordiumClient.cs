using Concordium.Sdk.Transactions;

using Grpc.Core;

namespace Concordium.Sdk.Client;

/// <summary>
/// A client for interacting with the Concordium GRPC API V2 exposed by nodes.
/// </summary>
public class ConcordiumClient : IDisposable
{
    /// <summary>
    /// The raw client.
    /// </summary>
    public RawClient Raw { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="endpoint">
    /// Endpoint of a resource where the V2 API is served. Any port specified in the URL is
    /// ignored.
    /// </param>
    /// <param name="port">
    /// Port of the resource where the V2 API is served. This will override any port
    /// specified in <paramref name="endpoint"/>.
    /// </param>
    /// <param name="timeout">The request timeout in seconds (default: <c>30</c>).</param>
    public ConcordiumClient(Uri endpoint, ushort port, ulong timeout = 30)
        : this(endpoint, port, new ClientConfiguration(timeout)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    /// </summary>
    /// <param name="endpoint">
    /// Endpoint of a resource where the V2 API is served. Any port specified in the URL is
    /// ignored.
    /// </param>
    /// <param name="port">
    /// Port of the resource where the V2 API is served. This will override any port
    /// specified in <paramref name="endpoint"/>.
    /// </param>
    /// <param name="configuration">The configuration to use with this client.</param>
    public ConcordiumClient(Uri endpoint, ushort port, ClientConfiguration configuration) =>
        this.Raw = new RawClient(endpoint, port, configuration);

    /// <summary>
    /// Send an account transaction to the node.
    /// </summary>
    /// <param name="transaction">The account transaction to send.</param>
    /// <returns>Hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    /// <exception cref="FormatException">The returned transaction hash has an invalid number of bytes.</exception>
    public Types.TransactionHash SendAccountTransaction<T>(SignedAccountTransaction<T> transaction)
        where T : AccountTransactionPayload<T>
    {
        // Send the transaction as a block item request.
        var txHash = this.Raw.SendBlockItem(transaction.ToSendBlockItemRequest());

        // Return the transaction hash as a "native" SDK type.
        return Types.TransactionHash.From(txHash.Value.ToByteArray());
    }

    /// <summary>
    /// Spawn a task which sends an account transaction to the node.
    ///
    /// Note that the task may throw a <see cref="FormatException"/> if the transaction hash it returns
    /// has an invalid number of bytes.
    /// </summary>
    /// <param name="transaction">The account transaction to send.</param>
    /// <returns>Task which returns the hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    public Task<Types.TransactionHash> SendAccountTransactionAsync<T>(
        SignedAccountTransaction<T> transaction
    )
        where T : AccountTransactionPayload<T> =>
        // Send the transaction as a block item request.
        this.Raw
            .SendBlockItemAsync(transaction.ToSendBlockItemRequest())
            // Continuation returning the "native" SDK type.
            .ContinueWith(txHash => Types.TransactionHash.From(txHash.Result.Value.ToByteArray()));

    /// <summary>
    /// Query the chain for the next sequence number of the account with the supplied
    /// address.
    ///
    /// Specifically, this is the best guess as to what the next account sequence
    /// number (nonce) should be for the given account.
    ///
    /// If all account transactions are finalized then this information is reliable.
    ///
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    /// <param name="accountAddress">An address of the account to get the next sequence number of.</param>
    /// <returns>
    /// A tuple containing the best guess of the next sequence number for the supplied account and a boolean
    /// indicating whether all transactions associated with the account are finalized. If the latter is the
    /// case, then the sequence number is reliable.
    /// </returns>
    /// <exception cref="RpcException">The call failed.</exception>
    public (Types.AccountSequenceNumber, bool) GetNextAccountSequenceNumber(
        Types.AccountAddress accountAddress
    )
    {
        var next = this.Raw.GetNextAccountSequenceNumber(accountAddress.ToProto());

        // Return the sequence number as a "native" SDK type.
        return (Types.AccountSequenceNumber.From(next.SequenceNumber.Value), next.AllFinal);
    }

    /// <summary>
    /// Spawn a task which queries the chain for the next sequence number of the
    /// account with the supplied address.
    ///
    /// Specifically, this is the best guess as to what the next account sequence
    /// number (nonce) should be for the given account.
    ///
    /// If all account transactions are finalized then this information is reliable.
    ///
    /// Otherwise this is the best guess, assuming all other transactions will be
    /// committed to blocks and eventually finalized.
    /// </summary>
    /// <param name="accountAddress">An address of the account to get the next sequence number of.</param>
    /// <returns>
    /// A task which returns a tuple containing the best guess of the next sequence number for the supplied
    /// account and a boolean indicating whether all transactions associated with the account are finalized.
    /// If the latter is the case, then the sequence number is reliable.
    /// </returns>
    /// <exception cref="RpcException">The call failed.</exception>
    public Task<(Types.AccountSequenceNumber, bool)> GetNextAccountSequenceNumberAsync(
        Types.AccountAddress accountAddress
    ) =>
        this.Raw
            .GetNextAccountSequenceNumberAsync(accountAddress.ToProto())
            // Continuation returning the "native" SDK type.
            .ContinueWith(
                next =>
                    (
                        Types.AccountSequenceNumber.From(next.Result.SequenceNumber.Value),
                        next.Result.AllFinal
                    )
            );

    #region IDisposable Support

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
            {
                this.Raw.Dispose();
            }

            this._disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~ConcordiumClient()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        // This simply serves as a fallback for the GC in case that Dispose was not
        // invoked.
        this.Dispose(false);
    }

    #endregion
}
