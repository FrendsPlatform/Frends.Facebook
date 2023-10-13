﻿namespace Frends.Facebook.Get.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Input
{
    /// <summary>
    /// Set reference type.
    /// </summary>
    /// <example>Insights</example>
    [DefaultValue(References.Insights)]
    public References Reference { get; set; }

    /// <summary>
    /// Reference is other. All reference types can be found from: https://developers.facebook.com/docs/graph-api/reference
    /// </summary>
    /// <example>Insights</example>
    [UIHint(nameof(Reference), "", References.Other)]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("Insight")]
    public string Other { get; set; }

    /// <summary>
    /// Object id of Insight, Page or AD.
    /// </summary>
    /// <example>123456789</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("123456789")]
    public string ObjectId { get; set; }

    /// <summary>
    /// List of parameters
    /// </summary>
    /// <example>[{ Name, Value }]</example>
    public Parameter[] Parameters { get; set; } = System.Array.Empty<Parameter>();

    /// <summary>
    /// Authentication bearer token.
    /// </summary>
    /// <example>BearerToken1234</example>
    [PasswordPropertyText]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("BearerToken1234")]
    public string Token { get; set; }
}