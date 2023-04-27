namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models a contract index.
///
/// A contract is identified by its unique contract address (modeled
/// by <see cref="ConcordiumNetSdk.Types.ContractAddress"/> in the SDK).
/// A contract address consists of a contract index which is a 64-bit
/// value and a contract subindex (modeled <see cref="ContractSubIndex"/>
/// in the SDK).
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
