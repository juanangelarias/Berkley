using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Imaging
{
    /// <summary>
    /// Binds P8 DocCategory to <see cref="ImagingDocumentCategory"/> enum
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DocumentCategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocCategoryAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DocumentCategoryAttribute(string name)
        {
            DocCategory = name;
        }
        /// <summary>
        /// Gets the string value of the document category.
        /// </summary>
        /// <value>
        /// The string value of the document category.
        /// </value>
        public string DocCategory { get; private set; }
    }
}
