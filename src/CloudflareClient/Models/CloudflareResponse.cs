using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  public record CloudflareResponse<T>(
    T Result,
    bool Success,
    IEnumerable<Error> Errors,
    IEnumerable<string> Messages,
    [property: JsonPropertyName("result_info")]
    ResultInfo ResultInfo);

  public record Error(
    int Code,
    string Message);

  public record ResultInfo(
    int Page,
    [property: JsonPropertyName("per_page")]
    int PagePage,
    int Count,
    [property: JsonPropertyName("total_count")]
    int TotalCount);
}
