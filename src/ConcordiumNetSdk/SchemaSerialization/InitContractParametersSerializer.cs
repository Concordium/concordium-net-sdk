using ConcordiumNetSdk.SchemaSerialization.Types;
using Module = ConcordiumNetSdk.SchemaSerialization.Types.Module;
using Type = ConcordiumNetSdk.SchemaSerialization.Types.Type;

namespace ConcordiumNetSdk.SchemaSerialization;

/// <summary>
/// Represents an init smart contract parameters serializer.
/// </summary>
public static class InitContractParametersSerializer
{
    /// <summary>
    /// Serializes init smart contract parameter to byte format.
    /// </summary>
    /// <param name="contractName">the contract name.</param>
    /// <param name="userInput">the input parameter argument specified by user.</param>
    /// <param name="module">the module that represents different contracts and their function parameter schemas.</param>
    /// <returns><see cref="T:byte[]"/> - serialized init smart contract parameter in byte format.</returns>
    public static byte[] Serialize(
        string contractName,
        dynamic userInput,
        Module module)
    {
        if (!module.ContractSchemas.TryGetValue(contractName, out Contract? contract)) throw new ArgumentException("Module not found. Please provide a valid schema file.");
        Type? initParamType = contract.Init;
        return ContractParametersSerializer.Serialize(initParamType, userInput);
    }
}
