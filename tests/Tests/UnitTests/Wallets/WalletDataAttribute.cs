using System.Collections.Generic;
using System.Reflection;

namespace Concordium.Sdk.Tests.UnitTests.Wallets;

/// <summary>
/// <see cref="EmbeddedResourceDataAttribute"/> used to specify data captured from
/// wallet key export formats embedded in the manifest as well as a key index, credential
/// index and a key value. This is used for the theory in <see cref="Xunit"/> tests of
/// wallets.
/// </summary>
public class WalletDataAttribute : EmbeddedResourceDataAttribute
{
    private readonly byte _keyIndex;
    private readonly byte _credentialIndex;
    private readonly string _key;

    /// <summary>
    /// Initializes a new instance of the <see cref="WalletDataAttribute"/> class.
    /// </summary>
    /// <param name="walletFilePath">A path to the manifest resource representing the exported wallet keys.</param>
    /// <param name="credentialIndex">A credential index.</param>
    /// <param name="keyIndex">A key index.</param>
    /// <param name="expectedKey">A string representing the expected key with the corresponding indices.</param>
    public WalletDataAttribute(
        string walletFilePath,
        byte credentialIndex,
        byte keyIndex,
        string expectedKey
    )
        : base(new string[] { walletFilePath })
    {
        this._credentialIndex = credentialIndex;
        this._keyIndex = keyIndex;
        this._key = expectedKey;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var result = new object[4];
        // Wallet file data.
        result[0] = ReadManifestData(this.Args[0]);
        // A credential index.
        result[1] = this._credentialIndex;
        // A key index.
        result[2] = this._keyIndex;
        // A sign key.
        result[3] = this._key;
        return new[] { result };
    }
}
