#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudflareClient.Client;
using CloudflareClient.Models;

namespace CloudflareClient.Api
{
  public class LoadBalancerPoolApi
  {
    private readonly ApiClient _apiClient;

    public LoadBalancerPoolApi(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    private IEnumerable<Origin> BalanceOriginWeight(IEnumerable<Origin> origins)
    {
      if (!origins.Any())
        return origins;

      var equalWeight = Math.Round((double)(1/origins.Count()), 2);
      var updatedOriginsWithWeight = origins.Select(origin => origin with { Weight = equalWeight });
      return updatedOriginsWithWeight;
    }

    private void ValidateLoadBalancerPool(LoadBalancerPool loadBalancerPool)
    {
      if (loadBalancerPool is null)
        throw new Exception("Load balancer pool was not provided");

      if (string.IsNullOrEmpty(loadBalancerPool.Id))
        throw new Exception("Load balancer pool id could not be found");

      if (!loadBalancerPool.Origins.Any())
      {
        throw new Exception("A pool must have at least 1 origin");
      }

      if (loadBalancerPool.Origins.Count() < loadBalancerPool.MinimumOrigins)
      {
        throw new Exception($"The load balancer pool has an origin count of {loadBalancerPool.Origins.Count()}, the minimum can't be set to {loadBalancerPool.MinimumOrigins}. Either add another origin or lower the" +
                            $" minimum origin value.");
      }
    }

    private void ValidateLoadBalancerPool(NewLoadBalancerPool loadBalancerPool)
    {
      if (loadBalancerPool is null)
        throw new Exception("Load balancer pool was not provided");

      if (!loadBalancerPool.Origins.Any())
      {
        throw new Exception("A pool must have at least 1 origin");
      }

      if (loadBalancerPool.Origins.Count() < loadBalancerPool.MinimumOrigins)
      {
        throw new Exception($"The load balancer pool has an origin count of {loadBalancerPool.Origins.Count()}, the minimum can't be set to {loadBalancerPool.MinimumOrigins}. Either add another origin or lower the" +
                            $" minimum origin value.");
      }
    }

    public async Task<LoadBalancerPool> AddOrigin(string loadBalancerPoolId, Origin newOrigin, bool updateIfExists = false, bool balanceWeightAcrossOrigins = true, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerPoolId))
        throw new ArgumentNullException(nameof(loadBalancerPoolId));

      // Get the pool details
      var pool = await LoadBalancerPoolDetailsAsync(loadBalancerPoolId, cancellationToken);

      if (pool is null)
        throw new Exception($"Could not find a load balancer pool with id '{loadBalancerPoolId}'");

      // Ensure there are no origins with that name
      var updatedOrigins = pool.Origins.ToList();
      var matchingOrigin = updatedOrigins.FirstOrDefault(q => q.Address.Equals(newOrigin.Address) || q.Name.Equals(newOrigin.Name));
      if (matchingOrigin != null)
      {
        if (!updateIfExists)
        {
          throw new Exception($"The load balancer already has an origin matching that address or name and the option to update if exists was set to false.");
        }

        updatedOrigins.Remove(matchingOrigin);
      }

      updatedOrigins.Add(newOrigin);

      var updatedPool = pool with { Origins = (balanceWeightAcrossOrigins ? BalanceOriginWeight(updatedOrigins) : updatedOrigins) };

      //Update the pool with origin collection
      var resp = await UpdateLoadBalancerPoolWithCloudflareInfoAsync(updatedPool, cancellationToken);
      return resp.Result;
    }

