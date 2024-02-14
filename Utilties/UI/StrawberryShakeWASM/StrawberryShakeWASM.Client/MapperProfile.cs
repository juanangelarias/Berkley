using AutoMapper;
using James.Shared.Model;
using System.Diagnostics;

namespace StrawberryShakeWASM.Client
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            Debug.WriteLine("Constructing MapperProfile");

            CreateMap<DateTimeOffset, DateTime>().ConvertUsing(typeof(DateTimeDateTimeOffsetConverter));
            CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing(typeof(DateTimeDateTimeOffsetNullableConverter));
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityAddresses_Address_StateCodeNavigation_CountryCodeNavigation, CountryDm>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityAddresses_Address_StateCodeNavigation, James.Shared.Model.State>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityAddresses_Address, Address>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityAddresses, LegalEntityAddress>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityPhones_PhoneNumber, PhoneNumber>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityPhones, LegalEntityPhone>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntityEmails, LegalEntityEmail>();
            CreateMap<IGetAccountByAccountNumber_Account_Agent_Agent, Agent>();
            CreateMap<IGetAccountByAccountNumber_Account_Agent, Agent>();
            CreateMap<IGetAccountByAccountNumber_Account_AgencyNumberNavigation_IdNavigation_LegalEntityAddresses_Address_Address, Address>();
            CreateMap<IGetAccountByAccountNumber_Account_AgencyNumberNavigation_IdNavigation_LegalEntityAddresses_Address, Address>();
            CreateMap<IGetAccountByAccountNumber_Account_AgencyNumberNavigation_IdNavigation_LegalEntityAddresses, LegalEntityAddress>();
            CreateMap<IGetAccountByAccountNumber_Account_HomeOfficeReviewByNavigation, Employee>();
            CreateMap<IGetAccountByAccountNumber_Account_BranchReviewByNavigation, Employee>();
            CreateMap<IGetAccountByAccountNumber_Account_Attorney_IdNavigation, LegalEntity>();
            CreateMap<IGetAccountByAccountNumber_Account_Attorney, LawEntity>();
            CreateMap<IGetAccountByAccountNumber_Account_Underwriter, Underwriter>();
            CreateMap<IGetAccountByAccountNumber_Account_AgencyNumberNavigation_IdNavigation, LegalEntity>();
            CreateMap<IGetAccountByAccountNumber_Account_AgencyNumberNavigation, Agency>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation_LegalEntity, LegalEntity>();
            CreateMap<IGetAccountByAccountNumber_Account_IdNavigation, LegalEntity>();
            CreateMap<IGetAccountByAccountNumber_Account, Account>();
        }
    }

    public class DateTimeDateTimeOffsetConverter : IValueConverter<DateTimeOffset, DateTime>
    {
        public DateTime Convert(DateTimeOffset sourceMember, ResolutionContext context) => sourceMember.DateTime;

    }
    public class DateTimeDateTimeOffsetNullableConverter : IValueConverter<DateTimeOffset?, DateTime?>
    {
        public DateTime? Convert(DateTimeOffset? sourceMember, ResolutionContext context) => sourceMember?.DateTime;

    }
}
