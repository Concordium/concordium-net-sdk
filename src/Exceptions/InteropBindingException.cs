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
    public Result Result { get; }

    internal static InteropBindingException Create(Result result, byte[]? message) =>
        message != null ? new InteropBindingException(result, Encoding.UTF8.GetString(message)) : Empty(result);

    private InteropBindingException(Result result, string message) : base(message) => this.Result = result;

    private static InteropBindingException Empty(Result result) => new(result, EmptyErrorMessage);
}
