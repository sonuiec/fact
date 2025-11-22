using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblFfAlertnesNetWorth
{
    public int NetWorthId { get; set; }

    public long ProfileId { get; set; }

    public decimal? CashInHand { get; set; }

    public decimal? EmployeeProvidendFund { get; set; }

    public decimal? Ppf { get; set; }

    public decimal? FixedDeposits { get; set; }

    public decimal? MutualFundsShares { get; set; }

    public decimal? PaidUpValueOfInsurancePolicies { get; set; }

    public decimal? OthersGratuity { get; set; }

    public decimal? Home1 { get; set; }

    public decimal? Home2 { get; set; }

    public decimal? Land { get; set; }

    public decimal? Car { get; set; }

    public decimal? CommercialProperty { get; set; }

    public decimal? Jewellery { get; set; }

    public decimal? ValueOfBusiness { get; set; }

    public decimal? OtherAssetsOther { get; set; }

    public decimal? Home2Loan { get; set; }

    public decimal? LandLoan { get; set; }

    public decimal? CommercialPropertyLoan { get; set; }

    public decimal? JewelleryLoan { get; set; }

    public decimal? BusinessLoan { get; set; }

    public decimal? OtherLoan { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal? Home1Loan { get; set; }

    public decimal? CarLoan { get; set; }
}
