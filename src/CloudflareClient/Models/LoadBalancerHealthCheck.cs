using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  /// Load balancer health check
  /// </summary>
  /// <param name="Id">API item identifier tag</param>
  /// <param name="Timestamp">Time of event</param>
  /// <param name="Pool">Associated pool to check</param>
  /// <param name="Origins">Associated origins to check</param>
  public record LoadBalancerHealthCheck (
    long Id,
    DateTimeOffset Timestamp,
    Pool Pool,
    IEnumerable<Origin> Origins);
}
