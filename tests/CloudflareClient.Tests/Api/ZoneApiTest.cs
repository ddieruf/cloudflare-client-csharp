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
  public class ZoneApiTest
  {
    private readonly ILogger<ZoneApiTest> _logger;
    private readonly ITestOutputHelper _outputHelper;
    private readonly Zone _expectedZone;
    private readonly IEnumerable<Zone> _expectedZones;
    private readonly string _xAuthEmail;
    private readonly string _xAuthKey;

    public ZoneApiTest(ITestOutputHelper outputHelper)
    {
      _outputHelper = outputHelper;
      _logger = LoggerFactory.Create(builder => builder.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Trace)).CreateLogger<ZoneApiTest>();

      _xAuthEmail = Environment.GetEnvironmentVariable("xAuthEmail");
      _xAuthKey = Environment.GetEnvironmentVariable("xAuthKey");

      _expectedZone = JsonSerializer.Deserialize<CloudflareResponse<Zone>>(Responses.ZoneDetails, ApiClient.SerializerOptions)?.Result;
      _expectedZones = JsonSerializer.Deserialize<CloudflareResponse<IEnumerable<Zone>>>(Responses.ListZones, ApiClient.SerializerOptions)?.Result;
    }

    [Fact(DisplayName = "Get zone details success")]
    public async Task UserDetailsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.ZoneDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var zoneApi = new ZoneApi(apiClient);

      var zone = await zoneApi.ZoneDetailsAsync(_expectedZone.Id);
      zone.Should().NotBeNull();
      zone.Id.Should().Be(_expectedZone.Id);
    }

    [Fact(DisplayName = "List zones success")]
    public async Task ListZonesSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.ListZones);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var zoneApi = new ZoneApi(apiClient);

      var zones = await zoneApi.ListZonesAsync();
      zones.Should().NotBeNull();
      zones.Count().Should().Be(_expectedZones.Count());
    }
  }
}
