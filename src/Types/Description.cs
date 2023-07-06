namespace Concordium.Sdk.Types;

/// <summary>
/// Description either of an anonymity revoker or identity provider.
/// Metadata that should be visible on the chain.
/// </summary>
public sealed record Description(string Name, string Url, string Info)
{
    internal static Description From(Grpc.V2.Description description) => new(description.Name, description.Url, description.Description_);
}
