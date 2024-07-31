using System.Diagnostics;

namespace James.Shared
{
    public static class SameOrDifferent
    {
        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        /// Compares two objects with similar property names.  
        /// </summary>
        /// <param name="source">source object</param>
        /// <param name="target">target object</param>
        /// <param name="root">Root property string.  Should be empty string on the root object, and path to current property from the root object if comparing a sub property</param>
        /// <param name="ignoreCreatedModified"></param>
        /// <returns>A <typeparam name="James.Shared.PropertyComparision"/> describing what is the same and what is different</returns>
        public static PropertyComparision Compare(object? source, object? target, string root = "", bool ignoreCreatedModified = true)
        //NOTE: If needed, add arguments here to limit which properties comparison cares about.  Otherwise YAGNI
        {
            if (source?.GetType().IsNonSimpleClass() == false && target?.GetType().IsNonSimpleClass() == false)
            {
                //Compare to simple (or null) objects
                return source.ToString() == target.ToString()
                    ? new PropertyComparision { SameProperties = [root] }
                    : new PropertyComparision
                    {
                        Differences = new() { [root] = new ComparedValues(source, target) }
                    };
            }
            if (root.Length > 0 && root.EndsWith("."))
                root += ".";
            //If both are null, then they are the same
            if (null == source && null == target)
                return new PropertyComparision { SameProperties = [root] };
            //If only one is null, then they are the different
            if (null == source ^ target == null)
                return new PropertyComparision
                {
                    Differences = new() { [root] = new ComparedValues(source, target) }
                };
            Debug.Assert(source != null, nameof(source) + " != null");
            var sourceProperties = source.GetType().GetProperties();
            Debug.Assert(target != null, nameof(target) + " != null");
            var targetProperties = target.GetType().GetProperties();
            var result = new PropertyComparision { TargetOnlyProperties = targetProperties.Select(tp => tp.Name).Except(sourceProperties.Select(sp => sp.Name)).Select(n => root + n).ToList() };

            //By default, created and modified should be ignored
            var ignoreList = new List<string>();
            if (ignoreCreatedModified)
            {
                ignoreList.Add("Created");
                ignoreList.Add("Modified");
            }
            foreach (var property in sourceProperties)
            {
                if (ignoreList.Contains(property.Name)) continue;
                var targetProperty = targetProperties.SingleOrDefault(tp => tp.Name == property.Name);
                if (null == targetProperty)
                {
                    result.SourceOnlyProperties.Add(property.Name);
                    continue;
                }

                var sourceVal = property.GetValue(source);
                var targetVal = targetProperty.GetValue(target);
                if (property.PropertyType.IsNonSimpleClass())
                {
                    if (null == sourceVal ^ targetVal == null)
                    {
                        result.Differences[root + property.Name] = new(sourceVal, targetVal);
                        continue;
                    }
                    if (null == sourceVal && targetVal == null)
                    {
                        result.SameProperties.Add(root + property.Name);
                        continue;
                    }
                    //Handle subclass
                    var subResult = Compare(sourceVal!, targetVal!, root + property.Name);
                    result.SameProperties.AddRange(subResult.SameProperties);
                    result.DifferentProperties.AddRange(subResult.DifferentProperties);
                    //result.DifferentPropertiesWithValues.AddRange(subResult.DifferentPropertiesWithValues);
                    foreach (var kvp in subResult.Differences)
                        result.Differences[kvp.Key] = kvp.Value;
                    result.SourceOnlyProperties.AddRange(subResult.SourceOnlyProperties);
                    result.TargetOnlyProperties.AddRange(subResult.TargetOnlyProperties);
                }
                else if (property.GetValue(source)?.ToString() == targetProperty.GetValue(target)?.ToString())
                    result.SameProperties.Add(root + property.Name);
                else
                    result.Differences[root + property.Name] = new(sourceVal, targetVal);
            }

            return result;
        }

        /// <summary>
        /// Copy certain properties from the source to the target
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="target">Target object</param>
        /// <param name="properties">Names of properties to copy.</param>
        public static void CopyTheseProperties(object source, object target, IEnumerable<string> properties)
        {
            foreach (var propertyName in properties)
            {
                var readPropertyInfo = source.GetType().GetProperty(propertyName);
                var writePropertyInfo = target.GetType().GetProperty(propertyName);
                Debug.Assert(writePropertyInfo != null, nameof(writePropertyInfo) + " != null");
                Debug.Assert(readPropertyInfo != null, nameof(readPropertyInfo) + " != null");
                writePropertyInfo.SetValue(target, readPropertyInfo.GetValue(source));
            }
        }
        /// <summary>
        /// List of types that are classes that it is OK to compare with an == instead of by sub properties.
        /// </summary>
        private static readonly Type[] _simpleTypes = [typeof(string), typeof(DateTime), typeof(DateTimeOffset), typeof(DateOnly)];
        private static bool IsNonSimpleClass(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);
            return !_simpleTypes.Contains(type) && type.IsClass;
        }
    }

    /// <summary>
    /// Summary of how two objects properties are the same or different
    /// </summary>
    public record PropertyComparision
    {
        public bool AreTheSame => DifferentProperties.Count == 0;
        public List<string> SameProperties { get; init; } = [];
        public List<string> DifferentProperties => [.. Differences.Keys];
        //[Obsolete]
        //public List<ComparisonValues> DifferentPropertiesWithValues { get; init; } = [];
        public Dictionary<string, ComparedValues> Differences { get; init; } = [];
        public List<string> SourceOnlyProperties { get; } = [];
        public List<string> TargetOnlyProperties { get; init; } = [];
    }

    /// <summary>
    /// Values that were compared
    /// </summary>
    /// <param name="SourceValue">Source Value</param>
    /// <param name="TargetValue">Target Value</param>
    public record ComparedValues(object? SourceValue, object? TargetValue)
    {
        public virtual bool Equals(ComparedValues? other)
        {
            return other?.SourceValue == SourceValue && other?.TargetValue == TargetValue;
        }

        public override string ToString()
        {
            return $"Source: {SourceValue}\r\nTarget: {TargetValue}";
        }

        public override int GetHashCode()
        {
            //HACK:  Good enough to match whether this should pass an equality check
            return SourceValue?.GetHashCode() ?? 0 + 579 * TargetValue?.GetHashCode() ?? 0;
        }
    }
}
