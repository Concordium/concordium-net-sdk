namespace Concordium.Sdk.Interop;

/// <summary>
/// Result type which on errors hold error type information.
/// </summary>
public enum Result
{
    /// <summary>
    /// No error
    /// </summary>
    NoError = 0,
    JsonError,
    SerdeJsonError,
    Utf8Error,
    VersionedSchemaErrorParseError,
    VersionedSchemaErrorMissingSchemaVersion,
    VersionedSchemaErrorNoContractInModule,
    VersionedSchemaErrorNoReceiveInContract,
    VersionedSchemaErrorNoInitInContract,
    VersionedSchemaErrorNoParamsInReceive,
    VersionedSchemaErrorNoParamsInInit,
    VersionedSchemaErrorNoErrorInReceive,
    VersionedSchemaErrorNoErrorInInit,
    VersionedSchemaErrorErrorNotSupported,
    VersionedSchemaErrorNoReturnValueInReceive,
    VersionedSchemaErrorReturnValueNotSupported,
    VersionedSchemaErrorNoEventInContract,
    VersionedSchemaErrorEventNotSupported,
}

internal static class ErrorExtensions
{
    internal static bool IsError(this Result result) => result != Result.NoError;
}
