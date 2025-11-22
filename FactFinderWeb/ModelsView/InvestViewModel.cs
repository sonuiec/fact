namespace FactFinderWeb.ModelsView
{
    public class InvestViewModel
    {
        public long Id { get; set; }
        public decimal? MonthlySavings  { get; set; }
        public decimal? IntendedSIPmonthly { get; set; }
        public decimal? AvailableLumpsum  { get; set; }
        public InvestMV investMV { get; set; } = new();

        public List<InvestMV> InvestMVList { get; set; } = new();
    } 
     


    public class InvestMV
    {
        public long Id { get; set; }

        public long? ProfileId { get; set; }

        public int? Goalid { get; set; }
        public int? GoalPriority { get; set; }

        public string? GoalName { get; set; }

        public decimal? LumpsumAmount { get; set; }

        public decimal? Sipamount { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }

     
}
