using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="SubdivisionCodeA2">Two-letter subdivision code followed in ISO 3166-2.</param>
  /// <param name="SubdivisionName">Subdivision name.</param>
  public record CountrySubdivision (
    [property: JsonPropertyName("subdivision_code_a2")]
    string SubdivisionCodeA2,
    [property: JsonPropertyName("subdivision_name")]
    string SubdivisionName);
}
