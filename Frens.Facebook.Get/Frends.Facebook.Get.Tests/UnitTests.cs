namespace Frends.Facebook.Get.Tests;

using Frends.Facebook.Get.Definitions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class UnitTests
{
    private readonly string _objectId = "objectId";
    private readonly string _clientId = "clientId";
    private readonly string _clientSecret = "clientSecret";

    private string _token = "";

    internal static readonly HttpClient _client = new HttpClient();

    [SetUp]
    public async Task SetUp()
    {
        // Fetch token

        try
        {
            var appTokenUrl = @"https://graph.facebook.com/v18.0/oauth/access_token?" +
                "client_id=" + _clientId +
                "&client_secret=" + _clientSecret +
                "&grant_type=client_credentials";
            var appToken = await GetTokenAsync(appTokenUrl);

            var pageAccessUrl = @"https://graph.facebook.com/" + _clientId +
                "/accounts?access_token=" + (string)appToken["access_token"];
            var pageToken = await GetTokenAsync(pageAccessUrl);

            _token = (string)pageToken["data"];
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static async Task<JObject> GetTokenAsync(string url)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
        };

        var responseMessage = _client.Send(request, CancellationToken.None);
        responseMessage.EnsureSuccessStatusCode();
        var responseString = await responseMessage.Content.ReadAsStringAsync(CancellationToken.None);
        Console.WriteLine($"Response: {responseString}");

        return JObject.Parse(responseString);
    }

    [Test]
    public void TestGetInsights()
    {
        var input = new Input
        {
            Reference = References.Insights,
            ObjectId = _objectId,
            Token = _token,
            Parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("metric", "page_impressions_unique"),
                new KeyValuePair<string, string>("metric", "post_reactions_love_total"),
            },
        };

        var ret = Facebook.Read(input, new Options(), default);
        Console.WriteLine(ret.Result.Message);
        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Result.Success);
    }

    [Test]
    public void TestGetADS()
    {
        var input = new Input
        {
            Reference = References.ADS,
            Token = _token,
            Parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("ad_reached_countries", "ALL"),
                new KeyValuePair<string, string>("ad_type", "POLITICAL_AND_ISSUE_ADS"),
            },
        };

        var ret = Facebook.Read(input, new Options(), default);
        Console.WriteLine(ret.Result.Message);
        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Result.Success);
    }

    [Test]
    public void TestGetOther()
    {
        var input = new Input
        {
            Reference = References.Other,
            Other = "me",
            Token = _token,
            Parameters = new List<KeyValuePair<string, string>>()
            { new KeyValuePair<string, string>("fields", "id,name") },
        };

        var ret = Facebook.Read(input, new Options(), default);
        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Result.Success);
        Assert.IsTrue(ret.Result.Message.Contains(_objectId));
    }

    [Test]
    public void TestGetOtherWithDuplicateParameters()
    {
        var input = new Input
        {
            Reference = References.Other,
            Other = "me",
            Token = _token,
            Parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("fields", "id"),
                new KeyValuePair<string, string>("fields", "name"),
            },
        };

        var ret = Facebook.Read(input, new Options(), default);
        Assert.IsNotNull(ret);
        Assert.IsTrue(ret.Result.Success);
        Assert.IsTrue(ret.Result.Message.Contains(_objectId));
    }

    [Test]
    public void TestThrowTokenEmptyError()
    {
        var input = new Input
        {
            Reference = References.Other,
            Other = "me",
            Token = string.Empty,
            Parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("fields", "id"),
                new KeyValuePair<string, string>("fields", "name"),
            },
        };

        var ret = Assert.ThrowsAsync<ArgumentNullException>(() => Facebook.Read(input, new Options(), default));
        Assert.IsNotNull(ret);
    }
}
