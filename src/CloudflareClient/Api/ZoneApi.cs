#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudflareClient.Client;
using CloudflareClient.Models;
using RestSharp.Validation;

namespace CloudflareClient.Api
{
  public class ZoneApi
  {
    private readonly ApiClient _apiClient;

    public ZoneApi(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Zone> CreateZoneAsync(string name, string accountId, bool jumpStart, bool fullZone, CancellationToken cancellationToken = default)
    {
      var resp = await CreateZoneWithCloudflareInfoAsync(name, accountId, jumpStart, fullZone, cancellationToken);
      return resp.Result;
    }

    public async Task<Zone> ZoneDetailsAsync(string zoneId, CancellationToken cancellationToken = default)
    {
      var resp = await ZoneDetailsWithCloudflareInfoAsync(zoneId, cancellationToken);
      return resp.Result;
    }

    public async Task DeleteZoneAsync(string zoneId, CancellationToken cancellationToken = default)
    {
      _ = await DeleteZoneWithCloudflareInfoAsync(zoneId, cancellationToken);
    }

    public async Task<IEnumerable<Zone>> ListZonesAsync(CancellationToken cancellationToken = default)
    {
      var resp = await ListZonesWithCloudflareInfoAsync(cancellationToken);
      return resp.Result;
    }

    public Task<CloudflareResponse<Zone>> CreateZoneWithCloudflareInfoAsync(string name, string accountId, bool jumpStart, bool fullZone, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
      if (string.IsNullOrEmpty(accountId)) throw new ArgumentNullException(nameof(accountId));

      var addStr = $"{{\"name\":\"{name}\",\"account\":{{\"id\":\"{accountId}\"}},\"jump_start\":{jumpStart},\"type\":\"{(fullZone ? "full" : "partial")}\"}}}}";
      var requestOptions = new RequestOptions {Data = addStr};

      // make the HTTP request
      return _apiClient.PostAsync<Zone>("/zones", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<Zone>> ZoneDetailsWithCloudflareInfoAsync(string zoneId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(zoneId))
        throw new ArgumentNullException(nameof(zoneId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("zone_id", zoneId);

      // make the HTTP request
      return _apiClient.GetAsync<Zone>("/zones/{zone_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<Zone?>> DeleteZoneWithCloudflareInfoAsync(string zoneId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(zoneId))
        throw new ArgumentNullException(nameof(zoneId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("zone_id", zoneId);

      // make the HTTP request
      return _apiClient.DeleteAsync<Zone?>("/zones/{zone_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<IEnumerable<Zone>>> ListZonesWithCloudflareInfoAsync(CancellationToken cancellationToken = default)
    {
      var requestOptions = new RequestOptions();

      // make the HTTP request
      return _apiClient.GetAsync<IEnumerable<Zone>>("/zones", requestOptions, cancellationToken);
    }
  }
}
