using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CloudflareClient.Models;
using Polly;
using RestSharp;
using RestSharp.Deserializers;

namespace CloudflareClient.Client
{
  public partial class ApiClient
  {
    /// <summary>
    /// The base URI of the service.
    /// </summary>
    public Uri BaseUri { get; set; }

    public static JsonSerializerOptions SerializerOptions => new JsonSerializerOptions
    {
      Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
      AllowTrailingCommas = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Allows for extending request processing for <see cref="ApiClient"/> generated code.
    /// </summary>
    /// <param name="request">The RestSharp request object</param>
    partial void InterceptRequest(IRestRequest request);

    /// <summary>
    /// Allows for extending response processing for <see cref="ApiClient"/> generated code.
    /// </summary>
    /// <param name="request">The RestSharp request object</param>
    /// <param name="response">The RestSharp response object</param>
    partial void InterceptResponse(IRestRequest request, IRestResponse response);

    private IReadableConfiguration _readableConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient" />
    /// </summary>
    /// <param name="configuration">The client configuration.</param>
    /// <exception cref="ArgumentException"></exception>
    public ApiClient(Configuration configuration)
    {
      _readableConfiguration = configuration;
    }

    public ApiClient(string xAuthEmail, string xAuthKey)
    {
      _readableConfiguration = new Configuration(xAuthEmail,xAuthKey);
    }

    /// <summary>
    /// Provides all logic for constructing a new RestSharp <see cref="RestRequest"/>.
    /// At this point, all information for querying the service is known. Here, it is simply
    /// mapped into the RestSharp request.
    /// </summary>
    /// <param name="method">The http verb.</param>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <returns>[private] A new RestRequest instance.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private RestRequest NewRequest(
        Method method,
        string path,
        RequestOptions options)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (options == null) throw new ArgumentNullException(nameof(options));

        var request = new RestRequest(method)
        {
            Resource = path,
            JsonSerializer = new CustomJsonCodec(SerializerOptions, _readableConfiguration)
        };

        if (options.PathParameters != null)
        {
            foreach (var pathParam in options.PathParameters)
            {
                request.AddParameter(pathParam.Key, pathParam.Value, ParameterType.UrlSegment);
            }
        }

        if (options.QueryParameters != null)
        {
            foreach (var (key,value) in options.QueryParameters)
            {
              request.AddQueryParameter(key, value);
            }
        }

        if (_readableConfiguration.DefaultHeaders != null)
        {
            foreach (var headerParam in _readableConfiguration.DefaultHeaders)
            {
                request.AddHeader(headerParam.Key, headerParam.Value);
            }
        }

        if (options.HeaderParameters != null)
        {
            foreach (var (key,value) in options.HeaderParameters)
            {
              request.AddHeader(key, value);
            }
        }

        if (options.FormParameters != null)
        {
            foreach (var (key,value) in options.FormParameters)
            {
                request.AddParameter(key, value);
            }
        }

        if (options.Data != null)
        {
            if (options.Data is Stream stream)
            {
                var contentType = "application/octet-stream";

                var bytes = ClientUtils.ReadAsBytes(stream);
                request.AddParameter(contentType, bytes, ParameterType.RequestBody);
            }
            else
            {
              // Here, we'll assume JSON APIs are more common. XML can be forced by adding produces/consumes to openapi spec explicitly.
              request.AddJsonBody(options.Data);
            }
        }

        request.RequestFormat = DataFormat.Json;

        if (options.FileParameters != null)
        {
            foreach (var fileParam in options.FileParameters)
            {
                var bytes = ClientUtils.ReadAsBytes(fileParam.Value);
                var fileStream = fileParam.Value as FileStream;
                if (fileStream != null)
                    request.Files.Add(FileParameter.Create(fileParam.Key, bytes, System.IO.Path.GetFileName(fileStream.Name)));
                else
                    request.Files.Add(FileParameter.Create(fileParam.Key, bytes, "no_file_name_provided"));
            }
        }

        if (options.Cookies != null && options.Cookies.Count > 0)
        {
            foreach (var cookie in options.Cookies)
            {
                request.AddCookie(cookie.Name, cookie.Value);
            }
        }

        request.AddHeader("X-Auth-Email", ClientUtils.ParameterToString(_readableConfiguration.xAuthEmail, _readableConfiguration)); // header parameter
        request.AddHeader("X-Auth-Key", ClientUtils.ParameterToString(_readableConfiguration.xAuthKey, _readableConfiguration)); // header parameter

        return request;
    }

