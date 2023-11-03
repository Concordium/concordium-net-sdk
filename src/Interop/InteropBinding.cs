using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Application.Exceptions;
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
    private static extern bool SchemaDisplay(string schema, FfiByteOption schema_version, ref IntPtr result);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_receive_contract_parameter")]
    private static extern bool GetReceiveContractParameter(string schema, FfiByteOption schema_version, string contract_name, string entrypoint, string value, ref IntPtr result);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_event_contract")]
    private static extern bool GetEventContract(string schema, FfiByteOption schema_version, string contract_name, string value, ref IntPtr result);

    /// <summary>
    /// Get module schema in a human interpretable form.
    /// </summary>
    /// <param name="schema">Module schema in hexadecimal</param>
    /// <param name="schemaVersion">Optional schema version if present from module</param>
    /// <returns>Module schema in a human interpretable form</returns>
    internal static string? SchemaDisplay(string schema, ModuleSchemaVersion? schemaVersion)
    {
        var ffiOption = FfiByteOption.Create(schemaVersion);
        var result = IntPtr.Zero;
        try
        {
            var schemaDisplay = SchemaDisplay(schema, ffiOption, ref result);
            var resultStringAnsi = Marshal.PtrToStringAnsi(result);

            if (schemaDisplay)
            {
                return resultStringAnsi;
            }

            var interopException = InteropBindingException.Create(resultStringAnsi);
            throw interopException;
        }
        finally
        {
            FreeIfNonzero(result);
        }
    }

    /// <summary>
    /// Get contract receive parameters in a human interpretable form.
    ///
    /// Receive parameters are those given to a contract entrypoint on a update call.
    /// </summary>
    /// <param name="schema">Module schema in hexadecimal</param>
    /// <param name="contractName">Contract name</param>
    /// <param name="entrypoint">Entrypoint of contract</param>
    /// <param name="value">Receive parameters in hexadecimal</param>
    /// <param name="schemaVersion">Optional schema version if present from module</param>
    /// <returns>Receive parameters in a human interpretable form</returns>
    internal static string? GetReceiveContractParameter(string schema, string contractName, string entrypoint, string value, ModuleSchemaVersion? schemaVersion)
    {
        var ffiOption = FfiByteOption.Create(schemaVersion);
        var result = IntPtr.Zero;
        try
        {
            var schemaDisplay =
                GetReceiveContractParameter(schema, ffiOption, contractName, entrypoint, value, ref result);
            var resultStringAnsi = Marshal.PtrToStringAnsi(result);

            if (schemaDisplay)
            {
                return resultStringAnsi;
            }

            var interopException = InteropBindingException.Create(resultStringAnsi);
            throw interopException;
        }
        finally
        {
            FreeIfNonzero(result);
        }
    }

    /// <summary>
    /// Get contract event in a human interpretable form.
    /// </summary>
    /// <param name="schema">Module schema in hexadecimal</param>
    /// <param name="contractName">Contract name</param>
    /// <param name="value">Contract event in hexadecimal</param>
    /// <param name="schemaVersion">Optional schema version if present from module</param>
    /// <returns>Contract event in a human interpretable form</returns>
    internal static string? GetEventContract(string schema, string contractName, string value, ModuleSchemaVersion? schemaVersion)
    {
        var ffiOption = FfiByteOption.Create(schemaVersion);
        var result = IntPtr.Zero;
        try
        {
            var schemaDisplay = GetEventContract(schema, ffiOption, contractName, value, ref result);
            var resultStringAnsi = Marshal.PtrToStringAnsi(result);

            if (schemaDisplay)
            {
                return resultStringAnsi;
            }

            var interopException = InteropBindingException.Create(resultStringAnsi);
            throw interopException;
        }
        finally
        {
            FreeIfNonzero(result);
        }
    }

    private static void FreeIfNonzero(IntPtr ptr)
    {
        if (ptr != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(ptr);
        }
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
    #pragma warning restore IDE1006
}
