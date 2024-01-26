namespace Concordium.Sdk.Interop;

/// <summary>
/// Result type which on errors hold error type information.
/// </summary>
public enum InteropError
{
    /// <summary>
    /// Represents errors occurring while deserializing to the schema JSON format.
    /// </summary>
    JsonError = 1,
    /// <summary>
    /// This type represents all possible errors that can occur when serializing or
    /// deserializing JSON data.
    /// </summary>
    SerdeJsonError = 2,
    /// <summary>
    /// Errors which can occur when attempting to interpret a sequence of bytes
    /// as a string.
    /// </summary>
    Utf8Error = 3,
    /// <summary>
    /// Versioned Schema Error - Parse error
    /// </summary>
    VersionedSchemaErrorParseError = 4,
    /// <summary>
    /// Versioned Schema Error - Missing Schema Version
    /// </summary>
    VersionedSchemaErrorMissingSchemaVersion = 5,
    /// <summary>
    /// Versioned Schema Error - Invalid Schema Version
    /// </summary>
    VersionedSchemaErrorInvalidSchemaVersion = 6,
    /// <summary>
    /// Versioned Schema Error - No Contract In Module
    /// </summary>
    VersionedSchemaErrorNoContractInModule = 7,
    /// <summary>
    /// Versioned Schema Error - Receive function schema not found in contract schema
    /// </summary>
    VersionedSchemaErrorNoReceiveInContract = 8,
    /// <summary>
    /// Versioned Schema Error - Init function schema not found in contract schema
    /// </summary>
    VersionedSchemaErrorNoInitInContract = 9,
    /// <summary>
    /// Versioned Schema Error - Receive function schema does not contain a parameter schema
    /// </summary>
    VersionedSchemaErrorNoParamsInReceive = 10,
    /// <summary>
    /// Versioned Schema Error - Init function schema does not contain a parameter schema
    /// </summary>
    VersionedSchemaErrorNoParamsInInit = 11,
    /// <summary>
    /// Versioned Schema Error - Receive function schema not found in contract schema
    /// </summary>
    VersionedSchemaErrorNoErrorInReceive = 12,
    /// <summary>
    /// Versioned Schema Error - Init function schema does not contain an error schema
    /// </summary>
    VersionedSchemaErrorNoErrorInInit = 13,
    /// <summary>
    /// Versioned Schema Error - Errors not supported for this module version
    /// </summary>
    VersionedSchemaErrorErrorNotSupported = 14,
    /// <summary>
    /// Versioned Schema Error - Receive function schema has no return value schema
    /// </summary>
    VersionedSchemaErrorNoReturnValueInReceive = 15,
    /// <summary>
    /// Versioned Schema Error - Return values not supported for this module version
    /// </summary>
    VersionedSchemaErrorReturnValueNotSupported = 16,
    /// <summary>
    /// Versioned Schema Error - Event schema not found in contract schema
    /// </summary>
    VersionedSchemaErrorNoEventInContract = 17,
    /// <summary>
    /// Versioned Schema Error - Events not supported for this module version
    /// </summary>
    VersionedSchemaErrorEventNotSupported = 18,
}

internal static class InteropErrorExtensions
{
    internal static bool TryMapError(byte result, out InteropError? error)
    {
        error = null;
        switch (result)
        {
            case 0:
                break;
            case 1:
                error = InteropError.JsonError;
                break;
            case 2:
                error = InteropError.SerdeJsonError;
                break;
            case 3:
                error = InteropError.Utf8Error;
                break;
            case 4:
                error = InteropError.VersionedSchemaErrorParseError;
                break;
            case 5:
                error = InteropError.VersionedSchemaErrorMissingSchemaVersion;
                break;
            case 6:
                error = InteropError.VersionedSchemaErrorInvalidSchemaVersion;
                break;
            case 7:
                error = InteropError.VersionedSchemaErrorNoContractInModule;
                break;
            case 8:
                error = InteropError.VersionedSchemaErrorNoReceiveInContract;
                break;
            case 9:
                error = InteropError.VersionedSchemaErrorNoInitInContract;
                break;
            case 10:
                error = InteropError.VersionedSchemaErrorNoParamsInReceive;
                break;
            case 11:
                error = InteropError.VersionedSchemaErrorNoParamsInInit;
                break;
            case 12:
                error = InteropError.VersionedSchemaErrorNoErrorInReceive;
                break;
            case 13:
                error = InteropError.VersionedSchemaErrorNoErrorInInit;
                break;
            case 14:
                error = InteropError.VersionedSchemaErrorErrorNotSupported;
                break;
            case 15:
                error = InteropError.VersionedSchemaErrorNoReturnValueInReceive;
                break;
            case 16:
                error = InteropError.VersionedSchemaErrorReturnValueNotSupported;
                break;
            case 17:
                error = InteropError.VersionedSchemaErrorNoEventInContract;
                break;
            case 18:
                error = InteropError.VersionedSchemaErrorEventNotSupported;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }

        return error != null;
    }
}
