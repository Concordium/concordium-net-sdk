namespace ConcordiumNetSdk.Types;

public readonly struct Index
{
    private readonly byte _value;

    private const int ByteMaxValue = 255;

    private Index(byte value)
    {
        _value = value;
    }

    public static Index Create(int index)
    {
        if (index < 0) throw new ArgumentException("Key index cannot be negative.");
        if (index > ByteMaxValue) throw new ArgumentException($"Key index cannot exceed one byte {ByteMaxValue} was {index}.");

        return new Index((byte) index);
    }

    public byte Value => _value;
}
