using System;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record User(
    string Id,
    string Email,
    [property: JsonPropertyName("first_name")]
    string FirstName,
    [property: JsonPropertyName("last_name")]
    string LastName,
    string Username,
    string Telephone,
    string Country,
    string Zipcode,
    [property: JsonPropertyName("created_on")]
    DateTime CreatedOn,
    [property: JsonPropertyName("modified_on")]
    DateTime ModifiedOn,
    [property: JsonPropertyName("two_factor_authentication_enabled")]
    bool TwoFactorAuthenticationEnabled,
    bool Suspended);
}
