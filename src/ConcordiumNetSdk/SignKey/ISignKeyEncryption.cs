namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents a sign key encryption.
/// </summary>
public interface ISignKeyEncryption
{
    /// <summary>
    /// Decrypts encrypted sign key.
    /// </summary>
    /// <param name="encryptedSignKey">encrypted sign key.</param>
    /// <returns><see cref="Ed25519SignKey"/> - hex encoded ed25519 sign key</returns>
    Ed25519SignKey Decrypt(EncryptedSignKey encryptedSignKey);
}
