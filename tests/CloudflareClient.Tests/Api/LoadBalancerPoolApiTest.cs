using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CloudflareClient.Api;
using CloudflareClient.Client;
using CloudflareClient.Models;
using CloudflareClient.Tests.Mock;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace CloudflareClient.Tests.Api
{
  public class LoadBalancerPoolApiTest
  {
    private readonly ILogger<LoadBalancerPoolApiTest> _logger;
    private readonly ITestOutputHelper _outputHelper;
    private readonly LoadBalancerPool _expectedLoadBalancerPool;
    private readonly IEnumerable<LoadBalancerPool> _expectedLoadBalancerPools;
    private readonly string _xAuthEmail;
    private readonly string _xAuthKey;

    public LoadBalancerPoolApiTest(ITestOutputHelper outputHelper)
    {
      _outputHelper = outputHelper;
      _logger = LoggerFactory.Create(builder => builder.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Trace)).CreateLogger<LoadBalancerPoolApiTest>();

      _xAuthEmail = Environment.GetEnvironmentVariable("xAuthEmail");
      _xAuthKey = Environment.GetEnvironmentVariable("xAuthKey");

      _expectedLoadBalancerPool = JsonSerializer.Deserialize<CloudflareResponse<LoadBalancerPool>>(Responses.LoadBalancerPoolDetails, ApiClient.SerializerOptions)?.Result;
      _expectedLoadBalancerPools = JsonSerializer.Deserialize<CloudflareResponse<IEnumerable<LoadBalancerPool>>>(Responses.ListLoadBalancerPools, ApiClient.SerializerOptions)?.Result;
    }

    [Fact(DisplayName = "Get loadBalancerPool details success")]
    public async Task LoadBalancerDetailsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerPoolDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);

      var loadBalancerPool = await loadBalancerPoolApi.LoadBalancerPoolDetailsAsync( _expectedLoadBalancerPool.Id);
      loadBalancerPool.Should().NotBeNull();
      loadBalancerPool.Id.Should().Be(_expectedLoadBalancerPool.Id);
    }

    [Fact(DisplayName = "List loadBalancerPools success")]
    public async Task ListLoadBalancerPoolsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.ListLoadBalancerPools);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);

      var loadBalancerPools = await loadBalancerPoolApi.ListLoadBalancerPoolsAsync();
      loadBalancerPools.Should().NotBeNull();
      loadBalancerPools.Count().Should().Be(_expectedLoadBalancerPools.Count());
    }

    [Fact(DisplayName = "Create loadBalancerPool success")]
    public async Task CreateLoadBalancerPoolSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.CreateLoadBalancerPool);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);

      var newLoadBalancerPool = new NewLoadBalancerPool(
        Name: "An origin cluster pool",
        Description: "The description or the origin",
        Enabled: true,
        CheckRegions: null,
        LoadShedding: new LoadShedding(DefaultPercent: 50, DefaultPolicy: PolicyType.Random, SessionPercent: 50, SessionPolicy: PolicyType.Random),
        MinimumOrigins: 2,
        MonitorId: "asdfa",
        Origins: new []{
          new Origin(
            Name: "Cloud 1",
            Address: "127.0.0.1",
            Enabled: true,
            Weight: 0.33,
            Header: null
          ),
          new Origin(
            Name: "Cloud 2",
            Address: "127.0.0.1",
            Enabled: true,
            Weight: 0.33,
            Header: null
          ),
          new Origin(
            Name: "Cloud 3",
            Address: "127.0.0.1",
            Enabled: true,
            Weight: 0.33,
            Header: null
          )
        },
        NotificationEmail: null,
        NotificationFilter: null,
        Latitude: null,
        Longitude: null
        );

      var loadBalancerPool = await loadBalancerPoolApi.CreateLoadBalancerPoolAsync(newLoadBalancerPool);
      loadBalancerPool.Should().NotBeNull();
      loadBalancerPool.Id.Should().Be(_expectedLoadBalancerPool.Id);
    }

    [Fact(DisplayName = "Delete loadBalancerPool success")]
    public async Task DeleteLoadBalancerPoolSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);

      await loadBalancerPoolApi.DeleteLoadBalancerPoolAsync(_expectedLoadBalancerPool.Id);
    }

    [Fact(DisplayName = "Update existing origin success")]
    public async Task UpdateExistingOriginSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerPoolDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);
      var updatedOrigin = _expectedLoadBalancerPool.Origins.First();

      await loadBalancerPoolApi.AddOrigin(_expectedLoadBalancerPool.Id, updatedOrigin, updateIfExists: true);
    }

    [Fact(DisplayName = "Add new origin success")]
    public async Task AddNewOriginSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerPoolDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);
      var newOrigin = new Origin(
        Name: "Cloud 9",
        Address: "127.0.0.100"
      );

      await loadBalancerPoolApi.AddOrigin(_expectedLoadBalancerPool.Id, newOrigin);
    }

    [Fact(DisplayName = "Update origin already exists")]
    public async Task UpdateOriginAlreadyExists()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerPoolDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);
      var updatedOrigin = _expectedLoadBalancerPool.Origins.First();

      Func<Task> act = () => loadBalancerPoolApi.AddOrigin(_expectedLoadBalancerPool.Id, updatedOrigin, updateIfExists: false);
      await act.Should().ThrowAsync<Exception>();
    }

    [Fact(DisplayName = "Remove origin success")]
    public async Task RemoveOriginSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerPoolDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);
      var originName = _expectedLoadBalancerPool.Origins.First().Name;

      await loadBalancerPoolApi.RemoveOrigin(_expectedLoadBalancerPool.Id, originName);
    }

    [Fact(DisplayName = "Set min origins success")]
    public async Task SetMinimumOriginsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerPoolDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);

      await loadBalancerPoolApi.SetMinimumOrigins(_expectedLoadBalancerPool.Id, 1);
    }
  }
}
