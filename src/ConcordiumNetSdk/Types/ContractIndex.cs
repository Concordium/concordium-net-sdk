namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a contract index.
///
/// A contract is identified by its unique <see cref="ContractAddress"/>.
/// A contract address consists of a <see cref="ContractIndex"/> and a
/// <see cref="ContractSubIndex"/> both represented as 64-bit unsigned
/// integers.
/// </summary>
public readonly struct ContractIndex
{
    public const UInt32 BytesLength = sizeof(UInt64);
    public readonly UInt64 Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractIndex"/> class.
    /// </summary>
    /// <param name="value">A contract index represented by a <see cref="UInt64"/>.</param>
    public ContractIndex(UInt64 value)
    {
        Value = value;
    }

    public static implicit operator ContractIndex(UInt64 value)
    {
        return new ContractIndex(value);
    }

    public static implicit operator UInt64(ContractIndex contractIndex)
    {
        return contractIndex.Value;
    }
}
