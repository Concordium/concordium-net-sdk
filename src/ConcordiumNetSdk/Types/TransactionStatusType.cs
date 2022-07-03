using System.Runtime.Serialization;

namespace ConcordiumNetSdk.Types;

public enum TransactionStatusType
{
    [EnumMember(Value = "received")]
    Received,

    [EnumMember(Value = "finalized")]
    Finalized,

    [EnumMember(Value = "committed")]
    Committed
}
