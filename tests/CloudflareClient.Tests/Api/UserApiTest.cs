using System;
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
  public class UserApiTest
  {
    private readonly ILogger<UserApiTest> _logger;
    private readonly ITestOutputHelper _outputHelper;
    private readonly string _xAuthEmail;
    private readonly string _xAuthKey;

    public UserApiTest(ITestOutputHelper outputHelper)
    {
      _outputHelper = outputHelper;
      _logger = LoggerFactory.Create(builder => builder.AddXUnit(outputHelper).SetMinimumLevel(LogLevel.Trace)).CreateLogger<UserApiTest>();

      _xAuthEmail = Environment.GetEnvironmentVariable("xAuthEmail");
      _xAuthKey = Environment.GetEnvironmentVariable("xAuthKey");
    }

    [Fact]
    public void ValidateResponses()
    {
      _ = JsonSerializer.Deserialize<CloudflareResponse<User>>(Responses.UserDetails, ApiClient.SerializerOptions);
      _ = JsonSerializer.Deserialize<CloudflareResponse<User>>(Responses.EditUser, ApiClient.SerializerOptions);
    }

    [Fact(DisplayName = "Get user details success")]
    public async Task UserDetailsSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.UserDetails);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var users = new UserApi(apiClient);

      var user = await users.UserDetailsAsync();
      user.Should().NotBeNull();

      _logger.LogInformation(user.ToString());
    }

    [Fact(DisplayName = "Update user info success")]
    public async Task EditUserSuccess()
    {
      using var server = new MockCloudflareApiServer(_outputHelper, MockCloudflareServerFlag.EditUser);
      var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey, basePath: server.Uri));
      var users = new UserApi(apiClient);
      var user = await users.UserDetailsAsync();
      var updatedUser = user with { Zipcode = "12345"};

      var editUser = await users.EditUserAsync(updatedUser);
      editUser.Should().NotBeNull();
      editUser.Zipcode.Should().Be("12345");
    }
  }
}
