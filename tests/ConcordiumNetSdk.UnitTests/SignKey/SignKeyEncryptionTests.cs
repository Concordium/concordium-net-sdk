using System.Security.Cryptography;
using ConcordiumNetSdk.SignKey;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.SignKey;

public class SignKeyEncryptionTests
{
    public ISignKeyEncryption SignKeyEncryption { get; }

    public SignKeyEncryptionTests()
    {
        SignKeyEncryption = new SignKeyEncryption();
    }

    [Fact]
    public void Should_correctly_decrypt_encrypted_sign_key()
    {
        // Arrange
        var salt = Salt.From("QsY4+h31LMs974pPN6QfsA==");
        var initializationVector = InitializationVector.From("kzyQ24xum3WibCKfvngMlg==");
        var password = "111111";
        var cipherText = CipherText.From("9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE=");
        var encryptedSignKeyMetadata = new EncryptedSignKeyMetadata(salt, initializationVector, 100000, HashAlgorithmName.SHA256);
        var encryptedSignKey = new EncryptedSignKey(password, encryptedSignKeyMetadata, cipherText);
        var expectedSignKey = "b53af4521a678b015bbae217277933e87b978a48a9a07d55cc369cdf5e1ac215";

        // Act
        var signKey = SignKeyEncryption.Decrypt(encryptedSignKey);

        // Assert
        signKey.Should().Be(expectedSignKey);
    }
}
