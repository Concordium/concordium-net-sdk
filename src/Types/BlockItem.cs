using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Transactions;
using CredentialDeploymentPayloadCase = Concordium.Grpc.V2.CredentialDeployment.PayloadOneofCase;
using UpdateInstructionPayloadCase = Concordium.Grpc.V2.UpdateInstructionPayload.PayloadOneofCase;
using BlockItemCase = Concordium.Grpc.V2.BlockItem.BlockItemOneofCase;

namespace Concordium.Sdk.Types;

/// <summary>
/// Update instructions are messages which can update the chain parameters. Including which keys are allowed
/// to make future update instructions.
/// </summary>
/// <param name="SignatureMap">A map from `UpdateKeysIndex` to `Signature`. Keys must not exceed 2^16.</param>
/// <param name="Header">The header of the UpdateInstruction.</param>
/// <param name="Payload">The payload of the UpdateInstruction. Can currently only be a `RawPayload`</param>
public record UpdateInstruction(
    SignatureMap SignatureMap, 
    UpdateInstructionHeader Header, 
    IUpdateInstructionPayload Payload
): BlockItemType {
    internal static UpdateInstruction From(Grpc.V2.UpdateInstruction updateInstruction) =>
        new UpdateInstruction(
            SignatureMap.From(updateInstruction.Signatures),
            UpdateInstructionHeader.From(updateInstruction.Header),
            updateInstruction.Payload.PayloadCase switch {
                UpdateInstructionPayloadCase.RawPayload => new UpdateInstructionPayloadRaw(updateInstruction.Payload.RawPayload.ToByteArray()),
                UpdateInstructionPayloadCase.None => throw new NotImplementedException(),
                _ => throw new MissingEnumException<UpdateInstructionPayloadCase>(updateInstruction.Payload.PayloadCase),
            }
        );
}

/// <summary>The header of an UpdateInstruction.</summary>
/// <param name="SequenceNumber">A sequence number that determines the ordering of update transactions.</param>
/// <param name="EffectiveTime">When the update takes effect.</param>
/// <param name="Timeout">Latest time the update instruction can included in a block.</param>
public record UpdateInstructionHeader(
    UpdateSequenceNumber SequenceNumber, 
    TransactionTime EffectiveTime, 
    TransactionTime Timeout
) {
    internal static UpdateInstructionHeader From(Grpc.V2.UpdateInstructionHeader header) =>
        new UpdateInstructionHeader(
            UpdateSequenceNumber.From(header.SequenceNumber),
            TransactionTime.From(header.EffectiveTime),
            TransactionTime.From(header.Timeout)
        );
}

/// <summary>
/// A sequence number that determines the ordering of update transactions.
/// Equivalent to `SequenceNumber` for account transactions.
/// Update sequence numbers are per update type and the minimum value is 1.
/// </summary>
public record UpdateSequenceNumber(UInt64 SequenceNumber) {
    internal static UpdateSequenceNumber From(Grpc.V2.UpdateSequenceNumber sequenceNumber) =>
        new UpdateSequenceNumber(sequenceNumber.Value);
}

/// <summary>The payload for an UpdateInstruction.</summary>
public interface IUpdateInstructionPayload{}

/// <summary>A raw payload encoded according to the format defined by the protocol.</summary>
public sealed record UpdateInstructionPayloadRaw(byte[] RawPayload) : IUpdateInstructionPayload;

/// <summary>
/// Credential deployments create new accounts. They are not paid for
/// directly by the sender. Instead, bakers are rewarded by the protocol for
/// including them.
/// </summary>
/// <param name="MessageExpiry">Latest time the credential deployment can included in a block.</param>
/// <param name="Payload">The payload of the credential deployment.</param>
public record CredentialDeployment(TransactionTime MessageExpiry, ICredentialPayload Payload): BlockItemType {
    internal static CredentialDeployment From(Grpc.V2.CredentialDeployment cred) =>
        new CredentialDeployment(
            TransactionTime.From(cred.MessageExpiry),
            cred.PayloadCase switch {
                CredentialDeploymentPayloadCase.RawPayload => new CredentialPayloadRaw(cred.RawPayload.ToByteArray()),
                CredentialDeploymentPayloadCase.None => throw new NotImplementedException(),
                _ => throw new MissingEnumException<CredentialDeploymentPayloadCase>(cred.PayloadCase),
            }
        );
}

/// <summary>The payload of a Credential Deployment.</summary>
public interface ICredentialPayload{};

/// <summary>
/// A raw payload, which is just the encoded payload.
/// A typed variant might be added in the future.
/// </summary>
public sealed record CredentialPayloadRaw(byte[] RawPayload) : ICredentialPayload;

/// <summary>A block item.</summary>
/// <param name="TransactionHash">The hash of the block item that identifies it to the chain.</param>
/// <param name="BlockItemType">Either a SignedAccountTransaction, CredentialDeployment or UpdateInstruction.</param>
public sealed record BlockItem(TransactionHash TransactionHash, BlockItemType BlockItemType)
{
    internal static BlockItem From(Grpc.V2.BlockItem blockItem) =>
        new BlockItem(
            TransactionHash.From(blockItem.Hash.ToString()),
            blockItem.BlockItemCase switch {
                BlockItemCase.AccountTransaction => SignedAccountTransaction.From(blockItem.AccountTransaction),
                BlockItemCase.CredentialDeployment => CredentialDeployment.From(blockItem.CredentialDeployment),
                BlockItemCase.UpdateInstruction => UpdateInstruction.From(blockItem.UpdateInstruction),
                _ => throw new MissingEnumException<BlockItemCase>(blockItem.BlockItemCase),
            }
        );
}

/// <summary>Either a SignedAccountTransaction, CredentialDeployment or UpdateInstruction.</summary>
public abstract record BlockItemType;
