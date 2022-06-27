using ConcordiumNetSdk.SchemaSerialization;
using ConcordiumNetSdk.SchemaSerialization.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents an update contract parameter.
/// </summary>
public class UpdateContractParameter
{
    private readonly bool _isEmpty;

    private UpdateContractParameter(
        string contractName,
        string receiveFunctionName,
        object userInput,
        Module module,
        bool isEmpty = false)
    {
        ContractName = contractName;
        ReceiveFunctionName = receiveFunctionName;
        UserInput = userInput;
        Module = module;
        _isEmpty = isEmpty;
    }

    /// <summary>
    /// Gets the contract name.
    /// </summary>
    public string ContractName { get; }

    /// <summary>
    /// Gets the receive function name.
    /// </summary>
    public string ReceiveFunctionName { get; }

    /// <summary>
    /// Gets the input parameter arguments specified by user.
    /// </summary>
    public object UserInput { get; }

    /// <summary>
    /// Gets the module that represents different contracts and their function parameter schemas.
    /// </summary>
    public Module Module { get; }

    /// <summary>
    /// Creates an update contract parameter.
    /// </summary>
    /// <param name="contractName">the contract name.</param>
    /// <param name="receiveFunctionName">the receive function name.</param>
    /// <param name="userInput">the input parameter argument specified by user.</param>
    /// <param name="module">the module that represents different contracts and their function parameter schemas.</param>
    public static UpdateContractParameter Create(
        string contractName,
        string receiveFunctionName,
        object userInput,
        Module module)
    {
        return new UpdateContractParameter(
            contractName,
            receiveFunctionName,
            userInput,
            module);
    }

    /// <summary>
    /// Creates an empty update contract parameter.
    /// </summary>
    public static UpdateContractParameter Empty()
    {
        return new UpdateContractParameter(
            String.Empty,
            String.Empty,
            new object(),
            new Module(new Dictionary<string, Contract>()),
            true);    
    }

    /// <summary>
    /// Serializes update contract parameter to byte format.
    /// </summary>
    /// <returns><see cref="T:byte[]"/> - serialized parameter in byte format.</returns>
    public byte[] SerializeToBytes()
    {
        if (_isEmpty) return Array.Empty<byte>();
        return UpdateContractParametersSerializer.Serialize(ContractName, ReceiveFunctionName, UserInput, Module);
    }
}
