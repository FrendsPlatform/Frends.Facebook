using System.ComponentModel.DataAnnotations;

namespace Frends.Facebook.Get.Definitions;

public class Parameter
{
    /// <summary>
    /// Name of the parameter.
    /// </summary>
    /// <example>Name</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Name { get; set; }

    /// <summary>
    /// Value of the parameter.
    /// </summary>
    /// <example>Value</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Value { get; set; }
}
