namespace Concordium.Sdk.Types;

/// <summary>
/// Internal short id of the baker.
/// </summary>
public readonly record struct BakerId(AccountIndex Id)
{
    internal static BakerId From(Grpc.V2.BakerId bakerId) => new(new AccountIndex(bakerId.Value));

    internal Grpc.V2.BakerId ToProto() => new() { Value = this.Id.Index };
}
