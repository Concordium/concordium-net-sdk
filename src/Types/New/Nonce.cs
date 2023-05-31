namespace Concordium.Sdk.Types.New;

public readonly struct Nonce 
{
    private readonly ulong _value;

    public Nonce(ulong value)
    {
        this._value = value;
    }

    public ulong AsUInt64 => this._value;

    public bool Equals(Nonce other)
    {
        return this._value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Nonce other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this._value.GetHashCode();
    }

    public static bool operator ==(Nonce left, Nonce right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Nonce left, Nonce right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return this._value.ToString();
    }

    public Nonce Increment()
    {
        return new Nonce(this._value + 1);
    }
}