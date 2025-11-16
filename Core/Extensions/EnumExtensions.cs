namespace Core.Extensions
{
    using System.Reflection;
    using Core.Attributes;

    public static class EnumExtensions
    {
        public static TEnum? ToEnumType<TEnum>(this string value) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var parsed)) return parsed;

            var type = typeof(TEnum);
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var aliases = field.GetCustomAttributes<EnumAliasAttribute>();
                foreach (var alias in aliases)
                {
                    if (string.Equals(alias.Alias, value, StringComparison.OrdinalIgnoreCase)) return (TEnum)field.GetValue(null)!;
                }
            }

            return null;
        }
    }
}