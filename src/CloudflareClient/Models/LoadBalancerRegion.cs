using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  /// Region-Country geographic mappings for load balancers
  /// </summary>
  /// <param name="IsoStandard">The ISO standard followed in the API</param>
  /// <param name="Regions">Associated region to load balancer</param>
  public record LoadBalancerRegion (
    [property: JsonPropertyName("iso_standard")]
    string IsoStandard,
    IEnumerable<Region> Regions);
}
