using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class TicketTask
{
    public int TicketId { get; set; }

    public DateTime TicketDate { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int? RefTicket { get; set; }

    public int? TaskCategory { get; set; }

    public int? TaskCategoryTitle { get; set; }

    public int? TaskOrder { get; set; }

    public string? TaskDetails { get; set; }

    public double? TaskTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }
}
