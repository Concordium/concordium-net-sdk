namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown if the node sends an invalid response.
/// </summary>
public class UnexpectedNodeResponseException : Exception
{
    internal UnexpectedNodeResponseException() :
        base($"Unexpected node response received.")
    { }
}
