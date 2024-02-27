using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    /// Add interfaces to legal entity objects to specify Type Extensions for GraphQL

    public partial class Account : ILegalEntityCompany { }
    public partial class Agency : ILegalEntityCompany { }
    public partial class Insurer : ILegalEntityCompany { }
    public partial class LawEntity : ILegalEntityIndividual { }
    public partial class Agent : ILegalEntityIndividual { }
    public partial class Indemnitor : ILegalEntityIndividual { }
    public partial class KeyPersonnel : ILegalEntityIndividual { }
    public partial class Obligee : ILegalEntityIndividual { }

    /// <summary>
    /// Objects that can have notebooks associated with them
    /// </summary>
    public interface INotebookOwner
    {
        Guid Id { get; }
    }

    /// <summary>
    /// Legal entities that cannot be individuals
    /// </summary>
    public interface ILegalEntityCompany : INotebookOwner
    {
        LegalEntity IdNavigation { get; set; }
    }

    /// <summary>
    /// Legal Entities that can be individuals or 
    /// </summary>
    public interface ILegalEntityIndividual : ILegalEntityCompany { }
}
