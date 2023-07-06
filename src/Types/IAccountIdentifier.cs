namespace Concordium.Sdk.Types;

/// <summary>
/// An account identifier used in queries.
/// </summary>
public interface IAccountIdentifier
{
    /// <summary>
    /// Converts type to account identifier input type.
    /// </summary>
    /// <returns></returns>
    public Grpc.V2.AccountIdentifierInput ToAccountIdentifierInput();
}


