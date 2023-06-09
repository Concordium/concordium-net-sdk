namespace Concordium.Sdk.Types.Mapped;

/// <summary>
/// The version of the node in semantic format.
/// </summary>
public record PeerVersion(int Major, int Minor, int Revision)
{
    public static PeerVersion Parse(string? value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var split = value.Split(".");

        if (split.Length != 3)
        {
            throw new ArgumentException($"Version of the node couldn't be parsed in semantic format: {value}");
        }

        var majorParse = int.TryParse(split[0], out var major);
        var minorParse = int.TryParse(split[1], out var minor);
        var patchParse = int.TryParse(split[2], out var patch);

        return new PeerVersion(major, minor, patch);
    }
};
