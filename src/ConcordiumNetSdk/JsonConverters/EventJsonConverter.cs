using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.TransactionStatusResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class EventJsonConverter : JsonConverter<Event>
{
    private static readonly Dictionary<string, Type> TypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // the key is discriminator value
        {"ModuleDeployed", typeof(TransactionEvent)},
        {"ContractInitialized", typeof(TransactionEvent)},
        {"AccountCreated", typeof(TransactionEvent)},
        {"CredentialDeployed", typeof(TransactionEvent)},
        {"BakerAdded", typeof(TransactionEvent)},
        {"BakerRemoved", typeof(TransactionEvent)},
        {"BakerStakeIncreased", typeof(TransactionEvent)},
        {"BakerStakeDecreased", typeof(TransactionEvent)},
        {"BakerSetRestakeEarnings", typeof(TransactionEvent)},
        {"BakerKeysUpdated", typeof(TransactionEvent)},
        {"CredentialKeysUpdated", typeof(TransactionEvent)},
        {"NewEncryptedAmount", typeof(TransactionEvent)},
        {"EncryptedAmountsRemoved", typeof(TransactionEvent)},
        {"AmountAddedByDecryption", typeof(TransactionEvent)},
        {"EncryptedSelfAmountAdded", typeof(TransactionEvent)},
        {"UpdateEnqueued", typeof(TransactionEvent)},
        // {"TransferredWithSchedule", typeof(TransactionEvent)},
        {"CredentialsUpdated", typeof(TransactionEvent)},
        {"DataRegistered", typeof(TransactionEvent)},
        {"Interrupted", typeof(TransactionEvent)},
        {"Resumed", typeof(TransactionEvent)},
        {"BakerSetOpenStatus", typeof(TransactionEvent)},
        {"BakerSetMetadataURL", typeof(TransactionEvent)},
        {"BakerSetTransactionFeeCommission", typeof(TransactionEvent)},
        {"BakerSetBakingRewardCommission", typeof(TransactionEvent)},
        {"BakerSetFinalizationRewardCommission", typeof(TransactionEvent)},
        {"DelegationStakeIncreased", typeof(TransactionEvent)},
        {"DelegationStakeDecreased", typeof(TransactionEvent)},
        {"DelegationSetRestakeEarnings", typeof(TransactionEvent)},
        {"DelegationSetDelegationTarget", typeof(TransactionEvent)},
        {"DelegationAdded", typeof(TransactionEvent)},
        {"DelegationRemoved", typeof(TransactionEvent)},
        {"Updated", typeof(UpdatedEvent)},
        {"Transferred", typeof(TransferredEvent)},
        {"TransferredWithSchedule", typeof(TransferredWithScheduleEvent)},
        {"TransferMemo", typeof(MemoEvent)}
    };

    public override Event? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        Utf8JsonReader readerClone = reader;
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref readerClone);

        JsonElement tag = jsonDocument.RootElement.GetProperty("tag");
        string? tagValue = tag.GetString();

        if (string.IsNullOrEmpty(tagValue))
            throw new JsonException("The event.tag value can not be null or empty.");

        if (!TypeMap.TryGetValue(tagValue, out Type? targetType))
            throw new JsonException($"The event.tag value: '{tagValue}' is not supported.");

        return JsonSerializer.Deserialize(ref reader, targetType, options) as Event;
    }

    public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
