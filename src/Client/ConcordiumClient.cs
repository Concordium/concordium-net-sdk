using Concordium.Sdk.Transactions;

using Grpc.Core;
using Grpc.Net.Client;

namespace Concordium.Sdk.Client;

/// <summary>
/// A client for interacting with the Concordium gRPC API V2 exposed by nodes.
/// </summary>
public sealed class ConcordiumClient : IDisposable
{
    /// <summary>
    /// The raw client.
    /// </summary>
    public RawClient Raw { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> class.
    ///
    /// Optionally use <paramref name="channelOptions"/> to specify connection settings
    /// such as the retry policy or keepalive ping.
    ///
    /// By default the policy is not to retry if a connection could not be established.
    ///
    /// See https://github.com/grpc/grpc/blob/master/doc/keepalive.md for default values
    /// for the keepalive ping parameters.
    /// </summary>
    /// <param name="endpoint">
    /// Endpoint of a resource where the V2 API is served. Any port specified in the URL is
    /// ignored.
    /// </param>
    /// <param name="port">
    /// Port of the resource where the V2 API is served. This will override any port
    /// specified in <paramref name="endpoint"/>.
    /// </param>
    /// <param name="timeout">The maximum permitted duration of a call made by this client, in seconds. <c>null</c> allows the call to run indefinitely.</param>
    /// <param name="channelOptions">The options for the channel that is used to communicate with the node.</param>
    public ConcordiumClient(Uri endpoint, ushort port, ulong? timeout = 30, GrpcChannelOptions? rawChannelOptions = null)
    => this.Raw = new(endpoint, port, timeout, rawChannelOptions);

    /// <summary>
    /// Send an account transaction to the node.
    /// </summary>
    /// <param name="transaction">The account transaction to send.</param>
    /// <returns>Hash of the transaction if it was accepted by the node.</returns>
    /// <exception cref="RpcException">The call failed, e.g. due to the node not accepting the transaction.</exception>
    /// <exception cref="FormatException">The returned transaction hash has an invalid number of bytes.</exception>
    public Types.TransactionHash SendAccountTransaction(SignedAccountTransaction transaction)
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
    public async Task<Types.TransactionHash> SendAccountTransactionAsync(
        SignedAccountTransaction transaction
    )
    {
        // Send the transaction as a block item request.
        var txHash = await this.Raw
            .SendBlockItemAsync(transaction.ToSendBlockItemRequest());

        // Return the "native" SDK type.
        return Types.TransactionHash.From(txHash.Value.ToByteArray());
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
    public async Task<(Types.AccountSequenceNumber, bool)> GetNextAccountSequenceNumberAsync(
        Types.AccountAddress accountAddress
    )
    {
        var next = await this.Raw
            .GetNextAccountSequenceNumberAsync(accountAddress.ToProto());

        return (Types.AccountSequenceNumber.From(next.SequenceNumber.Value), next.AllFinal);
    }

    public void Dispose() => this.Raw.Dispose();
}
