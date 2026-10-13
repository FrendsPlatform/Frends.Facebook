namespace Frends.Facebook.Request.Definitions;

/// <summary>
/// Error details returned when the request fails.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    /// <example>Invalid access token</example>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets additional information about the error.
    /// </summary>
    /// <example>Facebook API request failed</example>
    public object AdditionalInfo { get; set; }
}
