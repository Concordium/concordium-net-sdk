namespace Concordium.Sdk.Types;

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
    public readonly ulong Index { get; init; }

    /// <summary>
    /// The sub-index part of the address of a contract.
    /// </summary>
    public readonly ulong SubIndex { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractAddress"/> class.
    /// </summary>
    /// <param name="index">The contract index.</param>
    /// <param name="subIndex">The contract sub index.</param>
    private ContractAddress(ulong index, ulong subIndex)
    {
        this.Index = index;
        this.SubIndex = subIndex;
    }

    /// <summary>
    /// Creates an instance of contract address.
    /// </summary>
    /// <param name="index">the index value.</param>
    /// <param name="subIndex">the sub index value.</param>
    public static ContractAddress Create(ulong index, ulong subIndex) => new(index, subIndex);

    /// <summary>
    /// Converts the contract address to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="Client.RawClient"/>.
    /// </summary>
    public Grpc.V2.ContractAddress ToProto() => new() { Index = Index, Subindex = SubIndex };

    public bool Equals(ContractAddress other) => this.Index == other.Index && this.SubIndex == other.SubIndex;

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        var other = (ContractAddress)obj;
        return this.Equals(other);
    }

    public override int GetHashCode() => this.Index.GetHashCode() * this.SubIndex.GetHashCode();

    public static bool operator ==(ContractAddress? left, ContractAddress? right) => Equals(left, right);

    public static bool operator !=(ContractAddress? left, ContractAddress? right) => !Equals(left, right);
}
