using System;
using Concordium.Sdk.Types;
using FluentAssertions;
using Google.Protobuf;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public class ModuleReferenceTests
{
    [Fact]
    public void WhenUsingSameModuleReference_ThenEquals()
    {
        // Arrange
        var fromHexString = Convert.FromHexString("c2a8e8c81e72fabc3355ab38e4e91b7173ad770b8cdd2106276f073945192060");
        var base64String = Convert.ToBase64String(fromHexString);
        var first = new ModuleReference(ByteString.FromBase64(base64String));
        var second = new ModuleReference(ByteString.FromBase64(base64String));

        // Act
        var equals = first == second;

        // Assert
        equals.Should().BeTrue();
    }
}
