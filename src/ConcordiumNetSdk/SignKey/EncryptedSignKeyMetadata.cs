using System.Security.Cryptography;

namespace ConcordiumNetSdk.SignKey;

/// <summary>
/// Represents an encrypted sign key metadata.
/// </summary>
public record EncryptedSignKeyMetadata(
    Salt Salt,
    InitializationVector InitializationVector,
    int Iterations,
    HashAlgorithmName HashAlgorithmName,
    int KeySize = 256);
