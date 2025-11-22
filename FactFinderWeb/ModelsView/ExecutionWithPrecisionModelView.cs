namespace FactFinderWeb.ModelsView
{
    public class ExecutionWithPrecisionModelView
    {
        public List<WingsGoalStep5ExecutionDataMV> wingsGoalStep5ExecutionDataList { get; set; } = new();
        public WingsGoalStep5ExecutionData  wingsGoalStep5ExecutionData { get; set; } = new();


        
    }


    public class WingsGoalStep5ExecutionData
    {
        public long? Id { get; set; }

        public long? ProfileId { get; set; }

        public long? Step5ExecutionMasterid { get; set; }

        public int? Goalid { get; set; }

        public string? GoalName { get; set; }

        public string? ExecutionDescription { get; set; }

        public string? ExecutionValue { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }



    public class WingsGoalStep5ExecutionDataMV
    {
        public long? Id { get; set; }

        public long? ProfileId { get; set; }

        public long? Step5ExecutionMasterid { get; set; }

        public int? Goalid { get; set; }
        public int? GoalPriority { get; set; }
        
        public string? GoalName { get; set; }

        public string? ExecutionDescription { get; set; }

        public string? ExecutionValue { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }

}
