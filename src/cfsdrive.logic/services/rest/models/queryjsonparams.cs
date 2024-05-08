#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : queryjsonparams.cs 
 * 
 * Contents	: Implementation of the QueryJSON parameters
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using cfsdrive.logic.services.rest.core;
using Newtonsoft.Json;

namespace cfsdrive.logic.services.rest.models
{
    public abstract class QueryJsonParams : IJsonQueryParams
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ArrayQueryJsonParams<T> : IJsonQueryParams
    {
        T[] _array;
        public ArrayQueryJsonParams(T[] array)
        {
            _array = array;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_array);
        }
    }

    public class SimpleStringQueryJsonParams : IJsonQueryParams
    {
        private string _value;

        public SimpleStringQueryJsonParams(string value)
        {
            _value = value;
        }

        public string ToJson()
        {
            return $"\"{_value}\"";
        }
    }
}
