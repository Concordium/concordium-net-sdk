using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;


namespace Concordium.Sdk.Interop;

/// <summary>
/// Contains FFI bindings which can be compiled to work on platforms Linux, OSX and Windows.
/// </summary>
[SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments")]
internal static class InteropBinding
{
    private const string DllName = "rust_bindings";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "schema_display")]
    private static extern SchemaJsonResult SchemaDisplay(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema,
        int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_receive_contract_parameter")]
    private static extern SchemaJsonResult GetReceiveContractParameter(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema, int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string contract_name,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string entrypoint,
        [MarshalAs(UnmanagedType.LPArray)] byte[] value_ptr,
        int value_size,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_event_contract")]
    private static extern SchemaJsonResult GetEventContract(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema,
        int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string contract_name,
        [MarshalAs(UnmanagedType.LPArray)] byte[] value_ptr,
        int value_size,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "into_receive_parameter")]
    private static extern SchemaJsonResult IntoReceiveParameter(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema,
        int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string contract_name,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string function_name,
        [MarshalAs(UnmanagedType.LPArray)] byte[] json_ptr,
        int json_size,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "into_init_parameter")]
    private static extern SchemaJsonResult IntoInitParameter(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema,
        int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string contract_name,
        [MarshalAs(UnmanagedType.LPArray)] byte[] json_ptr,
        int json_size,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    /// <summary>
    /// Callback to set byte array result of interop call.
    /// </summary>
    private delegate void SetResultCallback(IntPtr ptr, int size);

    /// <summary>
    /// Get module schema in a human interpretable form.
    /// </summary>
    /// <param name="schema">Module schema</param>
    /// <returns>Module schema as json uft8 encoded.</returns>
    internal static Utf8Json SchemaDisplay(VersionedModuleSchema schema)
    {
        var ffiOption = FfiByteOption.Create(schema.Version);
        byte[]? result = null;

        var schemaDisplay = SchemaDisplay(schema.Schema, schema.Schema.Length, ffiOption,
            (ptr, size) =>
            {
                result = new byte[size];
                Marshal.Copy(ptr, result, 0, size);
            });

        if (!schemaDisplay.IsError() && result != null)
        {
            return new Utf8Json(result);
        }

        var interopException = SchemaJsonException.Create(schemaDisplay, result);
        throw interopException;
    }

    /// <summary>
    /// Get contract receive parameters in a human interpretable form.
    ///
    /// Receive parameters are those given to a contract entrypoint on a update call.
    /// </summary>
    /// <param name="schema">Module schema</param>
    /// <param name="contractName">Contract name</param>
    /// <param name="entrypoint">Entrypoint of contract</param>
    /// <param name="value">Receive parameters</param>
    /// <returns>Receive parameters as json uft8 encoded.</returns>
    internal static Utf8Json GetReceiveContractParameter(VersionedModuleSchema schema, ContractIdentifier contractName, EntryPoint entrypoint, Parameter value)
    {
        var ffiOption = FfiByteOption.Create(schema.Version);

        byte[]? result = null;

        var receiveContractParameter =
            GetReceiveContractParameter(schema.Schema, schema.Schema.Length, ffiOption,
                contractName.ContractName, entrypoint.Name, value.Param, value.Param.Length,
                (ptr, size) =>
                {
                    result = new byte[size];
                    Marshal.Copy(ptr, result, 0, size);
                });

        if (!receiveContractParameter.IsError() && result != null)
        {
            return new Utf8Json(result);
        }

        var interopException = SchemaJsonException.Create(receiveContractParameter, result);
        throw interopException;
    }

    /// <summary>
    /// Get contract event in a human interpretable form.
    /// </summary>
    /// <param name="schema">Module schema</param>
    /// <param name="contractName">Contract name</param>
    /// <param name="contractEvent">Contract event </param>
    /// <returns>Contract event as json uft8 encoded.</returns>
    internal static Utf8Json GetEventContract(VersionedModuleSchema schema, ContractIdentifier contractName, ContractEvent contractEvent)
    {
        var ffiOption = FfiByteOption.Create(schema.Version);

        var result = Array.Empty<byte>();

        var schemaDisplay = GetEventContract(schema.Schema, schema.Schema.Length, ffiOption,
            contractName.ContractName, contractEvent.Bytes, contractEvent.Bytes.Length,
            (ptr, size) =>
            {
                result = new byte[size];
                Marshal.Copy(ptr, result, 0, size);
            });

        if (!schemaDisplay.IsError() && result != null)
        {
            return new Utf8Json(result);
        }

        var interopException = SchemaJsonException.Create(schemaDisplay, result);
        throw interopException;
    }

    /// <summary>
    /// Contruct a smart contract receive parameter from a JSON string and the module schema.
    /// </summary>
    /// <param name="schema">Smart contract module schema</param>
    /// <param name="contractName">Name of the smart contract</param>
    /// <param name="functionName">Entrypoint of the contract to construct the parameter for</param>
    /// <param name="json">JSON representation of the smart contract parameter</param>
    /// <returns>Smart contract parameter</returns>
    internal static Parameter IntoReceiveParameter(
        VersionedModuleSchema schema,
        ContractIdentifier contractName,
        EntryPoint functionName,
        Utf8Json json
    )
    {
        var ffiOption = FfiByteOption.Create(schema.Version);
        var result = Array.Empty<byte>();

        var statusCode = IntoReceiveParameter(
            schema.Schema,
            schema.Schema.Length,
            ffiOption,
            contractName.ContractName,
            functionName.Name,
            json.Bytes,
            json.Bytes.Length,
            (ptr, size) =>
            {
                result = new byte[size];
                Marshal.Copy(ptr, result, 0, size);
            });

        if (!statusCode.IsError() && result != null)
        {
            return new Parameter(result);
        }

        var interopException = SchemaJsonException.Create(statusCode, result);
        throw interopException;
    }

    /// <summary>
    /// Contruct a smart contract init parameter from a JSON string and the module schema.
    /// </summary>
    /// <param name="schema">Smart contract module schema</param>
    /// <param name="contractName">Name of the smart contract</param>
    /// <param name="json">JSON representation of the smart contract parameter</param>
    /// <returns>Smart contract parameter</returns>
    internal static Parameter IntoInitParameter(
        VersionedModuleSchema schema,
        ContractIdentifier contractName,
        Utf8Json json
    )
    {
        var ffiOption = FfiByteOption.Create(schema.Version);
        var result = Array.Empty<byte>();

        var statusCode = IntoInitParameter(
            schema.Schema,
            schema.Schema.Length,
            ffiOption,
            contractName.ContractName,
            json.Bytes,
            json.Bytes.Length,
            (ptr, size) =>
            {
                result = new byte[size];
                Marshal.Copy(ptr, result, 0, size);
            });

        if (!statusCode.IsError() && result != null)
        {
            return new Parameter(result);
        }

        var interopException = SchemaJsonException.Create(statusCode, result);
        throw interopException;
    }

    /// <summary>
    /// A C# layout which compiled to a C interpretable structure. This is used as an optional parameter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct FfiByteOption
    {
        internal byte T { get; private init; }
        /// <summary>
        /// 1 is interpreted as true. <see cref="bool"/> are not used since it isn't a blittable type.
        /// </summary>
        /// <remarks>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types">Blittable and Non-Blittable Types</see>
        /// </remarks>
        internal byte IsSome { get; private init; }

        private static FfiByteOption None() => new() { IsSome = 0 };

        private static FfiByteOption Some(byte some) => new()
        {
            T = some,
            IsSome = 1
        };

        public static FfiByteOption Create(ModuleSchemaVersion? version) =>
            version is null or ModuleSchemaVersion.Undefined
                ? None()
                : Some((byte)version);
    }
}
