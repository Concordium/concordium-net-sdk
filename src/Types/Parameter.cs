namespace Concordium.Sdk.Types;

/// <summary>
/// Parameter to the init function or entrypoint.
/// </summary>
public sealed record Parameter(byte[] Param)
{
    /// <summary>
    /// Convert parameters to hex string.
    /// </summary>
    public string ToHex() => Convert.ToHexString(this.Param).ToLowerInvariant();
}
