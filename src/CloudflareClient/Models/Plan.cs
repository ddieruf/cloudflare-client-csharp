using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record Plan (
    string Id,
    string Name,
    long Price,
    string Currency,
    string Frequency,
    [property: JsonPropertyName("legacy_id")]
    string LegacyId,
    [property: JsonPropertyName("is_subscribed")]
    bool IsSubscribed,
    [property: JsonPropertyName("can_subscribe")]
    bool CanSubscribe);
}
