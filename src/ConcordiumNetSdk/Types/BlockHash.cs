namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base-16 encoded hash of a block (64 characters).
/// </summary>
public class BlockHash : Hash
{
    private BlockHash(byte[] bytes) : base(bytes)
    {
    }

    private BlockHash(string base16EncodedBlockHash) : base(base16EncodedBlockHash)
    {
    }

    /// <summary>
    /// Creates an instance from a 32 byte block hash.
    /// </summary>
    /// <param name="bytes">32 byte block hash.</param>
    public static BlockHash From(byte[] bytes)
    {
        return new BlockHash(bytes);
    }

    /// <summary>
    /// Creates an instance from a base-16 encoded string block hash.
    /// </summary>
    /// <param name="base16EncodedBlockHash">base-16 encoded block hash.</param>
    public static BlockHash From(string base16EncodedBlockHash)
    {
        return new BlockHash(base16EncodedBlockHash);
    }
}