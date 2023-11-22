using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;
using Concordium.Sdk.Interop;

namespace Concordium.Sdk.Types;

/// <summary>
/// A successful contract invocation produces a sequence of effects on smart
/// contracts and possibly accounts (if any contract transfers CCD to an
/// account).
/// </summary>
public interface IContractTraceElement { }

internal static class ContractTraceElementFactory
{
    internal static IContractTraceElement From(Grpc.V2.ContractTraceElement element) =>
        element.ElementCase switch
        {
            Grpc.V2.ContractTraceElement.ElementOneofCase.Updated =>
                new Updated(
                    element.Updated.ContractVersion.Into(),
                    ContractAddress.From(element.Updated.Address),
                    AddressFactory.From(element.Updated.Instigator),
                    element.Updated.Amount.ToCcd(),
                    new Parameter(element.Updated.Parameter.Value.ToByteArray()),
                    new ReceiveName(element.Updated.ReceiveName.Value),
                    element.Updated.Events.Select(e => new ContractEvent(e.Value.ToByteArray())).ToList()
                    ),
            Grpc.V2.ContractTraceElement.ElementOneofCase.Transferred =>
                new Transferred(
                    ContractAddress.From(element.Transferred.Sender),
                    element.Transferred.Amount.ToCcd(),
                    AccountAddress.From(element.Transferred.Receiver)),
            Grpc.V2.ContractTraceElement.ElementOneofCase.Interrupted =>
                new Interrupted(
                    ContractAddress.From(element.Interrupted.Address),
                    element.Interrupted.Events.Select(e => new ContractEvent(e.Value.ToByteArray())).ToList()
                ),
            Grpc.V2.ContractTraceElement.ElementOneofCase.Resumed =>
                new Resumed(ContractAddress.From(element.Resumed.Address), element.Resumed.Success),
            Grpc.V2.ContractTraceElement.ElementOneofCase.Upgraded =>
                new Upgraded(
                    Address: ContractAddress.From(element.Upgraded.Address),
                    From: new ModuleReference(element.Upgraded.From.Value),
                    To: new ModuleReference(element.Upgraded.To.Value)),
            Grpc.V2.ContractTraceElement.ElementOneofCase.None =>
                throw new MissingEnumException<Grpc.V2.ContractTraceElement.ElementOneofCase>(element.ElementCase),
            _ => throw new MissingEnumException<Grpc.V2.ContractTraceElement.ElementOneofCase>(element.ElementCase)
        };
}

