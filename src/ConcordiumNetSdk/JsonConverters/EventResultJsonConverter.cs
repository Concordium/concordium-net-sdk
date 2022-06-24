using System.Text.Json;
using System.Text.Json.Serialization;
using ConcordiumNetSdk.Responses.TransactionStatusResponse;

namespace ConcordiumNetSdk.JsonConverters;

public class EventResultJsonConverter : JsonConverter<EventResult>
{
    public override EventResult? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;
        JsonConverterHelper.EnsureTokenType(reader, JsonTokenType.StartObject);

        if (TryGetRejectedEventResult(reader, options, out EventResult? rejectedEventResult))
        {
            return rejectedEventResult;
        }

        if (TryGetTransferWithMemoEventResult(reader, options, out EventResult? transferWithMemoEventResult))
        {
            return transferWithMemoEventResult;
        }

        return JsonSerializer.Deserialize(ref reader, typeof(SuccessfulEventResult), options) as EventResult;
    }

    public override void Write(Utf8JsonWriter writer, EventResult value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    private bool TryGetRejectedEventResult(Utf8JsonReader reader, JsonSerializerOptions options, out EventResult? eventResult)
    {
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
        if (JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "rejectReason", out _))
        {
            eventResult = JsonSerializer.Deserialize(ref reader, typeof(RejectedEventResult), options) as EventResult;
            return true;
        }

        eventResult = null;
        return false;
    }

    private bool TryGetTransferWithMemoEventResult(Utf8JsonReader reader, JsonSerializerOptions options, out EventResult? eventResult)
    {
        using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!JsonConverterHelper.TryGetJsonElement(jsonDocument.RootElement, "events", out JsonElement eventsJsonElement))
        {
            eventResult = null;
            return false;
        }

        var eventJsonElementList = eventsJsonElement.EnumerateArray().ToList();
        if (eventJsonElementList.Count == 2 &&
            IsTransferredEventDiscriminatorExists(eventJsonElementList[0]) &&
            IsMemoEventDiscriminatorExists(eventJsonElementList[1]))
        {
            eventResult = JsonSerializer.Deserialize(ref reader, typeof(TransferWithMemoEventResult), options) as EventResult;
            return true;
        }

        eventResult = null;
        return false;
    }

    private bool IsTransferredEventDiscriminatorExists(JsonElement transferredEvent)
    {
        if (!JsonConverterHelper.TryGetJsonElement(transferredEvent, "tag", out JsonElement tag))
        {
            return false;
        }
        
        string? tagValue = tag.GetString();
        if (string.IsNullOrEmpty(tagValue)) throw new JsonException("The transferredEvent.tag value can not be null or empty.");

        if (tagValue != "Transferred")
        {
            return false;
        }

        return true;
    }

    private bool IsMemoEventDiscriminatorExists(JsonElement transferredEvent)
    {
        if (!JsonConverterHelper.TryGetJsonElement(transferredEvent, "tag", out JsonElement tag))
        {
            return false;
        }
        
        string? tagValue = tag.GetString();
        if (string.IsNullOrEmpty(tagValue)) throw new JsonException("The memoEvent.tag value can not be null or empty.");

        if (tagValue != "TransferMemo")
        {
            return false;
        }

        return true;
    }
}
