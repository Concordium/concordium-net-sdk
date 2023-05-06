namespace Concordium.Sdk.Wallets;

/// <summary>
/// Represents an error indicating failure in a receiver
/// of <see cref="IWalletDataSource.TryGetSignKeys"/> or
/// <see cref="IWalletDataSource.TryGetAddress"/>.
///
/// This could for instance be due to a parsing or IO error
/// depending on the underlying implementation.
/// </summary>
public class WalletDataSourceException : Exception
{
    public WalletDataSourceException() { }

    public WalletDataSourceException(string message)
        : base(message) { }

    public WalletDataSourceException(string message, Exception inner)
        : base(message, inner) { }
}
