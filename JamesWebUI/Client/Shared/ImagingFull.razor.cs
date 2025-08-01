using ClientBusinessLogic;
using James.Shared.Data;
using James.Shared.Imaging;
using James.Shared.Model;

namespace JamesWebUI.Client.Shared
{
    public partial class ImagingFull
    {
        List<ImagingTypeDocuments> _currentTypeDocuments = new();
        List<ImagingTabTypeDocuments> _currentTabTypeDocuments = new();
        List<VImagingCategoryTabDivisionType> _currentCatTabDivTypes = new();
        List<ImagingType> _imagingTypes = null!;
        List<ImagingDocumentDetails> _uploadFiles = new();
        bool _waitingForFileSelection = true, _uploadFailed, _docsLoading = true;

        private async Task LoadAll()
        {
            IDataAccessResult<List<VImagingCategoryTabDivisionType>> docCategoryTabDivisionTypeResult = null!;
            IDataAccessResult<List<ImagingType>> docTypesResult = null!;
            IDataAccessResult<List<ImagingDocument>> docsResult = null!;
            var loadDocCategoryTabDivisionType = new LoadItem()
            {
                Key = "GetAllImagingCategoryTabDivisionTypes",
                AsyncLoadTask = (async () =>
                {
                    docCategoryTabDivisionTypeResult = await DataAccess.GetAllImagingCategoryTabDivisionTypes();
                }),
                CacheLoadTask = (cache) => docCategoryTabDivisionTypeResult = new DataAccessResult<List<VImagingCategoryTabDivisionType>> { Data = (List<VImagingCategoryTabDivisionType>)cache! },
                ResultVariable = () => docCategoryTabDivisionTypeResult,
                AfterLoad = () => _currentCatTabDivTypes =
                    //Use business logic to determine which tabs and types are relevant to the page
                    docCategoryTabDivisionTypeResult.Data!.GetRelevantTabsAndTypes(DocumentCategory, DivisionCode)
            };
            var loadDocTypes = new LoadItem()
            {
                Key = "GetAllImagingTypes",
                AsyncLoadTask = async () => { docTypesResult = await DataAccess.GetAllImagingTypes(); },
                CacheLoadTask = (cache) => docTypesResult
                    = new DataAccessResult<List<ImagingType>> { Data = (List<ImagingType>)cache! },
                ResultVariable = () => docTypesResult,
                AfterLoad = () => _imagingTypes = docTypesResult.Data!
            };
            var loadDocs = new LoadItem()
            {
                Key = $"SearchDocuments{ImagingId}{DocumentCategory.DocumentCategory()}",
                AsyncLoadTask = async () =>
                {
                    docsResult = await DataAccess.SearchDocuments(ImagingId, DocumentCategory);
                },
                CacheLoadTask = (cache) => docsResult
                    = new DataAccessResult<List<ImagingDocument>> { Data = (List<ImagingDocument>)cache! },
                ResultVariable = () => docsResult
            };

            await DataAccess.ParallelGetCacheOrDataAsync(() =>
                PopulateDocuments(docsResult), 
                AddEventNotify(loadDocCategoryTabDivisionType, "imaging categories, types and divisions"),
                AddEventNotify(loadDocTypes, "imaging document types"),
                AddEventNotify(loadDocs, "imaging document data"));
        }

        private ImagingType _unknownType = ImagingRules.CreateUnknownType();

