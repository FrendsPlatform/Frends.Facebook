namespace Frends.Facebook.Request.Tests;

using Frends.Facebook.Request.Definitions;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

[TestFixture]
internal class ErrorHandlerTest
{
    private const string CustomErrorMessage = "CustomErrorMessage";

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var input = CreateInvalidInput();
        var options = new Options();

        Assert.ThrowsAsync<ValidationException>(() => Facebook.Request(input, options, CancellationToken.None));
    }

    [Test]
    public async Task Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var input = CreateInvalidInput();
        var options = new Options { ThrowErrorOnFailure = false };

        var result = await Facebook.Request(input, options, CancellationToken.None);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var input = CreateInvalidInput();
        var options = new Options { ErrorMessageOnFailure = CustomErrorMessage };

        var exception = Assert.ThrowsAsync<Exception>(() => Facebook.Request(input, options, CancellationToken.None));

        Assert.That(exception.Message, Does.Contain(CustomErrorMessage));
    }

    private static Input CreateInvalidInput()
    {
        return new Input
        {
            Method = Methods.GET,
            Reference = "me",
            AccessToken = string.Empty,
            ApiVersion = "18.0",
        };
    }
}
