using System.Text;

namespace Concordium.Sdk.Types;

/// <summary>
/// Holds a uft8 encoded JSON structure.
/// </summary>
/// <param name="Bytes">Bytes which holds the utf8 encoded JSON.</param>
public sealed record Utf8Json(byte[] Bytes)
{
    /// <summary>
    /// Returns the utf8 encoded JSON structure as a string.
    /// </summary>
    /// <returns>JSON formatted string</returns>
    public override string ToString() => Encoding.UTF8.GetString(this.Bytes);
}
