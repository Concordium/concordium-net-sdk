namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when a matched enum value could not be handled in a switch statement.
/// </summary>
public sealed class DeserialException : Exception
{
    internal DeserialException(string errorMessage) :
        base($"Deserialization error: {errorMessage}")
    { }
}

