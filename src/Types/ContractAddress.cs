namespace Concordium.Sdk.Types;

/// <summary>
/// Represents a contract address.
///
/// A contract is identified by its unique <see cref="ContractAddress"/>.
/// A contract address consists of a contract index and a contact sub-index
/// both represented as 64-bit unsigned integers.
/// </summary>
/// <param name="Index">The index part of the address of a contract.</param>
/// <param name="SubIndex">The sub-index part of the address of a contract.</param>
public sealed record ContractAddress(ulong Index, ulong SubIndex) : IAddress
{
    /// <summary>
    /// Creates an instance of contract address.
    /// </summary>
    /// <param name="index">the index value.</param>
    /// <param name="subIndex">the sub index value.</param>
    public static ContractAddress From(ulong index, ulong subIndex) => new(index, subIndex);

    internal static ContractAddress From(Grpc.V2.ContractAddress contractAddress) => new(contractAddress.Index, contractAddress.Subindex);

    /// <summary>
    /// Converts the contract address to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="Client.RawClient"/>.
    /// </summary>
    public Grpc.V2.ContractAddress ToProto() => new() { Index = this.Index, Subindex = this.SubIndex };
}
