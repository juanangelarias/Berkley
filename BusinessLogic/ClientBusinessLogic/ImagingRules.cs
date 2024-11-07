using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Shared.Imaging;
using James.Shared.Model;

namespace ClientBusinessLogic
{
    public static class ImagingRules
    {
        //TODO: Set the value of this max in code somewhere
        public static int MaxUploadSizeInBytes { get; set; } = 29360128;//Value pulled from Basis

        public static ImagingType CreateUnknownType()
        {
            return new ImagingType {Id = Guid.Parse("6C99DBC1-97BF-4D51-93C4-C57B811F84D2"), Type = "UNKNOWN", Description = "Unknown"  };
        }

        public static VImagingCategoryTabDivisionType CreateUnknownType(ImagingDocumentCategory documentCategory, string? division)
        {
            var unknownType = CreateUnknownType();
            return new VImagingCategoryTabDivisionType
            {
                Category = Enum.GetName(typeof(ImagingDocumentCategory), documentCategory)!,
                DivisionCode = division,
                TabName = "Old Surety Documents",
                TabDescription = "ACCT_ALL_Old Surety Documents",
                TypeDescription = unknownType.Description,
                Type = unknownType.Type
            };
        }
    }
}
