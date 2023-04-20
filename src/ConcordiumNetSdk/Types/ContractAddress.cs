namespace ConcordiumNetSdk.Types;

/// <summary>
/// A contract address.
/// </summary>
public class ContractAddress
{
    public const int BytesLength = 16;

    /// <summary>
    /// The index part of the address of a contract.
    /// </summary>
    private UInt64 _index;

    /// <summary>
    /// The sub-index part of the address of a contract.
    /// </summary>
    public UInt64 _subIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractAddress"/> class.
    /// </summary>
    /// <param name="index">The contract index.</param>
    /// <param name="subIndex">The contract sub index.</param>
    private ContractAddress(UInt64 index, UInt64 subIndex)
    {
        _index = index;
        _subIndex = subIndex;
    }

    /// <summary>
    /// Get the index part of the address of a contract.
    /// </summary>
    public UInt64 GetIndex()
    {
        return _index;
    }

    /// <summary>
    /// Get the sub-index part of the address of a contract.
    /// </summary>
    public UInt64 GetSubIndex()
    {
        return _subIndex;
    }

    /// <summary>
    /// Creates an instance of contract address.
    /// </summary>
    /// <param name="index">the index value.</param>
    /// <param name="subIndex">the sub index value.</param>
    public static ContractAddress Create(UInt64 index, UInt64 subIndex)
    {
        return new ContractAddress(index, subIndex);
    }

    /// <summary>
    /// Converts the contract address to its corresponding protocol buffer message instance.
    /// </summary>
    public Concordium.V2.ContractAddress ToProto()
    {
        return new Concordium.V2.ContractAddress() { Index = _index, Subindex = _subIndex };
    }
}
