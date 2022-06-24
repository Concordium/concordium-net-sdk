using ConcordiumNetSdk;
using ConcordiumNetSdk.Responses.TransactionStatusResponse;
using ConcordiumNetSdk.Types;

var jsonStr = @"{
""hash"": ""b3c35887c7d3e41c8016f80e7566c43545509af5c51638b58e47161988841e37"",
""sender"": ""45rzWwzY8hXFxQEAPMpR19RZJafAQV7iA3p3WP8xso49cVqArP"",
""cost"": ""354600"",
""energyCost"": 501,
""result"": {
    ""events"": [
    {
        ""amount"": ""100000000"",
        ""tag"": ""Transferred"",
        ""to"": {
            ""address"": ""3V3QhN4USoMB8FMnPFHx8zoLoJexv8f5ka1a1uS8sERoSrahbw"",
            ""type"": ""AddressAccount""
        },
        ""from"": {
            ""address"": ""45rzWwzY8hXFxQEAPMpR19RZJafAQV7iA3p3WP8xso49cVqArP"",
            ""type"": ""AddressAccount""
        }
    }
    ],
    ""outcome"": ""success""
},
""type"": {
    ""contents"": ""transfer"",
    ""type"": ""accountTransaction""
},
""index"": 0
}";
var res = CustomJsonSerializer.Deserialize<TransactionSummary>(jsonStr);
Console.WriteLine(res);


public record TestObject
{
    /// <summary>
    /// Gets or initiates the hash of the block (base 16 encoded).
    /// </summary>
    public TransactionStatusType Status { get; init; }

    /// <summary>
    /// Gets or initiates the list of JSON objects encoding the children of the block, similarly encoded.
    /// </summary>
    public ulong Outcomes { get; init; }
}
