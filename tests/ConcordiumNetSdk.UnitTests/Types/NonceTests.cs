using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class NonceTests
{
    [Fact]
    public void Create_when_valid_ulong_value_passed_should_create_correct_instance()
    {
        // Arrange
        var value = ulong.MaxValue;

        // Act
        var nonce = Nonce.Create(value);

        // Assert
        nonce.AsUInt64.Should().Be(value);
    }

    [Fact]
    public void SerializeToBytes_should_return_bytes_in_UInt64_Big_Endian_format()
    {
        // Arrange
        var nonce = Nonce.Create(100);
        var expectedSerializedNonce = new byte[] {0, 0, 0, 0, 0, 0, 0, 100};

        // Act
        var serializedNonce = nonce.SerializeToBytes();

        // Assert
        serializedNonce.Should().BeEquivalentTo(expectedSerializedNonce);
    }
}
