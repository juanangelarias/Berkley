using System.Reflection;
using System.Text.Json;

namespace James.Shared;

public static class Tools
{
    public static string TruncateString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }

    public static T Clone<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(json)!;
    }

    public static bool IsEqualTo<T>(this T obj, T other)
    {
        return EqualityComparer<T>.Default.Equals(obj, other);
    }

    public static bool PropertiesEqual<T>(this T obj, T other)
    {
        if (obj == null && other == null)
            return true;

        if (obj == null || other == null)
            return false;

        var type = typeof(T);

        foreach (PropertyInfo prop in type.GetProperties())
        {
            var value1 = prop.GetValue(obj);
            var value2 = prop.GetValue(other);

            if (!Equals(value1, value2))
                return false;
        }

        return true;
    }

    public static bool DeepEquals<T>(this T obj, T other)
    {
        if (ReferenceEquals(obj, other))
            return true;

        if (obj == null || other == null)
            return false;

        var type = typeof(T);

        if (type.IsPrimitive || type == typeof(string))
            return obj.Equals(other);

        foreach (var prop in type.GetProperties())
        {
            var val1 = prop.GetValue(obj);
            var val2 = prop.GetValue(other);

            if (!DeepEquals(val1, val2))
                return false;
        }

        return true;
    }
}