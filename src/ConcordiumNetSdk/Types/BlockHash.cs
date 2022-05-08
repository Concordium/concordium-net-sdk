namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base-16 encoded hash of a block.
/// </summary>
public class BlockHash : Hash
{
    /// <summary>
    /// Creates an instance from a base-16 encoded string representing block hash (64 characters).
    /// </summary>
    /// <param name="blockHashAsBase16String">the block hash as base16 encoded string.</param>  
    public BlockHash(string blockHashAsBase16String) : base(blockHashAsBase16String)
    {
    }

    /// <summary>
    /// Creates an instance from a 32 bytes representing block hash.
    /// </summary>
    /// <param name="blockHashAsBytes">the block hash as 32 bytes.</param>
    public BlockHash(byte[] blockHashAsBytes) : base(blockHashAsBytes)
    {
    }
}
