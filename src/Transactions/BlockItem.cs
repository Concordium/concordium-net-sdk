using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;
using BlockItemCase = Concordium.Grpc.V2.BlockItem.BlockItemOneofCase;

namespace Concordium.Sdk.Transactions;

/// <summary>A block item.</summary>
/// <param name="TransactionHash">The hash of the block item that identifies it to the chain.</param>
/// <param name="BlockItemType">Either a SignedAccountTransaction, CredentialDeployment or UpdateInstruction.</param>
public sealed record BlockItem(TransactionHash TransactionHash, BlockItemType BlockItemType)
{
    internal static BlockItem From(Grpc.V2.BlockItem blockItem) =>
        new(
            TransactionHash.From(blockItem.Hash),
            blockItem.BlockItemCase switch
            {
                BlockItemCase.AccountTransaction => SignedAccountTransaction.From(blockItem.AccountTransaction),
                BlockItemCase.CredentialDeployment => CredentialDeployment.From(blockItem.CredentialDeployment),
                BlockItemCase.UpdateInstruction => UpdateInstruction.From(blockItem.UpdateInstruction),
                BlockItemCase.None => throw new NotImplementedException(),
                _ => throw new MissingEnumException<BlockItemCase>(blockItem.BlockItemCase),
            }
        );
}

/// <summary>Either a SignedAccountTransaction, CredentialDeployment or UpdateInstruction.</summary>
public abstract record BlockItemType;
