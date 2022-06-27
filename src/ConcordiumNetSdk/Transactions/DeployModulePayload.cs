using ConcordiumNetSdk.Types;

namespace ConcordiumNetSdk.Transactions;

/// <summary>
/// Represents a deploy module transaction payload.
/// </summary>
public class DeployModulePayload : IAccountTransactionPayload
{
    private DeployModulePayload(byte[] content)
    {
        Content = content;
    }

    /// <summary>
    /// Creates an instance of deploy module transaction payload.
    /// </summary>
    /// <param name="content">the content of deploy module.</param>
    public static DeployModulePayload Create(byte[] content)
    {
        return new DeployModulePayload(content);
    }

    /// <summary>
    /// Gets the content of deploy module payload.
    /// </summary>
    public byte[] Content { get; }

    public byte[] SerializeToBytes()
    {
        int serializedLength = 1 + Content.Length;// 1 account transaction type + content length
        byte[] bytes = new byte[serializedLength];
        Span<byte> buffer = bytes;
        buffer[0] = (byte) AccountTransactionType.DeployModule;
        Content.CopyTo(buffer.Slice(1, Content.Length));
        return bytes;
    }
    public ulong GetBaseEnergyCost()
    {
        double cost = Math.Round(Content.Length / 10.0);
        return Convert.ToUInt64(cost);
    }
}
