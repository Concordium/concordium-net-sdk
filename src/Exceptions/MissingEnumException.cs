namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when a enum is matches in a switch statements which isn't handled.
/// </summary>
public class MissingEnumException<T> : Exception where T : Enum
{                                  
    internal MissingEnumException(T missingEnum) :
        base($"{typeof(T)} had unmatched value {missingEnum.ToString()}")
    {}
}