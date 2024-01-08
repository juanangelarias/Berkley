using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PersonalFinancialHeader
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly StatementDate { get; set; }

    public string? Type { get; set; }

    public string? Basis { get; set; }

    public string? TaxBasis { get; set; }

    public string? Quality { get; set; }

    public string? Scaling { get; set; }

    public string StatementFor { get; set; } = null!;

    public string? Comments { get; set; }

    public Guid PersonId { get; set; }

    public virtual StatementBasisDm? BasisNavigation { get; set; }

    public virtual LegalEntity Person { get; set; } = null!;

    public virtual ICollection<PersonalFinancialStatement> PersonalFinancialStatements { get; set; } = new List<PersonalFinancialStatement>();

    public virtual StatementQualityDm? QualityNavigation { get; set; }

    public virtual ScalingDm? ScalingNavigation { get; set; }

    public virtual TaxBasisDm? TaxBasisNavigation { get; set; }

    public virtual PersonalFinancialStatementTypeDm? TypeNavigation { get; set; }
}
