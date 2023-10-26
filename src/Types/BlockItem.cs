//using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Transactions;
//using GrpcEffect = Concordium.Grpc.V2.PendingUpdate.EffectOneofCase;

namespace Concordium.Sdk.Types;
/// <summary>
/// Minimum stake needed to become a baker. This only applies to protocol version 1-3.
/// </summary>
/// <param name="MinimumThresholdForBaking">Minimum threshold required for registering as a baker.</param>
public record BakerStakeThreshold(CcdAmount MinimumThresholdForBaking)
{
    internal static BakerStakeThreshold From(Grpc.V2.BakerStakeThreshold bakerStakeThreshold) => new(CcdAmount.From(bakerStakeThreshold.BakerStakeThreshold_));
};

/// <summary>
/// A pending update.
/// </summary>
/// <param name="EffectiveTime">The effective time of the update.</param>
/// <param name="Effect">The effect of the update.</param>
//public sealed record BlockItem(TransactionHash TransactionHash, BlockItemType BlockItemType])
//{
//    internal static BlockItem From(Grpc.V2.BlockItem blockItem) => { 
//        throw new NotImplementedException();
//    }
//}

/// <summary>The effect of the update.</summary>
public abstract record BlockItemType;

/// <summary>Updates to the root keys.</summary>
public sealed record AccountTransaction(
    AccountTransactionSignature accountTransactionSignature, 
    AccountTransactionHeader accountTransactionHeader, 
    AccountTransactionPayload accountTransactionPayload
) : BlockItemType {
    internal static AccountTransaction From(Grpc.V2.AccountTransaction accountTransaction) {
        return new AccountTransaction(
            AccountTransactionSignature.From(accountTransaction.Signature),
            AccountTransactionHeader.From(accountTransaction.Header),
            AccountTransactionPayload.From(accountTransaction.Payload)
        );
    }
}

//public sealed record AccountTransactionSignature
