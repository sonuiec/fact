using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffInvestWingsGoalMaster
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public decimal? MonthlySavings { get; set; }

    public decimal? IntendedSipmonthly { get; set; }

    public decimal? AvailableLumpsum { get; set; }

    public long? Addby { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
