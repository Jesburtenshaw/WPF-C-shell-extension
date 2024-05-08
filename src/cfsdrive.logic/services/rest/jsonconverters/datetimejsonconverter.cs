#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : datetimejsonconverter.cs 
 * 
 * Contents	: Implementation of Datetime Json Converter
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cfsdrive.logic.services.rest.jsonconverters
{
    public class DatetimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Integer || token.Type == JTokenType.String)
            {
                return DateTime.Parse(token.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }
            else if (token.Type == JTokenType.Date)
            {
                return token.Value<DateTime>();
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
