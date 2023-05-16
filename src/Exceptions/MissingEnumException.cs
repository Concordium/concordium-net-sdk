namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when a matched enum value could not be handled in a switch statement.
/// </summary>
public class MissingEnumException<T> : Exception where T : Enum
{                                  
    internal MissingEnumException(T missingEnum) :
        base($"{typeof(T)} had unmatched value {missingEnum.ToString()}")
    {}
}