using Xunit;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using System;

namespace Concordium.Sdk.Tests.UnitTests.Client;

public class ConcordiumClientOptionsTests {
    
    [Fact]
    public void GivenUri_WhenAddAppSettings_ThenParse() {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("./Data/appsettingsEndpoint.json")
            .Build();

        // Act
        var options = configuration.GetSection("ConcordiumClientOptions")
            .Get<Concordium.Sdk.Client.ConcordiumClientOptions>();

        // Assert
        options.Endpoint.Should().NotBeNull();
    }

    [Fact]
    public void GivenTimeout_WhenAddAppSettings_ThenParse() {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("./Data/appsettingsTimeout.json")
            .Build();
        var expected = TimeSpan.FromSeconds(42);

        // Act
        var options = configuration.GetSection("ConcordiumClientOptions")
            .Get<Concordium.Sdk.Client.ConcordiumClientOptions>();

        // Assert
        options.Timeout.Should().NotBeNull();
        options.Timeout.Should().Be(expected);
    }
}