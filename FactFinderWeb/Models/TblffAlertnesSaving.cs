using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesSaving
{
    public int Id { get; set; }

    public long? ProfileId { get; set; }

    public decimal? TotalCommittedSavings { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TblffAlertnesCommittedSaving> TblffAlertnesCommittedSavings { get; set; } = new List<TblffAlertnesCommittedSaving>();
}
