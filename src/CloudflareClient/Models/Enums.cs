using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public enum StatusType
  {
    Active,
    Pending,
    Initializing,
    Moved,
    Deleted,
    Deactivated,
  }

  public enum PolicyType
  {
    Random,
    Hash
  }

  public enum SessionAffinityType
  {
    None,
    Cookie,
    [JsonPropertyName("ip_cookie")]
    IpCookie,
  }

  public enum SteeringPolicyType{
    Off,
    Geo,
    Random,
    [JsonPropertyName("dynamic_latency")]
    Dynamic_Latency,
    Proximity,
  }

  public enum SaveSiteType
  {
    Auto,
    Lax,
    None,
    Strict
  }

  public enum SecureType
  {
    Auto,
    Always,
    Never
  }
}
