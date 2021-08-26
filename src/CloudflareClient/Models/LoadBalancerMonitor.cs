using System;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  /// Account-level Monitor configurations. Monitors define whether we check over HTTP, HTTPS or TCP, the status code(s) we look for, the interval at which we check, timeouts and response body matching.
  /// </summary>
  /// <param name="Id">API item identifier tag</param>
  /// <param name="CreatedOn">Creation time</param>
  /// <param name="ModifiedOn">Last modification time</param>
  /// <param name="Type">The protocol to use for the health check. Currently supported protocols are 'HTTP','HTTPS' and 'TCP'.</param>
  /// <param name="Description"></param>
  /// <param name="Method">The method to use for the health check. This defaults to 'GET' for HTTP/HTTPS based checks and 'connection_established' for TCP based health checks.</param>
  /// <param name="Path">The endpoint path to health check against. This parameter is only valid for HTTP and HTTPS monitors.</param>
  /// <param name="Header">The HTTP request headers to send in the health check. It is recommended you set a Host header by default. The User-Agent header cannot be overridden. This parameter is only valid for HTTP and HTTPS monitors.</param>
  /// <param name="Port">Port number to connect to for the health check. Required for TCP checks. HTTP and HTTPS checks should only define the port when using a non-standard port (HTTP: default 80, HTTPS: default 443).</param>
  /// <param name="Timeout">The timeout (in seconds) before marking the health check as failed</param>
  /// <param name="Retries">The number of retries to attempt in case of a timeout before marking the origin as unhealthy. Retries are attempted immediately.</param>
  /// <param name="Interval">The interval between each health check. Shorter intervals may improve failover time, but will increase load on the origins as we check from multiple locations.</param>
  /// <param name="ExpectedBody">A case-insensitive sub-string to look for in the response body. If this string is not found, the origin will be marked as unhealthy. This parameter is only valid for HTTP and HTTPS monitors.</param>
  /// <param name="ExpectedCodes">The expected HTTP response codes or code ranges of the health check, comma-separated. This parameter is only valid for HTTP and HTTPS monitors.</param>
  /// <param name="FollowRedirects">Follow redirects if returned by the origin. This parameter is only valid for HTTP and HTTPS monitors.</param>
  /// <param name="AllowInsecure">Do not validate the certificate when monitor use HTTPS. This parameter is currently only valid for HTTP and HTTPS monitors.</param>
  /// <param name="ConsecutiveUp">To be marked healthy the monitored origin must pass this healthcheck N consecutive times</param>
  /// <param name="ConsecutiveDown">To be marked unhealthy the monitored origin must fail this healthcheck N consecutive times</param>
  /// <param name="ProbeZone"></param>
  public record LoadBalancerMonitor (
    string Id,
    [property: JsonPropertyName("created_on")]
    DateTimeOffset CreatedOn,
    [property: JsonPropertyName("modified_on")]
    DateTimeOffset ModifiedOn,
    string Type,
    string Description,
    string Method,
    string Path,
    Header Header,
    long Port,
    long Timeout,
    long Retries,
    long Interval,
    [property: JsonPropertyName("expected_body")]
    string ExpectedBody,
    [property: JsonPropertyName("expected_codes")]
    string ExpectedCodes,
    [property: JsonPropertyName("follow_redirects")]
    bool FollowRedirects,
    [property: JsonPropertyName("allow_insecure")]
    bool AllowInsecure,
    [property: JsonPropertyName("consecutive_up")]
    long ConsecutiveUp,
    [property: JsonPropertyName("consecutive_down")]
    long ConsecutiveDown,
    [property: JsonPropertyName("probe_zone")]
    string ProbeZone);


}
