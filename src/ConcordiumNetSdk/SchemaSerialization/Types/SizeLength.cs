namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a size length enum using in smart contract parameter schema.
/// </summary>
public enum SizeLength
{
    /// <summary>
    /// Takes 1 byte and represents a possible size range of 0..255.
    /// </summary>
    U8 = 0,

    /// <summary>
    /// Takes 2 bytes and represents a possible size range of 0..65535.
    /// </summary>
    U16,

    /// <summary>
    /// Takes 4 bytes and represents a possible size range of 0..4294967295.
    /// </summary>
    U32,
 
    /// <summary>
    /// Takes 8 bytes and represents a possible size range of 0..2^64-1.
    /// </summary>
    U64
}
