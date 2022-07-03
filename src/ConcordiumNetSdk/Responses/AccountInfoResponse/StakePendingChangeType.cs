using System.Runtime.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a stake pending change type.
/// </summary>
public enum StakePendingChangeType
{
    [EnumMember(Value = "ReduceStake")]
    ReduceStake,

    [EnumMember(Value = "RemoveBaker")]
    RemoveStakeV0,

    [EnumMember(Value = "RemoveStake")]
    RemoveStakeV1
}
