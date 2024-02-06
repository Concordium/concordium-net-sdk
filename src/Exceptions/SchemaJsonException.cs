using System.Text;
using Concordium.Sdk.Interop;

namespace Concordium.Sdk.Exceptions;

/// <summary>
/// Thrown when a interop call failed with possible error as message.
/// </summary>
public sealed class SchemaJsonException : Exception
{
    private const string EmptyErrorMessage = "Empty error message returned";
    /// <summary>
    /// Type of error
    /// </summary>
    public SchemaJsonResult SchemaJsonResult { get; }

    internal static SchemaJsonException Create(SchemaJsonResult schemaJsonResult, byte[]? message) =>
        message != null ? new SchemaJsonException(schemaJsonResult, Encoding.UTF8.GetString(message)) : Empty(schemaJsonResult);

    private SchemaJsonException(SchemaJsonResult schemaJsonResult, string message) : base(message) => this.SchemaJsonResult = schemaJsonResult;

    private static SchemaJsonException Empty(SchemaJsonResult schemaJsonResult) => new(schemaJsonResult, EmptyErrorMessage);
}
