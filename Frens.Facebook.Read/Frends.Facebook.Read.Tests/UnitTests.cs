namespace Frends.Facebook.Read.Tests;

using Frends.Facebook.Read.Definitions;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System;

[TestFixture]
public class UnitTests
{
    private readonly string token = "EAAXZCI80w6eEBOzDQBX3Kyqt5ZAzpZABfV0nIOoD8ur44Bk9KItSbxNbGkZCtAALhw3aqm3c8ZB76ItXoW8ZC1MUjnlwiWAc6g5T9hHZAX5rozDlo6JnZCpMDpN68WXfZAcZB5fIBdt3P39ZCdSZAUtZCCabvJeAiKx5h8vNN0rA3i6sXqrltgoXYqrvZBQ9gFxL33OlddrZCZCaZCASKGgeihgTA3cO0fViwRQ97WmjL98ZAQ";

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
        //Console.WriteLine(ret.ToString());

        //Assert.That(ret.Message, Is.EqualTo("foobar, foobar, foobar"));
        Assert.IsNotNull(ret);
    }
}
