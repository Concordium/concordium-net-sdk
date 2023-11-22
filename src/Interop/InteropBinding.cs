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
    private const string DllName = "librust_bindings";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "schema_display")]
    private static extern bool SchemaDisplay(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema,
        int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_receive_contract_parameter")]
    private static extern bool GetReceiveContractParameter(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema, int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string contract_name,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string entrypoint,
        [MarshalAs(UnmanagedType.LPArray)] byte[] value_ptr,
        int value_size,
        [MarshalAs(UnmanagedType.FunctionPtr)] SetResultCallback callback);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_event_contract")]
    private static extern bool GetEventContract(
        [MarshalAs(UnmanagedType.LPArray)] byte[] schema,
        int schema_size,
        FfiByteOption schema_version,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string contract_name,
        [MarshalAs(UnmanagedType.LPArray)] byte[] value_ptr,
        int value_size,
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
    internal static byte[] SchemaDisplay(VersionedModuleSchema schema)
    {
        var ffiOption = FfiByteOption.Create(schema.Version);
        byte[]? result = null;

        var schemaDisplay = SchemaDisplay(schema.Schema, schema.Schema.Length, ffiOption,
            (ptr, size) =>
            {
                result = new byte[size];
                Marshal.Copy(ptr, result, 0, size);
            });

        if (schemaDisplay && result != null)
        {
            return result;
        }

        var interopException = InteropBindingException.Create(result);
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
    internal static byte[] GetReceiveContractParameter(VersionedModuleSchema schema, ContractIdentifier contractName, EntryPoint entrypoint, Parameter value)
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

        if (receiveContractParameter && result != null)
        {
            return result;
        }

        var interopException = InteropBindingException.Create(result);
        throw interopException;
    }

    /// <summary>
    /// Get contract event in a human interpretable form.
    /// </summary>
    /// <param name="schema">Module schema</param>
    /// <param name="contractName">Contract name</param>
    /// <param name="contractEvent">Contract event </param>
    /// <returns>Contract event as json uft8 encoded.</returns>
    internal static byte[] GetEventContract(VersionedModuleSchema schema, ContractIdentifier contractName, ContractEvent contractEvent)
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

        if (schemaDisplay && result != null)
        {
            return result;
        }

        var interopException = InteropBindingException.Create(result);
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
