using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondTransaction
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public int BondMod { get; set; }

    public int GroupNumber { get; set; }

    public string AccountNum { get; set; } = null!;

    public string? Type { get; set; }

    public string Branch { get; set; } = null!;

    public string? MidtermDescription { get; set; }

    public int? PaymentBondAmount { get; set; }

    public int Premium { get; set; }

    public int BondAmount { get; set; }

    public int? ContractAmount { get; set; }

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public DateTime CoverageDate { get; set; }

    public DateTime BillDate { get; set; }

    public double CommissionRate { get; set; }

    public double CommissionAmount { get; set; }

    public double? AdminFee { get; set; }

    public double Surcharge { get; set; }

    public double MunicipalTax { get; set; }

    public string? Comments { get; set; }

    public DateOnly AccountingDate { get; set; }

    public double PaidToDate { get; set; }

    public Guid AgencyId { get; set; }

    public double NetDue { get; set; }

    public double Earned { get; set; }

    public double Unearned { get; set; }

    public int SfaaCode { get; set; }

    public bool DirectBill { get; set; }

    public string Region { get; set; } = null!;

    public string AccountClass { get; set; } = null!;

    public bool SurchargeApply { get; set; }

    public Guid? AgencyChange { get; set; }

    public string? TransactionPurpose { get; set; }

    public Guid TransactionGroup { get; set; }

    public string? Rate { get; set; }

    public string? RateStructure { get; set; }

    public string? RateClass { get; set; }

    public Guid UnderwriterId { get; set; }

    public virtual AccountClassDm AccountClassNavigation { get; set; } = null!;

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Agency Agency { get; set; } = null!;

    public virtual Bond BondNumberNavigation { get; set; } = null!;

    public virtual ICollection<BondTransactionPurpose> BondTransactionPurposes { get; set; } = new List<BondTransactionPurpose>();

    public virtual RegionDm RegionNavigation { get; set; } = null!;

    public virtual Sfaa SfaaCodeNavigation { get; set; } = null!;

    public virtual Underwriter Underwriter { get; set; } = null!;
}
