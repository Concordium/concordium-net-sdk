using System.Security.Cryptography;
using ConcordiumNetSdk.SignKey;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.SignKeyTests;

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
        var expectedSignKey = "b53af4521a678b015bbae217277933e87b978a48a9a07d55cc369cdf5e1ac215";
        var encryptedSignKeyMetadata = new EncryptedSignKeyMetadata(
            new Salt("QsY4+h31LMs974pPN6QfsA=="),
            new InitializationVector("kzyQ24xum3WibCKfvngMlg=="),
            100000,
            HashAlgorithmName.SHA256);
        var encryptedSignKey = new EncryptedSignKey(
            "111111",
            encryptedSignKeyMetadata,
            new CipherText("9hTfvFaDb/AYD9xXZ2LVnJ2FrHQhP+daUOP3l6m1tKdP6sPrpvucnA1xcuSgjiX3jfLWCJYEvUMv8oubObe410tJU/PfRZeQeB4xUDs04eE="));

        // Act
        var actualSignKey = SignKeyEncryption.Decrypt(encryptedSignKey);
        
        // Assert
        actualSignKey.AsString.Should().Be(expectedSignKey);
    }
}
