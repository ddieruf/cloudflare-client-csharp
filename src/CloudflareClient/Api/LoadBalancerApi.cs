#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudflareClient.Client;
using CloudflareClient.Models;

namespace CloudflareClient.Api
{
  public class LoadBalancerApi
  {
    private readonly ApiClient _apiClient;

    public LoadBalancerApi(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<LoadBalancer> CreateLoadBalancerAsync(string zoneId, NewLoadBalancer loadBalancer, CancellationToken cancellationToken = default)
    {
      var resp = await CreateLoadBalancerWithCloudflareInfoAsync(zoneId, loadBalancer, cancellationToken);
      return resp.Result;
    }

    public async Task<LoadBalancer> LoadBalancerDetailsAsync(string zoneId, string loadBalancerId, CancellationToken cancellationToken = default)
    {
      var resp = await LoadBalancerDetailsWithCloudflareInfoAsync(zoneId, loadBalancerId, cancellationToken);
      return resp.Result;
    }

    public async Task DeleteLoadBalancerAsync(string zoneId, string loadBalancerId, CancellationToken cancellationToken = default)
    {
      _ = await DeleteLoadBalancerWithCloudflareInfoAsync(zoneId, loadBalancerId, cancellationToken);
    }

    public async Task<IEnumerable<LoadBalancer>> ListLoadBalancersAsync(string zoneId, CancellationToken cancellationToken = default)
    {
      var resp = await ListLoadBalancersWithCloudflareInfoAsync(zoneId, cancellationToken);
      return resp.Result;
    }

    public Task<CloudflareResponse<LoadBalancer>> CreateLoadBalancerWithCloudflareInfoAsync(string zoneId, NewLoadBalancer loadBalancer, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(zoneId))
        throw new ArgumentNullException(nameof(zoneId));

      if (loadBalancer is null)
        throw new ArgumentNullException(nameof(loadBalancer));

      var requestOptions = new RequestOptions {Data = loadBalancer};
      requestOptions.PathParameters.Add("zone_id", zoneId);

      // make the HTTP request
      return _apiClient.PostAsync<LoadBalancer>("/zones/{zone_id}/load_balancers", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancer>> LoadBalancerDetailsWithCloudflareInfoAsync(string zoneId, string loadBalancerId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(zoneId))
        throw new ArgumentNullException(nameof(zoneId));

      if (string.IsNullOrEmpty(loadBalancerId))
        throw new ArgumentNullException(nameof(loadBalancerId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("zone_id", zoneId);
      requestOptions.PathParameters.Add("loadBalancer_id", loadBalancerId);

      // make the HTTP request
      return _apiClient.GetAsync<LoadBalancer>("/zones/{zone_id}/load_balancers/{loadBalancer_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancer?>> DeleteLoadBalancerWithCloudflareInfoAsync(string zoneId, string loadBalancerId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(zoneId))
        throw new ArgumentNullException(nameof(zoneId));

      if (string.IsNullOrEmpty(loadBalancerId))
        throw new ArgumentNullException(nameof(loadBalancerId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("zone_id", zoneId);
      requestOptions.PathParameters.Add("loadBalancer_id", loadBalancerId);

      // make the HTTP request
      return _apiClient.DeleteAsync<LoadBalancer?>("/zones/{zone_id}/load_balancers/{loadBalancer_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<IEnumerable<LoadBalancer>>> ListLoadBalancersWithCloudflareInfoAsync(string zoneId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(zoneId))
        throw new ArgumentNullException(nameof(zoneId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("zone_id", zoneId);

      // make the HTTP request
      return _apiClient.GetAsync<IEnumerable<LoadBalancer>>("/zones/{zone_id}/load_balancers", requestOptions, cancellationToken);
    }
  }
}
