using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record FixedResponse (
    [property: JsonPropertyName("message_body")]
    string MessageBody,
    [property: JsonPropertyName("status_code")]
    long StatusCode,
    [property: JsonPropertyName("content_type")]
    string ContentType,
    string Location);
}