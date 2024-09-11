namespace Concordium.Sdk.Crypto;

/// <summary> Implementers of this interface hold keys for signing data. </summary>
public interface ISigner
{
    /// <summary>
    /// Signs bytes using concrete implementation of a sign key.
    /// </summary>
    /// <param name="bytes">A byte array representing the data to sign.</param>
    /// <returns>A byte array representing the data to sign.</returns>
    byte[] Sign(byte[] bytes);
}
