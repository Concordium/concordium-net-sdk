using ConcordiumNetSdk.Helpers;

namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a contract address.
///
/// A contract is identified by its unique <see cref="ContractAddress"/>.
/// A contract address consists of a contract index and a contact sub-index
/// both represented as 64-bit unsigned integers.
/// </summary>
public readonly struct ContractAddress : IEquatable<ContractAddress>
{
    /// <summary>
    /// The index part of the address of a contract.
    /// </summary>
    public readonly UInt64 Index { get; init; }

    /// <summary>
    /// The sub-index part of the address of a contract.
    /// </summary>
    public readonly UInt64 SubIndex { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractAddress"/> class.
    /// </summary>
    /// <param name="index">The contract index.</param>
    /// <param name="subIndex">The contract sub index.</param>
    private ContractAddress(UInt64 index, UInt64 subIndex)
    {
        Index = index;
        SubIndex = subIndex;
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
        return new Concordium.V2.ContractAddress() { Index = Index, Subindex = SubIndex };
    }

    public bool Equals(ContractAddress contractAddress)
    {
        return Index == contractAddress.Index && SubIndex == contractAddress.SubIndex;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (obj.GetType() != GetType())
            return false;

        var other = (ContractAddress)obj;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        byte[] indexBytes = Serialization.GetBytes(Index);
        byte[] subIndexBytes = Serialization.GetBytes(SubIndex);
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
