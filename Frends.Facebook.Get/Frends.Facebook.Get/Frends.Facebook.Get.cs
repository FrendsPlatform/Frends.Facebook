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
            throw new ArgumentNullException("Both values of " + nameof(input.ObjectId) + " or " + nameof(input.References) + " cannot be empty.");
        }

        try
        {
            var url = $@"https://graph.facebook.com/v{input.ApiVersion}/{input.ObjectId}{input.References}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            request.Headers.Add("Authorization", "Bearer " + input.AccessToken);

            var responseMessage = await _client.SendAsync(request, cancellationToken);
            responseMessage.EnsureSuccessStatusCode();
            var responseString = string.Empty;
            #if NETSTANDARD2_0
            responseString = await responseMessage.Content.ReadAsStringAsync();
            #elif NET6_0_OR_GREATER
            responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            #endif

            return new Result(true, responseString);
        }
        catch (Exception ex)
        {
            if (options.ThrowErrorOnFailure)
                throw new Exception(ex.Message, ex.InnerException);
            return new Result(false, ex.Message);
        }
    }
}
