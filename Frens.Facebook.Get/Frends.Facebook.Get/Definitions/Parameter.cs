namespace Frends.Facebook.Get.Definitions;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Parameter class includes name and value.
/// </summary>
public class Parameter
{
    /// <summary>
    /// Sets the name of the parameter.
    /// </summary>
    /// <example>Name.</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Name { get; set; }

    /// <summary>
    /// Sets the value of the parameter.
    /// </summary>
    /// <example>Value.</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Value { get; set; }
}
