using Concordium.Sdk.Exceptions;
using Concordium.Sdk.Types;
using PayloadCase = Concordium.Grpc.V2.AccountTransactionPayload.PayloadOneofCase;

namespace Concordium.Sdk.Transactions;

/// <summary>
/// Represents the payload of account transaction.
///
/// Inheriting records should implement data specific to the transaction they
/// model as well as helpers for constructing serialized transaction payloads
/// to be sent to the Concordium node.
/// </summary>
public abstract record AccountTransactionPayload
{
    /// <summary>
    /// Copies the on-chain data in the binary format expected by the node to a byte array.
    /// </summary>
    public abstract byte[] ToBytes();

    /// <summary>
    /// Converts the transaction to its corresponding protocol buffer message instance.
    /// </summary>
    public Grpc.V2.AccountTransactionPayload ToProto() =>
        new() { RawPayload = Google.Protobuf.ByteString.CopyFrom(this.ToBytes()) };

    internal static AccountTransactionPayload From(Grpc.V2.AccountTransactionPayload payload) => payload.PayloadCase switch
    {
        PayloadCase.TransferWithMemo => new TransferWithMemo(
            CcdAmount.From(payload.TransferWithMemo.Amount),
            AccountAddress.From(payload.TransferWithMemo.Receiver),
            OnChainData.From(payload.TransferWithMemo.Memo)
        ),
        PayloadCase.Transfer => new Transfer(
            CcdAmount.From(payload.Transfer.Amount),
            AccountAddress.From(payload.Transfer.Receiver)
        ),
        PayloadCase.RegisterData => new RegisterData(
            OnChainData.From(payload.RegisterData)
        ),
        PayloadCase.DeployModule => new DeployModule(
            VersionedModuleSourceFactory.From(payload.DeployModule)
        ),
        PayloadCase.RawPayload => ParseRawPayload(payload.RawPayload),
        PayloadCase.InitContract => throw new NotImplementedException(),
        PayloadCase.UpdateContract => throw new NotImplementedException(),
        PayloadCase.None => throw new NotImplementedException(),
        _ => throw new MissingEnumException<PayloadCase>(payload.PayloadCase),
    };

    private static AccountTransactionPayload ParseRawPayload(Google.Protobuf.ByteString payload)
    {
        (AccountTransactionPayload?, string?) parsedPayload = (null, null);

        switch ((TransactionType)payload.First())
        {
            case TransactionType.Transfer:
            {
                Transfer.TryDeserial(payload.ToArray(), out var output);
                parsedPayload = output;
                break;
            }
            case TransactionType.TransferWithMemo:
            {
                TransferWithMemo.TryDeserial(payload.ToArray(), out var output);
                parsedPayload = output;
                break;
            }
            case TransactionType.RegisterData:
            {
                RegisterData.TryDeserial(payload.ToArray(), out var output);
                parsedPayload = output;
                break;
            }
            case TransactionType.DeployModule:
            {
                DeployModule.TryDeserial(payload.ToArray(), out var output);
                parsedPayload = output;
                break;
            }

            case TransactionType.InitContract:
                break;
            case TransactionType.Update:
                break;
            case TransactionType.AddBaker:
                break;
            case TransactionType.RemoveBaker:
                break;
            case TransactionType.UpdateBakerStake:
                break;
            case TransactionType.UpdateBakerRestakeEarnings:
                break;
            case TransactionType.UpdateBakerKeys:
                break;
            case TransactionType.UpdateCredentialKeys:
                break;
            case TransactionType.EncryptedAmountTransfer:
                break;
            case TransactionType.TransferToEncrypted:
                break;
            case TransactionType.TransferToPublic:
                break;
            case TransactionType.TransferWithSchedule:
                break;
            case TransactionType.UpdateCredentials:
                break;
            case TransactionType.EncryptedAmountTransferWithMemo:
                break;
            case TransactionType.TransferWithScheduleAndMemo:
                break;
            case TransactionType.ConfigureBaker:
                break;
            case TransactionType.ConfigureDelegation:
                break;
            default:
                throw new NotImplementedException();
        };

        if (parsedPayload.Item2 != null)
        {
            throw new DeserialException(parsedPayload.Item2);
        }
        return parsedPayload.Item1;
    }

    /// <summary>
    /// Prepares the account transaction payload for signing. Will throw an
    /// exception if AccountTransaction is of subtype RawPayload. Should only
    /// be used for testing.
    /// </summary>
    /// <param name="sender">Address of the sender of the transaction.</param>
    /// <param name="sequenceNumber">Account sequence number to use for the transaction.</param>
    /// <param name="expiry">Expiration time of the transaction.</param>
    internal PreparedAccountTransaction PrepareWithException(
        AccountAddress sender,
        AccountSequenceNumber sequenceNumber,
        Expiry expiry
    ) => this switch
    {
        Transfer transfer => transfer.Prepare(sender, sequenceNumber, expiry),
        TransferWithMemo transferWithMemo => transferWithMemo.Prepare(sender, sequenceNumber, expiry),
        DeployModule deployModule => deployModule.Prepare(sender, sequenceNumber, expiry),
        RegisterData registerData => registerData.Prepare(sender, sequenceNumber, expiry),
        _ => throw new NotImplementedException(),
    };

}

