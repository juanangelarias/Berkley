using HotChocolate.Subscriptions;
using James.Shared;
using James.Shared.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        public async Task<string> Search(string searchTerm, //TODO: Allow settings to limit results
            [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            //TODO: Implement real searches
            //HACK: Not sure why making the _rnd.Next calls return 0 when done in line
            int actNumAmt = _rnd.Next(2, 10),
                actNumDelay = _rnd.Next(100) + _rnd.Next(100),
                actXNumAmt = _rnd.Next(0, 1),
                actXNumDelay = _rnd.Next(25, 50) + _rnd.Next(200),
                actNameAmt = _rnd.Next(2, 10),
                actNameDelay = _rnd.Next(25, 100) + _rnd.Next(100),
                actXNameAmt = _rnd.Next(0, 1),
                actXNameDelay = _rnd.Next(25, 100) + _rnd.Next(20),
                bndNumberAmt = _rnd.Next(2, 10),
                bndNumberDelay = _rnd.Next(20, 50) + _rnd.Next(20);
            var fakeAccountNumberSearch = new FakeSearch(i => new JamesSearchResult
            {
                Name = $"Account001{i}",
                Confidence = 100,
                Entity = new LegalEntity
                {
                    FullName = $"Account001{i}",
                    LegalEntityAddresses = new List<LegalEntityAddress>(1)
                            { new () { Type = "Main", Address = GetFakeAddress(i) } },
                    EntityType = "Account"
                },
                SearchString = _rnd.Next(01000, 99999).ToString("D5"),
                Type = SearchResultType.Account
            },
            actNumAmt, actNumDelay, loggingService);
            //_rnd.Next(2, 10), _rnd.Next(100) + _rnd.Next(100));
            var fakeAccountNameSearch = new FakeSearch(i => new JamesSearchResult
            {
                Name = $"Abc{searchTerm}xyz Account001{i}",
                Confidence = 90,
                Entity = new LegalEntity
                {
                    FullName = $"Abc{searchTerm}xyz Account001{i}",
                    LegalEntityAddresses = new List<LegalEntityAddress>(1)
                            { new () { Type = "Main", Address = GetFakeAddress(i+232) } },
                    EntityType = "Account"
                },
                SearchString = $"Abc{searchTerm}xyz Account001{i}",
                Type = SearchResultType.Account
            },
                actNameAmt, actNameDelay, loggingService);
            //_rnd.Next(2, 20), _rnd.Next(25, 100) + _rnd.Next(200));
            var fakeAccountExactNumberSearch = new FakeSearch(i => new JamesSearchResult
            {
                Name = $"Account{searchTerm}",
                Confidence = 105,
                Entity = new LegalEntity
                {
                    FullName = $"Account{searchTerm}",
                    LegalEntityAddresses = new List<LegalEntityAddress>(1)
                            { new () { Type = "Main", Address = GetFakeAddress(i) } },
                    EntityType = "Account"
                },
                SearchString = _rnd.Next(01000, 99999).ToString("D5"),
                Type = SearchResultType.Account
            },
            actXNumAmt, actXNumDelay, loggingService);
            //_rnd.Next(0, 1), _rnd.Next(25, 100) + _rnd.Next(20));
            var fakeAccountExactNameSearch = new FakeSearch(i => new JamesSearchResult
                {
                    Name = $"Abc{searchTerm}xyz Account001{i}",
                    Confidence = 110,
                    Entity = new LegalEntity
                    {
                        FullName = $"Abc{searchTerm}xyz Account001{i}",
                        LegalEntityAddresses = new List<LegalEntityAddress>(1)
                            { new () { Type = "Main", Address = GetFakeAddress(i+232) } },
                        EntityType = "Account"
                    },
                    SearchString = searchTerm,
                    Type = SearchResultType.Account
                },
                actXNameAmt, actXNameDelay, loggingService);
            var fakePeopleSearch = new FakeSearch(i => new JamesSearchResult
                {
                    Name = $"John \"{searchTerm}\" {(char)(i+65)} Smith",
                    Confidence = 110,
                    Entity = new LegalEntity
                    {
                        FullName = $"J{(char)(i + 65)}S Agency, inc",
                        LegalEntityAddresses = new List<LegalEntityAddress>(1)
                            { new () { Type = "Main", Address = GetFakeAddress(i+232) } },
                        EntityType = "Agency"
                    },
                    SearchString = searchTerm,
                    Type = SearchResultType.Account
                },
                actNameAmt, actXNameDelay, loggingService);
            var fakeBondNumberSearch = new FakeSearch(i=> new JamesSearchResult
            {
                Name=$"Bond No. {i}{searchTerm}",
                Confidence =  searchTerm.Length switch
                {
                    3=> 40,
                    4=> 50,
                    5=> 70,
                    6=> 85,
                    7=> 100,
                    8 => 115,
                    9 => 130,
                    10 => 140,
                    11 => 165,
                    12 => 170,
                    _ => 30
                },
                Entity = new LegalEntity()
                {
                    FullName = "Fake Account #" + i,
                },
                BondList = new List<Bond>
                {
                    new Bond
                    {
                        BondNumber = $"{i}{searchTerm}",
                        BondType = new BondTypeDm(){BondType = "Commercial"},
                        UnderWriter = GetFakeUnderwriter(i)
                    }
                },
                SearchString = $"{i}{searchTerm}",
                Type = SearchResultType.Bond
            }, bndNumberAmt, bndNumberDelay, loggingService );
            Task<List<JamesSearchResult>>[] StringSearches = [
                fakeAccountNameSearch.GetResults(searchTerm),
                fakeAccountExactNameSearch.GetResults(searchTerm),
                fakePeopleSearch.GetResults(searchTerm)
            ];
            Task<List<JamesSearchResult>>[] NumberSearches = [
                fakeAccountNumberSearch.GetResults(searchTerm),
                fakeAccountExactNumberSearch.GetResults(searchTerm),
                fakeBondNumberSearch.GetResults(searchTerm)
            ];
            var searches = new List<Task<List<JamesSearchResult>>>(StringSearches);
            if (searchTerm.IsDigitsOnly())
            {
                //If search term is a number, run searches on numeric fields first
                searches.InsertRange(0, NumberSearches);
            }
            Task.Run(() =>
            {
                //TODO: Implement for real
                RunSearches(searchTerm, eventSender, searches.ToArray());
            });
            return searchTerm;
        }

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

        private string GetFakeName(int i, bool isPerson)
        {
            string[] firstNames = ["Olivia", "Emma", "Liam", "Noah", "Oliver", "Jacob", "Joshua", "Sam"];
            string[] lastNames = ["Smith", "Johnson", "Brown", "Jones", "Garcia", "Davis", "Lopez"];
            string[] companyType = ["Incorporated", "Detective Agency", "Accounting", "Law", "Manufacturing"];
            return isPerson ? $"{firstNames[i % firstNames.Length]} {lastNames[i % lastNames.Length]}" : $"{firstNames[i % firstNames.Length]} {lastNames[i % lastNames.Length]} {companyType[i % companyType.Length]}";
        }
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

        private Underwriter GetFakeUnderwriter(int i)
        {
            return new Underwriter()
            {
                IdNavigation = new Employee
                {
                    Initials = $"{(char)(i + 65)}{(char)(90 -2*i)}{(char)(2*i + 70)}"
                }
            };
        }
        private class FakeSearch(Func<int, JamesSearchResult> makeFakeResult, int itemsToReturn, int delayMs, ILoggingService loggingService)//TODO:Stopped here.  Make default constructor here.
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
    }
}
