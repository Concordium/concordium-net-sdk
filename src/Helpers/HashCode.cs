namespace Concordium.Sdk.Helpers;

/// <summary>
/// Helpers for hash codes.
/// </summary>
public static class HashCode
{
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
