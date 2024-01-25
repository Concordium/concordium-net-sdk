using System.Buffers.Binary;
using Concordium.Sdk.Helpers;

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
    /// Byte size of <see cref="ContractAddress"/>.
    /// </summary>
    public const uint BytesLength = 16;

    /// <summary>
    /// Converts the contract address to its corresponding protocol buffer message instance.
    ///
    /// This can be used as the input for class methods of <see cref="Client.RawClient"/>.
    /// </summary>
    public Grpc.V2.ContractAddress ToProto() => new() { Index = this.Index, Subindex = this.SubIndex };

    /// <summary>
    /// Attempt to deserialize a contract address from a span of bytes.
    /// </summary>
    /// <param name="bytes">The span of bytes.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (ContractAddress? Address, string? Error) output)
    {
        if (bytes.Length < BytesLength)
        {
            var msg = $"Invalid length of input in `ContractAddress.TryDeserial`. Expected at least {BytesLength} bytes, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        var index = BinaryPrimitives.ReadUInt64BigEndian(bytes);
        var subindex = BinaryPrimitives.ReadUInt64BigEndian(bytes);

        output = (new ContractAddress(index, subindex), null);
        return true;
    }

    /// <summary>
    /// Serialize the ContractAddress in big-endian format.
    /// </summary>
    public byte[] ToBytes()
    {
        using var memoryStream = new MemoryStream((int)BytesLength); // Safe to cast since a payload will never be
        memoryStream.Write(Serialization.ToBytes(this.Index));
        memoryStream.Write(Serialization.ToBytes(this.SubIndex));
        return memoryStream.ToArray();
    }
}
