namespace Concordium.Sdk.Types;

/// <summary>
/// Rate of creation of new CCDs. For example, A value of `0.05` would mean an
/// increase of 5 percent per unit of time. This value does not specify the time
/// unit, and this differs based on the protocol version.
///
/// The representation is base-10 floating point number representation.
/// The value is `mantissa * 10^(-exponent)`.
/// </summary>
public readonly record struct MintRate
{
    /// <summary>
    /// This will never exceed 255.
    /// </summary>
    private readonly uint _exponent;
    private readonly uint _mantissa;

    private MintRate(uint exponent, uint mantissa)
    {
        this._exponent = exponent;
        this._mantissa = mantissa;
    }

    internal static MintRate From(Grpc.V2.MintRate mintRate) => new(mintRate.Exponent, mintRate.Mantissa);

    /// <summary>
    /// Get Exponent and Mantissa
    /// </summary>
    public (uint Exponent, uint Mantissa) GetValues() => (this._exponent, this._mantissa);
}
