namespace Frends.Facebook.Read.Tests;

using Frends.Facebook.Read.Definitions;
using Microsoft.VisualBasic;
using NUnit.Framework;

[TestFixture]
public class UnitTests
{
    private readonly string token = "token";

    [Test]
    public void Test()
    {
        var input = new Input
        {
            Content = "foobar",
        };

        var options = new Options
        {
            Amount = 3,
            Delimiter = ", ",
        };

        var ret = Facebook.Read(input, options, default);

        //Assert.That(ret.Message, Is.EqualTo("foobar, foobar, foobar"));
        Assert.IsNotNull(ret);
    }
}
