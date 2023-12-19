using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;
using CredentialDeploymentPayloadCase = Concordium.Grpc.V2.CredentialDeployment.PayloadOneofCase;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Credential deployments create new accounts. They are not paid for
/// directly by the sender. Instead, bakers are rewarded by the protocol for
/// including them.
/// </summary>
/// <param name="MessageExpiry">Latest time the credential deployment can included in a block.</param>
/// <param name="Payload">The payload of the credential deployment.</param>
public record CredentialDeployment(TransactionTime MessageExpiry, ICredentialPayload Payload) : BlockItemType
{
    internal static CredentialDeployment From(Grpc.V2.CredentialDeployment cred) =>
        new(
            TransactionTime.From(cred.MessageExpiry),
            cred.PayloadCase switch
            {
                CredentialDeploymentPayloadCase.RawPayload => new CredentialPayloadRaw(cred.RawPayload.ToByteArray()),
                CredentialDeploymentPayloadCase.None => throw new MissingEnumException<CredentialDeploymentPayloadCase>(cred.PayloadCase),
                _ => throw new MissingEnumException<CredentialDeploymentPayloadCase>(cred.PayloadCase),
            }
        );
}

/// <summary>The payload of a Credential Deployment.</summary>
public interface ICredentialPayload { };

/// <summary>
/// A raw payload, which is just the encoded payload.
/// A typed variant might be added in the future.
/// </summary>
public sealed record CredentialPayloadRaw(byte[] RawPayload) : ICredentialPayload;

