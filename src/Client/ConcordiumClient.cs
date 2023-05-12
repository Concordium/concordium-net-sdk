using Concordium.Grpc.V2;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using Grpc.Core;
using TransactionHash = Concordium.Grpc.V2.TransactionHash;

namespace Concordium.Sdk.Client;

/// <summary>
/// A client for interacting with the Concordium GRPC API V2 exposed by nodes.
/// </summary>
public class ConcordiumClient : IDisposable
{
    /// <summary>
    /// The raw client.
    /// </summary>
    public readonly RawClient Raw;

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
    public ConcordiumClient(Uri endpoint, UInt16 port, ulong timeout = 30)
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
    public ConcordiumClient(Uri endpoint, UInt16 port, ClientConfiguration configuration)
    {
        Raw = new RawClient(endpoint, port, configuration);
    }

    /// <summary>
    /// Send a transaction to the node.
    /// </summary>
    /// <param name="transaction">The transaction to send.</param>
    /// <returns>Hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    /// <exception cref="FormatException">The returned transaction hash has an invalid number of bytes.</exception>
    public Concordium.Sdk.Types.TransactionHash SendTransaction<T>(
        SignedAccountTransaction<T> transaction
    )
        where T : AccountTransactionPayload<T>
    {
        // Send the transaction as a block item request.
        TransactionHash txHash = Raw.SendBlockItem(transaction.ToSendBlockItemRequest());

        // Return the transaction hash as a "native" SDK type.
        return Concordium.Sdk.Types.TransactionHash.From(txHash.Value.ToByteArray());
    }

    /// <summary>
    /// Spawn a task which sends a transaction to the node.
    ///
    /// Note that the task may throw a <see cref="FormatException"/> if the transaction hash it returns
    /// has an invalid number of bytes.
    /// </summary>
    /// <param name="transaction">The transaction to send.</param>
    /// <returns>Task which returns the hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    public Task<Concordium.Sdk.Types.TransactionHash> SendTransactionAsync<T>(
        SignedAccountTransaction<T> transaction
    )
        where T : AccountTransactionPayload<T>
    {
        // Send the transaction as a block item request.
        return Raw.SendBlockItemAsync(transaction.ToSendBlockItemRequest())
            // Continuation returning the "native" SDK type.
            .ContinueWith(
                txHash =>
                    Concordium.Sdk.Types.TransactionHash.From(txHash.Result.Value.ToByteArray())
            );
    }

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
    /// <returns>The best guess of the next sequence number for the supplied account.</returns>
    /// <exception cref="RpcException">The call failed.</exception>
    public Concordium.Sdk.Types.AccountSequenceNumber GetNextAccountSequenceNumber(
        Concordium.Sdk.Types.AccountAddress accountAddress
    )
    {
        NextAccountSequenceNumber next = Raw.GetNextAccountSequenceNumber(accountAddress.ToProto());

        // Return the sequence number as a "native" SDK type.
        return Concordium.Sdk.Types.AccountSequenceNumber.From(next.SequenceNumber.Value);
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
    /// <returns>A task which returns the best guess of the next sequence number for the supplied account.</returns>
    /// <exception cref="RpcException">The call failed.</exception>
    public Task<Types.AccountSequenceNumber> GetNextAccountSequenceNumberAsync(
        Concordium.Sdk.Types.AccountAddress accountAddress
    )
    {
        return Raw.GetNextAccountSequenceNumberAsync(accountAddress.ToProto())
            // Continuation returning the "native" SDK type.
            .ContinueWith(
                next =>
                    Concordium.Sdk.Types.AccountSequenceNumber.From(
                        next.Result.SequenceNumber.Value
                    )
            );
    }

    /// <summary>
    /// Get the status of and information about a specific block item
    /// (transaction).
    /// </summary>
    public async Task<ITransactionStatus> GetBlockItemStatus(Types.TransactionHash transactionHash)
    {
        var blockItemStatus = await Raw.GetBlockItemStatusAsync(transactionHash.ToProto());

        return TransactionStatusFactory.CreateTransactionStatus(blockItemStatus);
    }

    #region IDisposable Support

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Raw.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    #endregion
}
