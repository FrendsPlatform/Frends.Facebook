namespace Frends.Facebook.Get;

using Frends.Facebook.Get.Definitions;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Facebook class.
/// </summary>
public static class Facebook
{
    internal static readonly HttpClient _client = new HttpClient();

    /// <summary>
    /// This is task for reading data from Facebook API.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Facebook.Get).
    /// </summary>
    /// <param name="input">Set reference type, parameters and token.</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { bool Success, dynamic Message }.</returns>
    public static async Task<Result> Get([PropertyTab] Input input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.Token))
        {
            throw new ArgumentNullException(nameof(input.Token) + " cannot be empty.");
        }

        try
        {
            var url = GetUrl(input, cancellationToken);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            request.Headers.Add("Authorization", "Bearer " + input.Token);

            var responseMessage = _client.Send(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

            return new Result(true, responseString);
        }
        catch (Exception ex)
        {
            return new Result(false, ex.Message);
        }
    }

    private static string GetUrl(Input input, CancellationToken cancellationToken)
    {
        var url = "https://graph.facebook.com/v18.0/";

        // Set url base
        switch (input.Reference)
        {
            case References.Insights:
                url += input.ObjectId + "/insights";
                break;
            case References.Pages:
                url += input.ObjectId;
                break;
            case References.ADS:
                url += "ads_archive";
                break;
            case References.Other:
                url += input.Other;
                break;
        }

        if (input.Parameters != null)
        {
            url += "?";

            for (int i = 0; i < input.Parameters.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (i != 0 && i != input.Parameters.Length)
                    url += "&";

                url += $"{input.Parameters[i].Name}={input.Parameters[i].Value}";
            }
        }

        return url;
    }
}
