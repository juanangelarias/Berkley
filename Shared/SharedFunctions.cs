using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Shared.Model;

namespace James.Shared
{
    public static class SharedFunctions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body, int dop = 6)
        {
            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current);
                }));
        }

        /// <summary>
        /// Determines if a string contains digits only
        /// </summary>
        /// <param name="str">string being checked</param>
        /// <returns>False if the string contains anything besides a digit 0-9</returns>
        /// <remarks>Benchmarked as fastest https://stackoverflow.com/questions/7461080/fastest-way-to-check-if-string-contains-only-digits-in-c-sharp</remarks>
        public static bool IsDigitsOnly(this string str)
        {
            foreach (var c in str)
            {
                if (c is < '0' or > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Converts Snake case to pascal case
        /// </summary>
        /// <param name="snakeCasedString">SNAKE_CASE_STRING or SNAKE-CASE-STRING</param>
        /// <returns>PascalCaseString</returns>
        public static string ToPascalCase(string snakeCasedString)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string result = textInfo.ToTitleCase(snakeCasedString.ToLower().Replace("_", " ").Replace("-", " "));
            return result.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Converts pascal case strings to snake case
        /// </summary>
        /// <param name="pascalCasedString">PascalCasedString</param>
        /// <returns>Snake_Cased_String</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToSnakeCase(this string pascalCasedString)
        {
            if (pascalCasedString == null)
            {
                throw new ArgumentNullException(nameof(pascalCasedString));
            }
            if (pascalCasedString.Length < 2)
            {
                return pascalCasedString.ToLowerInvariant();
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(pascalCasedString[0]));
            for (int i = 1; i < pascalCasedString.Length; ++i)
            {
                char c = pascalCasedString[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string CombineWithCommasAndAnd(this List<string> items)
        {
            if (items == null! || items.Count == 0)
                return string.Empty;
            if (items.Count == 1)
                return items[0];
            if (items.Count == 2)
                return $"{items[0]} and {items[1]}";

            return string.Join(", ", items.GetRange(0, items.Count - 1)) + " and " + items;
        }

    }
}
