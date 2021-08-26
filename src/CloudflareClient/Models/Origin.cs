using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace CloudflareClient.Models
{
  /// <summary>
  /// Load Balancer Origin
  /// </summary>
  /// <param name="Name">A short name (tag) for the pool. Only alphanumeric characters, hyphens and underscores are allowed.</param>
  /// <param name="Address">The IP address (IPv4 or IPv6) of the origin, or the publicly addressable hostname. Hostnames entered here should resolve directly to the origin, and not be a hostname proxied by Cloudflare.</param>
  /// <param name="Enabled">Whether to enable (the default) this origin within the Pool. Disabled origins will not receive traffic and are excluded from health checks. The origin will only be disabled for the current pool.</param>
  /// <param name="Weight">The weight of this origin relative to other origins in the Pool. Based on the configured weight the total traffic is distributed among origins within the Pool.</param>
  /// <param name="Header">The request header is used to pass additional information with an HTTP request. Currently supported header is 'Host'.</param>
  public record Origin (
    [NotNull]
    string Name,
    [NotNull]
    string Address,
    bool Enabled = true,
    double Weight = 1,
    Header Header = default
    /*string Ip,
    bool Healthy,
    [property: JsonPropertyName("failure_reason")]
    string FailureReason,
    bool Changed*/);
}
