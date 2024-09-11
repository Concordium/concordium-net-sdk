namespace Concordium.Sdk.Helpers;

/// <summary>
/// Helpers for hash codes.
/// </summary>
public static class HashCode
{

    /// <summary>
    /// Helper for getting a hash code for a byte array.
    /// </summary>
    public static int GetHashCodeByteArray(byte[] array)
    {
        unchecked
        {
            var hashValue = 31;

            foreach (var value in array)
            {
                hashValue = (37 * hashValue) + value.GetHashCode();
            }

            return hashValue;
        }
    }
}
