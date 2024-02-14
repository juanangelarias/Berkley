using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace StrawberryShakeWASM.Client
{
    public static class ThisToThat
    {

        public static TDest ToEntityType<TDest>(object source) where TDest : new()
        {
            var result = new TDest();
            var sProperties = source.GetType().GetProperties();
            var dProperties = result.GetType().GetProperties();
            var propMatches = from sProp in sProperties
                                join  dProp in dProperties
                                    on sProp.Name.ToLower() equals  dProp.Name.ToLower()
                                        select (sProp, dProp);
            foreach (var propmatch in propMatches)
            {
                if (propmatch.sProp.PropertyType == propmatch.dProp.PropertyType)
                {
                    propmatch.dProp.SetValue(result, propmatch.sProp.GetValue(source));
                }
                else
                {
                    Debug.WriteLine($"Property {propmatch.sProp.Name} skipped because source type was {propmatch.sProp.PropertyType} and destination type was {propmatch.dProp.PropertyType}");
                }
            }
            return result;
        }
    }
}
