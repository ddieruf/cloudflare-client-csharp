using System;
using System.Collections.Generic;
using System.Linq;
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
  public class LoadBalancerApiTest
  {
    private readonly ILogger<LoadBalancerApiTest> _logger;
    private readonly ITestOutputHelper _outputHelper;
    private readonly LoadBalancer _expectedLoadBalancer;
    private readonly IEnumerable<LoadBalancer> _expectedLoadBalancers;
    private readonly Zone _zone;
    private readonly string _xAuthEmail;
    private readonly string _xAuthKey;

    public LoadBalancerApiTest(ITestOutputHelper outputHelper)
    {
      _outputHelper = outputHelper;
      _logger = LoggerFactory.Create(builder => builder.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Trace)).CreateLogger<LoadBalancerApiTest>();

      _xAuthEmail = Environment.GetEnvironmentVariable("xAuthEmail");
      _xAuthKey = Environment.GetEnvironmentVariable("xAuthKey");

      _zone = JsonSerializer.Deserialize<CloudflareResponse<Zone>>(Responses.ZoneDetails, ApiClient.SerializerOptions)?.Result;
      _expectedLoadBalancer = JsonSerializer.Deserialize<CloudflareResponse<LoadBalancer>>(Responses.LoadBalancerDetails, ApiClient.SerializerOptions)?.Result;
      _expectedLoadBalancers = JsonSerializer.Deserialize<CloudflareResponse<IEnumerable<LoadBalancer>>>(Responses.ListLoadBalancers, ApiClient.SerializerOptions)?.Result;
    }

    [Fact(DisplayName = "Get loadBalancer details success")]
    public async Task UserDetailsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerApi = new LoadBalancerApi(apiClient);

      var loadBalancer = await loadBalancerApi.LoadBalancerDetailsAsync(_zone.Id, _expectedLoadBalancer.Id);
      loadBalancer.Should().NotBeNull();
      loadBalancer.Id.Should().Be(_expectedLoadBalancer.Id);
    }

    [Fact(DisplayName = "List loadBalancers success")]
    public async Task ListLoadBalancersSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.ListLoadBalancers);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerApi = new LoadBalancerApi(apiClient);

      var loadBalancers = await loadBalancerApi.ListLoadBalancersAsync(_zone.Id);
      loadBalancers.Should().NotBeNull();
      loadBalancers.Count().Should().Be(_expectedLoadBalancers.Count());
    }

    [Fact(DisplayName = "Create loadBalancer success")]
    public async Task CreateLoadBalancerSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.CreateLoadBalancer);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerApi = new LoadBalancerApi(apiClient);
      var newLoadBalancer = new NewLoadBalancer(
        HostName: "somewhere.kpush.io",
        DefaultPools: new []{"17b5962d775c646f3f9725cbc7a53df4"},
        FallbackPool: "17b5962d775c646f3f9725cbc7a53df4",
        Description: "The description",
        SteeringPolicy: SteeringPolicyType.Off,
        SessionAffinityAttributes: null,
        PopPools: null,
        Ttl: null,
        RegionPools: null,
        SessionAffinity: SessionAffinityType.None,
        Rules: null,
        Proxied: true,
        SessionAffinityTtl: null
      );

      var loadBalancer = await loadBalancerApi.CreateLoadBalancerAsync(_zone.Id, newLoadBalancer);
      loadBalancer.Should().NotBeNull();
      loadBalancer.Id.Should().Be(_expectedLoadBalancer.Id);
    }

    [Fact(DisplayName = "Delete loadBalancer success")]
    public async Task DeleteLoadBalancerSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerApi = new LoadBalancerApi(apiClient);

      await loadBalancerApi.DeleteLoadBalancerAsync(_zone.Id, _expectedLoadBalancer.Id);
    }
  }
}
