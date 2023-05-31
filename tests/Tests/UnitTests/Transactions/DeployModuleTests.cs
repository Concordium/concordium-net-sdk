using System;
using System.IO;
using Concordium.Sdk.Transactions;
using Concordium.Sdk.Types;
using FluentAssertions;
using Xunit;

namespace Concordium.Sdk.Tests.UnitTests.Transactions;

public class DeployModuleTests
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeployModule"/> transaction.
    /// </summary>
    public static DeployModule CreateDeployModule(string modulePath, ModuleVersion version)
    {
        var moduleBytes = File.ReadAllBytes(modulePath);
        var moduleSource = ModuleSource.From(moduleBytes, version);
        return new DeployModule(moduleSource);
    }


    [Theory]
    [InlineData(
        "./module.wasm.v1",
        ModuleVersion.V1,
        "", // Expected signature for credential 0, key 0
        "", // Expected signature for credential 0, key 1
        ""  // Expected signature for credential 1, key 0
    )]
    public void Prepare_ThenSign_ProducesCorrectSignatures(string modulePath, ModuleVersion version, string sig0, string sig1, string sig2)
    {
        // Create the transfer.
        var transfer = CreateDeployModule(modulePath, version);

        // Prepare the transfer.
        var preparedTransfer =
            TransactionTestHelpers.CreatePreparedAccountTransaction(transfer);

        // Sign the transfer.
        var signedTransfer =
            TransactionTestHelpers.CreateSignedTransaction(preparedTransfer);

        // Create the expected signature. It was generated using the corresponding method in the Concordium Rust SDK.
        var expectedSignature00 = Convert.FromHexString(sig0);
        var expectedSignature01 = Convert.FromHexString(sig1);
        var expectedSignature11 = Convert.FromHexString(sig2);

        var expectedSignature = TransactionTestHelpers.FromExpectedSignatures(
            expectedSignature00,
            expectedSignature01,
            expectedSignature11
        );

        signedTransfer.Signature.SignatureMap
            .Should()
            .BeEquivalentTo(expectedSignature.SignatureMap);
    }
}
