/*
using System;
using System.Threading.Tasks;
using ConcordiumNetSdk.SignKey;
using ConcordiumNetSdk.Transactions;
using ConcordiumNetSdk.Types;
using FluentAssertions;
using Moq;
using Xunit;
using Index = ConcordiumNetSdk.Types.Index;

namespace ConcordiumNetSdk.UnitTests.Transactions;

public class SendAccountTransactionTests
{
    public IConcordiumNodeClient ConcordiumNodeClient { get; }
    public IAccountTransactionService AccountTransactionService { get; }

    public SendAccountTransactionTests()
    {
        ConcordiumNodeClient = Mock.Of<IConcordiumNodeClient>();
        AccountTransactionService = new AccountTransactionService(ConcordiumNodeClient);

        Mock.Get(ConcordiumNodeClient)
            .Setup(x => x.SendTransactionAsync(It.IsAny<byte[]>(), It.IsAny<uint>()))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task When_account_exists_should_return_correct_data()
    {
        // Arrange
        var fromAccountAddress = AccountAddress.From("45rzWwzY8hXFxQEAPMpR19RZJafAQV7iA3p3WP8xso49cVqArP");
        var toAccountAddress = AccountAddress.From("3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw");
        var simpleTransferPayload = SimpleTransferPayload.Create(CcdAmount.FromCcd(100), toAccountAddress);
        var ed25519TransactionSigner = Ed25519SignKey.From("1ddce38dd4c6c4b98b9939542612e6a90928c35f8bbbf23aad218e888bb26fda");
        var transactionSigner = new TransactionSigner();
        transactionSigner.AddSignerEntry(Index.Create(0), Index.Create(0), ed25519TransactionSigner);
        var expiry = DateTimeOffset.MinValue.Add(TimeSpan.FromMinutes(30));
        var expectedTransactionHash = TransactionHash.From("d59d3a72ae4cb40f200da16a2a4b15db58b45dbb156129259e00b43806c3e650");

        // Act
        var actualTransactionHash = await AccountTransactionService.SendAccountTransactionAsync(
            fromAccountAddress,
            Nonce.Create(1),
            simpleTransferPayload,
            expiry,
            transactionSigner);

        // Assert
        actualTransactionHash.Should().Be(expectedTransactionHash);
    }
}

*/