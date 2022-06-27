using ConcordiumNetSdk.SchemaSerialization;
using ConcordiumNetSdk.SchemaSerialization.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents an init contract parameter.
/// </summary>
public class InitContractParameter
{
    private readonly bool _isEmpty;

    private InitContractParameter(
        string contractName,
        object userInput,
        Module module,
        bool isEmpty = false)
    {
        ContractName = contractName;
        UserInput = userInput;
        Module = module;
        _isEmpty = isEmpty;
    }

    /// <summary>
    /// Gets the contract name.
    /// </summary>
    public string ContractName { get; }

    /// <summary>
    /// Gets the input parameter arguments specified by user.
    /// </summary>
    public object UserInput { get; }

    /// <summary>
    /// Gets the module that represents different contracts and their function parameter schemas.
    /// </summary>
    public Module Module { get; }

    /// <summary>
    /// Creates an init contract parameters.
    /// </summary>
    /// <param name="contractName">the contract name.</param>
    /// <param name="userInput">the input parameter argument specified by user.</param>
    /// <param name="module">the module that represents different contracts and their function parameter schemas.</param>
    public static InitContractParameter Create(
        string contractName,
        object userInput,
        Module module)
    {
        return new InitContractParameter(
            contractName,
            userInput,
            module);
    }

    /// <summary>
    /// Creates an empty init contract parameter.
    /// </summary>
    public static InitContractParameter Empty()
    {
        return new InitContractParameter(
            String.Empty,
            new object(),
            new Module(new Dictionary<string, Contract>()),
            true);    
    }

    /// <summary>
    /// Serializes init contract parameter to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized parameter in byte format.</returns>
    public byte[] SerializeToBytes()
    {
        if (_isEmpty) return Array.Empty<byte>();
        return InitContractParametersSerializer.Serialize(ContractName, UserInput, Module);
    }
}
