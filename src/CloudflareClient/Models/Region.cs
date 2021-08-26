using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="RegionCode">A list of Cloudflare regions. WNAM: Western North America, ENAM: Eastern North America, WEU: Western Europe, EEU: Eastern Europe, NSAM: Northern South America, SSAM: Southern South America, OC: Oceania, ME: Middle East, NAF: North Africa, SAF: South Africa, SAS: Southern Asia, SEAS: South East Asia, NEAS: North East Asia)</param>
  /// <param name="Countries">An array of region-country mappings.</param>
  public record Region (
    string RegionCode,
    IEnumerable<Country> Countries);
}
