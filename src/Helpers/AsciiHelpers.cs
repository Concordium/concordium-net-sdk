namespace Concordium.Sdk.Helpers;

internal static class AsciiHelpers
{
    internal static bool IsAsciiAlphaNumeric(char c)
    {
        if (c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9'))
        {
            return true;
        }

        return false;
    }

    internal static bool IsAsciiPunctuation(char c)
    {
        if (c is (>= '!' and <= '/') or (>= ':' and <= '@') or (>= '[' and <= '`') or (>= '{' and <= '~'))
        {
            return true;
        }

        return false;
    }
}
