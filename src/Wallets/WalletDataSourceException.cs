namespace Concordium.Sdk.Wallets;

/// <summary>
/// Represents an error indicating failure in a receiver
/// of <see cref="IWalletDataSource.TryGetSignKeys"/> or
/// <see cref="IWalletDataSource.TryGetAccountAddress"/>.
///
/// This could for instance be due to a parsing or IO error
/// depending on the underlying implementation.
/// </summary>
public class WalletDataSourceException : Exception
{
    /// <summary>Initialize instance with no message.</summary>
    public WalletDataSourceException() { }

    /// <summary>Initialize instance with a message.</summary>
    public WalletDataSourceException(string message)
        : base(message) { }

    /// <summary>Initialize instance with a message and a reference to an inner exception.</summary>
    public WalletDataSourceException(string message, Exception inner)
        : base(message, inner) { }
}
