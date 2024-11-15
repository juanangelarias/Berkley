using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Shared.Imaging;

namespace James.Shared.Model
{
    public class ImagingDocument
    {
        public string Guid { get; set; }
        public string DocumentClass { get; set; }
        public string? FolderPath { get; set; }
        public DateTime EntryDate { get; set; }
        public List<ImagingProperty>? Properties { get; set; }
        public List<ImagingContent>? ContentList { get; set; }

        public string Description
        {
            get
            {
                var baseFilename = Properties.SingleOrDefault(p => p.Name == "DocRemarks").Value ??
                                   (null == ContentList || ContentList.Count == 0 ? "Unknown" : ContentList[0].Filename);
                return baseFilename;
            }
        }
        public string Filename
        {
            get
            {
                try
                {
                    var baseFilename = Properties.FirstOrDefault(p => p.Name == ImagingAccessBase.Filename)?.Value ??
                                       //Properties.SingleOrDefault(p => p.Name == ImagingAccessBase.DocRemarks)?.Value ??
                                       (null == ContentList || ContentList.Count == 0 ? "Unknown" : ContentList[0].Filename);
                    var fileType = Path.GetExtension(baseFilename);
                    if (string.Empty == fileType)
                    {
                        var mimeType = Properties.SingleOrDefault(p => p.Name == ImagingAccessBase.MimeType)?.Value ??
                                       (null == ContentList || ContentList.Count == 0 ? "" : ContentList[0].MimeType);
                        if (!string.IsNullOrWhiteSpace(mimeType))
                            baseFilename = Path.ChangeExtension(baseFilename, MimeTypes.ExtensionFromMimeType(mimeType));
                    }

                    return baseFilename;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }
        }

        public string? DocumentType
        {
            get => Properties?.SingleOrDefault(p => p.Name == ImagingAccessBase.DocType)?.Value;
            set
            {
                Properties ??= new List<ImagingProperty>(1);
                if (null == Properties.SingleOrDefault(p => p.Name == ImagingAccessBase.DocType))
                    Properties.Add(new ImagingProperty { Name = ImagingAccessBase.DocType, DisplayName = "Document Type" });
                Properties.SingleOrDefault(p => p.Name == ImagingAccessBase.DocType)!.Value = value;
            }
        }

        public override string ToString()
        {
            return $"Doc Class: {DocumentClass}\\r\\nGuid = {Guid}";
        }
    }

    public class ImagingContent
    {
        public string Filename { get; set; }
        public string MimeType { get; set; }
    }

    public class ImagingSearchCriteria
    {
        public string ContentSearchString { get; set; }
        public string DocClass { get; set; }
        public string Fields { get; set; }
        public int MaxResults { get; set; }
        public int SearchTimeoutSeconds { get; set; }
        public string WhereClause { get; set; }
    }

    public class ImagingChoice
    {
        public string DisplayValue { get; set; }
        public string Value { get; set; }
    }

    public class ImagingProperty
    {
        public ImagingChoice[]? ChoiceOption { get; set; }
        public ImagingPropertyDataType DataType { get; set; }
        public bool DataTypeSpecified { get; set; }
        public DateTime?[] DateListValue { get; set; }
        public DateTime DateValue { get; set; }
        public bool DateValueSpecified { get; set; }
        public string? DisplayName { get; set; }
        public string Description { get; set; }
        public string[] ListValue { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
        public bool ReadOnlySpecified { get; set; }
        public bool Required { get; set; }
        public bool RequiredSpecified { get; set; }
        public string RequiredFormat { get; set; }
        public string RequiredFormatRegex { get; set; }
        public bool SystemGenerated { get; set; }
        public bool SystemGeneratedSpecified { get; set; }
        public string? Value { get; set; }
    }
}

public enum ImagingPropertyDataType
{
    STRING,
    STRING_LIST,
    DATE,
    DATE_LIST,
    INTEGER,
    INTEGER_LIST,
    DOUBLE,
    DOUBLE_LIST,
    CHOICE_LIST_STRING,
    CHOICE_LIST_INTEGER,
    CHOICE_LIST_STRING_MULTI,
    CHOICE_LIST_INTEGER_MULTI,
    BOOLEAN,
    BOOLEAN_LIST,
    UNKOWN
}
