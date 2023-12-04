namespace Frends.Facebook.Post.Tests;

using Frends.Facebook.Post.Definitions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class UnitTests
{
    internal static readonly HttpClient Client = new HttpClient();
    private readonly string token = Environment.GetEnvironmentVariable("Facebook_token");

    private string objectId;
    /*private readonly string _clientId = "clientId";
    private readonly string _clientSecret = "clientSecret";
    privare readonly string _facebookId = "facebookId";*/

    // Todo: Create Page Access Token
    [SetUp]
    public async Task SetUp()
    {
        // Fetch token
        /*try
        {

            var appTokenUrl = @"https://graph.facebook.com/v18.0/oauth/access_token?" +
                "client_id=" + _clientId +
                "&client_secret=" + _clientSecret +
                "&grant_type=client_credentials";
            var appToken = await GetAsync(appTokenUrl);

            var pageAccessUrl = @"https://graph.facebook.com/" + _facebookId +
                "/accounts?access_token=" + (string)appToken["access_token"];
            var pageToken = await GetAsync(pageAccessUrl);

            _token = (string)pageToken["data"][0]["access_token"].ToString();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }*/

        // Fetch App Id
        try
        {
            var appUrl = @"https://graph.facebook.com/v18.0/me";
            var result = await GetAsync(appUrl, token);
            objectId = (string)result["id"];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [Test]
    public async Task TestPostToPageAsync()
    {
        var input = new Input
        {
            Reference = objectId + "/feed",
            AccessToken = token,
            ApiVersion = "18.0",
            Data = "{ \"message\": \"This is a test.\" }",
        };

        var ret = await Facebook.Post(input, new Options(), default);
        Assert.IsNotNull(ret);
        Console.WriteLine(ret.Message);

        // Test user has no permissions for this
        // Assert.IsTrue(ret.Result.Success);
        Assert.IsFalse(ret.Success);
    }

    [Test]
    public void TestThrowTokenEmptyError()
    {
        var input = new Input
        {
            Reference = "me",
            AccessToken = string.Empty,
            ApiVersion = "18.0",
            Data = "{ \"message\": \"This is a test.\" }",
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Post(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowDataEmptyError()
    {
        var input = new Input
        {
            Reference = "me",
            AccessToken = token,
            ApiVersion = "18.0",
            Data = string.Empty,
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Post(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowApiVersionEmptyError()
    {
        var input = new Input
        {
            Reference = "me",
            AccessToken = token,
            ApiVersion = string.Empty,
            Data = "{ \"message\": \"This is a test.\" }",
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Post(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowReferenceEmptyError()
    {
        var input = new Input
        {
            Reference = string.Empty,
            AccessToken = token,
            ApiVersion = "18.0",
            Data = "{ \"message\": \"This is a test.\" }",
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Post(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowErrorOnFailureError()
    {
        var input = new Input
        {
            Reference = objectId + "/feed",
            AccessToken = token,
            ApiVersion = "18.0",
            Data = "{ \"message\": \"This is a test.\" }",
        };

        var option = new Options
        {
            ThrowErrorOnFailure = true,
        };

        var ret = Assert.ThrowsAsync<Exception>(() => Facebook.Post(input, option, default));
        Assert.IsNotNull(ret);
    }

    private static async Task<JObject> GetAsync(string url, string token)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
        };
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", "Bearer " + token);
        }

        var responseMessage = Client.Send(request, CancellationToken.None);
        responseMessage.EnsureSuccessStatusCode();
        var responseString = await responseMessage.Content.ReadAsStringAsync(CancellationToken.None);

        return JObject.Parse(responseString);
    }
}
