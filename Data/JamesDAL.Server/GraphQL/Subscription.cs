using HotChocolate.Types;

namespace James.Data.Server.GraphQL
{
    public partial class Subscription
    {
        [Subscribe]
        [Topic(nameof(Subscription.OnAddressModified))]
        public Address OnAddressModified([EventMessage] Address address) => address;

        [Subscribe]
        [Topic(nameof(Subscription.OnLicenseModified))]
        public AgencyLicense OnLicenseModified([EventMessage] AgencyLicense license) => license;
    }
}
