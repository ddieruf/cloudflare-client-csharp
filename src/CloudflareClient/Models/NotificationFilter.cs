using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record NotificationFilter
  (
    PoolClass Origin,
    PoolClass Pool);
}
