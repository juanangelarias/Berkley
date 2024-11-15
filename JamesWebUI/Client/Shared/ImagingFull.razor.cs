using ClientBusinessLogic;
using James.Shared;
using James.Shared.Data;
using James.Shared.Imaging;
using James.Shared.Model;
using Radzen;

namespace JamesWebUI.Client.Shared
{
    public partial class ImagingFull
    {
        private async Task LoadAll()
        {
            IDataAccessResult<List<VImagingCategoryTabDivisionType>> docCategoryTabDivisionTypeResult = null!;
            IDataAccessResult<List<ImagingType>> docTypesResult = null!;
            IDataAccessResult<List<ImagingDocument>> docsResult = null!;
            await LoadInParallel((async () => { docCategoryTabDivisionTypeResult = await DataAccess.GetAllImagingCategoryTabDivisionTypes(); }), async () => { docTypesResult = await DataAccess.GetAllImagingTypes(); }, async () => { docsResult = await DataAccess.SearchDocuments(ImagingId, DocumentCategory); });



            if (docCategoryTabDivisionTypeResult!.Success)
                //TODO:Consider what part of the below should move to business logic
                _currentCatTabDivTypes = docCategoryTabDivisionTypeResult.Data!
                    .Where(ctdt => (ctdt.Category == DocumentCategory.Name()
                        && (DocumentCategory != ImagingDocumentCategory.Account || ctdt.DivisionCode == DivisionCode)))
                    .ToList();
            else
            {
                //TODO:Handle Errors
                NotificationService.Notify(severity: NotificationSeverity.Warning, "Load failure of imaging Tabs and Types");
            }
            if (docTypesResult!.Success)
                _imagingTypes = docTypesResult.Data!;
            else
            {
                //TODO:Handle Errors
                NotificationService.Notify(severity: NotificationSeverity.Warning, "Load failure of imaging types");
            }
            PopulateDocuments(docsResult);
        }

        private ImagingType _unknownType = ImagingRules.CreateUnknownType();

        //private void PopulateDocuments(List<ImagingDocument> documents)
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

                var stage2 = from tabType in stage1
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
                                 ).ToList()
                             }); //Combine documents for each tab and type
                _currentTabTypeDocuments.AddRange(stage2);

                _docsLoading = false;
                StateHasChanged();
            }
            else
            {
                //TODO:  Log the error and figure out how to show to user.
                NotificationService.Notify(severity: NotificationSeverity.Warning, "Load failure of documents");
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
                    case 1:
                        return
                            $"ImagingRepository/UploadDocument/{_uploadFiles.First().DocumentType}/{(int)DocumentCategory}/{ImagingId}?descriptions={_uploadFiles.First().Description}";
                    default:
                        return $"ImagingRepository/UploadDocuments/{string.Join(':', _uploadFiles.Select(uf => uf.DocumentType))}/{(int)DocumentCategory}/{ImagingId}?filenames={string.Join(':', _uploadFiles.Select(uf => uf.FileName))}";
                }
            }
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
