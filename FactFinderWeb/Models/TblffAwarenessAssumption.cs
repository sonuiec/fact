using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAwarenessAssumption
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public int Equity { get; set; }

    public int Debt { get; set; }

    public int Gold { get; set; }

    public int RealEstateReturn { get; set; }

    public int LiquidFunds { get; set; }

    public int InflationRates { get; set; }

    public int EducationInflation { get; set; }

    public int ApplicantRetirement { get; set; }

    public int? SpouseRetirement { get; set; }

    public int? ApplicantLifeExpectancy { get; set; }

    public int? SpouseLifeExpectancy { get; set; }
}
