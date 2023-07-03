using System;
using System.Text;
using Concordium.Sdk.Crypto;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.SignKey;

public sealed class Ed25519SignKeyTests
{
    [Fact]
    public void From_OnValidString_ToString_AreEqual()
    {
        var ed25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsHexString);
        ed25519SignKey.ToString().Should().Be(ed25519SignKeyAsHexString);
    }

    [Fact]
    public void From_OnValidBytes_ToBytes_AreEqual()
    {
        var ed25519SignKeyAsBytes = Convert.FromHexString(
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda"
        );
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsBytes);
        ed25519SignKey.ToBytes().Should().BeEquivalentTo(ed25519SignKeyAsBytes);
    }

    [Fact]
    public void From_TooShortString_ThrowsException()
    {
        var shortEd25519SignKeyAsHexString = "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbb";
        Action result = () => Ed25519SignKey.From(shortEd25519SignKeyAsHexString);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_TooLongString_ThrowsException()
    {
        var invalidEd25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fdaaa";
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsHexString);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_WrongParityHexString_ThrowsException()
    {
        var shortEd25519SignKeyAsHexString = "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbb";
        Action result = () => Ed25519SignKey.From(shortEd25519SignKeyAsHexString);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_NonHexString_ThrowsException()
    {
        var shortEd25519SignKeyAsHexString = "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbQ";
        Action result = () => Ed25519SignKey.From(shortEd25519SignKeyAsHexString);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_TooFewBytes_ThrowsException()
    {
        var invalidEd25519SignKeyAsBytes = new byte[63];
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_TooManyBytes_ThrowsException()
    {
        var invalidEd25519SignKeyAsBytes = new byte[65];
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Sign_ReturnsCorrectValue()
    {
        var ed25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsHexString);
        var bytesToBeEncoded = Encoding.ASCII.GetBytes(ed25519SignKeyAsHexString);
        // The expected signature was generated using the corresponding methods
        // in the Concordium Rust SDK.
        var expectedSignedBytes = new byte[]
        {
            153,
            69,
            0,
            90,
            66,
            134,
            134,
            131,
            45,
            249,
            200,
            222,
            65,
            211,
            121,
            228,
            215,
            116,
            69,
            132,
            17,
            150,
            12,
            243,
            17,
            126,
            56,
            17,
            141,
            170,
            158,
            10,
            197,
            204,
            186,
            148,
            36,
            118,
            113,
            88,
            132,
            52,
            32,
            201,
            176,
            196,
            30,
            239,
            138,
            149,
            140,
            74,
            35,
            241,
            4,
            33,
            251,
            0,
            33,
            74,
            234,
            45,
            162,
            8
        };
        var signedBytes = ed25519SignKey.Sign(bytesToBeEncoded);
        signedBytes.Should().BeEquivalentTo(expectedSignedBytes);
    }
}