    public async Task<LoadBalancerPool> RemoveOrigin(string loadBalancerPoolId, string originName, bool balanceWeightAcrossOrigins = true, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerPoolId))
        throw new ArgumentNullException(nameof(loadBalancerPoolId));

      // Get the pool details
      var pool = await LoadBalancerPoolDetailsAsync(loadBalancerPoolId, cancellationToken);

      if (pool is null)
        throw new Exception($"Could not find a load balancer pool with id '{loadBalancerPoolId}'");

      // Remove the origin from collection
      var updatedOrigins = pool.Origins.ToList();
      var matchingOrigin = updatedOrigins.FirstOrDefault(q => q.Name.ToLower().Equals(originName.ToLower()));

      if (matchingOrigin is null)
      {
        return pool;
      }

      updatedOrigins.Remove(matchingOrigin);

      var updatedPool = pool with { Origins = (balanceWeightAcrossOrigins ? BalanceOriginWeight(updatedOrigins) : updatedOrigins) };

      //Update the pool with origin collection
      var resp = await UpdateLoadBalancerPoolWithCloudflareInfoAsync(updatedPool, cancellationToken);
      return resp.Result;
    }

    public async Task<LoadBalancerPool> SetMinimumOrigins(string loadBalancerPoolId, int minimumOrigins, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerPoolId))
        throw new ArgumentNullException(nameof(loadBalancerPoolId));

      // Get the pool details
      var pool = await LoadBalancerPoolDetailsAsync(loadBalancerPoolId, cancellationToken);

      if (pool is null)
        throw new Exception($"Could not find a load balancer pool with id '{loadBalancerPoolId}'");

      // Set the min origins
      var updatedPool = pool with { MinimumOrigins = minimumOrigins };

      //Update the pool with origin collection
      var resp = await UpdateLoadBalancerPoolWithCloudflareInfoAsync(updatedPool, cancellationToken);
      return resp.Result;
    }

    public async Task<LoadBalancerPool> CreateLoadBalancerPoolAsync(NewLoadBalancerPool loadBalancerPool, CancellationToken cancellationToken = default)
    {
      var resp = await CreateLoadBalancerPoolWithCloudflareInfoAsync(loadBalancerPool, cancellationToken);
      return resp.Result;
    }

    public async Task<LoadBalancerPool> LoadBalancerPoolDetailsAsync(string loadBalancerPoolId, CancellationToken cancellationToken = default)
    {
      var resp = await LoadBalancerPoolDetailsWithCloudflareInfoAsync(loadBalancerPoolId, cancellationToken);
      return resp.Result;
    }

    public async Task DeleteLoadBalancerPoolAsync(string loadBalancerPoolId, CancellationToken cancellationToken = default)
    {
      _ = await DeleteLoadBalancerPoolWithCloudflareInfoAsync(loadBalancerPoolId, cancellationToken);
    }

    public async Task<IEnumerable<LoadBalancerPool>> ListLoadBalancerPoolsAsync(CancellationToken cancellationToken = default)
    {
      var resp = await ListLoadBalancerPoolsWithCloudflareInfoAsync(cancellationToken);
      return resp.Result;
    }

    public Task<CloudflareResponse<LoadBalancerPool>> CreateLoadBalancerPoolWithCloudflareInfoAsync(NewLoadBalancerPool loadBalancerPool, CancellationToken cancellationToken = default)
    {
      ValidateLoadBalancerPool(loadBalancerPool);

      var requestOptions = new RequestOptions {Data = loadBalancerPool};

      // make the HTTP request
      return _apiClient.PostAsync<LoadBalancerPool>("/user/load_balancers/pools", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancerPool>> LoadBalancerPoolDetailsWithCloudflareInfoAsync(string loadBalancerPoolId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerPoolId))
        throw new ArgumentNullException(nameof(loadBalancerPoolId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("loadBalancerPool_id", loadBalancerPoolId);

      // make the HTTP request
      return _apiClient.GetAsync<LoadBalancerPool>("user/load_balancers/pools/{loadBalancerPool_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancerPool?>> DeleteLoadBalancerPoolWithCloudflareInfoAsync(string loadBalancerPoolId, CancellationToken cancellationToken = default)
    {
      if (string.IsNullOrEmpty(loadBalancerPoolId))
        throw new ArgumentNullException(nameof(loadBalancerPoolId));

      var requestOptions = new RequestOptions();
      requestOptions.PathParameters.Add("loadBalancerPool_id", loadBalancerPoolId);

      // make the HTTP request
      return _apiClient.DeleteAsync<LoadBalancerPool?>("/user/load_balancers/pools/{loadBalancerPool_id}", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<IEnumerable<LoadBalancerPool>>> ListLoadBalancerPoolsWithCloudflareInfoAsync(CancellationToken cancellationToken = default)
    {
      var requestOptions = new RequestOptions();

      // make the HTTP request
      return _apiClient.GetAsync<IEnumerable<LoadBalancerPool>>("/user/load_balancers/pools", requestOptions, cancellationToken);
    }

    public Task<CloudflareResponse<LoadBalancerPool>> UpdateLoadBalancerPoolWithCloudflareInfoAsync(LoadBalancerPool loadBalancerPool, CancellationToken cancellationToken = default)
    {
      ValidateLoadBalancerPool(loadBalancerPool);

      var requestOptions = new RequestOptions {Data = loadBalancerPool};
      requestOptions.PathParameters.Add("loadBalancerPool_id", loadBalancerPool.Id);

      // make the HTTP request
      return _apiClient.PutAsync<LoadBalancerPool>("user/load_balancers/pools/{loadBalancerPool_id}", requestOptions, cancellationToken);
    }
  }
}
