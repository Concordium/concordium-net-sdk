using ConcordiumNetSdk.Types;
using ConcordiumNetSdk.Crypto;

namespace ConcordiumNetSdk.Wallets.Json;

/// <summary>
/// Represents (a subset of) a JSON object indexed by the
/// <c>value</c> field of the browser and genesis wallet
/// export JSON formats.
///
/// Such can be parsed into an instance of this class using
/// <see cref="Newtonsoft.Json.JsonConvert"/>.
/// </summary>
public class Value
{
    public AccountKeys? accountKeys { get; set; }
    public string? address { get; set; }
}
