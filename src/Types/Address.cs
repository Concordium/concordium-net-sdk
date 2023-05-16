using Concordium.Sdk.Exceptions;
using Google.Protobuf;

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
public interface IAddress{}

/// <summary>
/// Creates a address dependent on address returned from generated address code.
/// </summary>
internal static class AddressFactory
{
    internal static IAddress CreateAddress(Grpc.V2.Address address) =>
        address.TypeCase switch
        {
            Grpc.V2.Address.TypeOneofCase.Account => AccountAddress.From(address.Account.ToByteArray()),
            Grpc.V2.Address.TypeOneofCase.Contract => ContractAddress.From(address.Contract),
            Grpc.V2.Address.TypeOneofCase.None => throw new MissingEnumException<Grpc.V2.Address.TypeOneofCase>(
                address.TypeCase),
            _ => throw new MissingEnumException<Grpc.V2.Address.TypeOneofCase>(address.TypeCase)
        };
}
