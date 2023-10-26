namespace Concordium.Sdk.Types;

/// <summary>Election difficulty parameter.</summary>
/// <param name="Difficulty">The election difficulty.</param>
public sealed record ElectionDifficulty(AmountFraction Difficulty)
{
    internal static ElectionDifficulty From(Grpc.V2.ElectionDifficulty electionDifficulty) =>
        new(AmountFraction.From(electionDifficulty.Value));
}
