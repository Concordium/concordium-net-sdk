using System.Text;
using Concordium.Sdk.Interop;

namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when a interop call failed with possible error as message.
/// </summary>
public sealed class InteropBindingException : Exception
{
    private const string EmptyErrorMessage = "Empty error message returned";
    /// <summary>
    /// Type of error
    /// </summary>
    public InteropError InteropError { get; }

    internal static InteropBindingException Create(InteropError interopError, byte[]? message) =>
        message != null ? new InteropBindingException(interopError, Encoding.UTF8.GetString(message)) : Empty(interopError);

    private InteropBindingException(InteropError interopError, string message) : base(message) => this.InteropError = interopError;

    private static InteropBindingException Empty(InteropError interopError) => new(interopError, EmptyErrorMessage);
}
