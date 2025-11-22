using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffKnowledgeRisk
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string RiskCapacity { get; set; } = null!;

    public string RiskRequirement { get; set; } = null!;

    public string RiskTolerance { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? AddBy { get; set; }

    public string? TotalRiskProfileScore { get; set; }

    public string? PlannerAssessmentOnRiskProfile { get; set; }
}
