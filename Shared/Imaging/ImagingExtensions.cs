using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Imaging
{
    public static class ImagingExtensions
    {
        /// <summary>
        /// Use the enumeration as the source of truth for docCategory items.
        /// </summary>
        /// <param name="category">Enumeration value</param>
        /// <returns>P8 document category as string</returns>
        public static string DocumentCategory(this ImagingDocumentCategory category)
        {
            var fi = category.GetType().GetField(category.ToString());
            var docClassAttr =
                fi!.GetCustomAttributes(typeof(DocumentCategoryAttribute), false)
                    .OfType<DocumentCategoryAttribute>()
                    .FirstOrDefault();
            if (null == docClassAttr)
                throw new ArgumentException("Invalid docCategory", "category");
            return docClassAttr.DocCategory;
        }
        /// <summary>
        /// Shortcut for converting the enum to its name
        /// </summary>
        /// <param name="category">Enumeration value</param>
        /// <returns>Enumeration name as string</returns>
        public static string Name(this ImagingDocumentCategory category) => Enum.GetName(typeof(ImagingDocumentCategory), category)!;
    }
}
