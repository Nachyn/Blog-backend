using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Domain.Helpers
{
    public static class EnumHelper
    {
        public static TAttribute GetAttributeFromEnum<TEnum, TAttribute>(TEnum enumValue)
            where TAttribute : Attribute
        {
            var type = typeof(TEnum);
            var memInfo = type.GetMember(type.GetEnumName(enumValue));
            var attribute = memInfo[0]
                .GetCustomAttributes(typeof(TAttribute), false)
                .FirstOrDefault() as TAttribute;

            return attribute;
        }

        /// <summary>
        ///     Возвращает значение атрибута "Description" данного енама.
        /// </summary>
        public static string GetEnumDescription<TEnum>(this TEnum enumValue)
        {
            var descriptionAttribute =
                GetAttributeFromEnum<TEnum, DescriptionAttribute>(enumValue);

            return descriptionAttribute.Description ?? enumValue.ToString();
        }

        /// <summary>
        ///     Возвращает все члены перечисления.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<TEnum> GetAllMembers<TEnum>()
        {
            var type = typeof(TEnum);
            var members = Enum.GetValues(type)
                .Cast<TEnum>()
                .ToList();

            return members;
        }
    }
}