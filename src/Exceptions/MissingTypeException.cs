namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when matching a interface to a type isn't possible.
/// </summary>
/// <typeparam name="TInterface">Interface which the type should derive from.</typeparam>
public class MissingTypeException<TInterface> : Exception
{
    internal MissingTypeException(TInterface missing) :
        base($"{missing?.GetType()} is missing in switch from interface ${typeof(TInterface)} ")
    {
    }
}
