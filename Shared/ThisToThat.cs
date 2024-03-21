using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;

namespace James.Shared
{
    public static partial class ThisToThat
    {

        public static TDest ToEntityType<TDest>(object source) where TDest : new()
        {
            return (TDest)ToEntityType(source, typeof(TDest));
        }

        public static object ToEntityType(object source, Type destinationType)
        {
            if (null == source) 
                return null;
            var dConstructor = destinationType.GetConstructor(Array.Empty<Type>());
            if (dConstructor == null)
                throw new Exception("Destination type must have a no argument constructor");
            var result = dConstructor.Invoke(Array.Empty<object?>());
            var sProperties = source.GetType().GetProperties();
            var dProperties = result.GetType().GetProperties();
            var propMatches = from sProp in sProperties
                              join dProp in dProperties
                                  on sProp.Name.ToLower() equals dProp.Name.ToLower()
                              select (sProp, dProp);
            foreach (var propmatch in propMatches)
            {
                if (propmatch.sProp.PropertyType == propmatch.dProp.PropertyType)
                {
                    //The simplest case:  Scalar to scalar
                    propmatch.dProp.SetValue(result, propmatch.sProp.GetValue(source));
                }
                else if (propmatch.dProp.PropertyType.IsClass)
                {
                    //Object to Object: Try to convert recursively
                    propmatch.dProp.SetValue(result,
                        ToEntityType(propmatch.sProp.GetValue(source), propmatch.dProp.PropertyType));
                }
                else if (propmatch.sProp.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    //IEnumberable to IEnumearble: Copy list
                    var list = new List<object>();
                    //HACK: Will fail on multi-argument generic list.  I don't believe they will be encountered in these conversions.
                    var listType = propmatch.sProp.PropertyType.GenericTypeArguments.Single();
                    var dListType = propmatch.dProp.PropertyType.GenericTypeArguments.Single();
                    var sList = (IEnumerable)propmatch.sProp.GetValue(source);
                    foreach (var listItem in sList)
                    {
                        list.Add(ToEntityType(listItem, dListType));
                    }

                    if (propmatch.dProp.PropertyType.IsInterface)
                    {
                        var genListType = typeof(List<>);
                        var makeme = genListType.MakeGenericType(propmatch.dProp.PropertyType.GenericTypeArguments);
                        var genList = Activator.CreateInstance(makeme);
                        var addMethod = genList.GetType().GetMethod("Add");
                        foreach(var item in list)
                            addMethod.Invoke(genList, new object?[] {item});
                        propmatch.dProp.SetValue(result, genList);
                    } else
                    {
                        //HACK: Assumes there will be a constructor that will take an IEnumerable of values.
                        var dListConstructor = propmatch.dProp.PropertyType.GetConstructor(new Type[] {list.GetType()});
                        var dList = dListConstructor.Invoke(new object?[] { list });
                        propmatch.dProp.SetValue(result, dList);
                    }
                }
                else if (new Type[] {typeof(DateTimeOffset?), typeof(DateTimeOffset)}.Contains(propmatch.sProp
                             .PropertyType)
                         && new Type[] { typeof(DateTime?), typeof(DateTime) }.Contains(propmatch.dProp
                             .PropertyType))
                {
                    //Convert DataTimeOffset into DateTime
                    var val = propmatch.sProp.GetValue(source);
                    var dateTimeProperty = val?.GetType().GetProperty("DateTime");
                    var dtVal = (DateTime?)dateTimeProperty?.GetValue(val);
                    if (null == dtVal && propmatch.dProp.PropertyType == typeof(DateTime))
                        throw new ArgumentNullException($"{nameof(source)}.{propmatch.sProp.Name}",
                            $"Destination cannot accept a null value for property {propmatch.dProp.Name}");
                    propmatch.dProp.SetValue(result, dtVal);
                }
                else
                {
                    Debug.WriteLine($"Property {propmatch.sProp.Name} skipped because source type was {propmatch.sProp.PropertyType} and destination type was {propmatch.dProp.PropertyType}");
                }
            }
            return result;
        }

        /// <summary>
        /// Converts an <paramref name="exception"/> details to text
        /// </summary>
        /// <param name="exception">exception</param>
        /// <returns>Mulit-lined text with details about the exception</returns>
        public static string ToText(this Exception exception)
        {
            var sb = new StringBuilder();
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
        /// <returns>Mulit-lined text with details about the exception</returns>
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
