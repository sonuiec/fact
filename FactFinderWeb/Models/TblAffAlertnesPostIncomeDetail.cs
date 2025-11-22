using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblAffAlertnesPostIncomeDetail
{
    public int Id { get; set; }

    public long? ProfileId { get; set; }

    public decimal? PostIncome { get; set; }

    public decimal? MustHaveExpensesPercent { get; set; }

    public decimal? OptionalExpensesPercent { get; set; }

    public decimal? SavingsPercent { get; set; }

    public decimal? ProjectedGrowthRateNext5Years { get; set; }

    public decimal? ProjectedGrowthRate6To10Years { get; set; }

    public decimal? InflationRate { get; set; }

    public DateTime? CreatedAt { get; set; }
}
