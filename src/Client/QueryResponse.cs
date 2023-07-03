using Concordium.Sdk.Types;
using Grpc.Core;

namespace Concordium.Sdk.Client;

/// A query response with the addition of the block hash used by the query.
/// The block hash used for querying might be unknown when providing the block
/// as <see cref="Best"/> or <see cref="LastFinal"/>.
/// <param name="BlockHash">Block hash for which the query applies.</param>
/// <param name="Response">The result of the query.</param>
/// <typeparam name="T">Return type</typeparam>
public record QueryResponse<T>(BlockHash BlockHash, T Response)
{
    internal static async Task<QueryResponse<T>> From(Task<Metadata> metadata, T response)
    {
        var meta = await metadata.ConfigureAwait(false);
        var blockHash = GetBlockHashFromMetadata(meta);
        return new QueryResponse<T>(blockHash, response);
    }

    private static BlockHash GetBlockHashFromMetadata(Metadata metadata)
    {
        const string blockHashEntry = "blockhash";

        var blockHash = metadata.Get(blockHashEntry);
        if (blockHash == null)
        {
            throw new MissingMemberException(blockHashEntry);
        }

        return BlockHash.From(blockHash!.Value);
    }
}
