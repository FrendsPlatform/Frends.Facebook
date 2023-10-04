namespace Frends.Facebook.Read.Tests;

using Frends.Facebook.Read.Definitions;
using NUnit.Framework;

[TestFixture]
public class UnitTests
{
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

        var ret = Read.TaskName(input, options, default);

        Assert.That(ret.Output, Is.EqualTo("foobar, foobar, foobar"));
    }
}
