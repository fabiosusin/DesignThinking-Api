using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DTO.Surf.Enum
{
    public static class AppEnumConverter
    {
        public static T GetByDescriptionDtoConverter<T>(this T enumDefaultValue, string description) where T : System.Enum
        {
            var value = description == null ? null : enumDefaultValue.GetType().GetFields().FirstOrDefault(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description == description)?.GetValue(null);
            return value == null ? enumDefaultValue : (T)value;
        }
    }
}
