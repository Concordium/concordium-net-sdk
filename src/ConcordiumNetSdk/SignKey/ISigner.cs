namespace ConcordiumNetSdk.SignKey;

public interface ISigner
{
    /// <summary>
    /// Signs bytes using concrete implementation of sign key.
    /// </summary>
    /// <param name="bytes">the bytes to be signed.</param>
    /// <returns><see cref="T:byte[]"/> - thw signed bytes.</returns>
    byte[] Sign(byte[] bytes);
}
