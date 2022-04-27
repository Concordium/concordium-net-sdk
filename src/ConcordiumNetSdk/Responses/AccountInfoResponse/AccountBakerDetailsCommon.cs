namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

public abstract class AccountBakerDetailsCommon
{
    public int BakerId { get; init; }

    public string StakedAmount { get; init; }

    public bool RestakeEarnings { get; init; }

    public string BakerElectionVerifyKey { get; init; }

    public string BakerSignatureVerifyKey { get; init; }

    public string BakerAggregationVerifyKey { get; init; }
}
