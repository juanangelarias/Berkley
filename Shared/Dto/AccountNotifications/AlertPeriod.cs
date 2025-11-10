using System.ComponentModel.DataAnnotations;

namespace James.Shared.Dto;

public enum AlertPeriod
{
    [Display(Description ="Last 30 Days")]
    Last30Days,
    [Display(Description="Last 3 months")]
    Last3Months,
    [Display(Description="Last 6 months")]
    Last6Months,
    [Display(Description="Last Year")]
    LastYear
}