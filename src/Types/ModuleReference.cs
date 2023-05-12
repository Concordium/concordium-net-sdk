namespace Concordium.Sdk.Types;

/// <summary>
/// A reference to a smart contract module deployed on the chain.
/// </summary>
public record struct ModuleReference(HashBytes ModuleRef);