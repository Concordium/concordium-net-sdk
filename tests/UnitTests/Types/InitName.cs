using System;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Types;

public sealed class InitNameTests
{
    [Fact]
    public void IsAlphaNumeric()
    {
        var init = new InitName("init_some_ascii_here");
    }

    [Fact]
    public void IsAlphaNumeric_Negative() => Assert.Throws<ArgumentException>(() => new InitName("init_åå"));

    [Fact]
    public void Max100Length()
    {
        var name = "init______1_________2_________3_________4_________5_________6_________7_________8_________9_________";
        var init = new InitName(name);
        Assert.Equal(100, name.Length);
        Assert.Throws<ArgumentException>(() => new InitName("init_åå"));
    }

    [Fact]
    public void Max100Length_Negative()
    {
        var name = "init______1_________2_________3_________4_________5_________6_________7_________8_________9_________0";
        Assert.Equal(101, name.Length);
        Assert.Throws<ArgumentException>(() => new InitName(name));
    }

    [Fact]
    public void StartsWithInit_Negative()
    {
        Assert.Throws<ArgumentException>(() => new InitName("ini_contract"));
        Assert.Throws<ArgumentException>(() => new InitName("nit_contract"));
        Assert.Throws<ArgumentException>(() => new InitName("initcontract"));
    }

    [Fact]
    public void CanContainPunctuation()
    {
        var init = new InitName("init_,;:'\"(){}[]?!");
    }

    [Fact]
    public void DeserializesCorrectly()
    {
        var name = new InitName("init_name");
        var bytes = new byte[] {
            0,
            9,
            105,
            110,
            105,
            116,
            95,
            110,
            97,
            109,
            101
        };
        Assert.Equal(name.ToBytes(), bytes);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var name = new InitName("init_name");
        var x = InitName.TryDeserial(name.ToBytes(), out var deserial);
        if (x)
        {
            name.Should().Be(deserial.Name);
        }
        else
        {
            Assert.Fail(deserial.Error);
        }
    }
}
