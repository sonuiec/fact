using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesNewSaving
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? CurrentValue { get; set; }

    public decimal? MonthlyContribution { get; set; }

    public DateTime? TillWhen { get; set; }

    public bool? FollowUp { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? SavingsId { get; set; }
}
