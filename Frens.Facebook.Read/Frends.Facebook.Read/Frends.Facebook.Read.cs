namespace Frends.Facebook.Read;

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
    private static readonly HttpClient client = new HttpClient();

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

        var responseString = await client.GetStringAsync("graph.facebook.com/v18.0/me?fields=id,name");

        return new Result(true, responseString);
    }
}
