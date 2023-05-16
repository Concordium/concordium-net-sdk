namespace Concordium.Sdk.Types;

/// <summary>
/// A receive name (owned version). Expected format:
/// "<contract_name>.<func_name>". Most methods are available only on the
/// [`ReceiveName`] type, the intention is to access those via the
/// [`as_receive_name`](OwnedReceiveName::as_receive_name) method.
/// </summary>
public record struct OwnedReceiveName(string OwnedReceive);
