namespace Concordium.Sdk.Types.New;

public class RegisteredData
{
    private readonly byte[] _bytes;

    public RegisteredData(byte[] bytes)
    {
        this._bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
    }

    public static RegisteredData FromHexString(string hexString)
    {
        var bytes = Convert.FromHexString(hexString);
        return new RegisteredData(bytes);
    }
    
    public byte[] AsBytes => this._bytes;
    public string AsHex => Convert.ToHexString(this._bytes).ToLowerInvariant();

    protected bool Equals(RegisteredData other)
    {
        return this._bytes.SequenceEqual(other._bytes);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return this.Equals((RegisteredData)obj);
    }

    public override int GetHashCode()
    {
        return this._bytes.GetHashCode();
    }

    public static bool operator ==(RegisteredData? left, RegisteredData? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(RegisteredData? left, RegisteredData? right)
    {
        return !Equals(left, right);
    }
}