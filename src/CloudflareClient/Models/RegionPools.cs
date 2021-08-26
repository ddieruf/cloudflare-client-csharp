using System.Collections.Generic;

namespace CloudflareClient.Models
{
  public record RegionPools (
    IEnumerable<string> Wnam,
    IEnumerable<string> Enam);
}
