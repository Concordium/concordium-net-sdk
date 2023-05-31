namespace Concordium.Sdk.Types.New;

public class AccountBaker
{
    public ulong BakerId { get; init; }
    public AccountBakerPendingChange? PendingChange { get; init; }
    public bool RestakeEarnings { get; init; }
    public CcdAmount StakedAmount { get; init; }
    public BakerPoolInfo? BakerPoolInfo  { get; init; }
}