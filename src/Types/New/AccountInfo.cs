namespace Concordium.Sdk.Types.New;

public class AccountInfo
{
    public Nonce AccountNonce { get; init; }
    public CcdAmount AccountAmount { get; init; }
    public ulong AccountIndex { get; init; }
    public AccountAddress AccountAddress { get; init; }
    public AccountBaker? AccountBaker { get; init; }
    public AccountDelegation? AccountDelegation { get; init; }

    internal static AccountInfo From(Grpc.V2.AccountInfo accountInfoAsync)
    {
        throw new NotImplementedException();
    }
}