        private void PopulateDocuments(IDataAccessResult<List<ImagingDocument>> documentsResult)
        {
            if (documentsResult.Success)
            {
                var documents = documentsResult.Data!;
                //Add unknown types, if needed.
                var allTypes = new HashSet<string>(_imagingTypes.Select(t => t.Type).Distinct());
                var allDocs = documents!.ToList();
                var unknownTypes = allDocs.Select(d => d.DocumentType).Distinct()
                    .Where(ddt => null == ddt || !allTypes.Contains(ddt)).ToList();
                if (unknownTypes.Any())
                {
                    LoggingService.LogInformation("Non-standard document types found in imaging",
                        "Document types do not match standard types in database",
                        category: StandardLoggingCategories.Imaging,
                        data: new Dictionary<string, string>
                        {
                            { "Document Category", DocumentCategory.Name() },
                            { "ImagingId", ImagingId },
                            { "DivisionCode", DivisionCode ?? "NULL" }
                        });
                    var unknownTypeCat = ImagingRules.CreateUnknownType(DocumentCategory, division: DivisionCode);
                    if (!_imagingTypes.Contains(_unknownType))
                        _imagingTypes.Add(_unknownType);
                    _currentCatTabDivTypes.Add(unknownTypeCat);
                    var docsWithUnknownType = allDocs.Where(d => unknownTypes.Contains(d.DocumentType)).ToList();
                    foreach (var unknownTypeDoc in docsWithUnknownType)
                        unknownTypeDoc.DocumentType = unknownTypeCat.Type;
                }

                //Split documents into types
                var grouped = from doc in documents
                              group doc by doc.DocumentType
                    into docCategory
                              select new ImagingTypeDocuments
                              {
                                  Type = _imagingTypes.SingleOrDefault(it => it.Type == docCategory.Key) ??
                                         new ImagingType { Type = docCategory.Key, Description = "Not in DB" },
                                  Documents = docCategory.ToList()
                              };
                var typeDocuments = grouped.ToDictionary(g => g.Type, v => v.Documents);

                //Build data structure for display of documents
                _currentTabTypeDocuments.Clear();
                var tabs = from ctdt in _currentCatTabDivTypes
                           group ctdt by ctdt.TabName
                    into tabTypes
                           select tabTypes; //Get list of tabs

                var stage1 = (from tt in tabs
                              select new
                              {
                                  TabName = tt.Key,
                                  Types = tt.Select(t => _imagingTypes.Single(it => it.Type == t.Type)).ToList()
                              }).ToList(); //Get list of types in each tab

                var stage2 = (from tabType in stage1
                              select (new ImagingTabTypeDocuments()
                              {
                                  TabName = tabType.TabName,
                                  TypeDocuments = (
                                      from ty in tabType.Types
                                      where typeDocuments.ContainsKey(ty)
                                      select new ImagingTypeDocuments
                                      {
                                          Type = ty,
                                          Documents = typeDocuments[ty]
                                      }
                                  ).Where(itd => itd.Documents.Any()).ToList()
                              }))//Combine documents for each tab and type
                    .Where(tt => tt.TypeDocuments.Any(td => td.Documents.Any()));
                _currentTabTypeDocuments.AddRange(stage2);

                _docsLoading = false;
                StateHasChanged();
            }
            else
            {
                //Should never hit this since success should have been checked before calling.
                NotifyLoadError(documentsResult.Errors, "documents");
            }
        }

        private string UploadUrl
        {
            get
            {
                switch (_uploadFiles.Count)
                {
                    case 0:
                        return "/";
                    //case 1:
                    //    return
                    //        $"ImagingRepository/UploadDocument/{_uploadFiles.First().DocumentType}/{(int)DocumentCategory}/{ImagingId}?description={_uploadFiles.First().Description}";
                    default:
                        return $"ImagingRepository/UploadDocuments/{string.Join(':', _uploadFiles.Select(uf => uf.DocumentType))}/{(int)DocumentCategory}/{ImagingId}?descriptions={string.Join(':', _uploadFiles.Select(uf => uf.Description))}";
                }
            }
        }

        private class ImagingDocumentDetails
        {
            public required string FileName { get; set; }
            public string Description { get; set; } = "";
            public required string DocumentType { get; set; }
            public VImagingCategoryTabDivisionType DocumentCategoryType { get; set; } = new();
        }

        private class ImagingTypeDocuments
        {
            public required ImagingType Type { get; set; }
            public required List<ImagingDocument> Documents { get; set; }
        }

        private class ImagingTabTypeDocuments
        {
            public required string TabName { get; set; }
            public required List<ImagingTypeDocuments> TypeDocuments { get; set; }
        }

        private readonly List<ImagingTypeDocuments> _fakeData = [
            new ImagingTypeDocuments{ Type = new ImagingType() { Type = "AA", Description = "Category1" }, Documents = [
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File1.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File2.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File3.xls"}
            ]}]},
            new ImagingTypeDocuments{ Type = new ImagingType() { Type = "AB", Description = "Category2" } , Documents = [
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File1.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File2.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File3.xls"}
            ]}]},
            new ImagingTypeDocuments{ Type = new ImagingType() { Type = "CC", Description = "Category3" } , Documents = [
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File1.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File2.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File3.xls"}
            ]}]},
            new ImagingTypeDocuments{ Type = new ImagingType() { Type = "DD", Description = "Category4" } , Documents = [
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File1.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File2.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File3.xls"}
            ]}]},
            new ImagingTypeDocuments{ Type = new ImagingType() { Type = "EE", Description = "Category5" } , Documents = [
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File1.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File2.doc"}
            ]},
            new ImagingDocument(){EntryDate = DateTime.Now.AddMonths(-1),
            Properties = [new ImagingProperty{Name="DocRemarks", Value="File3.xls"}
            ]}]}];
    }
}
