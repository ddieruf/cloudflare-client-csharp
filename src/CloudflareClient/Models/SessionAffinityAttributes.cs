using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="SameSite">Configures the SameSite attribute on session affinity cookie. Value "Auto" will be translated to "Lax" or "None" depending if Always Use HTTPS is enabled. Note: when using value "None", the secure attribute can not be set to "Never".</param>
  /// <param name="Secure">Configures the Secure attribute on session affinity cookie. Value "Always" indicates the Secure attribute will be set in the Set-Cookie header, "Never" indicates the Secure attribute will not be set, and "Auto" will set the Secure attribute depending if Always Use HTTPS</param>
  /// <param name="DrainDuration">Configures the drain duration in seconds. This field is only used when session affinity is enabled on the load balancer.</param>
  public record SessionAffinityAttributes (
    SaveSiteType SameSite,
    SecureType Secure,
    [property: JsonPropertyName("drain_duration")]
    long DrainDuration);
}