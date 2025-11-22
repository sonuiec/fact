using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffWing
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string? GoalType { get; set; }

    public int? GoalPriority { get; set; }

    public string? GoalName { get; set; }

    public int? GoalPlanYear { get; set; }

    public int? GoalStartYear { get; set; }

    public int? GoalEndYear { get; set; }

    public int? TimeHorizon { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public byte? NewGoals { get; set; }

    public long? Goalid { get; set; }
}
