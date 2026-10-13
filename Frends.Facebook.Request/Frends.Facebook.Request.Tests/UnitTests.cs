namespace Frends.Facebook.Request.Tests;

using Frends.Facebook.Request.Definitions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
public class UnitTests
{
    internal static readonly HttpClient Client = new HttpClient();
    private readonly string token = Environment.GetEnvironmentVariable("Facebook_token");

    private string objectId;

    [SetUp]
    public async Task SetUp()
    {
        // Fetch App Id
        var appUrl = @"https://graph.facebook.com/v18.0/me";
        var result = await GetAsync(appUrl, token);
        objectId = (string)result["id"];
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Client.Dispose();
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

        var ret = Facebook.Request(input, new Options(), default);
        ClassicAssert.IsNotNull(ret);
        ClassicAssert.AreEqual(ret.Result.Statuscode, 200);
        ClassicAssert.IsTrue(ret.Result.Message.Contains(objectId));
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

        var ret = Facebook.Request(input, new Options { ThrowErrorOnFailure = false }, default);
        ClassicAssert.IsNotNull(ret);
        ClassicAssert.AreEqual(ret.Result.Statuscode, 200);
        ClassicAssert.IsTrue(ret.Result.Message.Contains(objectId));
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

        var ret = Assert.ThrowsAsync<ValidationException>(() => Facebook.Request(input, new Options(), default));
        ClassicAssert.IsNotNull(ret);
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

        var ret = await Facebook.Request(input, new Options { ThrowErrorOnFailure = false }, default);
        ClassicAssert.IsNotNull(ret);
        ClassicAssert.AreNotEqual(ret.Statuscode, 200);
    }

    private static async Task<JObject> GetAsync(string url, string token)
    {
        using var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
        };
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Add("Authorization", "Bearer " + token);
        }

        string responseString = null;

        try
        {
            var responseMessage = Client.Send(request, CancellationToken.None);
            responseMessage.EnsureSuccessStatusCode();
            responseString = await responseMessage.Content.ReadAsStringAsync(CancellationToken.None);
            request.Dispose();
        }
        catch (Exception)
        {
            request.Dispose();
        }

        return JObject.Parse(responseString);
    }
}
