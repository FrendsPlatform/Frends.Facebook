namespace Frends.Facebook.Get.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// The GET call was executed successfully.
    /// </summary>
    /// <example>True</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Returned message from the interface.
    /// </summary>
    /// <example>Example of the output.</example>
    public dynamic Message { get; private set; }

    internal Result(bool success, object message)
    {
        this.Success = success;
        this.Message = message;
    }
}