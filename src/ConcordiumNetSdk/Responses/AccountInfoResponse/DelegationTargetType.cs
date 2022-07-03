using System.Runtime.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about a delegation target type.
/// </summary>
public enum DelegationTargetType
{
    [EnumMember(Value = "Passive")]
    PassiveDelegation,

    [EnumMember(Value = "Baker")]
    Baker
}
