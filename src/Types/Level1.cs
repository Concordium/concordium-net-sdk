using Concordium.Grpc.V2;
using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;


/// <summary>
/// An update with level 1 keys of either level 1 or level 2 keys.
/// </summary>
public sealed record Level1(ILevel1 Level1Update) : IUpdatePayload
{
    internal static Level1 From(Grpc.V2.Level1Update level1Update) => new(Level1Factory.From(level1Update));
}

/// <summary>
/// An update with level 1 keys of either level 1 or level 2 keys. Each of the
/// updates must be a separate transaction.
/// </summary>
public interface ILevel1{}

internal static class Level1Factory
{
    internal static ILevel1 From(Level1Update level1Update) =>
        level1Update.UpdateTypeCase switch
        {
            Level1Update.UpdateTypeOneofCase.Level1KeysUpdate =>
                Level1KeysUpdate.From(level1Update.Level1KeysUpdate),
            Level1Update.UpdateTypeOneofCase.Level2KeysUpdateV0 =>
                Level2KeysUpdate.From(level1Update.Level2KeysUpdateV0),
            Level1Update.UpdateTypeOneofCase.Level2KeysUpdateV1 =>
                Level2KeysUpdateV1.From(level1Update.Level2KeysUpdateV1),
            Level1Update.UpdateTypeOneofCase.None =>
                throw new MissingEnumException<Level1Update.UpdateTypeOneofCase>(level1Update.UpdateTypeCase),
            _ => throw new MissingEnumException<Level1Update.UpdateTypeOneofCase>(level1Update.UpdateTypeCase)
        };
}
