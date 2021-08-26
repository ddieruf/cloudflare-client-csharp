using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record Zone (
    string Id,
    string Name,
    [property: JsonPropertyName("development_mode")]
    long DevelopmentMode,
    [property: JsonPropertyName("original_name_servers")]
    string[] OriginalNameServers,
    [property: JsonPropertyName("original_registrar")]
    string OriginalRegistrar,
    [property: JsonPropertyName("original_dnshost")]
    string OriginalDnshost,
    [property: JsonPropertyName("created_on")]
    DateTimeOffset CreatedOn,
    [property: JsonPropertyName("modified_on")]
    DateTimeOffset ModifiedOn,
    [property: JsonPropertyName("activated_on")]
    DateTimeOffset ActivatedOn,
    Owner Owner,
    Account Account,
    string[] Permissions,
    Plan Plan,
    [property: JsonPropertyName("plan_pending")]
    Plan PlanPending,
    StatusType Status,
    bool Paused,
    string Type,
    [property: JsonPropertyName("name_servers")]
    string[] NameServers);
}
