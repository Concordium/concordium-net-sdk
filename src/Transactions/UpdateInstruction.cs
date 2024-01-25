using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;
using UpdateInstructionPayloadCase = Concordium.Grpc.V2.UpdateInstructionPayload.PayloadOneofCase;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Update instructions are messages which can update the chain parameters. Including which keys are allowed
/// to make future update instructions.
/// </summary>
/// <param name="SignatureMap">A map from `UpdateKeysIndex` to `Signature`. Keys must not exceed 2^16.</param>
/// <param name="Header">The header of the UpdateInstruction.</param>
/// <param name="Payload">The payload of the UpdateInstruction. Can currently only be a `RawPayload`</param>
public record UpdateInstruction(
    UpdateInstructionSignatureMap SignatureMap,
    UpdateInstructionHeader Header,
    IUpdateInstructionPayload Payload
) : BlockItemType
{
    internal static UpdateInstruction From(Grpc.V2.UpdateInstruction updateInstruction) =>
        new(
            UpdateInstructionSignatureMap.From(updateInstruction.Signatures),
            UpdateInstructionHeader.From(updateInstruction.Header),
            updateInstruction.Payload.PayloadCase switch
            {
                UpdateInstructionPayloadCase.RawPayload => new UpdateInstructionPayloadRaw(updateInstruction.Payload.RawPayload.ToByteArray()),
                UpdateInstructionPayloadCase.None => throw new MissingEnumException<UpdateInstructionPayloadCase>(updateInstruction.Payload.PayloadCase),
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
)
{
    internal static UpdateInstructionHeader From(Grpc.V2.UpdateInstructionHeader header) =>
        new(
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
public record UpdateSequenceNumber(ulong SequenceNumber)
{
    internal static UpdateSequenceNumber From(Grpc.V2.UpdateSequenceNumber sequenceNumber) =>
        new(sequenceNumber.Value);
}

/// <summary>The payload for an UpdateInstruction.</summary>
public interface IUpdateInstructionPayload { }

/// <summary>A raw payload encoded according to the format defined by the protocol.</summary>
public sealed record UpdateInstructionPayloadRaw(byte[] RawPayload) : IUpdateInstructionPayload;
