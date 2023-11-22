namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when a interop call failed with possible error as message.
/// </summary>
internal sealed class InteropBindingException : Exception
{
    private const string EmptyErrorMessage = "Empty error message returned";

    internal static InteropBindingException Create(string? message) =>
        message != null ? new InteropBindingException(message) : Empty();

    private InteropBindingException(string message) : base(message)
    { }

    private static InteropBindingException Empty() => new(EmptyErrorMessage);
}

