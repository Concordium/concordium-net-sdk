namespace Concordium.Sdk.Types;

/// <summary>
/// Parameter to the init function or entrypoint.
/// </summary>
public readonly record struct Parameter(byte[] Param);
