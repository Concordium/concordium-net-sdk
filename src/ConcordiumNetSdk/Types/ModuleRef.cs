namespace ConcordiumNetSdk.Types;

/// <summary>
/// Represents a base16 encoded hash of a module ref.
/// </summary>
public class ModuleRef : Hash
{
    private ModuleRef(byte[] value) : base(value)
    {
    }

    private ModuleRef(string value) : base(value)
    {
    }

    /// <summary>
    /// Creates an instance from a base16 encoded string representing module ref (64 characters).
    /// </summary>
    /// <param name="moduleRefAsBase16String">the module ref as base16 encoded string.</param>  
    public static ModuleRef From(string moduleRefAsBase16String)
    {
        return new ModuleRef(moduleRefAsBase16String);
    }

    /// <summary>
    /// Creates an instance from a 32 bytes representing module ref.
    /// </summary>
    /// <param name="moduleRefAsBytes">the module ref as 32 bytes.</param>
    public static ModuleRef From(byte[] moduleRefAsBytes)
    {
        return new ModuleRef(moduleRefAsBytes);
    }
}
