using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesIncome
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }
}
