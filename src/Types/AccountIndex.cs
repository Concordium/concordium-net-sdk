using Concordium.Grpc.V2;

namespace Concordium.Sdk.Types;

/// <summary>
/// Index of the account in the account table. These are assigned sequentially
/// in the order of creation of accounts. The first account has index 0.
/// </summary>
/// <param name="Index">Index of account</param>
public readonly record struct AccountIndex(ulong Index) : IAccountIdentifier
{
    /// <inheritdoc/>
    public AccountIdentifierInput ToAccountIdentifierInput() => new AccountIdentifierInput
    {
        AccountIndex = new Grpc.V2.AccountIndex
        {
            Value = this.Index
        }
    };
}
