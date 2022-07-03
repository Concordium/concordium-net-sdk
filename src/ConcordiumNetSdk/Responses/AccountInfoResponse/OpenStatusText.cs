using System.Runtime.Serialization;

namespace ConcordiumNetSdk.Responses.AccountInfoResponse;

/// <summary>
/// Represents the information about an open status text.
/// </summary>
public enum OpenStatusText
{
    [EnumMember(Value = "openForAll")]
    OpenForAll,

    [EnumMember(Value = "closedForNew")]
    ClosedForNew,

    [EnumMember(Value = "closedForAll")]
    ClosedForAll
}
