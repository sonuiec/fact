using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblFfAlertnesGeneralInsurance
{
    public int Id { get; set; }

    public long ProfileId { get; set; }

    public string? InsuranceType { get; set; }

    public decimal? AmountOfCoverage { get; set; }

    public DateOnly? PremiumDueDate { get; set; }

    public DateOnly? MaturityDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