/// <summary>
/// A contract instance was updated.
/// </summary>
/// <param name="ContractVersion">Contract version</param>
/// <param name="Address">Address of the affected instance.</param>
/// <param name="Instigator">
/// The origin of the message to the smart contract. This can be either
/// an account or a smart contract.
/// </param>
/// <param name="Amount">The amount the method was invoked with.</param>
/// <param name="Message">The message passed to method.</param>
/// <param name="ReceiveName">The name of the method that was executed.</param>
/// <param name="Events">
/// Any contract events that might have been generated by the contract
/// execution.
/// </param>
public sealed record Updated(
    ContractVersion ContractVersion,
    ContractAddress Address,
    IAddress Instigator,
    CcdAmount Amount,
    Parameter Message,
    ReceiveName ReceiveName,
    IList<ContractEvent> Events) : IContractTraceElement
{
    /// <summary>
    /// Deserialize message from <see cref="schema"/>.
    /// </summary>
    /// <param name="schema">Versioned module schema.</param>
    /// <returns><see cref="Message"/> deserialized as json uft8 encoded.</returns>
    /// <exception cref="InteropBindingException">Thrown when message wasn't able to be deserialized form schema.</exception>
    public byte[] GetDeserializeMessage(VersionedModuleSchema schema) =>
        GetDeserializeMessage(schema, this.ReceiveName.GetContractName(), this.ReceiveName.GetEntrypoint(), this.Message);

    /// <summary>
    /// Deserialize message from <see cref="schema"/>.
    /// </summary>
    /// <param name="schema">Module schema.</param>
    /// <param name="contractIdentifier">Contract name.</param>
    /// <param name="entryPoint">Entrypoint on contract.</param>
    /// <param name="message">Message to entrypoint.</param>
    /// <returns><see cref="message"/> deserialized as json uft8 encoded.</returns>
    /// <exception cref="InteropBindingException">Thrown when message wasn't able to be deserialized from schema.</exception>
    public static byte[] GetDeserializeMessage(
        VersionedModuleSchema schema,
        ContractIdentifier contractIdentifier,
        EntryPoint entryPoint,
        Parameter message
    ) =>
        InteropBinding.GetReceiveContractParameter(schema, contractIdentifier, entryPoint, message);

    /// <summary>
    /// Deserialize events from <see cref="schema"/>.
    /// </summary>
    /// <param name="schema">Module schema.</param>
    /// <returns>List of deserialized json uft8 encoded events. Possible null if this was returned from deserialization.</returns>
    /// <exception cref="InteropBindingException">Thrown if an event wasn't able to be deserialized from schema.</exception>
    public IList<byte[]> GetDeserializedEvents(VersionedModuleSchema schema)
    {
        var deserialized = new List<byte[]>(this.Events.Count);
        foreach (var contractEvent in this.Events)
        {
            var deserializeEvent = contractEvent.GetDeserializeEvent(schema, this.ReceiveName.GetContractName());
            deserialized.Add(deserializeEvent);
        }

        return deserialized;
    }
}

/// <summary>
/// A contract transferred an amount to the account.
/// </summary>
/// <param name="From">Sender contract.</param>
/// <param name="Amount">Amount transferred.</param>
/// <param name="To">Receiver account.</param>
public sealed record Transferred(ContractAddress From, CcdAmount Amount, AccountAddress To) : IContractTraceElement;

/// <summary>
/// A contract was interrupted. This occurs when a contract invokes another contract or makes a transfer to an account.
/// </summary>
/// <param name="Address">The contract interrupted.</param>
/// <param name="Events">The events generated up until the interruption.</param>
public sealed record Interrupted(ContractAddress Address, IList<ContractEvent> Events) : IContractTraceElement
{
    /// <summary>
    /// Deserialize events from <see cref="schema"/>.
    /// </summary>
    /// <param name="schema">Module schema.</param>
    /// <param name="contractName">Contract name.</param>
    /// <returns>List of deserialized json uft8 encoded events. Possible null if this was returned from deserialization.</returns>
    /// <exception cref="InteropBindingException">Thrown if an event wasn't able to be deserialized from schema.</exception>
    public IList<byte[]> GetDeserializedEvents(VersionedModuleSchema schema, ContractIdentifier contractName)
    {
        var deserialized = new List<byte[]>(this.Events.Count);
        foreach (var contractEvent in this.Events)
        {
            var deserializeEvent = contractEvent.GetDeserializeEvent(schema, contractName);
            deserialized.Add(deserializeEvent);
        }

        return deserialized;
    }
}

/// <summary>
/// A previously interrupted contract was resumed.
/// </summary>
/// <param name="Address">The contract resumed.</param>
/// <param name="Success">Whether the action that caused the interruption (invoke contract or make transfer) was successful or not.</param>
public sealed record Resumed(ContractAddress Address, bool Success) : IContractTraceElement;

/// <summary>
/// Contract was upgraded.
/// </summary>
/// <param name="Address">Address of the instance that was upgraded.</param>
/// <param name="From">The existing module reference that is in effect before the upgrade.</param>
/// <param name="To">The new module reference that is in effect after the upgrade.</param>
public sealed record Upgraded(ContractAddress Address, ModuleReference From, ModuleReference To) : IContractTraceElement;
