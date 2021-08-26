using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="DefaultPercent">Percent of traffic to shed from the pool, according to the default policy. Applies to new sessions and traffic without session affinity.</param>
  /// <param name="DefaultPolicy">The default policy to use when load shedding. A random policy randomly sheds a given percent of requests. A hash policy computes a hash over the CF-Connecting-IP address and sheds all requests originating from a percent of IPs.</param>
  /// <param name="SessionPercent">Percent of existing sessions to shed from the pool, according to the session policy.</param>
  /// <param name="SessionPolicy">Only the hash policy is supported for existing sessions (to avoid exponential decay).</param>
  public record LoadShedding
  (
    [property: JsonPropertyName("default_percent")]
    long DefaultPercent,

    [property: JsonPropertyName("default_policy")]
    PolicyType DefaultPolicy,

    [property: JsonPropertyName("session_percent")]
    long SessionPercent,

    [property: JsonPropertyName("session_policy")]
    PolicyType SessionPolicy);
}