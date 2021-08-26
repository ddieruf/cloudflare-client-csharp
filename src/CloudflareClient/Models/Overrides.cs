using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record Overrides (
    [property: JsonPropertyName("session_affinity")]
    string SessionAffinity,
    [property: JsonPropertyName("session_affinity_ttl")]
    long SessionAffinityTtl,
    [property: JsonPropertyName("session_affinity_attributes")]
    SessionAffinityAttributes SessionAffinityAttributes,
    long Ttl,
    [property: JsonPropertyName("steering_policy")]
    SteeringPolicyType SteeringPolicy,
    [property: JsonPropertyName("fallback_pool")]
    string FallbackPool,
    [property: JsonPropertyName("default_pools")]
    IEnumerable<string> DefaultPools,
    [property: JsonPropertyName("pop_pools")]
    Dictionary<string, string[]> PopPools,
    [property: JsonPropertyName("region_pools")]
    RegionPools RegionPools);
}
