using Concordium.Sdk.Exceptions;
using Google.Protobuf;

namespace Concordium.Sdk.Types;

/// An address of either a contract or an account.
public sealed class Address
{
    /// <summary>
    /// Address of a account.
    /// Should only be called after TypeCase check.
    /// </summary>
    public AccountAddress AccountAddress { get; init; }
    /// <summary>
    /// Address of a contract.
    /// Should only be called after TypeCase check.  
    /// </summary>
    public ContractAddress ContractAddress { get; init; }
    /// <summary>
    /// Specify if this is a Account or Contract address. Should always be called first
    /// to determine address type.
    /// </summary>
    public AddressTypeCase TypeCase { get; init; }
    
    internal Address(Grpc.V2.Address address)
    {
        switch (address.TypeCase)
        {
            case Grpc.V2.Address.TypeOneofCase.Account:
                TypeCase = AddressTypeCase.Account;
                AccountAddress = AccountAddress.From(address.Account.ToByteArray());
                break;
            case Grpc.V2.Address.TypeOneofCase.Contract:
                TypeCase = AddressTypeCase.Contract;
                ContractAddress = ContractAddress.From(address.Contract);
                break;
            case Grpc.V2.Address.TypeOneofCase.None:
            default:
                throw new MissingEnumException<Grpc.V2.Address.TypeOneofCase>(address.TypeCase);
        }
    }

    /// <summary>
    /// Specify address type.
    /// </summary>
    public enum AddressTypeCase
    {
        Contract,
        Account
    }
}