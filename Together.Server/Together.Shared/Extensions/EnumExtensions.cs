using System.ComponentModel;
using System.Reflection;

namespace Together.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<TEnum>(this TEnum value)
        where TEnum : struct
    {
        var enumType = typeof(TEnum);
        if (!enumType.IsEnum) throw new InvalidOperationException();

        var name = Enum.GetName(enumType, value) ?? default!;
        
        var field = typeof(TEnum).GetField(name, BindingFlags.Static | BindingFlags.Public);
        return field!.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name;
    }
}