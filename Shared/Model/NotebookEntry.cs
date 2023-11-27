using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class NotebookEntry
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid NotebookId { get; set; }

    public string Type { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public string? Category { get; set; }

    public string Note { get; set; } = null!;

    public bool Completed { get; set; }

    public DateTime? Completion { get; set; }

    public DateTime? ReminderTime { get; set; }

    public bool ColumnName { get; set; }

    public virtual UserProfile CreatedByNavigation { get; set; } = null!;

    public virtual Notebook Notebook { get; set; } = null!;

    public virtual NotebookEntryTypeDm TypeNavigation { get; set; } = null!;
}
