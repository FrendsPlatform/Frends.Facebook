namespace Frends.Facebook.Read;

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frends.Facebook.Read.Definitions;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Facebook
{
    internal static readonly HttpClient _client = new HttpClient();

    /// <summary>
    /// This is Task.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/namespaceForTask).
    /// </summary>
    /// <param name="input">What to repeat.</param>
    /// <param name="options">Define if repeated multiple times. </param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { string Output }.</returns>
    public static async Task<Result> Read([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        /*Get Facebook user info with token
         graph.facebook.com/v18.0/{object-id}/insights/{metric}*/
        if (string.IsNullOrEmpty(input.Token))
        {
            throw new ArgumentNullException(nameof(input.Token) + " can not be empty.");
        }

        try
        {
            var url = GetUrl(input);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            request.Headers.Add("Authorization", "Bearer " + input.Token);

            var responseMessage = _client.Send(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine("Response:" + responseString);

            return new Result(true, responseString);
        }
        catch (Exception ex)
        {
            return new Result(false, ex.Message);
        }
    }

    private static string GetUrl(Input input)
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

            for (int i = 0; i < input.Parameters.Count; i++)
            {
                if (i != 0 && i != input.Parameters.Count)
                {
                    url += "&";
                }

                url += $"{input.Parameters[i].Key}={input.Parameters[i].Value}";
            }
        }

        return url;
    }
}
