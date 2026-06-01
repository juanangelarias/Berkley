using Newtonsoft.Json;

namespace James.Shared;

public static class Tools
{
    public static string TruncateString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
    
    public static T? Clone<T>(this T? obj)
    {
        var targetJson = JsonConvert.SerializeObject(obj);
        
        var target = JsonConvert.DeserializeObject<T>(targetJson);
        
        return target;
    }
}