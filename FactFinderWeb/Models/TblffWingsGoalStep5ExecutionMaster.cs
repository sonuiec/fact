using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffWingsGoalStep5ExecutionMaster
{
    public long Id { get; set; }

    public int? Goalid { get; set; }

    public string? GoalType { get; set; }

    public string? GoalName { get; set; }

    public int? ExecutionDescriptionOrder { get; set; }

    public string? ExecutionDescriptionName { get; set; }

    public string? ExecutionDescription { get; set; }

    public string? ExecutionValueType { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? DealType { get; set; }
}
