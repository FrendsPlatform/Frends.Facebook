﻿namespace Frends.Facebook.Read.Definitions;

using System.Collections.Generic;
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
    /// Reference is other.
    /// </summary>
    /// <example>Insight</example>
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
    /// <example>123456789</example>
    public List<KeyValuePair<string, string>> Parameters { get; set; }

    /// <summary>
    /// Authentication bearer token.
    /// </summary>
    /// <example>BearerToken1234</example>
    [PasswordPropertyText]
    [DefaultValue("BearerToken1234")]
    public string Token { get; set; }
}