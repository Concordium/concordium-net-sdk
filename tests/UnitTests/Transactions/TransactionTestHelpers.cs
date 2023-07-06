using System.Collections.Generic;
using Concordium.Sdk.Crypto;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Tests.UnitTests.Transactions;

public static class TransactionTestHelpers
{
    /// <summary>
    /// Prepares a transaction with the sender address set to
    /// <c>"3QuZ47NkUk5icdDSvnfX8HiJzCnSRjzi6KwGEmqgQ7hCXNBTWN"</c>,
    /// the account sequence number set to <c>123</c> and the expiry
    /// set to <c>65537</c> seconds after the UNIX epoch.
    /// </summary>
    /// <param name="transaction">The transaction to prepare for signing.</param>
    public static PreparedAccountTransaction CreatePreparedAccountTransaction(
        AccountTransactionPayload transaction
    )
    {
        var sender = AccountAddress.From("3QuZ47NkUk5icdDSvnfX8HiJzCnSRjzi6KwGEmqgQ7hCXNBTWN");
        var sequenceNumber = AccountSequenceNumber.From(123);
        var expiry = Expiry.From(65537);
        return transaction.Prepare(sender, sequenceNumber, expiry);
    }

    /// <summary>
    /// Signs a transaction.
    ///
    /// Uses an <see cref="ITransactionSigner"/> with the following secret keys:
    ///
    /// <c>"1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda"</c>
    /// at <see cref="AccountCredentialIndex"/> <c>0</c> and <see cref="AccountKeyIndex"/>
    /// <c>0</c>,
    ///
    /// <c>"68d7d0f3ae0581fd9b2b1c47daf1c9c7b5b8eddf3e48e4984ee16ca3c7efea32"</c>
    /// at <see cref="AccountCredentialIndex"/> <c>0</c> and <see cref="AccountKeyIndex"/>
    /// <c>1</c> and
    ///
    /// <c>"1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda"</c>
    /// at <see cref="AccountCredentialIndex"/> <c>1</c> and <see cref="AccountKeyIndex"/>
    /// <c>1</c>.
    /// </summary>
    /// <param name="preparedTransaction">The prepared transaction to sign.</param>
    public static SignedAccountTransaction CreateSignedTransaction(
        PreparedAccountTransaction preparedTransaction
    )
    {
        // Create a signer.
        var key00 = Ed25519SignKey.From(
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda"
        );
        var key01 = Ed25519SignKey.From(
            "68d7d0f3ae0581fd9b2b1c47daf1c9c7b5b8eddf3e48e4984ee16ca3c7efea32"
        );
        var key11 = Ed25519SignKey.From(
            "ebaf15cfd4182c98fdb81882591c9e96cf459870ebd1a0dda84288a7f9ab9211"
        );
        var signer = new TransactionSigner();
        signer.AddSignerEntry(new AccountCredentialIndex(0), new AccountKeyIndex(0), key00);
        signer.AddSignerEntry(new AccountCredentialIndex(0), new AccountKeyIndex(1), key01);
        signer.AddSignerEntry(new AccountCredentialIndex(1), new AccountKeyIndex(1), key11);

        // Sign the transfer using the signer.
        return preparedTransaction.Sign(signer);
    }

    /// <summary>
    /// Creates an account signature that is compatible with the signature of
    /// the output of <see cref="CreateSignedTransaction"/>.
    ///
    /// </summary>
    /// <param name="expectedSignature00">
    /// The expected signature produced by the key with <see cref="AccountCredentialIndex"/>
    /// <c>0</c> and <see cref="AccountKeyIndex"/> <c>0</c>.
    /// </param>
    /// <param name="expectedSignature01">
    /// The expected signature produced by the key with <see cref="AccountCredentialIndex"/>
    /// <c>0</c> and <see cref="AccountKeyIndex"/> <c>1</c>.
    /// </param>
    /// <param name="expectedSignature11">
    /// The expected signature produced by the key with <see cref="AccountCredentialIndex"/>
    /// <c>1</c> and <see cref="AccountKeyIndex"/> <c>1</c>.
    /// </param>
    public static AccountTransactionSignature FromExpectedSignatures(
        byte[] expectedSignature00,
        byte[] expectedSignature01,
        byte[] expectedSignature11
    )
    {
        var signatureDictionary = new Dictionary<
            AccountCredentialIndex,
            Dictionary<AccountKeyIndex, byte[]>
        >()
        {
            // Credential index 0.
            {
                new AccountCredentialIndex(0),
                new Dictionary<AccountKeyIndex, byte[]>()
                {
                    // Key index 0.
                    { new AccountKeyIndex(0), expectedSignature00 },
                    // Key index 1.
                    { new AccountKeyIndex(1), expectedSignature01 }
                }
            },
            // Credential index 1.
            {
                new AccountCredentialIndex(1),
                new Dictionary<AccountKeyIndex, byte[]>()
                {
                    // Key index 1.
                    { new AccountKeyIndex(1), expectedSignature11 }
                }
            },
        };

        return AccountTransactionSignature.Create(signatureDictionary);
    }
}
