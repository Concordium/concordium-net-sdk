namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models a contract sub-index.
///
/// A contract is identified by its unique contract address (modeled
/// by <see cref="ConcordiumNetSdk.Types.ContractAddress"/> in the SDK).
/// A contract address consists of a contract index (modeled by
/// <see cref="ContractIndex"/>) and a contract sub-index which is a
/// 64-bit value.
/// </summary>
public readonly struct ContractSubIndex
{
    public const UInt32 BytesLength = sizeof(UInt64);
    private readonly UInt64 Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractSubIndex"/> class.
    /// </summary>
    /// <param name="value">A contract index represented by a <see cref="UInt64"/>.</param>
    public ContractSubIndex(UInt64 value)
    {
        Value = value;
    }

    public static implicit operator ContractSubIndex(UInt64 value)
    {
        return new ContractSubIndex(value);
    }

    public static implicit operator UInt64(ContractSubIndex contractSubIndex)
    {
        return contractSubIndex.Value;
    }
}
