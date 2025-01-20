namespace Concordium.Sdk.Types;

/// <summary>
/// Validator score parameters (introduced in Concordium Protocol 8).
/// </summary>
/// <param name="MaximumMissedRounds">The maximal number of missed rounds before a validator gets suspended.</param>
public sealed record ValidatorScoreParameters(
    ulong MaximumMissedRounds
)
{
    internal static ValidatorScoreParameters From(Grpc.V2.ValidatorScoreParameters param) =>
        new(param.MaximumMissedRounds);
}
