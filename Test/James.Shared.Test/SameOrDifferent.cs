using James.Shared.Model;

namespace James.Shared.Test
{
    public class SameOrDifferentTests
    {
        [Theory]
        [MemberData(nameof(MatchingSimplePairs))]
        public void SimpleToSimpleMatches(object source, object target)
        {
            var result = SameOrDifferent.Compare(source, target);
            Assert.True(result.AreTheSame);
        }
        [Theory]
        [MemberData(nameof(NonMatchingSimplePairs))]
        public void SimpleToSimpleNonMatches(object source, object target)
        {
            var result = SameOrDifferent.Compare(source, target);
            Assert.False(result.AreTheSame);
        }

        [Fact]
        public void AddressCompare()
        {
            var address1 = new Address()
            {
                Address1 = "A1A1",
                Address2 = "A1A2",
                Address3 = "A1A3",
                City = "A1C",
                StateCode = "AL",
                PostalCode = "12345"
            };
            var address2 = new Address()
            {
                Address1 = "A2A1",
                Address2 = "A2A2",
                Address3 = "A2A3",
                City = "A2C",
                StateCode = "AL",
                PostalCode = "12345"
            };
            var result = SameOrDifferent.Compare(address1, address2);
            Assert.False(result.AreTheSame);
            Assert.Contains(nameof(address1.Address1), result.DifferentProperties);
            Assert.Contains(nameof(address1.Address2), result.DifferentProperties);
            Assert.Contains(nameof(address1.Address3), result.DifferentProperties);
            Assert.Contains(nameof(address1.City), result.DifferentProperties);
            Assert.Contains(nameof(address1.StateCode), result.SameProperties);
            Assert.Contains(nameof(address1.PostalCode), result.SameProperties);
            //Not set on either, defaults should match
            Assert.Contains(nameof(address1.Id), result.SameProperties);
            Assert.Contains(nameof(address1.Address2), result.Differences.Keys);
            Assert.Equal(address1.Address2, result.Differences[nameof(address1.Address2)].SourceValue);
            Assert.Equal(address2.Address2, result.Differences[nameof(address1.Address2)].TargetValue);
        }

        [Fact]
        public void AddressExtendedAddress()
        {

            var address1 = new Address()
            {
                Address1 = "A1A1",
                Address2 = "A1A2",
                Address3 = "A1A3",
                City = "A1C",
                StateCode = "AL",
                PostalCode = "12345"
            };
            var address2 = new ExpandedAddress()
            {
                Address1 = "A2A1",
                Address2 = "A2A2",
                Address3 = "A2A3",
                City = "A2C",
                StateCode = "AL",
                PostalCode = "12345"
            };
            var result = SameOrDifferent.Compare(address1, address2);
            Assert.False(result.AreTheSame);
            Assert.Contains(nameof(address1.Address1), result.DifferentProperties);
            Assert.Contains(nameof(address1.Address2), result.DifferentProperties);
            Assert.Contains(nameof(address1.Address3), result.DifferentProperties);
            Assert.Contains(nameof(address1.City), result.DifferentProperties);
            Assert.Contains(nameof(address1.StateCode), result.SameProperties);
            Assert.Contains(nameof(address1.PostalCode), result.SameProperties);
            //Not set on either, defaults should match
            Assert.Contains(nameof(address1.Id), result.SameProperties);

            Assert.Contains(nameof(address2.ExtraGuid), result.TargetOnlyProperties);
            Assert.Contains(nameof(address2.ExtraId), result.TargetOnlyProperties);

            //Reverse and test
            result = SameOrDifferent.Compare(address2, address1);

            Assert.False(result.AreTheSame);
            Assert.Contains(nameof(address1.Address1), result.DifferentProperties);
            Assert.Contains(nameof(address1.Address2), result.DifferentProperties);
            Assert.Contains(nameof(address1.Address3), result.DifferentProperties);
            Assert.Contains(nameof(address1.City), result.DifferentProperties);
            Assert.Contains(nameof(address1.StateCode), result.SameProperties);
            Assert.Contains(nameof(address1.PostalCode), result.SameProperties);
            //Not set on either, defaults should match
            Assert.Contains(nameof(address1.Id), result.SameProperties);

            Assert.Contains(nameof(address2.ExtraGuid), result.SourceOnlyProperties);
            Assert.Contains(nameof(address2.ExtraId), result.SourceOnlyProperties);

            var address3 = ThisToThat.ToEntityType<ExpandedAddress>(address1);
            var address4 = ThisToThat.ToEntityType<Address>(address3);
            result = SameOrDifferent.Compare(address3, address4);
            Assert.True(result.AreTheSame);
        }

        [Fact]
        public void CompareToNull()
        {
            var address1 = new Address()
            {
                Address1 = "A1A1",
                Address2 = "A1A2",
                Address3 = "A1A3",
                City = "A1C",
                StateCode = "AL",
                PostalCode = "12345"
            };
            var result = SameOrDifferent.Compare(address1, null);
            Assert.False(result.AreTheSame);
            Assert.Contains("", result.DifferentProperties);
        }

        public static IEnumerable<object?[]> MatchingSimplePairs =>
        [
            [null!, null!], [1, 1], ["A", "A"], [1.1, 1.1], ['B', 'B'], [
                new DateTime(2000,
                    1,
                    1),
                new DateTime(2000,
                    1,
                    1)
            ]
        ];
        public static IEnumerable<object?[]> NonMatchingSimplePairs =>
        [
            [null!, 1], [1, "A"], ["A", 1.1], [1.1, 'B'], ['B',new DateTime(2000,
                    1,
                    1)] , [
                new DateTime(2000,
                    1,
                    1),null
                
            ], [1,2], ['C', 'D'], [1.1,1.01], ["Chicken","Horse"],[
                new DateTime(2000,
                    1,
                    1),
                new DateTimeOffset( new DateTime(2000,
                    1,
                    1))]
        ];
    }

    internal class ExpandedAddress : Address
    {
        private static int _extraId;
        public Guid ExtraGuid { get; set; } = Guid.NewGuid();

        public int ExtraId
        {
            get
            {
                Interlocked.Increment(ref _extraId);
                return _extraId;
            }
        }
    }
}
