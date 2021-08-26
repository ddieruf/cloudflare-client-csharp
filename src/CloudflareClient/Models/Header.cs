using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  /// The request header is used to pass additional information with an HTTP request. Currently supported header is 'Host'.
  /// </summary>
  /// <param name="Host">The 'Host' header allows to override the hostname set in the HTTP request. Current support is 1 'Host' header override per origin.</param>
  /// <param name="XAppId"></param>
  public record Header (
    IEnumerable<string> Host,
    [property: JsonPropertyName("X-App-ID")]
    IEnumerable<string> XAppId);
}
