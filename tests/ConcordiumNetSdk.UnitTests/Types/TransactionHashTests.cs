﻿using System;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Xunit;

namespace ConcordiumNetSdk.UnitTests.Types;

public class TransactionHashTests
{
    [Fact]
    public void Same_TransactionHashes_AreEqual()
    {
        var transactionHashA = TransactionHash.From(
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        var transactionHashB = TransactionHash.From(
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        Assert.Equal(transactionHashA, transactionHashB);
    }

    [Fact]
    public void Different_TransactionHashes_AreNotEqual()
    {
        var transactionHashA = TransactionHash.From(
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        var transactionHashB = TransactionHash.From(
            "54c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af"
        );
        Assert.NotEqual(transactionHashA, transactionHashB);
    }

    [Fact]
    public void From_OnValidString_ToString_AreEqual()
    {
        var transactionHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);
        transactionHash.ToString().Should().Be(transactionHashAsBase16String);
    }

    [Theory]
    [InlineData("")]
    [InlineData("44c52f0dc89c")]
    [InlineData("44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494affffff")]
    [InlineData("æøå")]
    public void From_OnInvalidString_ThrowsException(string invalidTransactionHash)
    {
        Action result = () => TransactionHash.From(invalidTransactionHash);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_OnValidBytes_ToString_ReturnsCorrectValue()
    {
        var transactionHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);
        var transactionHashAsBytes = transactionHash.GetBytes();
        transactionHash.GetBytes().Should().BeEquivalentTo(transactionHashAsBytes);
    }

    [Fact]
    public void From_OnInvalidBytes_TooShort_ThrowsException()
    {
        var invalidHashAsBytes = new byte[2];
        Action result = () => TransactionHash.From(invalidHashAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_OnInvalidBytes_TooLong_ThrowsException()
    {
        var invalidHashAsBytes = new byte[33];
        Action result = () => TransactionHash.From(invalidHashAsBytes);
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetBytes_ReturnsCorrectValue()
    {
        var transactionHashAsBase16String =
            "44c52f0dc89c5244b494223c96f037b5e312572b4dc6658abe23832e3e5494af";
        var transactionHash = TransactionHash.From(transactionHashAsBase16String);
        var expectedTransactionHashAsBytes = Convert.FromHexString(transactionHashAsBase16String);
        var transactionHashAsBytes = transactionHash.GetBytes();
        transactionHashAsBytes.Should().BeEquivalentTo(expectedTransactionHashAsBytes);
    }
}