using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Models a contract address.
///
/// A contract is identified by its unique <see cref="ContractAddress"/>.
/// A contract address consists of a <see cref="ContractIndex"/> and a
/// <see cref="ContractSubIndex"/> both represented as 64-bit unsigned
/// integers.
/// </summary>
public readonly struct ContractAddress : IEquatable<ContractAddress>
{
    public const UInt32 BytesLength = ContractIndex.BytesLength + ContractSubIndex.BytesLength;

    /// <summary>
    /// The index part of the address of a contract.
    /// </summary>
    public readonly ContractIndex _index { get; init; }

    /// <summary>
    /// The sub-index part of the address of a contract.
    /// </summary>
    public readonly ContractSubIndex _subIndex { get; init; }

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

    public bool Equals(ContractAddress contractAddress)
    {
        return _index == contractAddress._index && _subIndex == contractAddress._subIndex;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (obj.GetType() != GetType())
            return false;

        var other = (Memo)obj;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        byte[] indexBytes = Serialization.GetBytes(_index);
        byte[] subIndexBytes = Serialization.GetBytes(_index);
        return indexBytes.Concat(subIndexBytes).GetHashCode();
    }

    public static bool operator ==(ContractAddress? left, ContractAddress? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ContractAddress? left, ContractAddress? right)
    {
        return !Equals(left, right);
    }
}
