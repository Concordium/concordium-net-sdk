namespace Concordium.Sdk.Types;

/// <summary>
/// A fraction of an amount with a precision of `1/100_000`.
/// </summary>
public sealed class AmountFraction
{
    private readonly uint _partsPerHundredThousands;
    private const decimal MultiplicationFactor = 1 / 100_000m;

    private AmountFraction(uint partsPerHundredThousands) => this._partsPerHundredThousands = partsPerHundredThousands;

    /// <summary>
    /// Get amount as decimal.
    /// </summary>
    public decimal AsDecimal() => this._partsPerHundredThousands * MultiplicationFactor;

    /// <summary>
    /// Constructs amount fraction from decimal.
    /// </summary>
    public static AmountFraction From(decimal number) => new((uint)Math.Floor(number / MultiplicationFactor));

    internal static AmountFraction From(Grpc.V2.AmountFraction fraction) => new(fraction.PartsPerHundredThousand);
}
