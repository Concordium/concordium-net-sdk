namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents an encrypted sign key.
/// </summary>
public record EncryptedSignKey(
    string Password,
    EncryptedSignKeyMetadata Metadata,
    CipherText CipherText);
