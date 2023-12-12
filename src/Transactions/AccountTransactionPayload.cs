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
    /// Gets the size (number of bytes) of the payload.
    /// </summary>
    internal abstract PayloadSize Size();

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
            case TransactionType.Update:
            case TransactionType.AddBaker:
            case TransactionType.RemoveBaker:
            case TransactionType.UpdateBakerStake:
            case TransactionType.UpdateBakerRestakeEarnings:
            case TransactionType.UpdateBakerKeys:
            case TransactionType.UpdateCredentialKeys:
            case TransactionType.EncryptedAmountTransfer:
            case TransactionType.TransferToEncrypted:
            case TransactionType.TransferToPublic:
            case TransactionType.TransferWithSchedule:
            case TransactionType.UpdateCredentials:
            case TransactionType.EncryptedAmountTransferWithMemo:
            case TransactionType.TransferWithScheduleAndMemo:
            case TransactionType.ConfigureBaker:
            case TransactionType.ConfigureDelegation:
                parsedPayload = (new RawPayload(payload.ToArray()), null);
                break;
            default:
                throw new MissingEnumException<TransactionType>((TransactionType)payload.First());
        };

        if (parsedPayload.Item2 != null)
        {
            throw new DeserialException(parsedPayload.Item2);
        }
        if (parsedPayload.Item1 == null)
        {
            throw new DeserialInvalidResultException();
        }
        return parsedPayload.Item1;
    }
}

