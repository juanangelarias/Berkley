using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
