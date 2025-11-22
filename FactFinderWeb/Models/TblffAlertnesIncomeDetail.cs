using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesIncomeDetail
{
    public long Id { get; set; }

    public long? ProfileId { get; set; }

    public decimal? Basic { get; set; }

    public decimal? Hra { get; set; }

    public decimal? EducationAllowance { get; set; }

    public decimal? MedicalAllowance { get; set; }

    public decimal? Lta { get; set; }

    public decimal? Conveyance { get; set; }

    public decimal? OtherAllowance { get; set; }

    public decimal? Pf { get; set; }

    public decimal? Gratuity { get; set; }

    public decimal? Reimbursement { get; set; }

    public decimal? BusinessIncome { get; set; }

    public decimal? FoodCoupon { get; set; }

    public decimal? MonthlyPension { get; set; }

    public decimal? InterestIncome { get; set; }

    public decimal? AnnualBonus { get; set; }

    public decimal? PerformanceLinked { get; set; }

    public decimal? AnnualTotalIncome { get; set; }

    public decimal? MonthlyIncome { get; set; }

    public decimal? PostItincomeOld { get; set; }

    public decimal? PostItincomeNew { get; set; }

    public decimal? SpouseBasic { get; set; }

    public decimal? SpouseHra { get; set; }

    public decimal? SpouseEducationAllowance { get; set; }

    public decimal? SpouseMedicalAllowance { get; set; }

    public decimal? SpouseLta { get; set; }

    public decimal? SpouseConveyance { get; set; }

    public decimal? SpouseOtherAllowance { get; set; }

    public decimal? SpousePf { get; set; }

    public decimal? SpouseGratuity { get; set; }

    public decimal? SpouseReimbursement { get; set; }

    public decimal? SpouseBusinessIncome { get; set; }

    public decimal? SpouseFoodCoupon { get; set; }

    public decimal? SpouseMonthlyPension { get; set; }

    public decimal? SpouseInterestIncome { get; set; }

    public decimal? SpouseMonthlyTotalIncome { get; set; }

    public decimal? SpouseConsolidatedIncome { get; set; }

    public decimal? SpouseAnnualBonus { get; set; }

    public decimal? SpousePerformanceLinked { get; set; }

    public decimal? SpouseAnnualTotalIncome { get; set; }

    public decimal? SpouseOverallMonthlyIncome { get; set; }

    public decimal? SpousePostItincomeOld { get; set; }

    public decimal? SpousePostItincomeNew { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public decimal? TotalExpense { get; set; }
}
