using System.Buffers.Binary;
using System.Text;
using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents an update contract transaction payload.
/// </summary>
public class UpdateContractPayload : IAccountTransactionPayload
{
    private UpdateContractPayload(
        CcdAmount amount,
        ContractAddress contractAddress,
        string receiveName,
        UpdateContractParameter parameter,
        ulong maxContractExecutionEnergy)
    {
        Amount = amount;
        ContractAddress = contractAddress;
        ReceiveName = receiveName;
        Parameter = parameter;
        MaxContractExecutionEnergy = maxContractExecutionEnergy;
    }

    /// <summary>
    /// Gets the amount to transfer.
    /// </summary>
    public CcdAmount Amount { get; }

    /// <summary>
    /// Gets the address of contract instance consisting of an index and a subindex.
    /// </summary>
    public ContractAddress ContractAddress { get; }

    // todo: think to implement as own type
    /// <summary>
    /// Gets the name of receive function including contract name prefix.
    /// </summary>
    public string ReceiveName { get; }

    /// <summary>
    /// Gets the parameter argument for the update function.
    /// </summary>
    public UpdateContractParameter Parameter { get; }

    /// <summary>
    /// Gets the amount of energy that can be used for contract execution. The base energy amount for transaction verification will be added to this cost.
    /// </summary>
    public ulong MaxContractExecutionEnergy { get; }

    /// <summary>
    /// Creates an instance of deploy module transaction payload.
    /// </summary>
    /// <param name="amount">the amount to transfer.</param>
    /// <param name="contractAddress">the address of contract instance consisting of an index and a subindex.</param>
    /// <param name="receiveName">the name of receive function including contract name prefix.</param>
    /// <param name="parameter">the parameter argument for the update function.</param>
    /// <param name="maxContractExecutionEnergy">the amount of energy that can be used for contract execution.</param>
    public static UpdateContractPayload Create(
        CcdAmount amount,
        ContractAddress contractAddress,
        string receiveName,
        UpdateContractParameter parameter,
        ulong maxContractExecutionEnergy)
    {
        return new UpdateContractPayload(
            amount,
            contractAddress,
            receiveName,
            parameter,
            maxContractExecutionEnergy);
    }

    public byte[] SerializeToBytes()
    {
        byte[] accountTransactionTypeAsBytes = {(byte) AccountTransactionType.UpdateSmartContractInstance};
        byte[] amountAsBytes = Amount.SerializeToBytes();
        byte[] contractAddressAsBytes = ContractAddress.SerializeToBytes();
        byte[] receiveNameAsBytes = Encoding.UTF8.GetBytes(ReceiveName);
        int receiveNameBytesLength = receiveNameAsBytes.Length;
        byte[] receiveNameLengthAsBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(receiveNameLengthAsBytes, Convert.ToUInt16(receiveNameBytesLength));
        byte[] parameterAsBytes = Parameter.SerializeToBytes();
        int parameterBytesLength = parameterAsBytes.Length;
        byte[] parameterLengthAsBytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(parameterLengthAsBytes, Convert.ToUInt16(parameterBytesLength));
        return accountTransactionTypeAsBytes
            .Concat(amountAsBytes)
            .Concat(contractAddressAsBytes)
            .Concat(receiveNameLengthAsBytes)
            .Concat(receiveNameAsBytes)
            .Concat(parameterLengthAsBytes)
            .Concat(parameterAsBytes)
            .ToArray();
    }

    public ulong GetBaseEnergyCost()
    {
        return MaxContractExecutionEnergy;
    }
}
