namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when deserialization fails and is explicitly meant not to.
/// </summary>
public sealed class DeserialException : Exception
{
    internal DeserialException(string errorMessage) :
        base($"Deserialization error: {errorMessage}")
    { }
}

