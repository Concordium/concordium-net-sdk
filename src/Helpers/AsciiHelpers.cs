namespace Concordium.Sdk.Helpers;

internal static class AsciiHelpers
{
    internal static bool IsAsciiAlphaNumeric(char c)
    {
        if (c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9')
        {
            return true;
        }

        return false;
    }

    internal static bool IsAsciiPunctuation(char c)
    {
        for (var i = '!'; i <= '/'; i++)
        {
            if (c == i)
            {
                return true;
            }
        }
        for (var i = ':'; i <= '@'; i++)
        {
            if (c == i)
            {
                return true;
            }
        }
        for (var i = '['; i <= '`'; i++)
        {
            if (c == i)
            {
                return true;
            }
        }
        for (var i = '{'; i <= '~'; i++)
        {
            if (c == i)
            {
                return true;
            }
        }

        return false;
    }
}
