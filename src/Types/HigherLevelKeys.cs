namespace Concordium.Sdk.Types;

/// <summary>
/// Either root or level1 access structure. They all have the same
/// structure, keys and a threshold. The enum type parameter is used for
/// to distinguish different access structures in different contexts.
/// </summary>
/// <param name="Keys">Public keys that can sign updates.</param>
/// <param name="Threshold">
/// A lower bound on the number of signatures needed to sign a valid update
/// message of a particular type. This is never 0.
/// </param>
public abstract record HigherLevelKeys(IList<UpdatePublicKey> Keys, UpdateKeysThreshold Threshold);

/// <summary>
/// Root Keys
/// </summary>
public sealed record RootKeys(IList<UpdatePublicKey> Keys, UpdateKeysThreshold Threshold) : HigherLevelKeys(Keys,
    Threshold)
{
    internal static RootKeys From(Grpc.V2.HigherLevelKeys keys) =>
        new(keys.Keys.Select(UpdatePublicKey.From).ToArray(), UpdateKeysThreshold.From(keys.Threshold));
}

/// <summary>
/// Level1 Keys
/// </summary>
public sealed record Level1Keys(IList<UpdatePublicKey> Keys, UpdateKeysThreshold Threshold) : HigherLevelKeys(Keys,
    Threshold)
{
    internal static Level1Keys From(Grpc.V2.HigherLevelKeys keys) =>
        new(keys.Keys.Select(UpdatePublicKey.From).ToArray(), UpdateKeysThreshold.From(keys.Threshold));
}
