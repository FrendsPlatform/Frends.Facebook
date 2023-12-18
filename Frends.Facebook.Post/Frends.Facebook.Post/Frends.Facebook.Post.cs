﻿using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frends.Facebook.Post.Definitions;

namespace Frends.Facebook.Post;

/// <summary>
/// Facebook class.
/// </summary>
public static class Facebook
{
    /// <summary>
    /// This is task for reading data from Facebook API.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Facebook.Post).
    /// </summary>
    /// <param name="input">Set reference type, parameters, data message and token.</param>
    /// <param name="options">Optional parameters.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { bool Success, dynamic Message }.</returns>
    public static async Task<Result> Post([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.AccessToken))
        {
            throw new ArgumentNullException(nameof(input.AccessToken) + " cannot be empty.");
        }
        else if (string.IsNullOrEmpty(input.ApiVersion))
        {
            throw new ArgumentNullException(nameof(input.ApiVersion) + " cannot be empty.");
        }
        else if (string.IsNullOrEmpty(input.Reference))
        {
            throw new ArgumentNullException(nameof(input.Reference) + " cannot be empty.");
        }
        else if (string.IsNullOrEmpty(input.Data))
        {
            throw new ArgumentNullException(nameof(input.Data) + " cannot be empty.");
        }

        //using HttpClient client = new HttpClient();

        try
        {
            var url = $@"https://graph.facebook.com/v{input.ApiVersion}/" + (!string.IsNullOrEmpty(input.QueryParameters) ? input.Reference + "?" + input.QueryParameters : input.Reference);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
            };
            request.Headers.Add("Authorization", "Bearer " + input.AccessToken);

            using HttpClient client = new HttpClient();
            var responseMessage = await client.SendAsync(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = string.Empty;
#if NET471
            responseString = await responseMessage.Content.ReadAsStringAsync();
#elif NET6_0_OR_GREATER
            responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
#endif

            //client.Dispose();
            return new Result(true, responseString);
        }
        catch (Exception ex)
        {
            //client.Dispose();
            return options.ThrowErrorOnFailure ? throw new Exception(ex.Message, ex.InnerException) : new Result(false, ex.Message);
        }
    }
}
