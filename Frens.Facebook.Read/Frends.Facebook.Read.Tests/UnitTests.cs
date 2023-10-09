namespace Frends.Facebook.Read.Tests;

using Frends.Facebook.Read.Definitions;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System;

[TestFixture]
public class UnitTests
{
    private readonly string _token = "token";
    private readonly string _objectId = "objectId";

    [Test]
    public void GetInsightsTest()
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
    }
}
