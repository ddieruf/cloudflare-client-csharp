using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="Description">A human-readable description of the pool.</param>
  /// <param name="Name">A short name (tag) for the pool. Only alphanumeric characters, hyphens and underscores are allowed.</param>
  /// <param name="Enabled">Whether to enable (the default) this pool. Disabled pools will not receive traffic and are excluded from health checks. Disabling a pool will cause any load balancers using it to failover to the next pool (if any).</param>
  /// <param name="CheckRegions">A list of regions from which to run health checks. Null means every Cloudflare datacenter.</param>
  /// <param name="LoadShedding">Configure load shedding policies and percentages for the pool.</param>
  /// <param name="MinimumOrigins">The minimum number of origins that must be healthy for this pool to serve traffic. If the number of healthy origins falls below this number, the pool will be marked unhealthy and we wil failover to the next available pool.</param>
  /// <param name="MonitorId">The ID of the Monitor to use for health checking origins within this pool.</param>
  /// <param name="Origins">The list of origins within this pool. Traffic directed at this pool is balanced across all currently healthy origins, provided the pool itself is healthy.</param>
  /// <param name="NotificationEmail">The email address to send health status notifications to. This can be an individual mailbox or a mailing list. Multiple emails can be supplied as a comma delimited list.</param>
  /// <param name="NotificationFilter">Filter pool and origin health notifications by resource type or health status. Use null to reset.</param>
  /// <param name="Latitude">The latitude of datacenter containing the origins used in this pool in decimal degrees. If this is set longitude must also be.</param>
  /// <param name="Longitude">The longitude of datacenter containing the origins used in this pool in decimal degrees. If this is set latitude must also be.</param>
  public record NewLoadBalancerPool(
    [NotNull]
    string Name,
    [NotNull]
    IEnumerable<Origin> Origins,
    [NotNull]
    [property: JsonPropertyName("monitor")]
    string MonitorId,
    [NotNull]
    string Description,
    bool Enabled = true,
    [property: JsonPropertyName("load_shedding")]
    LoadShedding LoadShedding = default,
    [property: JsonPropertyName("minimum_origins")]
    long MinimumOrigins = 1,
    [property: JsonPropertyName("check_regions")]
    List<string> CheckRegions = default,
    [property: JsonPropertyName("notification_email")]
    string NotificationEmail = default,
    [property: JsonPropertyName("notification_filter")]
    NotificationFilter NotificationFilter = default,
    long? Latitude = null,
    long? Longitude = null
  );
}
