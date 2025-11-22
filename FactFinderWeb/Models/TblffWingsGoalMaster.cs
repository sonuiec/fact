using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffWingsGoalMaster
{
    public long Id { get; set; }

    public string? GoalName { get; set; }

    public string? GoalType { get; set; }

    public int? GoalSequence { get; set; }

    public int GoalPriority { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
