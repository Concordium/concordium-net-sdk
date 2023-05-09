using Google.Protobuf;

namespace Concordium.Sdk.Types;

public class Address
{
    public AccountAddress AccountAddress { get; init; }
    public ContractAddress ContractAddress { get; init; }
    public AddressTypeCase TypeCase { get; init; }
    
    public Address(Grpc.V2.Address address)
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
                throw new MappingException<Grpc.V2.Address>(address);
            default:
                throw new MappingException<Grpc.V2.Address>(address);
        }
    }

    public enum AddressTypeCase
    {
        Contract,
        Account
    }
    
}