namespace Frends.Facebook.Read.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// Contains the input repeated the specified number of times.
    /// </summary>
    /// <example>Example of the output.</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Contains the input repeated the specified number of times.
    /// </summary>
    /// <example>Example of the output.</example>
    public object Message { get; private set; }

    internal Result(bool success, object message)
    {
        this.Success = success;
        this.Message = message;
    }
}
