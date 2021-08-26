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
  public class LoadBalancerMonitorApiTest
  {
    private readonly ILogger<LoadBalancerMonitorApiTest> _logger;
    private readonly ITestOutputHelper _outputHelper;
    private readonly LoadBalancerMonitor _expectedLoadBalancerMonitor;
    private readonly IEnumerable<LoadBalancerMonitor> _expectedLoadBalancerMonitors;
    private readonly string _xAuthEmail;
    private readonly string _xAuthKey;

    public LoadBalancerMonitorApiTest(ITestOutputHelper outputHelper)
    {
      _outputHelper = outputHelper;
      _logger = LoggerFactory.Create(builder => builder.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Trace)).CreateLogger<LoadBalancerMonitorApiTest>();

      _xAuthEmail = Environment.GetEnvironmentVariable("xAuthEmail");
      _xAuthKey = Environment.GetEnvironmentVariable("xAuthKey");

      _expectedLoadBalancerMonitor = JsonSerializer.Deserialize<CloudflareResponse<LoadBalancerMonitor>>(Responses.LoadBalancerMonitorDetails, ApiClient.SerializerOptions)?.Result;
      _expectedLoadBalancerMonitors = JsonSerializer.Deserialize<CloudflareResponse<IEnumerable<LoadBalancerMonitor>>>(Responses.ListLoadBalancerMonitors, ApiClient.SerializerOptions)?.Result;
    }

    [Fact(DisplayName = "Get loadBalancerMonitor details success")]
    public async Task UserDetailsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerMonitorDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerMonitorApi = new LoadBalancerMonitorApi(apiClient);

      var loadBalancerMonitor = await loadBalancerMonitorApi.LoadBalancerMonitorDetailsAsync( _expectedLoadBalancerMonitor.Id);
      loadBalancerMonitor.Should().NotBeNull();
      loadBalancerMonitor.Id.Should().Be(_expectedLoadBalancerMonitor.Id);
    }

    [Fact(DisplayName = "List loadBalancerMonitors success")]
    public async Task ListLoadBalancerMonitorsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.ListLoadBalancerMonitors);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerMonitorApi = new LoadBalancerMonitorApi(apiClient);

      var loadBalancerMonitors = await loadBalancerMonitorApi.ListLoadBalancerMonitorsAsync();
      loadBalancerMonitors.Should().NotBeNull();
      loadBalancerMonitors.Count().Should().Be(_expectedLoadBalancerMonitors.Count());
    }

    [Fact(DisplayName = "Create loadBalancerMonitor success")]
    public async Task CreateLoadBalancerMonitorSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.LoadBalancerMonitorDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerMonitorApi = new LoadBalancerMonitorApi(apiClient);

      var newLoadBalancerMonitor = new NewLoadBalancerMonitor(
        ExpectedCodes: new [] { HttpStatusCode.OK },
        Port: 443,
        Method: CheckMethodType.Get,
        Timeout: 5,
        Path: "\\health",
        Interval: 30,
        Retries: 2,
        FollowRedirects: true,
        ExpectedBody: "",
        Header: null,
        AllowInsecure: false,
        Type: ProtocolType.Https,
        ProbeZone: null,
        Description: ""
      );

      var loadBalancerMonitor = await loadBalancerMonitorApi.CreateLoadBalancerMonitorAsync(newLoadBalancerMonitor);
      loadBalancerMonitor.Should().NotBeNull();
      loadBalancerMonitor.Id.Should().Be(_expectedLoadBalancerMonitor.Id);
    }

    [Fact(DisplayName = "Delete loadBalancerMonitor success")]
    public async Task DeleteLoadBalancerMonitorSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var loadBalancerMonitorApi = new LoadBalancerMonitorApi(apiClient);

      await loadBalancerMonitorApi.DeleteLoadBalancerMonitorAsync(_expectedLoadBalancerMonitor.Id);
    }
  }
}
