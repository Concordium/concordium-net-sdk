using Concordium.Grpc.V2;
using Concordium.Sdk.Types;

namespace Concordium.Sdk.Helpers;

internal static class GRpcMappingExtensions
{
    internal static CcdAmount ToCcd(this Amount amount) => CcdAmount.From(amount);

}
