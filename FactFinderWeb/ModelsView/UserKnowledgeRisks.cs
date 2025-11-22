using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace FactFinderWeb.ModelsView
{
    public class UserKnowledgeRisks
    {

    }

    public partial class MVKnowledgeRiskList
    {
        public List<MVKnowledgeRisk> KnowledgeRiskList { get; set; } = new List<MVKnowledgeRisk>();
    } 

    public partial class MVKnowledgeRisk
    {
        public long Id { get; set; }

        //public long UserID { get; set; }
        public long? ProfileId { get; set; }

        [Required(ErrorMessage = "Please select risk capacity.")]
        public string RiskCapacity { get; set; } = null!;

        [Required(ErrorMessage = "Please select risk requirement.")]
        public string RiskRequirement { get; set; } = null!;

        [Required(ErrorMessage = "Please select risk tolerance.")]
        public string RiskTolerance { get; set; } = null!;

        public string? TotalRiskProfileScore { get; set; }

        public string? PlannerAssessmentOnRiskProfile { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
