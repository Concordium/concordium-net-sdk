using Concordium.Sdk.Types;
using Grpc.Core;

namespace Concordium.Sdk.Client;

/// A query response with the addition of the block hash used by the query.
/// The block hash used for querying might be unknown when providing the block
/// as <see cref="Best"/> or <see cref="LastFinal"/>.
/// <param name="BlockHash">Block hash for which the query applies.</param>
/// <param name="Response">The result of the query.</param>
/// <typeparam name="T">Return type</typeparam>
public sealed record QueryResponse<T>(BlockHash BlockHash, T Response)
{
    internal static async Task<QueryResponse<T>> From(Task<Metadata> metadata, T response)
    {
        var meta = await metadata.ConfigureAwait(false);
        var blockHash = GetBlockHashFromMetadata(meta);
        return new QueryResponse<T>(blockHash, response);
    }

    /// <summary>
    /// Maps incoming stream to output. If there was an error getting the
    /// result block hash will not be present and hence we will have a exception when trying to access
    /// it.
    /// We want however the original exception to be thrown, which is what is triggered in the catch
    /// block.
    /// </summary>
    internal static async Task<QueryResponse<IAsyncEnumerable<TResult>>>
        From<TSource, TResult>(
            AsyncServerStreamingCall<TSource> response,
            Func<TSource, TResult> mapping,
            CancellationToken token) where TSource : class
    {
        try
        {
            var meta = await response.ResponseHeadersAsync.ConfigureAwait(false);
            var blockHash = GetBlockHashFromMetadata(meta);
            var result = response.ResponseStream.ReadAllAsync(token).Select(mapping);
            return new QueryResponse<IAsyncEnumerable<TResult>>(blockHash, result);
        }
        catch (MissingMemberException)
        {
            // Try propagate any original error
            await response.ResponseStream.MoveNext();

            // if none then throw caught
            throw;
        }
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
