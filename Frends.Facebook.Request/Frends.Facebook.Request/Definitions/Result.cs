namespace Frends.Facebook.Request.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="statuscode">Indicates whether GET call was executed succesfully.</param>
    /// <param name="message">Returns the message from the interface.</param>
    internal Result(int statuscode, object message)
    {
        this.Statuscode = statuscode;
        this.Message = message;
    }

    /// <summary>
    /// Gets a value indicating whether GET call was executed successfully.
    /// </summary>
    /// <example>True.</example>
    public int Statuscode { get; private set; }

    /// <summary>
    /// Gets message from the interface.
    /// </summary>
    /// <example>{ "id": 123456789, "name": "UserName" }</example>
    public dynamic Message { get; private set; }
}