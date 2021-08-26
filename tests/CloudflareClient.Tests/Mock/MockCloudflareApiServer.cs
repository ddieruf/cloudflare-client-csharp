using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using RestSharp.Serialization;
using Xunit.Abstractions;

namespace CloudflareClient.Tests.Mock
{
  /// <summary>
  /// Flags to configure how the server will respond to requests
  /// </summary>
  public enum MockCloudflareServerFlag
  {
    Throw500,
    UserDetails,
    EditUser,
    CreateLoadBalancer,
    ListLoadBalancers,
    ZoneDetails,
    ListZones,
    CreateZone,
    LoadBalancerDetails,
    LoadBalancerPoolDetails,
    ListLoadBalancerPools,
    CreateLoadBalancerPool,
    LoadBalancerMonitorDetails,
    CreateLoadBalancerMonitor,
    ListLoadBalancerMonitors
  }

  internal class MockCloudflareApiServer : IDisposable
  {
    public Uri Uri => _webHost.ServerFeatures.Get<IServerAddressesFeature>().Addresses.Select(a => new Uri(a)).First();

    private readonly IWebHost _webHost;
    private readonly MockCloudflareServerFlag[] _serverFlags;
    private bool _disposed;

    public MockCloudflareApiServer(ITestOutputHelper testOutput, params MockCloudflareServerFlag[] serverFlags)
    {
      _serverFlags = serverFlags;
      _webHost = BuildWebHost(testOutput, ShouldNext, null, null); // to avoid making ShouldNext static
      _webHost.Start();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      // Execute if resources have not already been disposed.
      if (!_disposed)
      {
        // If the call is from Dispose, free managed resources.
        if (disposing)
        {
          _webHost.StopAsync();
          _webHost.WaitForShutdown();
          _webHost.Dispose();
        }
      }

      _disposed = true;
    }

    private async Task WriteServerResponse(HttpContext httpContext, string reponseLine)
    {
      const string crlf = "\r\n";
      await httpContext.Response.WriteAsync(Regex.Replace(reponseLine, @"\r\n?|\n", "").Replace("  ","")).ConfigureAwait(false);
      await httpContext.Response.WriteAsync(crlf).ConfigureAwait(false);
      await httpContext.Response.Body.FlushAsync().ConfigureAwait(false);
    }

    private async Task<bool> ShouldNext(HttpContext httpContext)
    {
      bool Contains(MockCloudflareServerFlag flag)
      {
        return _serverFlags is { } && _serverFlags.Contains(flag);
      }

      var returnStatusCode = (Contains(MockCloudflareServerFlag.Throw500) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK);

      httpContext.Response.StatusCode = (int)returnStatusCode;
      httpContext.Response.ContentType = ContentType.Json;
      httpContext.Response.ContentLength = null;

      if (!Contains(MockCloudflareServerFlag.Throw500))
      {
        if (Contains(MockCloudflareServerFlag.CreateZone))
          await WriteServerResponse(httpContext, Responses.ZoneDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.EditUser))
          await WriteServerResponse(httpContext, Responses.EditUser).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.ListZones))
          await WriteServerResponse(httpContext, Responses.ListZones).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.UserDetails))
          await WriteServerResponse(httpContext, Responses.UserDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.ZoneDetails))
          await WriteServerResponse(httpContext, Responses.ZoneDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.CreateLoadBalancer))
          await WriteServerResponse(httpContext, Responses.LoadBalancerDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.ListLoadBalancers))
          await WriteServerResponse(httpContext, Responses.ListLoadBalancers).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.LoadBalancerDetails))
          await WriteServerResponse(httpContext, Responses.LoadBalancerDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.LoadBalancerPoolDetails))
          await WriteServerResponse(httpContext, Responses.LoadBalancerPoolDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.CreateLoadBalancerPool))
          await WriteServerResponse(httpContext, Responses.LoadBalancerPoolDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.ListLoadBalancerPools))
          await WriteServerResponse(httpContext, Responses.ListLoadBalancerPools).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.LoadBalancerMonitorDetails))
          await WriteServerResponse(httpContext, Responses.LoadBalancerMonitorDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.CreateLoadBalancerMonitor))
          await WriteServerResponse(httpContext, Responses.LoadBalancerMonitorDetails).ConfigureAwait(false);

        if (Contains(MockCloudflareServerFlag.ListLoadBalancerMonitors))
          await WriteServerResponse(httpContext, Responses.ListLoadBalancerMonitors).ConfigureAwait(false);
      }

      return false;
    }

    private IWebHost BuildWebHost(ITestOutputHelper testOutput, Func<HttpContext, Task<bool>> shouldNext, Action<ListenOptions> listenConfigure, string resp)
    {
      shouldNext = shouldNext ?? (_ => Task.FromResult(true));
      listenConfigure = listenConfigure ?? (_ => { });

      return WebHost.CreateDefaultBuilder().Configure(app => app.Run(async httpContext =>
      {
        if (await shouldNext(httpContext).ConfigureAwait(false))
        {
          await httpContext.Response.WriteAsync(resp).ConfigureAwait(false);
        }
      })).UseKestrel(options => { options.Listen(IPAddress.Loopback, 0, listenConfigure); }).ConfigureLogging((p) => p.AddXUnit()).Build();
    }
  }
}
