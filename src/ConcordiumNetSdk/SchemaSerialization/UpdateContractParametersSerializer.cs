using ConcordiumNetSdk.SchemaSerialization.Types;
using Type = ConcordiumNetSdk.SchemaSerialization.Types.Type;

namespace ConcordiumNetSdk.SchemaSerialization;

/// <summary>
/// Represents an update smart contract parameter serializer.
/// </summary>
public static class UpdateContractParametersSerializer
{
    /// <summary>
    /// Serializes update smart contract parameter to byte format.
    /// </summary>
    /// <param name="contractName">the contract name.</param>
    /// <param name="receiveFunctionName">the receive function name.</param>
    /// <param name="userInput">the input parameter argument specified by user.</param>
    /// <param name="module">the module that represents different contracts and their function parameter schemas.</param>
    /// <returns><see cref="T:byte[]"/> - serialized update smart contract parameter in byte format.</returns>
    public static byte[] Serialize(
        string contractName,
        string receiveFunctionName,
        object userInput,
        Module module)
    {
        if (!module.ContractSchemas.TryGetValue(contractName, out Contract? contract)) throw new ArgumentException("Schema module not found. Please provide a valid schema file.");
        if (!contract.Receive.TryGetValue(receiveFunctionName, out Type? receiveType)) throw new ArgumentException("Could not find the receive function name provided.");
        return ContractParametersSerializer.Serialize(receiveType, userInput);
    }
}
