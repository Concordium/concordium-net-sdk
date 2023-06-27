namespace Concordium.Sdk.Types;

/// <summary>
/// The init name of a smart contract function. Expected format:
/// "init_&lt;contract_name&gt;". It must only consist of at most 100 ASCII
/// alphanumeric or punctuation characters, must not contain a '.' and must
/// start with 'init_'.
/// </summary>
/// /// <param name="Name">A contract name with format: "init_&lt;contract_name&gt;".</param>
public sealed record InitName(string Name)
{
    internal static InitName From(Grpc.V2.InitName initName) => new(initName.Value);
}
