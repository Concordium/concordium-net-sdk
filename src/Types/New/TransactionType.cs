
using Concordium.Sdk.Types.Mapped;

namespace Concordium.Sdk.Types.New;

public abstract class TransactionType
{
    public BlockItemKind Kind { get; }
    public object? Type { get; }
    protected TransactionType(BlockItemKind kind, object? type)
    {
        this.Kind = kind;
        this.Type = type;
    }

    public static TransactionType<Types.TransactionType> Get(Types.TransactionType? type)
    {
        return new TransactionType<Types.TransactionType>(BlockItemKind.AccountTransactionKind, type);
    }

    public static TransactionType<CredentialType> Get(CredentialType? type)
    {
        return new TransactionType<CredentialType>(BlockItemKind.CredentialDeploymentKind, type);
    }

    public static TransactionType<UpdateTransactionType> Get(UpdateTransactionType? type)
    {
        return new TransactionType<UpdateTransactionType>(BlockItemKind.UpdateInstructionKind, type);
    }
}

public sealed class TransactionType<T> : TransactionType where T : struct, Enum
{
    public new T? Type { get; }

    internal TransactionType(BlockItemKind kind, T? type) : base(kind, type)
    {
        this.Type = type;
    }

    public override string ToString()
    {
        return $"{this.Kind}.{this.Type}";
    }

    private bool Equals(TransactionType<T> other)
    {
        return Equals(this.Type, other.Type)
               && this.Kind.Equals(other.Kind);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TransactionType<T> other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Type.HasValue ? this.Type.Value.GetHashCode() : 0;
    }

    public static bool operator ==(TransactionType<T>? left, TransactionType<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TransactionType<T>? left, TransactionType<T>? right)
    {
        return !Equals(left, right);
    }
}
