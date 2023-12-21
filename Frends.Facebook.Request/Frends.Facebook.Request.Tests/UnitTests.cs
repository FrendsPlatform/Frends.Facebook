namespace Frends.Facebook.Request.Tests;

using Frends.Facebook.Request.Definitions;
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
    public void TestGetInsights()
    {
        var input = new Input
        {
            Method = Methods.GET,
            QueryParameters = "metric=page_impressions_unique&metric=post_reactions_love_total",
            Reference = "insights",
            AccessToken = token,
            ApiVersion = "18.0",
        };

        var ret = Facebook.Request(input, new Options(), default);
        Assert.IsNotNull(ret);

        // Assert.IsTrue(ret.Result.Success);
        // Test user has no permissions for this
        Assert.IsFalse(ret.Result.Success);
    }

    [Test]
    public void TestGetADS()
    {
        var input = new Input
        {
            Method = Methods.GET,
            Reference = "ads_archive",
            QueryParameters = "ad_reached_countries=ALL&ad_type=POLITICAL_AND_ISSUE_ADS",
            AccessToken = token,
            ApiVersion = "18.0",
        };

        var ret = Facebook.Request(input, new Options(), default);
        Assert.IsNotNull(ret);

        // Assert.IsTrue(ret.Result.Success);
        // Test user has no permissions for this
        Assert.IsFalse(ret.Result.Success);
    }

    [Test]
    public void TestGetOther()
    {
        var input = new Input
        {
            Method = Methods.GET,
            Reference = "me",
            QueryParameters = "fields=id,name",
            AccessToken = token,
            ApiVersion = "18.0",
        };

        var ret = Facebook.Request(input, new Options { ThrowErrorOnFailure = true }, default);
        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Result.Success);
        Assert.IsTrue(ret.Result.Message.Contains(objectId));
    }

    [Test]
    public void TestGetOtherWithDuplicateParameters()
    {
        var input = new Input
        {
            Method = Methods.GET,
            Reference = "me",
            QueryParameters = "fields=id&fields=name",
            AccessToken = token,
            ApiVersion = "18.0",
        };

        var ret = Facebook.Request(input, new Options(), default);
        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Result.Success);
        Assert.IsTrue(ret.Result.Message.Contains(objectId));
    }

    [Test]
    public void TestThrowTokenEmptyError()
    {
        var input = new Input
        {
            Method = Methods.GET,
            Reference = "me",
            AccessToken = string.Empty,
            ApiVersion = "18.0",
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Request(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public async Task TestPostToPageAsync()
    {
        var input = new Input
        {
            Method = Methods.POST,
            Reference = objectId + "/feed",
            AccessToken = token,
            ApiVersion = "18.0",
            Message = "{ \"message\": \"This is a test.\" }",
        };

        var ret = await Facebook.Request(input, new Options(), default);
        Assert.IsNotNull(ret);
        Assert.IsFalse(ret.Success);
    }

    [Test]
    public async Task TestThroErrorOnFailure()
    {
        var input = new Input
        {
            Method = Methods.GET,
            Reference = objectId + "/abcdefg",
            AccessToken = token,
            ApiVersion = "18.0",
        };

        var option = new Options { ThrowErrorOnFailure = true };

        var ret = Assert.ThrowsAsync<Exception>(() => Facebook.Request(input, option, default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowMessageEmptyError()
    {
        var input = new Input
        {
            Method = Methods.POST,
            Reference = "me",
            AccessToken = token,
            ApiVersion = "18.0",
            Message = string.Empty,
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Request(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowApiVersionEmptyError()
    {
        var input = new Input
        {
            Method = Methods.POST,
            Reference = "me",
            AccessToken = token,
            ApiVersion = string.Empty,
            Message = "{ \"message\": \"This is a test.\" }",
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Request(input, new Options(), default));
        Assert.IsNotNull(ret);
    }

    [Test]
    public void TestThrowReferenceEmptyError()
    {
        var input = new Input
        {
            Method = Methods.POST,
            Reference = string.Empty,
            AccessToken = token,
            ApiVersion = "18.0",
            Message = "{ \"message\": \"This is a test.\" }",
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Request(input, new Options(), default));
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
