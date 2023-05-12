namespace Concordium.Sdk.Types;

/// <summary>
/// Parameter to the init function or entrypoint. Owned version.
/// </summary>
public record struct OwnedParameter(byte[] OwnedParam);