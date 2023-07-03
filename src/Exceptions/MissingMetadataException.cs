namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when entry in metadata from gRPC response is missing.
/// </summary>
public sealed class MissingMetadataException : Exception
{
    internal MissingMetadataException(string metadataKey) :
        base($"{metadataKey} entry was missing from metadata response")
    { }
}
