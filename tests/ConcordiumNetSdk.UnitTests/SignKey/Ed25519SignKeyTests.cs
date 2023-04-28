using System;
using System.Text;
using ConcordiumNetSdk.SignKey;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.SignKey;

public class Ed25519SignKeyTests
{
    [Fact]
    public void From_when_valid_string_value_passed_should_create_correct_instance()
    {
        var ed25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsHexString);
        ed25519SignKey.ToString().Should().Be(ed25519SignKeyAsHexString);
    }

    [Fact]
    public void From_when_string_value_is_too_short_should_throw_appropriate_exception()
    {
        var invalidEd25519SignKeyAsHexString = "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbb";
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsHexString);
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The sign key hex encoded string length must be 64.");
    }

    [Fact]
    public void From_when_string_value_is_too_long_should_throw_appropriate_exception()
    {
        var invalidEd25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fdaa";
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsHexString);
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The sign key hex encoded string length must be 64.");
    }

    [Fact]
    public void From_when_valid_bytes_value_passed_should_create_correct_instance()
    {
        var ed25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKeyAsBytes = Convert.FromHexString(ed25519SignKeyAsHexString);
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsBytes);
        ed25519SignKey.ToString().Should().Be(ed25519SignKeyAsHexString);
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_short_should_throw_appropriate_exception()
    {
        var invalidEd25519SignKeyAsBytes = new byte[63];
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsBytes);
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The sign key bytes length must be 32.");
    }

    [Fact]
    public void From_when_bytes_value_length_is_too_long_should_throw_appropriate_exception()
    {
        var invalidEd25519SignKeyAsBytes = new byte[65];
        Action result = () => Ed25519SignKey.From(invalidEd25519SignKeyAsBytes);
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The sign key bytes length must be 32.");
    }

    [Fact]
    public void AsString_should_return_correct_data()
    {
        var expectedEd25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKey = Ed25519SignKey.From(expectedEd25519SignKeyAsHexString);
        var ed25519SignKeyAsHexString = ed25519SignKey.ToString();
        ed25519SignKeyAsHexString.Should().BeEquivalentTo(expectedEd25519SignKeyAsHexString);
    }

    [Fact]
    public void AsBytes_should_return_correct_data()
    {
        var ed25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsHexString);
        var expectedEd25519SignKeyAsBytes = Convert.FromHexString(ed25519SignKeyAsHexString);
        var ed25519SignKeyAsBytes = ed25519SignKey.GetBytes();
        ed25519SignKeyAsBytes.Should().BeEquivalentTo(expectedEd25519SignKeyAsBytes);
    }

    [Fact]
    public void Sign_should_return_correct_signed_bytes()
    {
        var ed25519SignKeyAsHexString =
            "1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda";
        var ed25519SignKey = Ed25519SignKey.From(ed25519SignKeyAsHexString);
        var bytesToBeEncoded = Encoding.ASCII.GetBytes(ed25519SignKeyAsHexString);
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
