namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents an index.
/// </summary>
public readonly struct Index
{
    private const int ByteMaxValue = 255;

    private readonly byte _value;

    private Index(byte value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets the index value.
    /// </summary>
    public byte Value => _value;
    
    /// <summary>
    /// Creates the instance of index.
    /// </summary>
    /// <param name="index">the index value.</param>
    public static Index Create(int index)
    {
        if (index < 0) throw new ArgumentException("Key index cannot be negative.");
        if (index > ByteMaxValue) throw new ArgumentException($"Key index cannot exceed the max value '{ByteMaxValue}'. Passed '{index}'.");

        return new Index((byte) index);
    }

    /// <summary>
    /// Serializes index to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized index in byte format.</returns>
    public byte[] SerializeToBytes()
    {
        return new byte[] {_value};
    }
}
