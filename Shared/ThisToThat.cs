using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using James.Shared.Data;

namespace James.Shared
{
    public static partial class ThisToThat
    {
        public static ILoggingService? LoggingService { get; set; }
        public static TDest ToEntityType<TDest>(object? source) where TDest : new()
        {
            try
            {
                return (TDest)ToEntityType(source, typeof(TDest));
            }
            catch (InvalidCastException icex)
            {
                LoggingService?.LogException(icex, $"Invalid Cast in ThisToThat", category:StandardLoggingCategories.DataAccess, data: new Dictionary<string, string>{{"sourceType", source?.GetType().Name ?? "null"}, {"destType" ,typeof(TDest).Name}});
                throw;
            }
        }

        private static readonly Type _genericListType = typeof(List<>);

        private static object ToEntityType(object? source, Type destinationType)
        {
            if (null == source)
                return null!;
            if (destinationType.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                return source is not IEnumerable enumerable 
                    ? throw new Exception("Source type is not IEnumerable, perhaps your subproperty is wrong or missing. Check your 'ExecuteGet'") 
                    : CopyIEnumerable(enumerable, destinationType);
            }

            var dConstructor = destinationType.GetConstructor([]) ??
                               throw new Exception("Destination type must have a no argument constructor");
            var result = dConstructor.Invoke([]);
            var sProperties = source.GetType().GetProperties();
            var dProperties = result.GetType().GetProperties().Where(pi => pi.CanWrite).ToArray();
            var propMatches = from sProp in sProperties
                              join dProp in dProperties
                                  on sProp.Name.ToLower() equals dProp.Name.ToLower()
                              select (sProp, dProp);
            foreach (var propMatch in propMatches)
                try
                {
                    if (propMatch.sProp.PropertyType == propMatch.dProp.PropertyType)
                    {
                        //The simplest case:  Scalar to scalar
                        propMatch.dProp.SetValue(result, propMatch.sProp.GetValue(source));
                    }
                    else if (propMatch.sProp.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                    {

                        var destEnumerableType = propMatch.dProp.PropertyType;
                        var sList = (IEnumerable)propMatch.sProp.GetValue(source)!;
                        if (sList != null!)
                            propMatch.dProp.SetValue(result, CopyIEnumerable(sList, destEnumerableType));
                    }
                    else if(propMatch.dProp.PropertyType == typeof(string))
                        //Simply cast source to string
                        propMatch.dProp.SetValue(result, propMatch.sProp.GetValue(source)?.ToString());
                    else if (propMatch.dProp.PropertyType.IsClass)
                    {
                        //Object to Object: Try to convert recursively
                        propMatch.dProp.SetValue(result,
                            ToEntityType(propMatch.sProp.GetValue(source)!, propMatch.dProp.PropertyType));
                    }
                    else if (new[] { typeof(DateTimeOffset?), typeof(DateTimeOffset) }.Contains(propMatch.sProp
                                 .PropertyType)
                             && new[] { typeof(DateTime?), typeof(DateTime) }.Contains(propMatch.dProp
                                 .PropertyType))
                    {
                        //Convert DataTimeOffset into DateTime
                        var val = propMatch.sProp.GetValue(source);
                        var dateTimeProperty = val?.GetType().GetProperty("DateTime");
                        var dtVal = (DateTime?)dateTimeProperty?.GetValue(val);
                        if (null == dtVal && propMatch.dProp.PropertyType == typeof(DateTime))
                            throw new ArgumentNullException($"{nameof(source)}.{propMatch.sProp.Name}",
                                $"Destination cannot accept a null value for property {propMatch.dProp.Name}");
                        propMatch.dProp.SetValue(result, dtVal);
                    }
                    else if (new[] { typeof(DateTime?), typeof(DateTime) }.Contains(propMatch.sProp.PropertyType)
                             && new[] { typeof(DateOnly?), typeof(DateOnly) }.Contains(propMatch.dProp.PropertyType))
                    {
                        //Convert DateTime to DateOnly
                        var val = propMatch.sProp.GetValue(source);
                        if (null != val)
                        {
                            if (null == val && propMatch.dProp.PropertyType == typeof(DateOnly))
                                throw new ArgumentNullException($"{nameof(source)}.{propMatch.sProp.Name}",
                                    $"Destination cannot accept a null value for property {propMatch.dProp.Name}");
                            var dateOnlyProperty = DateOnly.FromDateTime((DateTime)val!);

                            propMatch.dProp.SetValue(result, dateOnlyProperty);
                        }
                    }
                    else if(propMatch.sProp.PropertyType.IsEnum && propMatch.dProp.PropertyType.IsEnum )
                    {
                        var enumText = Enum.GetName(propMatch.sProp.PropertyType, propMatch.sProp.GetValue(source)!);
                        var destVal = Enum.Parse(propMatch.dProp.PropertyType, enumText!, true);
                        propMatch.dProp.SetValue(result, destVal);
                    }
                    else
                    {
                        Debug.WriteLine(
                            $"Property {propMatch.sProp.Name} skipped because source type was {propMatch.sProp.PropertyType} and destination type was {propMatch.dProp.PropertyType}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception converting property {propMatch.sProp.Name}", ex);
                }
            return result;
        }

        private static readonly HashSet<Type> _validIEnumerableTypes = new();
        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        /// Copies from an IEnumerable to a typed destination
        /// </summary>
        /// <param name="source">Source IEnumerable</param>
        /// <param name="destType">Destination type.  Generic List<> objects are recommended</param>
        /// <returns>concrete version of the destination type with converted data</returns>
        private static IEnumerable CopyIEnumerable(IEnumerable source, Type destType)
        {
            //HACK: Will fail on multi-argument generic list.  I don't believe they will be encountered in these conversions.
            if (null! == source) return null!;
            //Confirm destination type is a generic IEnumerable and cache the result to avoid reflection hit.
            //TODO:Performance test this
            if (!_validIEnumerableTypes.Contains(destType))
            {
                if (destType.GetInterfaces().All(i => i.Name != "IEnumerable`1"))
                    throw new NotSupportedException("destType must be a generic IEnumerable");
                _validIEnumerableTypes.Add(destType);
            }

            var dListType = destType.GenericTypeArguments.Single();
            var list = (from object? item in source select ToEntityType(item, dListType)).ToList();

            IEnumerable? MakeConcreteList(Type type)
            {
                var genList = Activator.CreateInstance(type)!;
                var addMethod = type.GetMethod("Add");
                if (addMethod == null) return null;
                foreach (var item in list)
                    addMethod?.Invoke(genList, [item]);
                return (IEnumerable)genList;
            }

            var finalType = destType.IsInterface ? _genericListType.MakeGenericType(destType.GenericTypeArguments) :
                    destType;
            var finalList = MakeConcreteList(finalType);
            if (finalList != null) return finalList;

            //HACK: Assumes there will be a constructor that will take an IEnumerable of values.
            var dListConstructor = destType.GetConstructor([list.GetType()]);
            var dList = dListConstructor?.Invoke([list])!;
            return (IEnumerable)dList;
        }

        /// <summary>
        /// Converts an <paramref name="exception"/> details to text
        /// </summary>
        /// <param name="exception">exception</param>
        /// <returns>Multi-lined text with details about the exception</returns>
        public static string ToText(this Exception exception)
        {
            var sb = new StringBuilder(100);
            var current = exception;
            var level = 0;
            while (null != current)
            {
                if (level > 0)
                {
                    sb.Append(new string('\t', level - 1));
                    sb.AppendLine("Inner Exception:");
                }
                sb.Append(new string('\t', level));
                sb.Append(current.GetType());
                sb.Append(": ");
                sb.AppendLine(current.Message);
                if (current.Data.Count > 0)
                {
                    sb.Append(new string('\t', level));
                    sb.AppendLine("Data:");
                    var dataKeys = current.Data.Keys.OfType<object>().Select(k => k.ToString()).ToArray();
                    var dataValues = current.Data.Values.OfType<object>().Select(v => v.ToString()).ToArray();
                    for (var i = 0; i < current.Data.Count; i++)
                    {
                        sb.Append(new string('\t', level + 1));
                        sb.Append("Key: ");
                        sb.Append(i);
                        sb.Append(":\t");
                        sb.Append(dataKeys[i]);
                        sb.Append("\t\tValue: ");
                        sb.Append(i);
                        sb.Append(":\t");
                        sb.Append(current.Data[i]);
                        sb.AppendLine(dataValues[i]);
                    }
                }
                if (null != exception.StackTrace)  //Is null during unit test
                    using (var sr = new StringReader(exception.StackTrace))
                        while (sr.Peek() > -1)
                        {
                            sb.Append(new string('\t', level));
                            sb.AppendLine(sr.ReadLine());
                        }
                level++;
                current = current.InnerException;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts an <paramref name="exception"/> details to text
        /// </summary>
        /// <param name="exception">exception</param>
        /// <returns>Multi-lined text with details about the exception</returns>
        public static string ToHtml(this Exception exception)
        {
            var detailText = exception.ToText();
            detailText = LineBreakRegex().Replace(detailText.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"), "<br />");
            return detailText;
        }

        [GeneratedRegex("[\r\n]+")]
        private static partial Regex LineBreakRegex();
    }
}
