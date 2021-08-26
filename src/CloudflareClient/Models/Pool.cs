using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record Pool (
    string Id,
    string Name,
    bool Healthy,
    bool Changed,
    long MinimumOrigins);
}
