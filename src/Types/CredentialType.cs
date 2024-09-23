using Concordium.Sdk.Exceptions;

namespace Concordium.Sdk.Types;

/// <summary>
/// Enumeration of the types of credentials.
/// </summary>
public enum CredentialType
{
    /// <summary>
    /// Initial credential is a credential that is submitted by the identity
    /// provider on behalf of the user. There is at most one initial credential
    /// per identity.
    /// </summary>
    Initial = 0,
    /// <summary>
    /// A normal credential is one where the identity behind it is only known to
    /// the owner of the account, unless the identity disclosure process
    /// has been initiated.
    /// </summary>
    Normal = 1
}

internal static class CredentialTypeFactory
{
    internal static CredentialType Into(this Grpc.V2.CredentialType type) =>
        type switch
        {
            Grpc.V2.CredentialType.Initial => CredentialType.Initial,
            Grpc.V2.CredentialType.Normal => CredentialType.Normal,
            _ => throw new MissingEnumException<Grpc.V2.CredentialType>(type)
        };
}
