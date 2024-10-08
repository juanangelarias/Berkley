using HotChocolate.Authorization;
using James.Data.Imaging;
using James.Shared.Imaging;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {

        [Authorize]
        public async Task<List<ImagingDocument>> SearchDocumentsAsync(string id, ImagingDocumentCategory docCategory,
                                                                string? documentType,
                                                                [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
                                                                [Service] ServerImagingAccess imagingAccess)
        {
            var searchCriteria = await GetImagingSearchCriteria(id, docCategory, contextFactory);
            //TODO: Handle errors
            if (null != documentType)
                searchCriteria.WhereClause += " AND " + ImagingAccessBase.DocType + " = '" + documentType + "'";

            var results = await SearchDocumentsAsync(searchCriteria, imagingAccess);
            return results;
        }

        [Authorize]
        public async Task<List<ImagingDocument>> SearchDocumentsAsync(ImagingSearchCriteria criteria,
                                                                [Service] ServerImagingAccess imagingAccess,
                                                                KeyValuePair<string, string>[]? searchOptions = null,
                                                                KeyValuePair<string, string>[]? additionalParams = null)
        {
            return await imagingAccess.SearchDocumentsAsync(criteria, searchOptions, additionalParams);
        }

        [Authorize]
        public async Task<ImagingSearchCriteria> GetImagingSearchCriteria(string id,
                                                                        ImagingDocumentCategory docCategory,
                                                                        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
                                                                        bool useDocCategoryAsCriteria = true)
        {
            id = id.Trim();
            var criteria = new ImagingSearchCriteria()
            {
                ContentSearchString = "",
                MaxResults = 2000,
                Fields = string.Join(",", ImagingAccessBase.DocumentPropertyFields)
            };
            if (useDocCategoryAsCriteria)
            {
                criteria.DocClass = docCategory.DocumentCategory();
            }

            string? bondNumber;
            switch (docCategory)
            {
                case ImagingDocumentCategory.Account: //1
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{id}'";
                    break;
                case ImagingDocumentCategory.Bond: //2
                    criteria.WhereClause = $"{ImagingAccessBase.PolicyNo} = '{id}'";
                    var bidBondType = await GetBondRequestNumberType(id, contextFactory);
                    //TODO: Handle errors above
                    if (null != bidBondType)
                        criteria.WhereClause =
                            $"({criteria.WhereClause} OR {(string.Equals(bidBondType.Type, "CONTRACT", StringComparison.InvariantCultureIgnoreCase) ? ImagingAccessBase.ContBidId : ImagingAccessBase.CommBidId)} = '{bidBondType.BondRequestNumber}')";
                    break;
                case ImagingDocumentCategory.Agency: //3
                    criteria.WhereClause = $"{ImagingAccessBase.AgencyNo} = '{id}'";
                    break;
                case ImagingDocumentCategory.CommBid: //4
                    criteria.WhereClause = $"{ImagingAccessBase.CommBidId} = '{id}'";
                    bondNumber = await GetBondNumber(id, contextFactory);
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause = $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.ContBid: //5
                    criteria.WhereClause = $"{ImagingAccessBase.ContBidId} = '{id}'";
                    bondNumber = await GetBondNumber(id, contextFactory);
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause = $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.Billing: //SearchBillingDocuments
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{id}''";
                    break;
                default:
                    throw new ArgumentException("Invalid docCategory", nameof(docCategory));
            }
            return criteria;
        }
    }
}
