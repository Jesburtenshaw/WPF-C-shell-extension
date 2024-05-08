#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : enumextension.cs 
 * 
 * Contents	: Implementation of Enum extensions
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace cfsdrive.logic.extensions
{
    public static class EnumExtension
    {
        public static TAttribute GetEnumAttribute<TAttribute>(this Enum enumVal) where TAttribute : Attribute
        {
            var memberInfo = enumVal.GetType().GetMember(enumVal.ToString());
            return memberInfo[0].GetCustomAttributes(typeof(TAttribute), false).OfType<TAttribute>().FirstOrDefault();
        }

        public static string GetDescription(this Enum enumValue) => enumValue.GetEnumAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();

        public static string ToEnumMemberAttrValue(this Enum @enum)
        {
            var attr = @enum.GetType().GetMember(@enum.ToString()).FirstOrDefault()?.
                GetCustomAttributes(false).OfType<EnumMemberAttribute>().
                FirstOrDefault();
            return attr == null ? @enum.ToString() : attr.Value;
        }

        public static E GetEnumFromString<E>(this string source)
        {
            var enumType = typeof(E);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (string.Compare(enumMemberAttribute.Value, source, StringComparison.OrdinalIgnoreCase) == 0)
                    return (E)Enum.Parse(enumType, name);
            }
            return default;
        }

        public static bool IsOneOf(this Enum enumeration, params Enum[] enums)
        {
            return enums.Contains(enumeration);
        }
    }
}
