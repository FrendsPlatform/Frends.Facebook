namespace Frends.Facebook.Read.Tests;

using Frends.Facebook.Read.Definitions;
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
    private readonly string _redirectUrl = "redirectUrl";
    private readonly string _code = "code";

    private string _token = "token";

    //internal static readonly HttpClient _client = new HttpClient();

    [SetUp]
    public async Task SetUp()
    {
        // Fetch token

        /*var url = @"https://graph.facebook.com/v18.0/oauth/access_token?\" +
            "client_id=" + _clientId +
            "&redirect_uri=" + _redirectUrl +
            "&client_secret=" + _clientSecret +
            "&code=" + _code;

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
        };

        var responseMessage = _client.Send(request, CancellationToken.None);
        responseMessage.EnsureSuccessStatusCode();
        var responseString = await responseMessage.Content.ReadAsStringAsync(CancellationToken.None);
        Console.WriteLine($"Response: {responseString}");

        _token = responseString;*/
    }

    [Test]
    public void TestGetInsights()
    {
        var input = new Input
        {
            Reference = References.Insights,
            ObjectId = _objectId,
            Token = _token
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
}
