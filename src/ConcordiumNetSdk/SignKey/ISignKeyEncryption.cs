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
    /// <returns><see cref="string"/> - hex encoded sign key.</returns>
    string Decrypt(EncryptedSignKey encryptedSignKey);
}
