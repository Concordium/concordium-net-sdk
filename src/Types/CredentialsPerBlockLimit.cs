namespace Concordium.Sdk.Types;

/// <summary>
/// Limit on the number of credential deployments in a block. Since credential
/// deployments create accounts, this is in effect a limit on the number of
/// accounts that can be created in a block.
/// </summary>
public readonly record struct CredentialsPerBlockLimit(uint Limit)
{
    internal static CredentialsPerBlockLimit From(Grpc.V2.CredentialsPerBlockLimit credentialsPerBlockLimit) =>
        new(credentialsPerBlockLimit.Value);
}
