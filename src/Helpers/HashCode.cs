namespace Concordium.Sdk.Helpers;

/// <summary>
/// Helpers for hash codes.
/// </summary>
public class HashCode
{
    public static int GetHashCodeByteArray(byte[] array)
    {
        var hashValue = 31;

        foreach (var value in array)
        {
            hashValue = (37 * hashValue) + value.GetHashCode();
        }

        return hashValue;
    }
}
