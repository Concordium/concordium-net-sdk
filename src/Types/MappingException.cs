namespace Concordium.Sdk.Types;

public class MappingException<T> : Exception
{
    public MappingException(T errorObject) : 
        base($"not able to map returned object {errorObject}") 
    {}
}