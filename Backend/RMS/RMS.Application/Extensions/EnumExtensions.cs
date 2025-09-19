using RMS.Application.DTOs.Dropdowns;
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts an enum to a list of DropdownDto.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>A list of key-value pairs representing the enum values.</returns>
        public static Result<List<DropdownDto>> BuildDropdown<T>() where T : Enum
        {
            try
            {
                var result = Enum.GetValues(typeof(T))
                                 .Cast<T>()
                                 .Where(e => !EqualityComparer<T>.Default.Equals(e, default))
                                 .Select(value => new DropdownDto()
                                 {
                                     Value = Convert.ToInt32(value),
                                     Label = GetEnumDescription(value)
                                 })
                                 .ToList();

                return Result<List<DropdownDto>>.Success(result);
            }
            catch (Exception)
            {
                return Result<List<DropdownDto>>.Success([]);
            }
        }

        /// <summary>
        /// Retrieves the description attribute of an enum value.  
        /// If the enum value does not have a description attribute, the enum name is returned instead.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The description of the enum value, or its name if no description is found.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided value is not a valid enum type.</exception>
        public static string GetEnumDescription<TEnum>(TEnum value) where TEnum : Enum
        {
            var field = typeof(TEnum).GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }
    }
}
