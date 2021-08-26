using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record PoolClass
  (
    bool Disable,
    object Healthy);
}
