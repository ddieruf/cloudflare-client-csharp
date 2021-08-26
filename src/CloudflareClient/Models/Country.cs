using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="CountryCodeA2">Two-letter alpha-2 country code followed in ISO 3166-1.</param>
  /// <param name="CountryName">Country name.</param>
  /// <param name="CountrySubdivisions"></param>
  public record Country (
    [property: JsonPropertyName("country_code_a2")]
    string CountryCodeA2,
    [property: JsonPropertyName("country_name")]
    string CountryName,
    [property: JsonPropertyName("country_subdivisions")]
    CountrySubdivision[] CountrySubdivisions);
}
