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
    /// <param name="success">Indicates whether the request completed successfully.</param>
    /// <param name="error">Error details when the request fails.</param>
    internal Result(int statuscode, object message, bool success = true, Error error = null)
    {
        Success = success;
        Statuscode = statuscode;
        Message = message;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the request completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Gets error details. Null when the request succeeds.
    /// </summary>
    /// <example>null</example>
    public Error Error { get; private set; }

    /// <summary>
    /// Gets the HTTP status code returned by the request.
    /// </summary>
    /// <example>200</example>
    public int Statuscode { get; private set; }

    /// <summary>
    /// Gets the response message from Facebook.
    /// </summary>
    /// <example>{ "id": 123456789, "name": "UserName" }</example>
    public dynamic Message { get; private set; }
}