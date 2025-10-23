using HotChocolate.Subscriptions;
using James.Shared;
using James.Shared.Data;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        public Task<string> Search(string searchTerm,
            SearchOptions options,
            [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            //TODO: Implement real searches
            //HACK: Not sure why making the _rnd.Next calls return 0 when done in line
            int //actNumAmt = _rnd.Next(2, 10),
                //actNumDelay = _rnd.Next(100) + _rnd.Next(100),
                //actXNumAmt = _rnd.Next(0, 1),
                //actXNumDelay = _rnd.Next(25, 50) + _rnd.Next(200),
                //actXNameAmt = _rnd.Next(0, 1),
                //actXNameDelay = _rnd.Next(25, 100) + _rnd.Next(20),
                //bndNumberAmt = _rnd.Next(2, 4),
                //bndNumberDelay = _rnd.Next(20, 50) + _rnd.Next(20),
                actNameAmt = _rnd.Next(2, 4),
                actNameDelay = _rnd.Next(25, 100) + _rnd.Next(100);
            //var fakeAccountNameSearch = new FakeSearch(i => new JamesSearchResult
            //{
            //    Name = $"Abc{searchTerm}xyz Account001{i}",
            //    Confidence = 90,
            //    Entity = new LegalEntity
            //    {
            //        FullName = $"Abc{searchTerm}xyz Account001{i}",
            //        LegalEntityAddresses = new List<LegalEntityAddress>(1)
            //                { new () { Type = "Main", Address = GetFakeAddress(i+232) } },
            //        EntityType = "Account"
            //    },
            //    AccountNum = $"001{i}",
            //    SearchString = $"Abc{searchTerm}xyz Account001{i}",
            //    Type = SearchResultType.Account
            //},
            //    actNameAmt, actNameDelay, loggingService);
            //var fakeAccountExactNameSearch = new FakeSearch(i => new JamesSearchResult
            //{
            //    Name = $"Abc{searchTerm}xyz Account001{i}",
            //    Confidence = 110,
            //    Entity = new LegalEntity
            //    {
            //        FullName = $"Abc{searchTerm}xyz Account001{i}",
            //        LegalEntityAddresses = new List<LegalEntityAddress>(1)
            //                { new () { Type = "Main", Address = GetFakeAddress(i+232) } },
            //        EntityType = "Account"
            //    },
            //    AccountNum = $"001{i}",
            //    SearchString = searchTerm + " fake account",
            //    Type = SearchResultType.Account
            //},
            //    actXNameAmt, actXNameDelay, loggingService);
            var fakePeopleSearch = new FakeSearch(i => new JamesSearchResult
            {
                Name = $"John \"{searchTerm}\" {(char)(i + 65)} Smith",
                Confidence = 90,
                AccountNum = $"people{i}",
                Entity = new LegalEntity
                {
                    FullName = $"J{(char)(i + 65)}S Agency, inc",
                    LegalEntityAddresses = new List<LegalEntityAddress>(1)
                            { new () { Type = "Main", Address = GetFakeAddress(i+232) } },
                    EntityType = "Agency"
                },
                SearchString = searchTerm + " fake person",
                Type = SearchResultType.Account
            },
                actNameAmt, actNameDelay, loggingService);
            //var fakeBondNumberSearch = new FakeSearch(i => new JamesSearchResult
            //{
            //    Name = $"Bond No. {i}{searchTerm}",
            //    Confidence = searchTerm.Length switch
            //    {
            //        3 => 40,
            //        4 => 50,
            //        5 => 70,
            //        6 => 85,
            //        7 => 100,
            //        8 => 115,
            //        9 => 130,
            //        10 => 140,
            //        11 => 165,
            //        12 => 170,
            //        _ => 30
            //    },
            //    Entity = new LegalEntity()
            //    {
            //        FullName = "Fake Account #" + i,
            //    },
            //    BondList = new List<Bond>
            //    {
            //        new Bond
            //        {
            //            BondNumber = $"{i}{searchTerm}",
            //            BondType = new BondTypeDm(){BondType = "Commercial"},
            //            UnderWriter = GetFakeUnderwriter(i)
            //        }
            //    },
            //    SearchString = $"{i}{searchTerm}",
            //    Type = SearchResultType.Bond
            //}, bndNumberAmt, bndNumberDelay, loggingService);

            var activeOnly = options.ActiveOnly;
            //Adding in SearchOptions filter
            Task<List<JamesSearchResult>>[] stringSearches =
            [
                //fakeAccountNameSearch.GetResults(searchTerm),
                //fakeAccountExactNameSearch.GetResults(searchTerm),
                options.People ? fakePeopleSearch.GetResults(searchTerm) : NoResults,
                options.Account
                    ? AccountNameSearch(searchTerm, activeOnly, contextFactory)
                    : NoResults,
                options.Agency
                    ? AgencySearch(searchTerm, activeOnly, contextFactory)
                    : NoResults
            ];
            Task<List<JamesSearchResult>>[] numberSearches = [
                options.Account? AccountNumberSearch(searchTerm, activeOnly, contextFactory):NoResults
            ];
            var searches = new List<Task<List<JamesSearchResult>>>(stringSearches);
            if (options.Bond &&
                    searchTerm.Length >= 5 && StandardRegularExpressions.BondNumberPattern.IsMatch(searchTerm))
                //Only do bond searches on 5 characters or more that match the bond number pattern
                searches.Add(BondNumberSearch(searchTerm, activeOnly, contextFactory));
            if (searchTerm.IsDigitsOnly())
            {
                //If search term is a number, run searches on numeric fields first
                searches.InsertRange(0, numberSearches);
            }
            Task.Run(() =>
            {
                //TODO: Implement for real
                RunSearches(searchTerm, eventSender, searches.ToArray());
            });
            return Task.FromResult(searchTerm);
        }

        private Task<List<JamesSearchResult>> NoResults => Task.FromResult(new List<JamesSearchResult>());

        private async void RunSearches(string searchTerm, ITopicEventSender eventSender, params Task<List<JamesSearchResult>>[] searches)
        {
            var searchOperations = searches.Select<Task<List<JamesSearchResult>>, Task>(async srch =>
                {

                    var result = await srch;
                    if (result.Any())
                        await eventSender.SendAsync("Srch_" + searchTerm, new SubscriptionResult<List<JamesSearchResult>>
                        {
                            Identifier = searchTerm,
                            Result = result
                        }
                        ).AsTask();
                }
            ).ToArray();
            await Task.WhenAll(searchOperations.ToArray());
        }

        #region Fake data for early testing

        //private string GetFakeName(int i, bool isPerson)
        //{
        //    string[] firstNames = ["Olivia", "Emma", "Liam", "Noah", "Oliver", "Jacob", "Joshua", "Sam"];
        //    string[] lastNames = ["Smith", "Johnson", "Brown", "Jones", "Garcia", "Davis", "Lopez"];
        //    string[] companyType = ["Incorporated", "Detective Agency", "Accounting", "Law", "Manufacturing"];
        //    return isPerson ? $"{firstNames[i % firstNames.Length]} {lastNames[i % lastNames.Length]}" : $"{firstNames[i % firstNames.Length]} {lastNames[i % lastNames.Length]} {companyType[i % companyType.Length]}";
        //}
        private Address GetFakeAddress(int i)
        {
            string[] streetNames = ["Spring", "Douglas", "114th", "Willow", "University", "Grand"];
            string[] streetTypes = ["St", "Ave", "Blvd", "Cir", "Pkwy"];
            string[] cityNames = ["Franklin", "Washington", "Greenville", "Bristol", "Clinton", "Fairview", "Salem"];
            string[] states = ["IA", "HI", "NC", "SD", "NE", "IL", "CA"];
            return new Address
            {
                Address1 = $"{_rnd.Next(1000, 9999):D4} {streetNames[i % streetNames.Length]} {streetTypes[i % streetTypes.Length]}",
                City = cityNames[i % cityNames.Length],
                StateCode = states[i % states.Length],
                PostalCode = _rnd.Next(01000, 99999).ToString("D5")
            };
        }

        //private Underwriter GetFakeUnderwriter(int i)
        //{
        //    return new Underwriter()
        //    {
        //        IdNavigation = new Employee
        //        {
        //            Initials = $"{(char)(i + 65)}{(char)(90 - 2 * i)}{(char)(2 * i + 70)}"
        //        }
        //    };
        //}

        private class FakeSearch(Func<int, JamesSearchResult> makeFakeResult, int itemsToReturn, int delayMs, ILoggingService loggingService)
        {
            public Func<int, JamesSearchResult> MakeFakeResult { get; set; } = makeFakeResult;
            public int ItemsToReturn { get; set; } = itemsToReturn;
            public int DelayMs { get; set; } = delayMs;

            private JamesSearchResult SetSearchTerm(JamesSearchResult result, string searchTerm)
            {
                result.SearchString = searchTerm;
                return result;
            }

            public async Task<List<JamesSearchResult>> GetResults(string searchTerm)
            {
                await Task.Delay(DelayMs);
                try
                {
                    return Enumerable.Range(0, ItemsToReturn).Select(i => SetSearchTerm(MakeFakeResult(i), searchTerm)).ToList();
                }
                catch (Exception ex)
                {
                    loggingService.LogException(ex, "Exception generating fake results");
                    throw;
                }
            }
        }

        #endregion
        //TODO: Unit Test

        private async Task<List<JamesSearchResult>> BondNumberSearch(string searchString, bool activeOnly, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var matchingBonds = await SearchBondsByBondNumber(searchString, activeOnly, contextFactory);
            var results = matchingBonds.Select(bnd =>
                new JamesSearchResult
                {
                    Name = "Bond No. " + bnd.BondNumber,
                    Confidence = bnd.BondNumber.Length == searchString.Length ?
                        //Exact match = 140 confidence
                        140 :
                        //Partial match:  Higher confidence on longer search terms, lower confidence the longer the bond number is. 
                        90 - bnd.BondNumber.Length + searchString.Length * 3
                        //Rank matches at the beginning of the bond number higher
                        - bnd.BondNumber.IndexOf(searchString, StringComparison.Ordinal),
                    Entity = bnd.AccountNumNavigation.IdNavigation,
                    SearchString = bnd.BondNumber,
                    Type = SearchResultType.Bond,
                    BondList = [bnd]
                }).ToList();
            if (results.Count > JamesSearchResults.MaximumSearchResults)
            {
                //Prune results before sending a silly amount of results.
                results.Sort(SearchResultComparer.Instance);
                results.RemoveRange(JamesSearchResults.MaximumSearchResults, results.Count - JamesSearchResults.MaximumSearchResults);
            }

            return results;
        }

        private async Task<List<JamesSearchResult>> AccountNumberSearch(string searchString, bool activeOnly, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var matchingAccounts = await SearchAccountsByAccountNumber(searchString, activeOnly, contextFactory);
            return matchingAccounts.Select(acct =>
                new JamesSearchResult
                {
                    Name = acct.IdNavigation.FullName,
                    Confidence = acct.AccountNum.Length == searchString.Length ? 140 :
                        100 - acct.AccountNum.Length + searchString.Length * 2,
                    Entity = acct.IdNavigation,
                    AccountNum = acct.AccountNum,
                    SearchString = acct.AccountNum,
                    Type = SearchResultType.Account
                }).ToList();
        }

        private async Task<List<JamesSearchResult>> AccountNameSearch(string searchString, bool activeOnly, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var matchingAccounts = await SearchAccountsByName(searchString, activeOnly, contextFactory);
            return matchingAccounts.Select(acct =>
                new JamesSearchResult
                {
                    Name = acct.IdNavigation.FullName,
                    Confidence = acct.IdNavigation.FullName.Length == searchString.Length ? 160 :
                        110 - acct.IdNavigation.FullName.Length + searchString.Length * 2,
                    Entity = acct.IdNavigation,
                    AccountNum = acct.AccountNum,
                    SearchString = acct.IdNavigation.FullName,
                    Type = SearchResultType.Account
                }).ToList();
        }

        private async Task<List<JamesSearchResult>> AgencySearch(string searchString, bool activeOnly,
            IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            throw new NotImplementedException();
            // ToDo: Check this with the team.  It should be re-done.
            
            /*var matchingAgencies = await SearchAgencies(searchString, activeOnly, contextFactory);
            return matchingAgencies.Select(agency =>
                new JamesSearchResult
                {
                    Name = agency.IdNavigation.FullName,
                    Confidence = agency.AgencyNumber.Contains(searchString) ? (agency.AgencyNumber.Length == searchString.Length ? 140 :
                        100 - agency.AgencyNumber.Length + searchString.Length * 2) :
                    (agency.IdNavigation.FullName.Length == searchString.Length ? 160 : 110 - agency.IdNavigation.FullName.Length + searchString.Length * 2),
                    Entity = agency.IdNavigation,
                    AgencyNumber = agency.AgencyNumber,
                    SearchString = agency.AgencyNumber.Contains(searchString) ? searchString : agency.IdNavigation.FullName,
                    Type = SearchResultType.Agency
                }).ToList();*/
        }
    }
}
