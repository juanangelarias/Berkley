using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ProfitCenter
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string ProfitCenter1 { get; set; } = null!;

    public string DivisionCode { get; set; } = null!;

    public string LineOfBusiness { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string LocationOverview { get; set; } = null!;

    public Guid BudgetDefault { get; set; }

    public Guid? UnderWriterId { get; set; }

    public DateOnly Effective { get; set; }

    public DateOnly Expiration { get; set; }

    public string? CommercialRegion { get; set; }

    public virtual ProfitCenter? BudgetDefaultNavigation { get; set; } = null!;

    public virtual CommercialRegionDm? CommercialRegionNavigation { get; set; }

    public virtual DivisionDm DivisionCodeNavigation { get; set; } = null!;

    public virtual ICollection<ProfitCenter> InverseBudgetDefaultNavigation { get; set; } = new List<ProfitCenter>();

    public virtual LineOfBusinessDm LineOfBusinessNavigation { get; set; } = null!;

    public virtual Underwriter? UnderWriter { get; set; }
}
