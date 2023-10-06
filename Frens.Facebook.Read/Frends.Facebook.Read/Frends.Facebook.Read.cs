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
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://graph.facebook.com/v18.0/me?fields=id,name"),
            };
            request.Headers.Add("Authorization", "Bearer token");

            var responseMessage = Client.Send(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine("Response: " + responseString);

            return new Result(true, responseMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Result(false, ex.Message);
        }
    }
}
