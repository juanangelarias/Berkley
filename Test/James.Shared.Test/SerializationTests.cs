using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using James.Shared.Model;
using Xunit.Abstractions;

namespace James.Shared.Test
{
    public class SerializationTests
    {
        private readonly ITestOutputHelper output;
        public SerializationTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestJamesSearchResults()
        {
            var testObject = new JamesSearchResults
            {
                RootSearchTerm = "TestRoot",
                ResultItems = Enumerable.Range(0,5).Select(i=>new JamesSearchResult
                    {
                        Name = "Test Name "+i,
                        SearchString = "TestRoot"+i,
                        FromDescription = 0 == i%2,
                        Confidence = 50+i*5,
                        Entity = new LegalEntity
                        {
                            Id = Guid.NewGuid(),
                            FullName = "Test Name "+i,
                            LegalEntityAddresses = new List<LegalEntityAddress>
                            {
                                new LegalEntityAddress(){Address = new Address{Address1 = "123 Main St", City = "Omaha", StateCode = "NE", StateCodeNavigation = new State{Code="NE", CountryCode = "US"}}, Type = "Main"}
                            }
                        }
                }).ToList()
            };
            var jsonOptions = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
            var jsonString = JsonSerializer.Serialize(testObject, jsonOptions);
            output.WriteLine(jsonString);
            var clonedObject = JsonSerializer.Deserialize<JamesSearchResults>(jsonString, jsonOptions);
            Assert.NotNull(clonedObject);
            Assert.Equal(testObject.RootSearchTerm, clonedObject.RootSearchTerm);
            Assert.Equal(testObject.ResultItems.Count, clonedObject.ResultItems.Count);
        }
    }
}
