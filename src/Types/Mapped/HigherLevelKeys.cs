namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// Either root or level1 access structure. They all have the same
/// structure, keys and a threshold. The enum type parameter is used for
/// to distinguish different access structures in different contexts.
/// </summary>
public class HigherLevelKeys
{
    /// <summary>
    /// Public keys that can sign updates.
    /// </summary>
    public IList<UpdatePublicKey> Keys { get; }
    /// <summary>
    /// A lower bound on the number of signatures needed to sign a valid update
    /// message of a particular type. This is never 0.
    /// </summary>
    public UpdateKeysThreshold Threshold { get; }
    private readonly KeysKind _kind;

    internal HigherLevelKeys(IList<UpdatePublicKey> keys, UpdateKeysThreshold threshold, KeysKind kind)
    {
        this.Keys = keys;
        this.Threshold = threshold;
        this._kind = kind;
    }

    /// <summary>
    /// A tag for added type safety when using HigherLevelKeys.
    /// It is meant to exist purely as a type-level marker.
    /// </summary>
    public enum KeysKind
    {
        Root,
        Level1
    }
}