    private ApiResponse<T> ToApiResponse<T>(IRestResponse<T> response)
    {
      if (response is null)
      {
        return null;
      }

      var result = response.Data;
      var rawContent = response.Content;

      var transformed = new ApiResponse<T>(response.StatusCode, new Dictionary<string, string>(), result, rawContent)
      {
          ErrorText = response.ErrorMessage,
          Cookies = new List<Cookie>()
      };

      foreach (var responseHeader in response.Headers)
      {
        if(responseHeader.Name is null) continue;

        transformed.Headers.Add(responseHeader.Name, ClientUtils.ParameterToString(responseHeader.Value, _readableConfiguration));
      }

      foreach (var responseCookies in response.Cookies)
      {
        transformed.Cookies.Add(
          new Cookie(
            responseCookies.Name,
            responseCookies.Value,
            responseCookies.Path,
            responseCookies.Domain)
        );
      }

      return transformed;
    }

    private async Task<T> ExecAsync<T>(
      RestRequest req,
      CancellationToken cancellationToken = default)
    {
        RestClient client = new RestClient(_readableConfiguration.BasePath);

        client.ClearHandlers();
        if (req.JsonSerializer is IDeserializer existingDeserializer)
        {
            client.AddHandler("application/json", () => existingDeserializer);
            client.AddHandler("text/json", () => existingDeserializer);
            client.AddHandler("text/x-json", () => existingDeserializer);
            client.AddHandler("text/javascript", () => existingDeserializer);
            client.AddHandler("*+json", () => existingDeserializer);
        }
        else
        {
            var customDeserializer = new CustomJsonCodec(SerializerOptions, _readableConfiguration);
            client.AddHandler("application/json", () => customDeserializer);
            client.AddHandler("text/json", () => customDeserializer);
            client.AddHandler("text/x-json", () => customDeserializer);
            client.AddHandler("text/javascript", () => customDeserializer);
            client.AddHandler("*+json", () => customDeserializer);
        }

        var xmlDeserializer = new XmlDeserializer();
        client.AddHandler("application/xml", () => xmlDeserializer);
        client.AddHandler("text/xml", () => xmlDeserializer);
        client.AddHandler("*+xml", () => xmlDeserializer);
        client.AddHandler("*", () => xmlDeserializer);

        client.Timeout = _readableConfiguration.Timeout;

        if (_readableConfiguration.Proxy != null)
        {
            client.Proxy = _readableConfiguration.Proxy;
        }

        if (_readableConfiguration.UserAgent != null)
        {
            client.UserAgent = _readableConfiguration.UserAgent;
        }

        if (_readableConfiguration.ClientCertificates != null)
        {
            client.ClientCertificates = _readableConfiguration.ClientCertificates;
        }

        InterceptRequest(req);

        IRestResponse<T> response;
        if (RetryConfiguration.AsyncRetryPolicy != null)
        {
            var policy = RetryConfiguration.AsyncRetryPolicy;
            var policyResult = await policy.ExecuteAndCaptureAsync((ct) => client.ExecuteAsync(req, ct), cancellationToken).ConfigureAwait(false);
            response = (policyResult.Outcome == OutcomeType.Successful) ? client.Deserialize<T>(policyResult.Result) : new RestResponse<T>
            {
                Request = req,
                ErrorException = policyResult.FinalException
            };
        }
        else
        {
            response = await client.ExecuteAsync<T>(req, cancellationToken).ConfigureAwait(false);
        }

        if (typeof(T).Name == "Stream") // for binary response
        {
            response.Data = (T)(object)new MemoryStream(response.RawBytes);
        }

        InterceptResponse(req, response);

        var result = ToApiResponse(response);
        result.ErrorText = response.ErrorMessage;

        if (response.Cookies.Count > 0)
        {
            if (result.Cookies == null) result.Cookies = new List<Cookie>();
            foreach (var restResponseCookie in response.Cookies)
            {
                var cookie = new Cookie(
                    restResponseCookie.Name,
                    restResponseCookie.Value,
                    restResponseCookie.Path,
                    restResponseCookie.Domain
                )
                {
                    Comment = restResponseCookie.Comment,
                    CommentUri = restResponseCookie.CommentUri,
                    Discard = restResponseCookie.Discard,
                    Expired = restResponseCookie.Expired,
                    Expires = restResponseCookie.Expires,
                    HttpOnly = restResponseCookie.HttpOnly,
                    Port = restResponseCookie.Port,
                    Secure = restResponseCookie.Secure,
                    Version = restResponseCookie.Version
                };

                result.Cookies.Add(cookie);
            }
        }

        if (response.StatusCode == HttpStatusCode.OK)
          return response.Data;

        throw new ApiException((int)response.StatusCode, response.Data.ToString());
    }

