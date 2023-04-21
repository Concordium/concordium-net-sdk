using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class NonceTests
{
    [Fact]
    public void Create_when_valid_ulong_value_passed_should_create_correct_instance()
    {
        var value = ulong.MaxValue;
        var nonce = AccountNonce.Create(value);
        nonce.GetValue().Should().Be(value);
    }

    [Fact]
    public void SerializeToBytes_should_return_bytes_in_UInt64_Big_Endian_format()
    {
        var nonce = AccountNonce.Create(100);
        var expectedSerializedNonce = new byte[] { 0, 0, 0, 0, 0, 0, 0, 100 };
        var serializedNonce = nonce.GetBytes();
        serializedNonce.Should().BeEquivalentTo(expectedSerializedNonce);
    }
}
