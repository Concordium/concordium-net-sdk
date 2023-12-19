using Concordium.Grpc.V2;
using Google.Protobuf;
namespace Concordium.Sdk.Types;

/// <summary>
/// A reference to a smart contract module deployed on the chain.
/// </summary>
public sealed record ModuleReference : Hash
{
    internal ModuleReference(ByteString byteString) : base(byteString.ToArray())
    { }

    internal static ModuleReference From(ModuleRef moduleRef) => new(moduleRef.Value);

    internal ModuleRef Into() => new() { Value = ByteString.CopyFrom(this.AsSpan()) };

    /// <summary>
    /// Create a parameter from a byte array.
    /// </summary>
    /// <param name="bytes">The serialized parameters.</param>
    /// <param name="output">Where to write the result of the operation.</param>
    public static bool TryDeserial(ReadOnlySpan<byte> bytes, out (ModuleReference? Ref, string? Error) output)
    {
        if (bytes.Length < BytesLength)
        {
            var msg = $"Invalid length of input in `Hash.TryDeserial`. Expected at least {BytesLength}, found {bytes.Length}";
            output = (null, msg);
            return false;
        };

        output = (new ModuleReference(bytes[..BytesLength].ToArray()), null);
        return true;
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="hexString">A hash represented as a length-64 hex encoded string.</param>
    /// <exception cref="ArgumentException">The supplied string is not a 64-character hex encoded string.</exception>
    public ModuleReference(string hexString) : base(hexString) { }

    private ModuleReference(byte[] bytes) : base(bytes) { }
}
