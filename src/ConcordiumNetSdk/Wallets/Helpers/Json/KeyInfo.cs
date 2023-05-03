namespace ConcordiumNetSdk.Wallets.Helpers.Json;

public class KeyInfo
{
    public Dictionary<string, Key>? keys { get; set; }
    public int? threshold { get; set; }
}
