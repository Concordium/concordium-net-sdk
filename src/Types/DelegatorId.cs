namespace Concordium.Sdk.Types;

/// <summary>
/// Internal short id of the delegator.
/// </summary>
public readonly record struct DelegatorId(AccountIndex Id)
{
    internal static DelegatorId From(Grpc.V2.DelegatorId id) => new(new AccountIndex(id.Id.Value));
}
