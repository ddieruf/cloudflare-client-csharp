using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  /// User-level Monitor configurations. Monitors define whether we check over HTTP, HTTPS or TCP, the status code(s) we look for, the interval at which we check, timeouts and response body matching.
  /// </summary>
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
  /// <param name="ProbeZone"></param>
  public record NewLoadBalancerMonitor (
    [NotNull]
    [property: JsonPropertyName("expected_codes")]
    HttpStatusCode[] ExpectedCodes,
    [NotNull]
    string Description,
    int Port = 443,
    CheckMethodType Method = CheckMethodType.Get,
    long Timeout = 5,
    string Path = "\\",
    long Interval = 60,
    long Retries = 2,
    [property: JsonPropertyName("follow_redirects")]
    bool FollowRedirects = false,
    [property: JsonPropertyName("allow_insecure")]
    bool AllowInsecure = false,
    ProtocolType Type = ProtocolType.Https,
    [property: JsonPropertyName("probe_zone")]
    string ProbeZone = default,
    [property: JsonPropertyName("expected_body")]
    string ExpectedBody = default,
    Header Header = default);

  public enum ProtocolType
  {
    Http,
    Https,
    Tcp
  }

  public enum CheckMethodType
  {
    Get,
    [JsonPropertyName("connection_established")]
    ConnectionEstablished
  }
}
