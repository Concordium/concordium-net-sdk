namespace Concordium.Sdk.Types.New;

public class UnixTimeSeconds
{
    public UnixTimeSeconds(long value)
    {
        this.AsLong = value;
    }

    public long AsLong { get; }
    public DateTimeOffset AsDateTimeOffset => DateTimeOffset.FromUnixTimeSeconds(this.AsLong);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        var other = (UnixTimeSeconds)obj;
        return this.AsLong == other.AsLong;
    }

    public override int GetHashCode()
    {
        return this.AsLong.GetHashCode();
    }

    public static bool operator ==(UnixTimeSeconds? left, UnixTimeSeconds? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(UnixTimeSeconds? left, UnixTimeSeconds? right)
    {
        return !Equals(left, right);
    }
}
