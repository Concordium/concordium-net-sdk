using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents api for interacting with account transactions.
/// </summary>
public interface IAccountTransactionService
{
    // todo: think where and how to specify expiry time in concrete implementation
    /// <summary>
    /// Sends any account transaction using default expire time and automatically gained next account Nonce.
    /// </summary>
    /// <param name="sender">the sender account address.</param>
    /// <param name="transactionPayload">the actual contents of the specific transaction.</param>
    /// <param name="transactionSigner">the map from the index of the credential to another map from the key index to the actual signature.</param>
    /// <returns><see cref="TransactionHash"/> - hash of successfully send transaction.</returns>
    Task<TransactionHash> SendAccountTransactionAsync(
        AccountAddress sender,
        IAccountTransactionPayload transactionPayload,
        ITransactionSigner transactionSigner);

    // todo: think where and how to specify expiry time in concrete implementation
    /// <summary>
    /// Sends any account transaction using default expire time.
    /// </summary>
    /// <param name="sender">the sender account address.</param>
    /// <param name="nextAccountNonce">the next account nonce.</param>
    /// <param name="transactionPayload">the actual contents of the specific transaction.</param>
    /// <param name="transactionSigner">the map from the index of the credential to another map from the key index to the actual signature.</param>
    /// <returns><see cref="TransactionHash"/> - hash of successfully send transaction.</returns>
    Task<TransactionHash> SendAccountTransactionAsync(
        AccountAddress sender,
        Nonce nextAccountNonce,
        IAccountTransactionPayload transactionPayload,
        ITransactionSigner transactionSigner);

    /// <summary>
    /// Sends any account transaction.
    /// </summary>
    /// <param name="sender">the account address of sender.</param>
    /// <param name="nextAccountNonce">the next account nonce.</param>
    /// <param name="transactionPayload">the actual contents of the specific transaction.</param>
    /// <param name="expiry">the absolute expiration time after which transaction will not be executed. Measured in seconds since unix epoch.</param>
    /// <param name="transactionSigner">the map from the index of the credential to another map from the key index to the actual signature.</param>
    /// <returns><see cref="TransactionHash"/> - hash of successfully send transaction.</returns>
    Task<TransactionHash> SendAccountTransactionAsync(
        AccountAddress sender,
        Nonce nextAccountNonce,
        IAccountTransactionPayload transactionPayload,
        DateTimeOffset expiry,
        ITransactionSigner transactionSigner);
}
