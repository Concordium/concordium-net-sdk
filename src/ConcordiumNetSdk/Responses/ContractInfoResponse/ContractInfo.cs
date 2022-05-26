using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Responses.ContractInfoResponse;

/// <summary>
/// Represents the information about a specific smart contract instance as response data of <see cref="IConcordiumNodeClient"/>.<see cref="IConcordiumNodeClient.GetInstanceInfoAsync"/>.
/// See <a href="https://github.com/Concordium/concordium-node/edit/main/docs/grpc.md#getinstanceinfo--blockhash---contractaddress---contractinfo">here</a>.
/// </summary>
public record ContractInfo
{
    /// <summary>
    /// Gets or initiates the amount of GTU the contract owns.
    /// </summary>
    public CcdAmount Amount  { get; init; }
    
    /// <summary>
    /// Gets or initiates the source module for the code of the smart contract.
    /// </summary>
    public ModuleRef SourceModule  { get; init; }
    
    /// <summary>
    /// Gets or initiates the address of the account that created the smart contract.
    /// </summary>
    public AccountAddress Owner { get; init; }
    
    /// <summary>
    /// Gets or initiates the list of methods that the smart contract supports.
    /// </summary>
    public string[] Methods { get; init; }
    
    /// <summary>
    /// Gets or initiates the name of the smart contract.
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Gets or initiates the hex-encoded state of the smart contract.
    /// </summary>
    public string Model { get; init; }
}
