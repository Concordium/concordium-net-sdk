namespace ConcordiumNetSdk.SchemaSerialization.Types;

/// <summary>
/// Represents a parameter type enum using in smart contract parameter schema.
/// </summary>
public enum ParameterType
{
    /// <summary>
    /// Represents a nothing.
    /// </summary>
    Unit = 0,

    /// <summary>
    /// Represents a boolean (`true` or `false`).
    /// </summary>
    Bool,

    /// <summary>
    /// Represents a unsigned 8-bit integer.
    /// </summary>
    U8,

    /// <summary>
    /// Represents a unsigned 16-bit integer.
    /// </summary>
    U16,

    /// <summary>
    /// Represents a unsigned 32-bit integer.
    /// </summary>
    U32,

    /// <summary>
    /// Represents a unsigned 64-bit integer.
    /// </summary>
    U64,

    /// <summary>
    /// Represents a signed 8-bit integer.
    /// </summary>
    I8,

    /// <summary>
    /// Represents a signed 16-bit integer.
    /// </summary>
    I16,

    /// <summary>
    /// Represents a signed 32-bit integer.
    /// </summary>
    I32,

    /// <summary>
    /// Represents a signed 64-bit integer.
    /// </summary>
    I64,

    /// <summary>token amount in microGTU (10^-6 GTU).
    /// </summary>
    Amount,

    /// <summary>
    /// Represents a sender account address.
    /// </summary>
    AccountAddress,

    /// <summary>
    /// Represents an address of the contract instance consisting of an index and a subindex.
    /// </summary>
    ContractAddress,

    /// <summary>
    /// Represents an unsigned 64-bit integer storing milliseconds since UNIX epoch and representing a timestamp.
    /// </summary>
    Timestamp,

    /// <summary>
    /// Represents an unsigned 64-bit integer storing milliseconds and representing a duration.
    /// </summary>
    Duration,

    /// <summary>
    /// Represents a tuple pair.
    /// </summary>
    Pair,

    /// <summary>
    /// Represents a variable size list.
    /// </summary>
    List,
 
    /// <summary>
    /// Represents an unordered collection of unique elements.
    /// </summary>
    Set,

    /// <summary>
    /// Represents an unordered map from keys to values.
    /// </summary>
    Map,

    /// <summary>
    /// Represents a fixed size array.
    /// </summary>
    Array,

    /// <summary>
    /// Represents a struct.
    /// </summary>
    Struct,

    /// <summary>
    /// Represents an enum.
    /// </summary>
    Enum,

    /// <summary>
    /// Represents a string.
    /// </summary>
    String,

    /// <summary>
    /// Represents an unsigned 128-bit integer.
    /// </summary>
    U128,

    /// <summary>
    /// Represents a signed 128-bit integer.
    /// </summary>
    I128,

    /// <summary>
    /// Represents a name of the contract.
    /// </summary>
    ContractName,

    /// <summary>
    /// Represents a receive function name.
    /// </summary>
    ReceiveName
}
