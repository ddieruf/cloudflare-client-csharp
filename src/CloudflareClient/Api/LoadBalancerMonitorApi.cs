#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudflareClient.Client;
using CloudflareClient.Models;

namespace CloudflareClient.Api
{
  public class LoadBalancerMonitorApi
  {
    private readonly ApiClient _apiClient;

    public LoadBalancerMonitorApi(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<LoadBalancerMonitor> CreateLoadBalancerMonitorAsync(NewLoadBalancerMonitor loadBalancerMonitor, CancellationToken cancellationToken = default)
    {
      var resp = await CreateLoadBalancerMonitorWithCloudflareInfoAsync(loadBalancerMonitor, cancellationToken);
      return resp.Result;
    }

    public async Task<LoadBalancerMonitor> LoadBalancerMonitorDetailsAsync(string loadBalancerMonitorId, CancellationToken cancellationToken = default)
    {
      var resp = await LoadBalancerMonitorDetailsWithCloudflareInfoAsync(loadBalancerMonitorId, cancellationToken);
      return resp.Result;
    }

    public async Task DeleteLoadBalancerMonitorAsync(string loadBalancerMonitorId, CancellationToken cancellationToken = default)
    {
      _ = await DeleteLoadBalancerMonitorWithCloudflareInfoAsync(loadBalancerMonitorId, cancellationToken);
    }

    public async Task<IEnumerable<LoadBalancerMonitor>> ListLoadBalancerMonitorsAsync(CancellationToken cancellationToken = default)
    {
      var resp = await ListLoadBalancerMonitorsWithCloudflareInfoAsync(cancellationToken);
      return resp.Result;
    }

    public Task<CloudflareResponse<LoadBalancerMonitor>> CreateLoadBalancerMonitorWithCloudflareInfoAsync(NewLoadBalancerMonitor loadBalancerMonitor, CancellationToken cancellationToken = default)
    {
      if (loadBalancerMonitor is null)
        throw new ArgumentNullException(nameof(loadBalancerMonitor));

      var requestOptions = new RequestOptions {Data = loadBalancerMonitor};

      // make the HTTP request
      return _apiClient.PostAsync<LoadBalancerMonitor>("/user/load_balancers/pools", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancerMonitor>> LoadBalancerMonitorDetailsWithCloudflareInfoAsync(string loadBalancerMonitorId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerMonitorId))
        throw new ArgumentNullException(nameof(loadBalancerMonitorId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("loadBalancerMonitor_id", loadBalancerMonitorId);

      // make the HTTP request
      return _apiClient.GetAsync<LoadBalancerMonitor>("user/load_balancers/pools/{loadBalancerMonitor_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancerMonitor?>> DeleteLoadBalancerMonitorWithCloudflareInfoAsync(string loadBalancerMonitorId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerMonitorId))
        throw new ArgumentNullException(nameof(loadBalancerMonitorId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("loadBalancerMonitor_id", loadBalancerMonitorId);

      // make the HTTP request
      return _apiClient.DeleteAsync<LoadBalancerMonitor?>("/user/load_balancers/pools/{loadBalancerMonitor_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<IEnumerable<LoadBalancerMonitor>>> ListLoadBalancerMonitorsWithCloudflareInfoAsync(CancellationToken cancellationToken = default)
    {
      var requestOptions = new RequestOptions();

      // make the HTTP request
      return _apiClient.GetAsync<IEnumerable<LoadBalancerMonitor>>("/user/load_balancers/pools", requestOptions, cancellationToken);
    }
  }
}
