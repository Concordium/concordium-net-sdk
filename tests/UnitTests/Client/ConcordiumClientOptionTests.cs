using System;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Client;

public sealed class ConcordiumClientOptionsTests
{
    [Fact]
    public void WhenUriIsNull_ThenParseAsNull()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("./Data/appsettingsTimeout.json")
            .Build();

        // Act
        var options = configuration.GetSection("ConcordiumClientOptions")
            .Get<Sdk.Client.ConcordiumClientOptions>();

        // Assert
        options.Endpoint.Should().BeNull();
    }


    [Fact]
    public void GivenUri_WhenAddAppSettings_ThenParse()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("./Data/appsettingsEndpoint.json")
            .Build();

        // Act
        var options = configuration.GetSection("ConcordiumClientOptions")
            .Get<Sdk.Client.ConcordiumClientOptions>();

        // Assert
        options.Endpoint.Should().NotBeNull();
        options.Endpoint.Host.Should().Be("foobar.com");
    }

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
