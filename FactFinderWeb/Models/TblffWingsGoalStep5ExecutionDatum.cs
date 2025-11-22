using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffWingsGoalStep5ExecutionDatum
{
    public long Id { get; set; }

    public long? ProfileId { get; set; }

    public long? Step5ExecutionMasterid { get; set; }

    public int? Goalid { get; set; }

    public string? GoalName { get; set; }

    public string? ExecutionDescription { get; set; }

    public string? ExecutionValueType { get; set; }

    public string? ExecutionValue { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? Wingid { get; set; }
}
