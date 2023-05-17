namespace Concordium.Sdk.Types;

/// <summary>
/// A receive name of the contract function called. Expected format: "<contract_name>.<func_name>".
/// </summary>
public record struct ReceiveName(string Receive);
