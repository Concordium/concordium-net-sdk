namespace Concordium.Sdk.Types;

/// <summary>
/// An identifier of a key that can sign update instructions. A signature of an
/// update instruction is a collection of signatures. An <see cref="UpdateKeysIndex"/>
/// identifies keys that correspond to the signatures.
/// </summary>
public readonly record struct UpdateKeysIndex(ushort Index)
{
    internal static UpdateKeysIndex From(Grpc.V2.UpdateKeysIndex index) => new((ushort)index.Value);
}
