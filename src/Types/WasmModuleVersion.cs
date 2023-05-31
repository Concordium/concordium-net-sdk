namespace Concordium.Sdk.Types;

/// <summary>
/// Represents the version of a Wasm smart contract module.
/// </summary>
public enum WasmModuleVersion
{
    /// <summary>
    /// The initial smart contracts version. This has a simple state API that
    /// has very limited capacity. <c>V0</c> contracts also use message-passing as
    /// the interaction method.
    /// </summary>
    V0 = 0,
    /// <summary>
    /// <c>V1</c> contracts were introduced with protocol version 4. In comparison
    /// to <c>V0</c> contracts they use synchronous calls as the interaction method,
    /// and they have access to a more fine-grained state API allowing for unlimited
    /// (apart from NRG costs) state size.
    /// </summary>
    V1 = 1
}
