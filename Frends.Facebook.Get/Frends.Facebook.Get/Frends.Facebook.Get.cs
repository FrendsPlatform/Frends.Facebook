#pragma warning disable SA1200 //Using directives should be placed correctly.

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frends.Facebook.Get.Definitions;

namespace Frends.Facebook.Get;

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
    /// <returns>Object { bool Success, dynamic Message }.</returns>
    public static async Task<Result> Get([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.AccessToken))
        {
            throw new ArgumentNullException(nameof(input.AccessToken) + " cannot be empty.");
        }
        else if (string.IsNullOrEmpty(input.ApiVersion))
        {
            throw new ArgumentNullException(nameof(input.ApiVersion) + " cannot be empty.");
        }
        else if (string.IsNullOrEmpty(input.ObjectId) && string.IsNullOrEmpty(input.References))
        {
            throw new ArgumentNullException("Both values " + nameof(input.ObjectId) + " or " + nameof(input.References) + " cannot be empty.");
        }

        try
        {
            HttpClient client = new HttpClient();

            var url = GetUrl(input, cancellationToken);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            request.Headers.Add("Authorization", "Bearer " + input.AccessToken);

            var responseMessage = await client.SendAsync(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = string.Empty;
#if NETSTANDARD2_0
            responseString = await responseMessage.Content.ReadAsStringAsync();
#elif NET471
            responseString = await responseMessage.Content.ReadAsStringAsync();
#elif NET6_0_OR_GREATER
            responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
#endif

            return new Result(true, responseString);
        }
        catch (Exception ex)
        {
            if (options.ThrowErrorOnFailure)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return new Result(false, ex.Message);
        }
    }

    private static string GetUrl(Input input, CancellationToken cancellationToken)
    {
        var url = $@"https://graph.facebook.com/v{input.ApiVersion}/";

        if (!string.IsNullOrEmpty(input.ObjectId) && !string.IsNullOrEmpty(input.References))
        {
            url += input.ObjectId + "/" + input.References;
        }
        else if (string.IsNullOrEmpty(input.ObjectId) && !string.IsNullOrEmpty(input.References))
        {
            url += input.References;
        }
        else
        {
            url += input.ObjectId;
        }

        return url;
    }
}
