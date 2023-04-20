namespace ConcordiumNetSdk.SignKey;

public interface ISigner
{
    /// <summary>
    /// Signs bytes using concrete implementation of sign key.
    /// </summary>
    /// <param name="bytes">The bytes to be signed.</param>
    /// <returns>The signed bytes.</returns>
    byte[] Sign(byte[] bytes);
}
