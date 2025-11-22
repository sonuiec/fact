using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesDebt
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public decimal? GoldLoan { get; set; }

    public decimal? CreditCard { get; set; }

    public decimal? PersonalLoan { get; set; }

    public decimal? BadLoanOthers { get; set; }

    public decimal? HomeLoan { get; set; }

    public decimal? EducationLoan { get; set; }

    public decimal? BusinessLoan { get; set; }

    public decimal? GoodLoanOthers { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
