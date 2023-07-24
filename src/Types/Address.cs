using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// A interface implemented by addresses like Account- and Contract address.
/// </summary>
/// <example>
/// <code>
/// internal static void Example(Grpc.V2.Address address)
/// {
///     var addressMapped = CreateAddress(address);
///
///     switch (addressMapped)
///     {
///         case AccountAddress aa:
///             Console.WriteLine($"Do something with: {aa}");
///             break;
///         case ContractAddress ca:
///             Console.WriteLine($"Do something with: {ca}");
///             break;
///     }
/// }
/// </code>
/// </example>
public interface IAddress { }

/// <summary>
/// Creates a address dependent on address returned from generated address code.
/// </summary>
internal static class AddressFactory
{
    /// <summary>
    /// Create a instance of a address
    /// </summary>
    /// <param name="address">Address returned in response</param>
    /// <returns>Address</returns>
    /// <exception cref="MissingEnumException{TypeOneofCase}">When address type not known</exception>
    internal static IAddress From(Grpc.V2.Address address) =>
        address.TypeCase switch
        {
            Grpc.V2.Address.TypeOneofCase.Account => AccountAddress.From(address.Account),
            Grpc.V2.Address.TypeOneofCase.Contract => ContractAddress.From(address.Contract),
            Grpc.V2.Address.TypeOneofCase.None => throw new MissingEnumException<Grpc.V2.Address.TypeOneofCase>(
                address.TypeCase),
            _ => throw new MissingEnumException<Grpc.V2.Address.TypeOneofCase>(address.TypeCase)
        };
}
