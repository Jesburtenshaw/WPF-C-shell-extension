#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : querystringparams.cs 
 * 
 * Contents	: Implementation of the QueryString parameters
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using cfsdrive.logic.services.rest.core;

namespace cfsdrive.logic.services.rest.models
{
    public abstract class QueryStringParams : IQueryStringParams
    {
        public virtual string ToQueryString()
        {
            var props = GetType().GetProperties().Where(a => a.GetCustomAttributes<DisplayNameAttribute>().Any())
                .Select(
                    a => new
                    {
                        DisplayNameAttr = a.GetCustomAttributes<DisplayNameAttribute>().First(),
                        Prop = a
                    });

            var @params = props.Select(a =>
                {
                    if (a.Prop.PropertyType == typeof(DateTime?))
                    {
                        return $"{a.DisplayNameAttr.DisplayName}={((DateTime?)a.Prop.GetValue(this))?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}";
                    }

                    if (a.Prop.PropertyType == typeof(bool?) || a.Prop.PropertyType == typeof(bool))
                    {
                        return $"{a.DisplayNameAttr.DisplayName}={((bool?)a.Prop.GetValue(this))?.ToString().ToLowerInvariant()}";
                    }

                    if (a.Prop.PropertyType == typeof(string) && string.IsNullOrEmpty(a.Prop.GetValue(this).ToString()))
                    {
                        return string.Empty;
                    }
                    return $"{a.DisplayNameAttr.DisplayName}={HttpUtility.UrlEncode(a.Prop.GetValue(this)?.ToString())}";
                }).Where(x => !string.IsNullOrEmpty(x));

            return $"{string.Join("&", @params)}";
        }
    }
}
