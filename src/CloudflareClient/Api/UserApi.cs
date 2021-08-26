using System;
using System.Threading;
using System.Threading.Tasks;
using CloudflareClient.Client;
using CloudflareClient.Models;

namespace CloudflareClient.Api
{
  /// <summary>
  /// Represents a collection of functions to interact with the API endpoints
  /// </summary>
  public class UserApi
    {
      private readonly ApiClient _apiClient;

      /// <summary>
      /// Initializes a new instance of the <see cref="UserApi"/> class
      /// using <see cref="ApiClient"/> object
      /// </summary>
      /// <param name="apiClient">The client</param>
      public UserApi(ApiClient apiClient)
      {
        _apiClient = apiClient;
      }

      /// <summary>
      /// Edit User
      /// </summary>
      /// <param name="user"></param>
      /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
      /// <returns>Updated User</returns>
      public async Task<User> EditUserAsync(User user, CancellationToken cancellationToken = default)
      {
        var resp = await EditUserWithCloudflareInfoAsync(user, cancellationToken);
        return resp.Result;
      }

      /// <summary>
      /// User Details
      /// </summary>
      /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
      /// <returns>User</returns>
      public async Task<User> UserDetailsAsync(CancellationToken cancellationToken = default)
      {
        var resp = await UserDetailsWithCloudflareInfoAsync(cancellationToken);
        return resp.Result;
      }

      /// <summary>
      /// Edit User
      /// </summary>
      /// <exception cref="ApiException">Thrown when fails to make API call</exception>
      /// <param name="user"></param>
      /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
      /// <returns>Task of CloudflareResponse</returns>
      public Task<CloudflareResponse<User>> EditUserWithCloudflareInfoAsync(User user, CancellationToken cancellationToken = default)
      {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var requestOptions = new RequestOptions {Data = user};

        // make the HTTP request
        return _apiClient.PatchAsync<User>("/user", requestOptions, cancellationToken);
      }

      /// <summary>
      /// User Details
      /// </summary>
      /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
      /// <returns>Task of CloudflareResponse</returns>
      public Task<CloudflareResponse<User>> UserDetailsWithCloudflareInfoAsync(CancellationToken cancellationToken = default)
      {
        var requestOptions = new RequestOptions();

        // make the HTTP request
        return _apiClient.GetAsync<User>("/user", requestOptions, cancellationToken);
      }

    }
}
