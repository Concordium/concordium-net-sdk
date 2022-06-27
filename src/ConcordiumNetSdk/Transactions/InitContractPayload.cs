using System.Buffers.Binary;
using System.Text;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents an init contract transaction payload.
/// </summary>
public class InitContractPayload : IAccountTransactionPayload
{
    private InitContractPayload(
        CcdAmount amount,
        ModuleRef moduleRef,
        string initName,
        InitContractParameter parameter,
        ulong maxContractExecutionEnergy)
    {
        Amount = amount;
        ModuleRef = moduleRef;
        InitName = initName;
        Parameter = parameter;
        MaxContractExecutionEnergy = maxContractExecutionEnergy;
    }

    /// <summary>
    /// Gets the amount to transfer.
    /// </summary>
    public CcdAmount Amount { get; }

    /// <summary>
    /// Gets the deployed module reference.
    /// </summary>
    public ModuleRef ModuleRef { get; }

    // todo: think of making it as separate type (it should be passed with init_ or without)
    /// <summary>
    /// Gets the name of init function including 'init_' prefix.
    /// </summary>
    public string InitName { get; }

    /// <summary>
    /// Gets the parameter argument for the init function.
    /// </summary>
    public InitContractParameter Parameter { get; }

    /// <summary>
    /// Gets the amount of energy that can be used for contract execution. The base energy amount for transaction verification will be added to this cost.
    /// </summary>
    public ulong MaxContractExecutionEnergy { get; }

    /// <summary>
    /// Creates an instance of deploy module transaction payload.
    /// </summary>
    /// <param name="amount">the amount to transfer.</param>
    /// <param name="moduleRef">the deployed module reference.</param>
    /// <param name="initName">the name of init function including 'init_' prefix.</param>
    /// <param name="parameter">the parameter argument for init contract.</param>
    /// <param name="maxContractExecutionEnergy">the amount of energy that can be used for contract execution.</param>
    public static InitContractPayload Create(
        CcdAmount amount,
        ModuleRef moduleRef,
        string initName,
        InitContractParameter parameter,
        ulong maxContractExecutionEnergy)
    {
        return new InitContractPayload(
            amount,
            moduleRef,
            initName,
            parameter,
            maxContractExecutionEnergy);
    }

    // todo: think is it a good idea to separate all dif serializations
    // todo: decide what and where to use AsBytes or SerializeToBytes
    public byte[] SerializeToBytes()
    {
        byte[] accountTransactionTypeAsBytes = {(byte) AccountTransactionType.InitializeSmartContractInstance};
        byte[] amountAsBytes = Amount.SerializeToBytes();
        byte[] moduleRefAsBytes = ModuleRef.AsBytes;
        byte[] initNameAsBytes = Encoding.UTF8.GetBytes(InitName);
        int initNameBytesLength = initNameAsBytes.Length;
        byte[] initNameLengthAsBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(initNameLengthAsBytes, Convert.ToUInt16(initNameBytesLength));
        byte[] parameterAsBytes = Parameter.SerializeToBytes();
        int parameterBytesLength = parameterAsBytes.Length;
        byte[] parameterLengthAsBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(parameterLengthAsBytes, Convert.ToUInt16(parameterBytesLength));
        return accountTransactionTypeAsBytes
            .Concat(amountAsBytes)
            .Concat(moduleRefAsBytes)
            .Concat(initNameLengthAsBytes)
            .Concat(initNameAsBytes)
            .Concat(parameterLengthAsBytes)
            .Concat(parameterAsBytes)
            .ToArray();
    }

    public ulong GetBaseEnergyCost()
    {
        return MaxContractExecutionEnergy;
    }
}
