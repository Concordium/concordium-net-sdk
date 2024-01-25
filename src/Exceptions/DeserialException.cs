namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when deserialization fails and is explicitly meant not to.
/// </summary>
public class DeserialException : Exception
{
    internal DeserialException(string errorMessage) :
        base($"Deserialization error: {errorMessage}")
    { }
}

/// <summary>
/// Thrown when deserialization fails but no error message is present. This
/// should, by construction, be impossible.
/// </summary>
public sealed class DeserialNullException : DeserialException
{
    internal DeserialNullException() :
        base($"Deserialization error: The parsed output is null, but no error was found. This should not be possible.")
    { }
}

