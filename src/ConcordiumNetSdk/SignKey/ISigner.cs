namespace ConcordiumNetSdk.SignKey;

public interface ISigner
{
    /// <summary>
    /// Gets the length of the signature produced by this signer.
    /// </summary>
    public UInt32 GetSignatureLength();

    /// <summary>
    /// Signs bytes using concrete implementation of a sign key.
    /// </summary>
    /// <param name="bytes">A byte array representing the data to sign.</param>
    /// <returns>A byte array representing the data to sign.</returns>
    byte[] Sign(byte[] bytes);
}
