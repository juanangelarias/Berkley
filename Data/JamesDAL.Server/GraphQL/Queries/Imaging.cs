//Uncomment below to have random exceptions thrown.  Set the frequency by setting ChaosFrequencyPercentage below
//TODO:  Move this compiler flag into the build for dev, int and tst
#define ChaosMonkey

using HotChocolate.Authorization;
using James.Data.Imaging;
using James.Shared.Imaging;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
#if ChaosMonkey
        /// <summary>
        /// Percentage of tries that should throw exceptions (0-100)
        /// </summary>
        private const int ChaosFrequencyPercentage = 20;
        private static readonly Random _rnd = new();

        /// <summary>
        /// Randomly throws a chaos monkey exception based on ChaosFrequencyPercentage
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ThrowIfChaos()
        {
            if (_rnd.Next(0, 100) <= ChaosFrequencyPercentage)
                throw new Exception("Chaos Monkey strikes again!!");
        }
#endif

        /// <summary>
        /// Returns document metadata from the imaging system for a given document category, document type and imaging id
        /// </summary>
        /// <param name="imagingId">the id of the object associated with the document category</param>
        /// <param name="docCategory">The document category to search</param>
        /// <param name="documentType">The document type to search for</param>
        /// <param name="contextFactory">database context factory</param>
        /// <param name="imagingAccess">Imaging access object</param>
        /// <returns></returns>
        [Authorize]
        public async Task<List<ImagingDocument>> SearchDocumentsAsync(string imagingId, ImagingDocumentCategory docCategory,
                                                                string? documentType,
                                                                [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
                                                                [Service] ServerImagingAccess imagingAccess)
        {
            var searchCriteria = await GetImagingSearchCriteria(imagingId, docCategory, contextFactory);
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
#if ChaosMonkey
            ThrowIfChaos();
#endif
            return await imagingAccess.SearchDocumentsAsync(criteria, searchOptions, additionalParams);
        }

        [Authorize]
        public async Task<ImagingDocument?> GetDocumentDetails(ImagingDocumentCategory docCategory, Guid documentId,
            [Service] ServerImagingAccess imagingAccess)
        {
            //TODO:  Cache these results
            var searchCriteria = new ImagingSearchCriteria
            {
                DocClass = docCategory.DocumentCategory(),
                WhereClause = $"Id = '{documentId}'",
                MaxResults = 1,
                Fields = string.Join(",", ImagingAccessBase.DocumentPropertyFields)
            };
            var results = await imagingAccess.SearchDocumentsAsync(searchCriteria);
            return results.SingleOrDefault();
        }

        [Authorize]
        public async Task<ImagingSearchCriteria> GetImagingSearchCriteria(string imagingId,
                                                                        ImagingDocumentCategory docCategory,
                                                                        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
                                                                        bool useDocCategoryAsCriteria = true)
        {
            imagingId = imagingId.Trim();
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
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{imagingId}'";
                    break;
                case ImagingDocumentCategory.Bond: //2
                    criteria.WhereClause = $"{ImagingAccessBase.PolicyNo} = '{imagingId}'";
                    var bidBondType = await GetBondRequestNumberType(imagingId, contextFactory);
                    //TODO: Handle errors above
                    if (null != bidBondType)
                        criteria.WhereClause =
                            $"({criteria.WhereClause} OR {(string.Equals(bidBondType.Type, "CONTRACT", StringComparison.InvariantCultureIgnoreCase) ? ImagingAccessBase.ContBidId : ImagingAccessBase.CommBidId)} = '{bidBondType.BondRequestNumber}')";
                    break;
                case ImagingDocumentCategory.Agency: //3
                    criteria.WhereClause = $"{ImagingAccessBase.AgencyNo} = '{imagingId}'";
                    break;
                case ImagingDocumentCategory.CommBid: //4
                    criteria.WhereClause = $"{ImagingAccessBase.CommBidId} = '{imagingId}'";
                    bondNumber = await GetBondNumber(imagingId, contextFactory);
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause = $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.ContBid: //5
                    criteria.WhereClause = $"{ImagingAccessBase.ContBidId} = '{imagingId}'";
                    bondNumber = await GetBondNumber(imagingId, contextFactory);
                    //TODO:Handle GraphQl errors
                    if (!string.IsNullOrWhiteSpace(bondNumber))
                        criteria.WhereClause = $"({criteria.WhereClause} OR {ImagingAccessBase.PolicyNo} = '{bondNumber}')";
                    break;
                case ImagingDocumentCategory.Billing: //SearchBillingDocuments
                    criteria.WhereClause = $"{ImagingAccessBase.AccountId} = '{imagingId}''";
                    break;
                default:
                    throw new ArgumentException("Invalid docCategory", nameof(docCategory));
            }
            return criteria;
        }

        [Authorize]
        public async Task<List<ImagingType>> GetAllImagingTypes(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
#if ChaosMonkey
            ThrowIfChaos();
#endif
            var ctx = await contextFactory.CreateDbContextAsync();
            return ctx.ImagingTypes.ToList();
        }

        [Authorize]
        public async Task<List<VImagingCategoryTabDivisionType>> GetAllImagingCategoryTabDivisionType(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
#if ChaosMonkey
            ThrowIfChaos();
#endif
            var ctx = await contextFactory.CreateDbContextAsync();
            return ctx.VImagingCategoryTabDivisionTypes.OrderBy(ctdt => ctdt.TabName).ThenBy(ctdt => ctdt.Type).ToList();
        }
    }
}
