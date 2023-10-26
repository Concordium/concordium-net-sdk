using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Transactions;
using GrpcPayload = Concordium.Grpc.V2.CredentialDeployment.PayloadOneofCase;

namespace Concordium.Sdk.Types;

public record CredentialDeployment(TransactionTime MessageExpiry, ICredentialPayload Payload) {
    internal static CredentialDeployment From(Grpc.V2.CredentialDeployment cred) =>
        new CredentialDeployment(
            TransactionTime.From(cred.MessageExpiry),
            cred.PayloadCase switch {
                GrpcPayload.RawPayload => CredentialRawPayload.From(cred.RawPayload)
                GrpcPayload.None => throw new NotImplementedException(),
                _ => throw new MissingEnumException<CredentialPayload>(cred.PayloadCase),
            }
            
        );
}

/// <summary> The payload of a Credential Deployment. </summary>
public interface ICredentialPayload{};

/// <summary>
/// A raw payload, which is just the encoded payload.
/// A typed variant might be added in the future.
/// </summary>
public sealed record CredentialRawPayload(byte[] RawPayload) : ICredentialPayload;

/// <summary>
/// A pending update.
/// </summary>
/// <param name="EffectiveTime">The effective time of the update.</param>
/// <param name="Effect">The effect of the update.</param>
public sealed record BlockItem(TransactionHash TransactionHash, BlockItemType BlockItemType)
{
    internal static BlockItem From(Grpc.V2.BlockItem blockItem) =>
        throw new NotImplementedException();
}

/// <summary>The effect of the update.</summary>
public abstract record BlockItemType;

/// <summary>Updates to the root keys.</summary>
public sealed record AccountTransaction(
    AccountTransactionSignature accountTransactionSignature, 
    AccountTransactionHeader accountTransactionHeader, 
    AccountTransactionPayload accountTransactionPayload
) : BlockItemType {
    internal static AccountTransaction From(Grpc.V2.AccountTransaction accountTransaction) {
        return new AccountTransaction(
            AccountTransactionSignature.From(accountTransaction.Signature),
            AccountTransactionHeader.From(accountTransaction.Header),
            AccountTransactionPayload.From(accountTransaction.Payload)
        );
    }
}

//public sealed record AccountTransactionSignature
