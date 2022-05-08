using ConcordiumNetSdk.Responses.AccountInfoResponse;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk;

public interface IConcordiumNodeClient
{
    /// <summary>
    /// Retrieves an information about a state of account corresponding to account address and block hash.
    /// </summary>
    /// <param name="accountAddress">the base-58 check with version byte 1 encoded address (with Bitcoin mapping table).</param>
    /// <param name="blockHash">the base-16 encoded hash of a block (64 characters).</param>
    /// <returns><see cref="AccountInfo"/> - state of an account in the given block.</returns>
    Task<AccountInfo?> GetAccountInfoAsync(AccountAddress accountAddress, BlockHash blockHash);
}
