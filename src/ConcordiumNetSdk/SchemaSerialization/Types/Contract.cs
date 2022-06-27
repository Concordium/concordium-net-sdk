namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a parameter schemas for a whole contract.
/// </summary>
/// <param name="State">the parameter schema of state.</param>
/// <param name="Init">the parameter schema of init function.</param>
/// <param name="Receive">the parameter schemas of receive functions where key is receive name and value is receive parameter schema.</param>
public record Contract(Type? State, Type? Init, Dictionary<string, Type> Receive);
