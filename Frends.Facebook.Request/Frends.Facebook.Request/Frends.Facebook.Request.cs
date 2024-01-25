#pragma warning disable SA1200 //Using directives should be placed correctly.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Frends.Facebook.Request.Definitions;

namespace Frends.Facebook.Request;

/// <summary>
/// Facebook class.
/// </summary>
public static class Facebook
{
    /// <summary>
    /// This is task for reading data from Facebook API.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Facebook.Get).
    /// </summary>
    /// <param name="input">Set reference type, parameters and token.</param>
    /// <param name="options">Optional parameters.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { int StatusCode, dynamic Message }.</returns>
    public static async Task<Result> Request([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.AccessToken))
            throw new ArgumentNullException(nameof(input.AccessToken) + " cannot be empty.");
        else if (string.IsNullOrEmpty(input.ApiVersion))
            throw new ArgumentNullException(nameof(input.ApiVersion) + " cannot be empty.");
        else if (string.IsNullOrEmpty(input.Reference))
            throw new ArgumentNullException(nameof(input.Reference) + " cannot be empty.");
        else if (string.IsNullOrEmpty(input.Message) && Enum.GetNames(typeof(SendMethods)).Contains(input.Method.ToString()))
            throw new ArgumentNullException(nameof(input.Message) + " cannot be empty.");

        var headers = GetHeaderDictionary(input);

        using var content = GetContent(input, headers);
        var url = $@"https://graph.facebook.com/v{input.ApiVersion}/" + (!string.IsNullOrEmpty(input.QueryParameters) ? input.Reference + "?" + input.QueryParameters : input.Reference);
        using var responseMessage = await GetHttpRequestResponseAsync(
                new HttpClient(),
                input.Method.ToString(),
                url,
                content,
                headers,
                cancellationToken)
            .ConfigureAwait(false);

        var hbody = string.Empty;

#if NET471
        hbody = responseMessage.Content != null ? await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false) : null;
        content.Dispose();
        responseMessage.Dispose();
#elif NET6_0_OR_GREATER
        hbody = responseMessage.Content != null ? await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false) : null;
#endif
        var hstatusCode = (int)responseMessage.StatusCode;

        if (options.ThrowErrorOnFailure && hstatusCode != 200)
            throw new Exception(hbody);

        return new Result(hstatusCode, hbody);
    }

    private static IDictionary<string, string> GetHeaderDictionary(Input inputs)
    {
        var authHeader = new Header
        {
            Name = "Authorization",
            Value = $"Bearer {inputs.AccessToken}",
        };
        var headers = new[] { authHeader }.ToArray();

        // Ignore case for headers and key comparison
        return headers.ToDictionary(key => key.Name, value => value.Value, StringComparer.InvariantCultureIgnoreCase);
    }

    private static HttpContent GetContent(Input input, IDictionary<string, string> headers)
    {
        // Check if Content-Type exists and is set and valid
        var contentTypeIsSetAndValid = false;
        MediaTypeWithQualityHeaderValue validContentType = null;
        if (headers.TryGetValue("content-type", out string contentTypeValue))
            contentTypeIsSetAndValid = MediaTypeWithQualityHeaderValue.TryParse(contentTypeValue, out validContentType);

        return contentTypeIsSetAndValid
            ? new StringContent(input.Message ?? string.Empty, Encoding.GetEncoding(validContentType.CharSet ?? Encoding.UTF8.WebName))
            : new StringContent(input.Message ?? string.Empty);
    }

    private static async Task<HttpResponseMessage> GetHttpRequestResponseAsync(
        HttpClient httpClient,
        string method,
        string url,
        HttpContent content,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Only POST, PUT, PATCH and DELETE can have content, otherwise the HttpClient will fail
        var isContentAllowed = Enum.TryParse(method, ignoreCase: true, result: out SendMethods _);

        using (var request = new HttpRequestMessage(new HttpMethod(method), new Uri(url))
        {
            Content = isContentAllowed ? content : null,
        })
        {
            // Clear default headers
            content.Headers.Clear();
            foreach (var header in headers)
            {
                var requestHeaderAddedSuccessfully = request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                if (!requestHeaderAddedSuccessfully && request.Content != null)
                {
                    // Could not add to request headers try to add to content headers
                    // this check is probably not needed anymore as the new HttpClient does not seem fail on malformed headers
                    var contentHeaderAddedSuccessfully = content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    if (!contentHeaderAddedSuccessfully)
                    {
                        Trace.TraceWarning($"Could not add header {header.Key}:{header.Value}");
                    }
                }
            }

            HttpResponseMessage response;
            try
            {
                response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException canceledException)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // Cancellation is from outside -> Just throw
                    throw;
                }

                // Cancellation is from inside of the request, mostly likely a timeout
                throw new Exception("HttpRequest was canceled, most likely due to a timeout.", canceledException);
            }

            request.Dispose();
            httpClient.Dispose();

            return response;
        }
    }
}
