using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileNetP8SoapService;
using James.Shared.Imaging;
using James.Shared.Model;

namespace James.Data.Imaging
{
    public static class ImagingExtensions
    {
        public static string? GetProperty(this document? doc, string propertyName)
        {
            return doc.properties.GetProperty(propertyName);
        }

        public static string? GetProperty(this property[] props, string propertyName)
        {
            //Match first on name, then DisplayName if name it does not exist.
            var prop =
                props.Where(p => p.name == propertyName)
                    .Union(props.Where(p => p.displayName == propertyName))
                    .ToArray();
            if (prop.Length == 0) return null;
            return prop[0].value;
        }

        //TODO:Investigate of automapper or similar can do there conversions
        public static entry ToEntry(this KeyValuePair<string, string> entry)
        {
            return new entry(entry.Key, entry.Value);
        }

        //TODO:Investigate of automapper or similar can do there conversions
        public static ImagingDocument ToImagingDocument(this document doc)
        {
            return new ImagingDocument
            {
                Guid = doc.guid, DocumentClass = doc.documentClass, FolderPath = doc.folderPath,
                ContentList = doc.contentList?.Select(c=>c.ToImagingContent()).ToList(),
                Properties = doc.properties?.Select(p=>p.ToImagingProperty()).ToList(),
                EntryDate = doc.EntryDate()
            };
        }

        public static ImagingContent ToImagingContent(this content content)
        {
            return new ImagingContent
            {
                Filename = content.fileName,
                MimeType = content.mimeType
            };
        }

        //TODO:Investigate of automapper or similar can do there conversions
        public static ImagingProperty ToImagingProperty(this property prop)
        {
            return new ImagingProperty
            {
                Name = prop .name,
                Value = prop.value,
                ChoiceOption = prop.choiceOption?.Select(co=>new ImagingChoice{DisplayValue=co.displayValue, Value=co.value}).ToArray(),
                DataType = (ImagingPropertyDataType)Enum.Parse(typeof(ImagingPropertyDataType), Enum.GetName(prop.dataType)!),
                DataTypeSpecified = prop.dataTypeSpecified,
                DateListValue = prop.dateListValue,
                DateValue = prop.dateValue,
                DateValueSpecified = prop.dateValueSpecified,
                Description = prop.description,
                DisplayName = prop.displayName,
                ListValue = prop.listValue,
                ReadOnly = prop.readOnly,
                ReadOnlySpecified = prop.readOnlySpecified,
                Required = prop.required,
                RequiredFormat = prop.requiredFormat,
                RequiredFormatRegex = prop.requiredFormatRegex,
                RequiredSpecified = prop.requiredSpecified,
                SystemGenerated = prop.systemGenerated,
                SystemGeneratedSpecified = prop.systemGeneratedSpecified
            };
        }

        public static ImagingSearchCriteria ToImagingSearchCriteria(this searchCriteria criteria) =>
            new ImagingSearchCriteria
            {
                ContentSearchString = criteria.contentSearchString, WhereClause = criteria.whereClause,
                DocClass = criteria.docClass,
                Fields = criteria.fields,
                MaxResults = int.Parse(criteria.maxResults),
                SearchTimeoutSeconds = criteria.searchTimeoutSeconds
            };

        public static DateTime GetPropertyDate(this document doc, string propertyName)
        {
            return doc.properties.GetPropertyDate(propertyName);
        }

        public static DateTime?[] GetPropertyDateList(this property[] props, string propertyName)
        {
            //Match first on name, then DisplayName if name it does not exist.
            var prop =
                props.Where(p => p.name == propertyName)
                    .Union(props.Where(p => p.displayName == propertyName))
                    .ToArray();
            if (prop.Length == 0) return null;
            return prop[0].dateListValue;
        }
        public static DateTime GetPropertyDate(this property[] props, string propertyName)
        {
            //Match first on name, then DisplayName if name it does not exist.
            var prop =
                props.Where(p => p.name == propertyName)
                    .Union(props.Where(p => p.displayName == propertyName))
                    .ToArray();
            if (prop.Length == 0) return new DateTime();
            return prop[0].dateValue;
        }

        public static DateTime EntryDate(this document doc)
        {
            DateTime ret = doc.GetPropertyDate(ImagingAccessBase.ScanDate);
            if (ret.Ticks == 0)
                ret = doc.GetPropertyDate(ImagingAccessBase.EntryDate);
            if (ret.Ticks == 0)
                ret = doc.GetPropertyDate("DateCreated");
            return ret;
        }

        /// <summary>
        /// Creates text debugging information for a <see cref="document"/>.
        /// </summary>
        /// <param name="doc">The <see cref="document"/>.</param>
        /// <returns>Text information on the document.</returns>
        public static string DocumentDebugText(document doc)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Doc Class: {0}\r\nFolder Path = {1}\r\nGuid = {2}\r\nProperties:\r\n\t{3}\r\n\tContent Count= {4}\r\n\tEntryDate= {5}", doc.documentClass ?? "", doc.folderPath ?? "", doc.guid ?? "",
                String.Join("\r\n\t",
                    doc.properties.Select(
                        p =>
                            $"{p.name ?? ""}({p.displayName ?? "" ?? ""}) = {((p.name ?? "").IndexOf("date", StringComparison.InvariantCultureIgnoreCase) >= 0 ? p.dateValue.ToShortDateString() : p.value ?? "")}")),
                null == doc.contentList ? 0 : doc.contentList.Length, doc.EntryDate());
            return sb.ToString();
        }

        /// <summary>
        /// Creates text debugging information for a <see cref="document"/>.
        /// </summary>
        /// <param name="doc">The <see cref="document"/>.</param>
        /// <returns>Text information on the document.</returns>
        public static string DocumentDebugText(ImagingDocument doc)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Doc Class: {0}\r\nFolder Path = {1}\r\nGuid = {2}\r\nProperties:\r\n\t{3}", doc.DocumentClass ?? "", doc.FolderPath ?? "", doc.Guid ?? "",
                doc.Properties == null?"": string.Join("\r\n\t",
                    doc.Properties.Select(
                        p =>
                            $"{p.Name ?? ""}({p.DisplayName ?? "" ?? ""}) = {((p.Name ?? "").IndexOf("date", StringComparison.InvariantCultureIgnoreCase) >= 0 ? p.DateValue.ToShortDateString() : p.Value ?? "")}")));
            return sb.ToString();
        }
    }
}