    #region IAsynchronousClient
    /// <summary>
    /// Make a HTTP GET request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> GetAsync<T>(string path, RequestOptions options, CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.GET, path, options), cancellationToken);
    }

    /// <summary>
    /// Make a HTTP POST request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> PostAsync<T>(string path, RequestOptions options,CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.POST, path, options), cancellationToken);
    }

    /// <summary>
    /// Make a HTTP PUT request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> PutAsync<T>(string path, RequestOptions options, CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.PUT, path, options), cancellationToken);
    }

    /// <summary>
    /// Make a HTTP DELETE request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> DeleteAsync<T>(string path, RequestOptions options, CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.DELETE, path, options), cancellationToken);
    }

    /// <summary>
    /// Make a HTTP HEAD request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> HeadAsync<T>(string path, RequestOptions options, CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.HEAD, path, options), cancellationToken);
    }

    /// <summary>
    /// Make a HTTP OPTION request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> OptionsAsync<T>(string path, RequestOptions options, CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.OPTIONS, path, options), cancellationToken);
    }

    /// <summary>
    /// Make a HTTP PATCH request (async).
    /// </summary>
    /// <param name="path">The target path (or resource).</param>
    /// <param name="options">The additional request options.</param>
    /// <param name="cancellationToken">Token that enables callers to cancel the request.</param>
    /// <returns>A Task containing ApiResponse</returns>
    public Task<CloudflareResponse<T>> PatchAsync<T>(string path, RequestOptions options, CancellationToken cancellationToken = default)
    {
        return ExecAsync<CloudflareResponse<T>>(NewRequest(Method.PATCH, path, options), cancellationToken);
    }
    #endregion IAsynchronousClient

    /// <summary>
    /// Allows RestSharp to Serialize/Deserialize JSON using our custom logic, but only when ContentType is JSON.
    /// </summary>
    private class CustomJsonCodec : RestSharp.Serializers.ISerializer, RestSharp.Deserializers.IDeserializer
    {
        private readonly IReadableConfiguration _configuration;
        private static readonly string _contentType = "application/json";
        private readonly JsonSerializerOptions _serializerOptions;

        public CustomJsonCodec(JsonSerializerOptions serializerOptions, IReadableConfiguration configuration)
        {
          _serializerOptions = serializerOptions;
          _configuration = configuration;
        }

        /// <summary>
        /// Serialize the object into a JSON string.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <returns>A JSON string.</returns>
        public string Serialize(object obj)
        {
          return JsonSerializer.Serialize(obj, _serializerOptions);
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var result = (T)Deserialize(response, typeof(T));
            return result;
        }

        /// <summary>
        /// Deserialize the JSON string into a proper object.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="type">Object type.</param>
        /// <returns>Object representation of the JSON string.</returns>
        public object Deserialize(IRestResponse response, Type type)
        {
            if (type == typeof(byte[])) // return byte array
            {
                return response.RawBytes;
            }

            if (type == typeof(Stream))
            {
                var bytes = response.RawBytes;
                var filePath = string.IsNullOrEmpty(_configuration.TempFolderPath)
                    ? Path.GetTempPath()
                    : _configuration.TempFolderPath;
                var regex = new Regex(@"Content-Disposition=.*filename=['""]?([^'""\s]+)['""]?$");
                foreach (var header in response.Headers)
                {
                    var match = regex.Match(header.ToString());
                    if (match.Success)
                    {
                        string fileName = filePath + ClientUtils.SanitizeFilename(match.Groups[1].Value.Replace("\"", "").Replace("'", ""));
                        File.WriteAllBytes(fileName, bytes);
                        return new FileStream(fileName, FileMode.Open);
                    }
                }
                var stream = new MemoryStream(bytes);
                return stream;
            }

            if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
            {
                return DateTime.Parse(response.Content, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (type == typeof(string) || type.Name.StartsWith("System.Nullable")) // return primitive type
            {
                return Convert.ChangeType(response.Content, type);
            }

            // at this point, it must be a model (json)
            try
            {
                return JsonSerializer.Deserialize(response.Content, type, _serializerOptions);
            }
            catch (Exception e)
            {
                throw new ApiException(500, e.Message);
            }
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public string ContentType
        {
            get { return _contentType; }
            set { throw new InvalidOperationException("Not allowed to set content type."); }
        }
    }
  }
}
