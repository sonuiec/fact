using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesOneTimeLoanRepayment
{
    public int Id { get; set; }

    public long? ProfileId { get; set; }

    public decimal? TotalEmi { get; set; }

    public decimal? NetCashflows { get; set; }

    public decimal? OneTimeLoanRepayment { get; set; }
}
