using System.Security.Cryptography;
using System.Text;

namespace ConcordiumNetSdk.SignKey;

public class SignKeyEncryption : ISignKeyEncryption
{
    public Ed25519SignKey Decrypt(EncryptedSignKey encryptedSignKey)
    {
        byte[] initialVectorBytes = encryptedSignKey.Metadata.InitializationVector.AsBytes;
        byte[] saltValueBytes = encryptedSignKey.Metadata.Salt.AsBytes;
        byte[] cipherTextBytes = encryptedSignKey.CipherText.AsBytes;
        byte[] passwordBytes = Encoding.ASCII.GetBytes(encryptedSignKey.Password);
        int passwordIterations = encryptedSignKey.Metadata.Iterations;
        HashAlgorithmName hashAlgorithmName = encryptedSignKey.Metadata.HashAlgorithmName;
        int keySize = encryptedSignKey.Metadata.KeySize;
        var derivedPassword = new Rfc2898DeriveBytes(passwordBytes, saltValueBytes, passwordIterations, hashAlgorithmName);
        byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
        using var symmetricKey = Aes.Create();
        symmetricKey.Mode = CipherMode.CBC;
        Span<byte> plainTextBytes = new byte[cipherTextBytes.Length];
        using ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes);
        using var memStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
        int totalRead = 0;
        while (totalRead < plainTextBytes.Length)
        {
            int bytesRead = cryptoStream.Read(plainTextBytes.Slice(totalRead));
            if (bytesRead == 0) break;
            totalRead += bytesRead;
        }

        cryptoStream.Close();
        memStream.Close();
        symmetricKey.Clear();

        var signKey = Encoding.ASCII.GetString(plainTextBytes.Slice(1, 64));
        return new Ed25519SignKey(signKey);
    }
}
