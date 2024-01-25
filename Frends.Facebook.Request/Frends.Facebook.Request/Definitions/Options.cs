namespace Frends.Facebook.Request.Definitions;

using System.ComponentModel;

/// <summary>
/// Options class usually contains parameters that are optional.
/// </summary>
public class Options
{
    /// <summary>
    /// Gets or sets a value indicating whether error should throw an exception (true) or stop the Task and return result object containing Result.Success = false and Result.ErrorMessage = 'exception message' (false).
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; }
}
