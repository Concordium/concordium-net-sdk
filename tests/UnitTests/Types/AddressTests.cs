using System;
using Concordium.Grpc.V2;
using Concordium.Sdk.Types;
using FluentAssertions;
using Google.Protobuf;
using Xunit;
using AccountAddress = Concordium.Sdk.Types.AccountAddress;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class AddressTests
{
    [Fact]
    public void WhenGivenGrpcAccountAddress_ThenMap()
    {
        // Arrange
        const string expected = "TIDFSgXWdUVHEOXdeTONB5WGRL9mN8JdEHjvgOThaIE=";
        var fromBase64 = ByteString.FromBase64(expected);

        var address = new Address(new Address
        {
            Account = new Grpc.V2.AccountAddress
            {
                Value = fromBase64
            }
        });

        // Act
        var from = AddressFactory.From(address);

        // Assert
        from.Should().BeOfType<AccountAddress>();
        var accountAddress = from as AccountAddress;
        var actual = Convert.ToBase64String(accountAddress!.AsSpan());
        actual.Should().Be(expected);
    }
}
