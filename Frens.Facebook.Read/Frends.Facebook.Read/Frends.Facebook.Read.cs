namespace Frends.Facebook.Read;

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Frends.Facebook.Read.Definitions;
using RestSharp;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Facebook
{
    private static readonly HttpClient Client = new HttpClient();

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

        try
        {
            var url = "https://graph.facebook.com/v18.0/";

            // Set url base
            switch (input.Reference)
            {
                case References.Insights:
                    url += input.ObjectId + "/insights/";
                    break;
                case References.Pages:
                    url += input.ObjectId;
                    break;
                case References.ADS:
                    url += "ads_archive";
                    break;
                case References.Other:
                    url += input.ObjectId;
                    break;
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            request.Headers.Add("Authorization", "Bearer " + input.Token);

            var responseMessage = Client.Send(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine("Response: " + responseString);

            return new Result(true, responseMessage);
        }
        catch (Exception ex)
        {
            return new Result(false, ex.Message);
        }
    }
}
