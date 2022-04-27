namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

// todo: ask do we need to implement AccountBaker hierarchy as in js sdk https://github.com/Concordium/concordium-node-sdk-js/blob/8d1e79b66a5756fe23625284b36e57cfc3a33894/src/types.ts#:~:text=export%20type-,AccountBakerDetailsV0,-%3D%20AccountBakerDetailsCommon%3B
public record AccountBaker
{
    public int BakerId { get; init; }

    public string StakedAmount { get; init; }

    public bool RestakeEarnings { get; init; }

    public string BakerElectionVerifyKey { get; init; }

    public string BakerSignatureVerifyKey { get; init; }

    public string BakerAggregationVerifyKey { get; init; }
}
