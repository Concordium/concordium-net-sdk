namespace Concordium.Sdk.Types;

public class ContractInitializedEvent
{
    public ContractVersion ContractVersion { get; init; }
    public ModuleReference ModuleReference { get; init; }
    public ContractAddress ContractAddress { get; init; }
    public CcdAmount Amount { get; init; }
    public OwnedContractName InitName { get; init; }
    public IList<ContractEvent> Events { get; init; }
    
    internal ContractInitializedEvent(Concordium.Grpc.V2.ContractInitializedEvent initializedEvent)
    {
        ContractVersion = initializedEvent.ContractVersion switch
        {
            Grpc.V2.ContractVersion.V0 => ContractVersion.V0,
            Grpc.V2.ContractVersion.V1 => ContractVersion.V1,
        };
        ModuleReference = new ModuleReference(new HashBytes(initializedEvent.OriginRef.Value));
        ContractAddress = ContractAddress.From(initializedEvent.Address);
        Amount = CcdAmount.FromMicroCcd(initializedEvent.Amount.Value);
        InitName = new OwnedContractName(initializedEvent.InitName.Value);
        Events = initializedEvent.Events
            .Select(e => new ContractEvent(e.Value.ToByteArray()))
            .ToList();
    }
}