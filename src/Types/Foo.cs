namespace Concordium.Sdk.Types;

public record struct AccountIndex(ulong Index);

public record struct BakerId(AccountIndex Id);

public record struct ModuleReference(HashBytes ModuleRef);

public record struct OwnedContractName(string OwnedContract);

public record struct OwnedReceiveName(string OwnedReceive);

public record struct OwnedParameter(byte[] OwnedParam);

public enum ContractVersion
{
    V0 = 0,
    V1 = 1
}                            

public record struct ContractEvent(byte[] Bytes);