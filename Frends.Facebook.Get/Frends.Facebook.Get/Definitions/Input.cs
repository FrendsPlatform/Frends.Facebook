namespace Frends.Facebook.Get.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Input
{
    /// <summary>
    /// Gets or sets reference type. All reference types can be found from: https://developers.facebook.com/docs/graph-api/reference.
    /// </summary>
    /// <example>/insights?metrics=id,name</example>
    [DefaultValue("Insights")]
    public string References { get; set; }

    /// <summary>
    /// Gets or sets object id of Insight, Page or AD.
    /// </summary>
    /// <example>123456789.</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("123456789")]
    public string ObjectId { get; set; }

    /// <summary>
    /// Gets or sets API version.
    /// </summary>
    /// <example>18.0</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("18.0")]
    public string ApiVersion { get; set; }

    /// <summary>
    /// Gets or sets authentication bearer token.
    /// </summary>
    /// <example>BearerToken1234.</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("BearerToken1234")]
    public string AccessToken { get; set; }
}