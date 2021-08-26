using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace CloudflareClient.Models
{
  /// <summary>
  /// Zone-level Load Balancer
  /// </summary>
  /// <param name="Id">API item identifier tag</param>
  /// <param name="CreatedOn">Creation time</param>
  /// <param name="ModifiedOn">Last modification time</param>
  /// <param name="Description">Object description</param>
  /// <param name="HostName">The DNS hostname to associate with your Load Balancer. If this hostname already exists as a DNS record in Cloudflare's DNS, the Load Balancer will take precedence and the DNS record will not be used.</param>
  /// <param name="Enabled">Whether to enable (the default) this load balancer.</param>
  /// <param name="Ttl">Time to live (TTL) of the DNS entry for the IP address returned by this load balancer. This only applies to gray-clouded (unproxied) load balancers.</param>
  /// <param name="FallbackPool">The pool ID to use when all other pools are detected as unhealthy.</param>
  /// <param name="DefaultPools">A list of pool IDs ordered by their failover priority. Pools defined here are used by default, or when region_pools are not configured for a given region.</param>
  /// <param name="RegionPools">A mapping of region/country codes to a list of pool IDs (ordered by their failover priority) for the given region. Any regions not explicitly defined will fall back to using default_pools.</param>
  /// <param name="PopPools">(Enterprise only): A mapping of Cloudflare PoP identifiers to a list of pool IDs (ordered by their failover priority) for the PoP (datacenter). Any PoPs not explicitly defined will fall back to using default_pools.</param>
  /// <param name="Proxied">Whether the hostname should be gray clouded (false) or orange clouded (true).</param>
  /// <param name="SteeringPolicy">Steering Policy for this load balancer. "off": use default_pools, "geo": use region_pools/pop_pools, "random": select a pool randomly, "dynamic_latency": use round trip time to select the closest pool in default_pools (requires pool health checks), "proximity": use the pools latitude and longitude to select the closest pool using the cloudflare pop location for proxied requests or geoip / edns0-client-subnet for non-proxied, "": will map to "geo" if you use region_pools/pop_pools otherwise "off"</param>
  /// <param name="SessionAffinity">The session_affinity specifies the type of session affinity the loadbalancer should use unless specified as "none" or ""(default). The supported types are "cookie" and "ip_cookie". "cookie" - On the first request to a proxied load balancer, a cookie is generated, encoding information of which origin the request will be forwarded to. Subsequent requests, by the same client to the same load balancer, will be sent to the origin server the cookie encodes, for the duration of the cookie and as long as the origin server remains healthy. If the cookie has expired or the origin server is unhealthy then a new origin server is calculated and used. "ip_cookie" behaves the same as "cookie" except the initial origin selection is stable and based on the client’s ip address.</param>
  /// <param name="SessionAffinityAttributes">Configure cookie attributes for session affinity cookie.</param>
  /// <param name="SessionAffinityTtl">Time, in seconds, until this load balancers session affinity cookie expires after being created. This parameter is ignored unless a supported session affinity policy is set. The current default of 23 hours will be used unless session_affinity_ttl is explicitly set. The accepted range of values is between [1800, 604800]. Once the expiry time has been reached, subsequent requests may get sent to a different origin server.</param>
  /// <param name="Rules">BETA Field Not General Access: A list of rules for this load balancer to execute.</param>
  public record LoadBalancer (
    [property: JsonPropertyName("name")]
    string HostName,
    string Description,
    string Id,
    [property: JsonPropertyName("created_on")]
    DateTimeOffset CreatedOn,
    [property: JsonPropertyName("modified_on")]
    DateTimeOffset ModifiedOn,
    bool Enabled,
    long Ttl,
    [property: JsonPropertyName("fallback_pool")]
    string FallbackPool,
    [property: JsonPropertyName("default_pools")]
    IEnumerable<string> DefaultPools,
    [property: JsonPropertyName("region_pools")]
    RegionPools RegionPools,
    [property: JsonPropertyName("pop_pools")]
    Dictionary<string, string[]> PopPools,
    bool Proxied,
    [property: JsonPropertyName("steering_policy")]
    SteeringPolicyType SteeringPolicy,
    [property: JsonPropertyName("session_affinity")]
    SessionAffinityType SessionAffinity,
    [property: JsonPropertyName("session_affinity_attributes")]
    SessionAffinityAttributes SessionAffinityAttributes,
    [property: JsonPropertyName("session_affinity_ttl")]
    long SessionAffinityTtl,
    IEnumerable<Rule> Rules);
}
