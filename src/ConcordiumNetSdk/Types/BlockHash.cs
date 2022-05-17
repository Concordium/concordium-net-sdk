namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base16 encoded hash of a block.
/// </summary>
public class BlockHash : Hash
{
    private BlockHash(string blockHashAsBase16String) : base(blockHashAsBase16String)
    {
    }

    private BlockHash(byte[] blockHashAsBytes) : base(blockHashAsBytes)
    {
    }

    /// <summary>
    /// Creates an instance from a base16 encoded string representing block hash (64 characters).
    /// </summary>
    /// <param name="blockHashAsBase16String">the block hash as base16 encoded string.</param>  
    public static BlockHash From(string blockHashAsBase16String)
    {
        return new BlockHash(blockHashAsBase16String);
    }

    /// <summary>
    /// Creates an instance from a 32 bytes representing block hash.
    /// </summary>
    /// <param name="blockHashAsBytes">the block hash as 32 bytes.</param>
    public static BlockHash From(byte[] blockHashAsBytes)
    {
        return new BlockHash(blockHashAsBytes);
    }
}
