using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Helpers;

namespace Concordium.Sdk.Types;

/// <summary>
/// Details of a block item summary, split by the kind of block item it is for.
/// </summary>
public interface IBlockItemSummaryDetails{}

internal static class BlockItemSummaryDetailsFactory
{
    internal static IBlockItemSummaryDetails From(Grpc.V2.BlockItemSummary blockItemSummary) =>
        blockItemSummary.DetailsCase switch
        {
            Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountTransaction => AccountTransactionDetails.From(
                blockItemSummary.AccountTransaction),
            Grpc.V2.BlockItemSummary.DetailsOneofCase.AccountCreation => AccountCreationDetails.From(blockItemSummary
                .AccountCreation),
            Grpc.V2.BlockItemSummary.DetailsOneofCase.Update => UpdateDetails.From(blockItemSummary.Update),
            Grpc.V2.BlockItemSummary.DetailsOneofCase.None =>
                throw new MissingEnumException<Grpc.V2.BlockItemSummary.DetailsOneofCase>(blockItemSummary.DetailsCase),
            _ => throw new MissingEnumException<Grpc.V2.BlockItemSummary.DetailsOneofCase>(blockItemSummary.DetailsCase)
        };
}

/// <summary>
/// Details of an account transaction. This always has a sender and is paid for,
/// and it might have some other effects on the state of the chain.
/// </summary>
/// <param name="Cost">
/// The amount of CCD the sender paid for including this transaction in
/// the block.
/// </param>
/// <param name="Sender">Sender of the transaction.</param>
/// <param name="Effects">Effects of the account transaction, if any.</param>
public sealed record AccountTransactionDetails
    (CcdAmount Cost, AccountAddress Sender, IAccountTransactionEffects Effects) : IBlockItemSummaryDetails
{
    internal static AccountTransactionDetails From(Grpc.V2.AccountTransactionDetails accountTransactionDetails) =>
        new(
            Cost: accountTransactionDetails.Cost.ToCcd(),
            Sender: AccountAddress.From(accountTransactionDetails.Sender),
            Effects: AccountTransactionEffectsFactory.From(accountTransactionDetails.Effects)
        );

    internal IEnumerable<AccountAddress> GetAffectedAccountAddresses()
    {
        var sender = this.Sender;
        switch (this.Effects)
        {
            case ContractUpdateIssued contractUpdateIssued:
                yield return sender;
                foreach (var address in contractUpdateIssued.GetAffectedAccountAddresses())
                {
                    if (address == sender)
                    {
                        continue;
                    }
                    yield return address;
                }
                break;
            case AccountTransfer accountTransfer:
                yield return sender;
                foreach (var address in accountTransfer.GetAffectedAccountAddresses())
                {
                    if (address == sender)
                    {
                        continue;
                    }
                    yield return address;
                }
                break;
            case EncryptedAmountTransferred encryptedAmountTransferred:
                foreach (var address in encryptedAmountTransferred.GetAffectedAccountAddresses())
                {
                    yield return address;
                }
                break;
            case TransferredToEncrypted transferredToEncrypted:
                foreach (var address in transferredToEncrypted.GetAffectedAccountAddresses())
                {
                    yield return address;
                }
                break;
            case TransferredToPublic transferredToPublic:
                foreach (var address in transferredToPublic.GetAffectedAccountAddresses())
                {
                    yield return address;
                }
                break;
            case TransferredWithSchedule transferredWithSchedule:
                yield return sender;
                foreach (var address in transferredWithSchedule.GetAffectedAccountAddresses())
                {
                    yield return address;
                }
                break;
            case None:
            case ModuleDeployed:
            case ContractInitialized:
            case BakerAdded:
            case BakerRemoved:
            case BakerStakeUpdated:
            case BakerRestakeEarningsUpdated:
            case BakerKeysUpdated:
            case CredentialKeysUpdated:
            case CredentialsUpdated:
            case DataRegistered:
            case BakerConfigured:
            case DelegationConfigured:
                yield return sender;
                break;
            default:
                throw new MissingTypeException<IAccountTransactionEffects>(this.Effects);
        }
    }

    internal bool TryGetRejectedReason(out IRejectReason? rejectReason)
    {
        switch (this.Effects)
        {
            case None none:
                rejectReason = none.RejectReason;
                return true;
            case ModuleDeployed:
            case ContractInitialized:
            case ContractUpdateIssued:
            case AccountTransfer:
            case BakerAdded:
            case BakerRemoved:
            case BakerStakeUpdated:
            case BakerRestakeEarningsUpdated:
            case BakerKeysUpdated:
            case EncryptedAmountTransferred:
            case TransferredToEncrypted:
            case TransferredToPublic:
            case TransferredWithSchedule:
            case CredentialKeysUpdated:
            case CredentialsUpdated:
            case DataRegistered:
            case BakerConfigured:
            case DelegationConfigured:
                rejectReason = null;
                return false;
            default:
                throw new MissingTypeException<IAccountTransactionEffects>(this.Effects);
        }
    }
}

/// <summary>
/// Details of an account creation. These transactions are free, and we only
/// ever get a response for them if the account is created, hence no failure
/// cases.
/// </summary>
/// <param name="credentialType">Whether this is an initial or normal account.</param>
/// <param name="address">Address of the newly created account.</param>
/// <param name="RegId">Credential registration ID of the first credential.</param>
public sealed record AccountCreationDetails
    (CredentialType credentialType, AccountAddress address, CredentialRegistrationId RegId) : IBlockItemSummaryDetails
{
    internal static AccountCreationDetails From(Grpc.V2.AccountCreationDetails details) =>
        new(
            details.CredentialType.Into(),
            AccountAddress.From(details.Address),
            CredentialRegistrationId.From(details.RegId)
        );
}

/// <summary>
/// Details of an update instruction. These are free, and we only ever get a
/// response for them if the update is successfully enqueued, hence no failure
/// cases.
/// </summary>
/// <param name="EffectiveTime">Transaction time</param>
/// <param name="Payload">Update information</param>
public sealed record UpdateDetails(DateTimeOffset EffectiveTime, IUpdatePayload Payload) : IBlockItemSummaryDetails
{
    internal static UpdateDetails From(Grpc.V2.UpdateDetails details) => new(
        details.EffectiveTime.ToDateTimeOffset(), UpdatePayloadFactory.From(details.Payload));
}
