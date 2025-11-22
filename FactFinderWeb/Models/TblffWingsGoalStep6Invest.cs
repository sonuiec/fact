using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffWingsGoalStep6Invest
{
    public long Id { get; set; }

    public long? ProfileId { get; set; }

    public int? Goalid { get; set; }

    public string? GoalName { get; set; }

    public decimal? LumpsumAmount { get; set; }

    public decimal? Sipamount { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
