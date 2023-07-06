using System;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Client;

public sealed class ConcordiumClientOptionsTests
{
    [Fact]
    public void GivenTimeout_WhenAddAppSettings_ThenParse()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("./Data/appsettingsTimeout.json")
            .Build();
        var expected = TimeSpan.FromSeconds(42);

        // Act
        var options = configuration.GetSection("ConcordiumClientOptions")
            .Get<Sdk.Client.ConcordiumClientOptions>();

        // Assert
        options.Timeout.Should().NotBeNull();
        options.Timeout.Should().Be(expected);
    }
}
