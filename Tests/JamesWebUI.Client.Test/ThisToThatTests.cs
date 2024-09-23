using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Model;

namespace JamesWebUI.Client.Test
{
    public class ThisToThatTests
    {

        [Fact]
        public void Test1()
        {
            var acct = GetFakeAccount();
            var gqlAcct = MakeIntoGraphQLObject(acct);
            var entityForm = ThisToThat.ToEntityType<Account>(gqlAcct);

            Assert.NotNull(entityForm);
            Assert.Equal(acct.AccountNum, entityForm.AccountNum);
            Assert.Equal(acct.Bank, entityForm.Bank);
            Assert.Equal(acct.IdNavigation.FullName, entityForm.IdNavigation.FullName);
            Assert.Equal(acct.IdNavigation.LegalEntityPhones.Count, entityForm.IdNavigation.LegalEntityPhones.Count);
            Assert.Equal(acct.IdNavigation.LegalEntityEmails.Count, entityForm.IdNavigation.LegalEntityEmails.Count);
            MatchLegalAddresses(acct.IdNavigation.LegalEntityAddresses, entityForm.IdNavigation.LegalEntityAddresses);
            for (var i = 0; i < acct.IdNavigation.LegalEntityPhones.Count; i++)
            {
                Assert.Equal(acct.IdNavigation.LegalEntityPhones.ToArray()[i].Type, entityForm.IdNavigation.LegalEntityPhones.ToArray()[i].Type);
                Assert.Equal(acct.IdNavigation.LegalEntityPhones.ToArray()[i].PhoneNumber.MainNumber, entityForm.IdNavigation.LegalEntityPhones.ToArray()[i].PhoneNumber.MainNumber);
                Assert.Equal(acct.IdNavigation.LegalEntityPhones.ToArray()[i].PhoneNumber.Extension, entityForm.IdNavigation.LegalEntityPhones.ToArray()[i].PhoneNumber.Extension);
                Assert.Equal(acct.IdNavigation.LegalEntityPhones.ToArray()[i].PhoneNumber.CountryCode, entityForm.IdNavigation.LegalEntityPhones.ToArray()[i].PhoneNumber.CountryCode);
            }
            for (var i = 0; i < acct.IdNavigation.LegalEntityEmails.Count; i++)
            {
                Assert.Equal(acct.IdNavigation.LegalEntityEmails.ToArray()[i].Type, entityForm.IdNavigation.LegalEntityEmails.ToArray()[i].Type);
                Assert.Equal(acct.IdNavigation.LegalEntityEmails.ToArray()[i].EmailAddress, entityForm.IdNavigation.LegalEntityEmails.ToArray()[i].EmailAddress);
            }

            Assert.Equal(acct.Agent?.IdNavigation.FullName, entityForm.Agent?.IdNavigation.FullName);
            Assert.Equal(acct.AgencyNumberNavigation?.IdNavigation.FullName, entityForm.AgencyNumberNavigation?.IdNavigation.FullName);
            MatchLegalAddresses(acct.AgencyNumberNavigation!.IdNavigation.LegalEntityAddresses, entityForm.AgencyNumberNavigation!.IdNavigation.LegalEntityAddresses, true);
            Assert.Equal(acct.Division, entityForm.Division);
            Assert.Equal(acct.HomeOfficeReviewByNavigation?.FullName, entityForm.HomeOfficeReviewByNavigation?.FullName);
            Assert.Equal(acct.HomeOfficeReviewed, entityForm.HomeOfficeReviewed);
            Assert.Equal(acct.BranchReviewByNavigation?.FullName, entityForm.BranchReviewByNavigation?.FullName);
            Assert.Equal(acct.BranchReviewed, entityForm.BranchReviewed);
            Assert.Equal(acct.Attorney?.IdNavigation.FullName, acct.Attorney?.IdNavigation.FullName);
            Assert.Equal(acct.Attorney?.MartindaleHubbellRating, acct.Attorney?.MartindaleHubbellRating);
            Assert.Equal(acct.Underwriter?.IdNavigation.FullName, acct.Underwriter?.IdNavigation.FullName);
            Assert.Equal(acct.Underwriter?.IdNavigation.Initials, acct.Underwriter?.IdNavigation.Initials);
            Assert.Equal(acct.Underwriter?.IdNavigation.Title, acct.Underwriter?.IdNavigation.Title);
            Assert.Equal(acct.Underwriter?.IdNavigation.Email, acct.Underwriter?.IdNavigation.Email);
            Assert.Equal(acct.Underwriter?.ReportsTo, acct.Underwriter?.ReportsTo);
        }

        static void MatchLegalAddresses(ICollection<LegalEntityAddress> expected,
            ICollection<LegalEntityAddress> actual, bool cityStateOnly = false)
        {
            Assert.Equal(expected.Count, actual.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected.ToArray()[i].Address.City, actual.ToArray()[i].Address.City);
                Assert.Equal(expected.ToArray()[i].Address.StateCode, actual.ToArray()[i].Address.StateCode);
                if (cityStateOnly == false)
                {
                    Assert.Equal(expected.ToArray()[i].Address.Address1, actual.ToArray()[i].Address.Address1);
                    Assert.Equal(expected.ToArray()[i].Address.Address2, actual.ToArray()[i].Address.Address2);
                    Assert.Equal(expected.ToArray()[i].Address.Address3, actual.ToArray()[i].Address.Address3);
                    Assert.Equal(expected.ToArray()[i].Address.StateCodeNavigation?.CountryCode, actual.ToArray()[i].Address.StateCodeNavigation?.CountryCode);
                    Assert.Equal(expected.ToArray()[i].Address.StateCodeNavigation?.CountryCodeNavigation?.Name, actual.ToArray()[i].Address.StateCodeNavigation?.CountryCodeNavigation?.Name);
                    Assert.Equal(expected.ToArray()[i].Address.PostalCode, actual.ToArray()[i].Address.PostalCode);
                }
            }
        }

        static Random _rnd = new Random();
        Account GetFakeAccount()
        {
            var id = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            var phoneId = Guid.NewGuid();
            var agentId = Guid.NewGuid();
            var agencyId = Guid.NewGuid();
            var agencyNumber = _rnd.Next(1000, 9999).ToString();
            var accountNumber = _rnd.Next(10000, 99999).ToString();
            var attorneyId = Guid.NewGuid();
            var underwriterId = Guid.NewGuid();
            var acct = new Account
            {
                Id = id,
                Bank = "Fake Bank",
                IdNavigation = new LegalEntity
                {
                    Id = id,
                    FullName = "Fake TestCompany",
                    LegalEntityAddresses = new List<LegalEntityAddress>
                    {
                        new LegalEntityAddress
                        {
                            Type = "Main",
                            AddressId = addressId,
                            Address = new Address
                            {
                                Id=addressId,
                                Address1 = "123 Main St",
                                Address2 = "Suite 200",
                                City = "Ames",
                                StateCode = "IA",
                                StateCodeNavigation = new State
                                {
                                    Code = "IA",
                                    CountryCode = "US",
                                    CountryCodeNavigation =
                                        new CountryDm
                                        {
                                            Code = "US",
                                            Name = "United States"
                                        }
                                },
                                PostalCode = "50010"
                            }
                        }
                    },
                    LegalEntityPhones = new List<LegalEntityPhone>
                    {
                        new LegalEntityPhone
                        {
                            Type="Main",
                            PhoneNumberId = phoneId,
                            PhoneNumber = new PhoneNumber
                            {
                                Id = phoneId,
                                MainNumber = "5155551234",
                                Extension = "x1"
                            }
                        }
                    },
                    LegalEntityEmails = new List<LegalEntityEmail>
                    {
                        new LegalEntityEmail
                        {
                            Type = "Main",
                            EmailAddress = "Bob@bob.com"
                        }
                    }
                },
                AgentId = agentId,
                Agent = new Agent
                {
                    Id = agentId,
                    IdNavigation = new LegalEntity
                    {
                        Id = agentId,
                        FullName = "Allen the Agent"
                    }
                },
                AgencyNumber = agencyNumber,
                AgencyNumberNavigation = new Agency
                {
                    Id = agencyId,
                    IdNavigation = new LegalEntity
                    {
                        Id = agencyId,
                        FullName = "Agency of Awesome",
                        LegalEntityAddresses = new List<LegalEntityAddress>
                        {
                            new LegalEntityAddress
                            {
                                Type = "Main",
                                Address = new Address
                                {
                                    Address1 = "4000 Corporate Way",
                                    City = "Dallas",
                                    StateCode = "TX"
                                }
                            }
                        }
                    }
                },
                AccountNum = accountNumber,
                Division = "Commercial",
                HomeOfficeReviewed = new DateTime(2020, 2, 2, 2, 2, 2),
                HomeOfficeReviewByNavigation = new UserProfile
                {
                    FullName = "Chuck Schumer"
                },
                BranchReviewed = new DateTime(2021, 1, 1, 1, 1, 1),
                BranchReviewByNavigation = new UserProfile
                {
                    FullName = "Joe Branch"
                },
                AttorneyId = attorneyId,
                Attorney = new LawEntity
                {
                    Id = attorneyId,
                    IdNavigation = new LegalEntity
                    {
                        Id = attorneyId,
                        FullName = "Snidely Whiplash"
                    },
                    MartindaleHubbellRating = "12"
                },
                UnderwriterId = underwriterId,
                Underwriter = new Underwriter
                {
                    Id = underwriterId,
                    IdNavigation = new Employee
                    {
                        FullName = "Ursula Underwriter",
                        Initials = "UUU",
                        Title = "Underwriter",
                        Email = "UUU@berkeysurety.com"
                    },
                    ReportsTo = new Guid()
                }
            };
            return acct;
        }

       
    }
}
