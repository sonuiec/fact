using FactFinderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FactFinderWeb.ModelsView
{
    public class AlertnessViewModel
    {

       
        public long? ProfileId { get; set; }

        [Display(Name = "ID")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Basic is required.")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Basic must be a valid number.")]
        [Display(Name = "Basic")]
        public decimal? Basic { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "HRA must be a valid number.")]
        [Display(Name = "HRA")]
        public decimal? Hra { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Education Allowance must be a valid non-negative number.")]
        [Display(Name = "Education Allowance")]
        public decimal? EducationAllowance { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Medical Allowance must be a valid non-negative number.")]
        [Display(Name = "Medical Allowance")]
        public decimal? MedicalAllowance { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "LTA must be a valid non-negative number.")]
        [Display(Name = "LTA")]
        public decimal? LTA { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Conveyance must be a valid non-negative number.")]
        [Display(Name = "Conveyance")]
        public decimal? Conveyance { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Other Allowance must be a valid non-negative number.")]
        [Display(Name = "Other Allowance")]
        public decimal? OtherAllowance { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "PF must be a valid non-negative number.")]
        [Display(Name = "PF")]
        public decimal? PF { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Gratuity must be a valid non-negative number.")]
        [Display(Name = "Gratuity")]
        public decimal? Gratuity { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Reimbursement must be a valid non-negative number.")]
        [Display(Name = "Reimbursement")]
        public decimal? Reimbursement { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Business & Other Income must be a valid non-negative number.")]
        [Display(Name = "Business & Other Income")]
        public decimal? BusinessIncome { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Food Coupon must be a valid non-negative number.")]
        [Display(Name = "Food Coupon")]
        public decimal? FoodCoupon { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Monthly Pension must be a valid non-negative number.")]
        [Display(Name = "Monthly Pension")]
        public decimal? MonthlyPension { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Interest Income must be a valid non-negative number.")]
        [Display(Name = "Interest Income from Bank SB/AC")]
        public decimal? InterestIncome { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Annual Bonus must be a valid non-negative number.")]
        [Display(Name = "Annual Bonus")]
        public decimal? AnnualBonus { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Performance Linked Pay must be a valid non-negative number.")]
        [Display(Name = "Performance Linked Pay")]
        public decimal? PerformanceLinked { get; set; }


        [Display(Name = "Annual Total Income")]
        public decimal? AnnualTotalIncome { get; set; }

        [Display(Name = "Overall Monthly Income")]
        public decimal? OverallMonthlyIncome { get; set; }

        [Display(Name = "Post-IT Income (Old Regime)")]
        public decimal? PostITIncomeOld { get; set; }

        [Display(Name = "Post-IT Income (New Regime)")]
        public decimal? PostITIncomeNew { get; set; }

        /// <summary>
        /// Spouce detail
        /// </summary>
        [Display(Name = "Basic")]
        public decimal? SpouseBasic { get; set; }

        [Display(Name = "HRA")]
        public decimal? SpouseHRA { get; set; }

        [Display(Name = "Education Allowance")]
        public decimal? SpouseEducationAllowance { get; set; }

        [Display(Name = "Medical Allowance")]
        public decimal? SpouseMedicalAllowance { get; set; }

        [Display(Name = "LTA")]
        public decimal? SpouseLTA { get; set; }

        [Display(Name = "Conveyance")]
        public decimal? SpouseConveyance { get; set; }

        [Display(Name = "Other Allowance")]
        public decimal? SpouseOtherAllowance { get; set; }

        [Display(Name = "PF")]
        public decimal? SpousePF { get; set; }

        [Display(Name = "Gratuity")]
        public decimal? SpouseGratuity { get; set; }

        [Display(Name = "Reimbursement")]
        public decimal? SpouseReimbursement { get; set; }

        [Display(Name = "Business & Other Income")]
        public decimal? SpouseBusinessIncome { get; set; }

        [Display(Name = "Food Coupon")]
        public decimal? SpouseFoodCoupon { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Monthly Pension must be a valid non-negative number.")]
        [Display(Name = "Monthly Pension")]
        public decimal? SpouseMonthlyPension { get; set; }

        [Display(Name = "Interest Income from Bank SB/AC")]
        public decimal? SpouseInterestIncome { get; set; }

        [Display(Name = "Monthly Total Income")]
        public decimal? SpouseMonthlyTotalIncome { get; set; }

        [Display(Name = "Consolidated Income")]
        public decimal? SpouseConsolidatedIncome { get; set; }

        [Display(Name = "Annual Bonus")]
        public decimal? SpouseAnnualBonus { get; set; }

        [Display(Name = "Performance Linked Pay")]
        public decimal? SpousePerformanceLinked { get; set; }

        [Display(Name = "Annual Total Income")]
        public decimal? SpouseAnnualTotalIncome { get; set; }

        [Display(Name = "Overall Monthly Income")]
        public decimal? SpouseOverallMonthlyIncome { get; set; }

        [Display(Name = "Post IT Income Monthly (Disposable Income)")]
        public decimal? SpousePostITIncomeOld { get; set; }

        [Display(Name = "Post-IT Income (New Regime)")]
        public decimal? SpousePostITIncomeNew { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Total Expense")]
        public decimal? TotalExpense { get; set; }
        


        ////[Required(ErrorMessage = "IncomeExtras is required.")]
        public List<TblffAlertnesIncomeExtra>? IncomeExtras { get; set; } = new();
        public List<TblffAlertnesSpouseIncomeExtra>? SpouseExtras { get; set; } = new();

        // Expenses
        // public TblffAlertnesExpense? ExpenseDetail { get; set; }
        // Replace generated class with validated ViewModel for expenses
        public ExpenseViewModel? ExpenseDetail { get; set; }

        public List<TblffAlertnesExpenseNew>? ExpenseNew { get; set; } = new();

        //3step for saving
        public TblffAlertnesSaving? Savings { get; set; }

        public CommittedSavingViewModel? CommittedSaving { get; set; }
        public List<TblffAlertnesNewSaving>? NewSavings { get; set; }

        //Step-4 EMI
        // Database use Tables-(tblff_Alertnes_EMI_Details,tblff_OneTimeLoanRepayment)

        public EmiOneTimeLoanRepayment? EmiOneTimeLoanRepayment { get; set; }
        public List<TblffAlertnesEmiDetail>? EmiDetail { get; set; }


        //Step-5
        public PostIncomeDetailsViewModel? postIncomeDetailsViewModel { get; set; }

        //Step-6
        public NetWorthViewModel? netWorthViewModel { get; set; }

        // Child Collections
        public List<TblFfAlertnesNewInvestment>? NewInvestments { get; set; }
        public List<TblFfAlertnesNewOtherAsset>? NewOtherAssets { get; set; }
        public List<TblFfAlertnesNewLiability>? NewLiabilities { get; set; }
        


        //Step-7 LoanDetailViewModel   //[dbo].[tblff_Alertnes_Debt]

        public LoanDebtDetailViewModel? loanDebtDetailViewModel { get; set; }

        //step-8 Insurance (both)
        public List<TblFfAlertnesLifeInsurance>? LifeInsuranceViewModel { get; set; }
        public List<TblFfAlertnesGeneralInsurance>? GeneralInsuranceViewModel { get; set; }
    }
    public class SpouseIncomeExtra
    {
        [Required]
        public string FieldName { get; set; }

        [Required]
        public decimal FieldValue { get; set; }

        [Required]
        public string Type { get; set; } // e.g. Monthly / Annual
    }
    public class IncomeExtra
    {
        [Required]
        public string FieldName { get; set; }

        [Required]
        public decimal FieldValue { get; set; }

        [Required]
        public string Type { get; set; } // e.g. Monthly / Annual
    }

    public class ExpenseViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Grocery/Provision/Milk is required")]
        [Range(0, double.MaxValue)]
        [Display(Name = "Grocery / Provision / Milk")]
        public decimal? GroceryProvisionMilk { get; set; }


        [AmountRange]
        [Display(Name = "Domestic Help")]
        public decimal? DomesticHelp { get; set; }

        [AmountRange]
        [Display(Name = "Iron / Laundry")]
        public decimal? IronLaundry { get; set; }

        [AmountRange]
        [Display(Name = "Driver")]
        public decimal? Driver { get; set; }

        [AmountRange]
        [Display(Name = "Fuel")]
        public decimal? Fuel { get; set; }

        [AmountRange]
        [Display(Name = "Car Cleaning")]
        public decimal? CarCleaning { get; set; }

        [AmountRange]
        [Display(Name = "Maintenance")]
        public decimal? Maintenance { get; set; }

        [AmountRange]
        [Display(Name = "Taxi / Public Transport")]
        public decimal? TaxiPublicTransport { get; set; }

        [AmountRange]
        [Display(Name = "Air / Train Travel")]
        public decimal? AirTrainTravel { get; set; }

        [AmountRange]
        [Display(Name = "Mobile")]
        public decimal? Mobile { get; set; }

        [AmountRange]
        [Display(Name = "Landline / Broadband")]
        public decimal? LandlineBroadband { get; set; }

        [AmountRange]
        [Display(Name = "Data Card")]
        public decimal? DataCard { get; set; }

        [AmountRange]
        [Display(Name = "Electricity")]
        public decimal? Electricity { get; set; }

        [AmountRange]
        [Display(Name = "House Tax")]
        public decimal? HouseTax { get; set; }

        [AmountRange]
        [Display(Name = "Society Charges")]
        public decimal? SocietyCharge { get; set; }

        [AmountRange]
        [Display(Name = "Rents")]
        public decimal? Rents { get; set; }

        [AmountRange]
        [Display(Name = "Cable")]
        public decimal? Cable { get; set; }

        [AmountRange]
        [Display(Name = "LPG")]
        public decimal? Lpg { get; set; }

        [AmountRange]
        [Display(Name = "Water Bill")]
        public decimal? WaterBill { get; set; }

        [AmountRange]
        [Display(Name = "Newspaper")]
        public decimal? NewsPaper { get; set; }

        [AmountRange]
        [Display(Name = "School Fees")]
        public decimal? SchoolFees { get; set; }

        [AmountRange]
        [Display(Name = "Tuitions")]
        public decimal? Tuitions { get; set; }

        [AmountRange]
        [Display(Name = "Uniforms / Accessories")]
        public decimal? UniformsAccessories { get; set; }

        [AmountRange]
        [Display(Name = "Books / Stationery")]
        public decimal? BooksStationery { get; set; }

        [AmountRange]
        [Display(Name = "Picnics / Activities")]
        public decimal? PicnicsActivities { get; set; }

        [AmountRange]
        [Display(Name = "Movies / Theatre")]
        public decimal? MoviesTheatre { get; set; }

        [AmountRange]
        [Display(Name = "Dining Out")]
        public decimal? DiningOut { get; set; }

        [AmountRange]
        [Display(Name = "Clubhouse Expenses")]
        public decimal? ClubhouseExpenses { get; set; }

        [AmountRange]
        [Display(Name = "Parties at Home")]
        public decimal? PartiesAtHome { get; set; }

        [AmountRange]
        [Display(Name = "Clothing / Grooming")]
        public decimal? ClothingGrooming { get; set; }

        [AmountRange]
        [Display(Name = "Vacation / Travel")]
        public decimal? VacationTravel { get; set; }

        [AmountRange]
        [Display(Name = "Festivals")]
        public decimal? Festivals { get; set; }

        [AmountRange]
        [Display(Name = "Kids' Birthdays")]
        public decimal? KidsBirthdays { get; set; }

        [AmountRange]
        [Display(Name = "Family Functions")]
        public decimal? FamilyFunctions { get; set; }

        [AmountRange]
        [Display(Name = "Medical")]
        public decimal? Medical { get; set; }

        [AmountRange]
        [Display(Name = "Vehicle Servicing")]
        public decimal? VehicleServicing { get; set; }

        [AmountRange]
        [Display(Name = "Home Repair")]
        public decimal? HomeRepair { get; set; }

        [AmountRange]
        [Display(Name = "New Home Appliances")]
        public decimal? NewHomeAppliances { get; set; }

        [AmountRange]
        [Display(Name = "Life Insurance")]
        public decimal? LifeInsurance { get; set; }

        [AmountRange]
        [Display(Name = "Home Insurance")]
        public decimal? HomeInsurance { get; set; }

        [AmountRange]
        [Display(Name = "Medical Insurance")]
        public decimal? MedicalInsurance { get; set; }

        [AmountRange]
        [Display(Name = "Car Insurance")]
        public decimal? CarInsurance { get; set; }

        [AmountRange]
        [Display(Name = "Fee to CA")]
        public decimal? FeeToCa { get; set; }

        [AmountRange]
        [Display(Name = "Other Consultant")]
        public decimal? OtherConsultant { get; set; }

        [AmountRange]
        [Display(Name = "Donations")]
        public decimal? Donations { get; set; }
    }
    public class AmountRangeAttribute : RangeAttribute
    {
        public AmountRangeAttribute()
            : base(0, double.MaxValue)
        {
            ErrorMessage = "Enter Amount without ',' commas";
        }
    }

    public class ExpenseNew
    {
        [Required(ErrorMessage = "Section is required")]

        public string? Section { get; set; }
        [Required(ErrorMessage = "Field Name is required")]
        public string? FieldName { get; set; }
        [Required(ErrorMessage = "Value is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number")]
        public decimal FieldValue { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string? Type { get; set; }
    }




    //TblffAlertnesCommittedSaving
    public class CommittedSavingViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Saving Name")]
        public string? Name { get; set; }

        [Display(Name = "Current Value")]
        [Range(0, double.MaxValue, ErrorMessage = "Current Value must be positive.")]
        public decimal? CurrentValue { get; set; }

        [Display(Name = "Monthly Contribution")]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Contribution must be positive.")]
        public decimal? MonthlyContribution { get; set; }

        [Display(Name = "Till When (dd/MM/yyyy)")]
        public string? TillWhen { get; set; }

        [NotMapped]
        public DateTime? TillWhenDateParsed =>
            DateTime.TryParseExact(TillWhen, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                ? dt
                : (DateTime?)null;

        [Display(Name = "Follow Up Required?")]
        public bool? FollowUp { get; set; }
    }
    public class NewSavings
    {
        [Required(ErrorMessage = "Saving Name is required.")]
        [StringLength(100, ErrorMessage = "Saving Name can't be longer than 100 characters.")]
        [Display(Name = "Saving Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Current Value is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Current Value must be a positive number.")]
        [Display(Name = "Current Value")]
        public decimal? CurrentValue { get; set; }

        //[Required(ErrorMessage = "Monthly Contribution is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Contribution must be a positive number.")]
        [Display(Name = "Monthly Contribution")]
        public decimal? MonthlyContribution { get; set; }

        [Required(ErrorMessage = "Till When date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Till When")]
        public DateTime? TillWhen { get; set; }


        //[Required(ErrorMessage = "Till When date is required.")]
        //public string? TillWhen { get; set; }



        [Required(ErrorMessage = "Please select Follow-Up.")]
        public bool? FollowUp { get; set; }

        public DateTime? CreatedAt { get; set; }

        // [Required(ErrorMessage = "Savings ID is required.")]
        public int SavingsId { get; set; }

    }

    //Step-4 EMI
    public class EmiOneTimeLoanRepayment
    {
        public long? ProfileId { get; set; }

        [Display(Name = "Total EMI")]
        public decimal? TotalEmi { get; set; }

        [Display(Name = "Net Cashflows")]
        public decimal? NetCashflows { get; set; }

        [Display(Name = "One-Time Loan Repayment")]
        public decimal? OneTimeLoanRepayment { get; set; }
    }
    public class EmiDetail
    {


        public long? ProfileId { get; set; }

        [Required(ErrorMessage = "EMI Name is required.")]
        [StringLength(100, ErrorMessage = "Loan Name can't be longer than 100 characters.")]
        [Display(Name = "EMI Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Outstanding Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Outstanding Amount must be a positive number.")]
        [Display(Name = "Outstanding Amount")]
        public decimal? Outstanding { get; set; }

        [Required(ErrorMessage = "Interest Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Interest must be a positive number.")]
        [Display(Name = "Interest")]
        public decimal? Interest { get; set; }

        [Required(ErrorMessage = "Principal Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Principal must be a positive number.")]
        [Display(Name = "Principal")]
        public decimal? Principal { get; set; }

        [Required(ErrorMessage = "Monthly EMI is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly EMI must be a positive number.")]
        [Display(Name = "Monthly EMI")]
        public decimal? Monthly { get; set; }

        [Required(ErrorMessage = "Till date is required.")]
        [Display(Name = "EMI Till (End Date)")]
        public DateOnly? Till { get; set; }

        [Display(Name = "Follow Up Required?")]
        public bool? FollowUp { get; set; }

        public bool? IsNew { get; set; }
    }

    //Step-5 PostIncomeDetails
    public class PostIncomeDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Profile Id")]
        public long ProfileId { get; set; }

        [Display(Name = "Post Income")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Maximum two decimal places allowed.")]
        public decimal? PostIncome { get; set; }

        [Display(Name = "Must-Have Expenses (%)")]
        [Range(0, 100, ErrorMessage = "Must-Have Expenses must be between 0 and 100%.")]
        public decimal? MustHaveExpensesPercent { get; set; }

        [Display(Name = "Optional Expenses (%)")]
        [Range(0, 100, ErrorMessage = "Optional Expenses must be between 0 and 100%.")]
        public decimal? OptionalExpensesPercent { get; set; }

        [Display(Name = "Savings (%)")]
        [Range(0, 100, ErrorMessage = "Savings must be between 0 and 100%.")]
        public decimal? SavingsPercent { get; set; }

        [Display(Name = "Projected Growth Rate (Next 5 Years) (%)")]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal? ProjectedGrowthRateNext5Years { get; set; }

        [Display(Name = "Projected Growth Rate (6 to 10 Years) (%)")]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal? ProjectedGrowthRate6To10Years { get; set; }

        [Display(Name = "Inflation Rate (%)")]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal? InflationRate { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }
    }

    //Step-6

    public class NetWorthViewModel
    {
        public int NetWorthId { get; set; }

        [Required]
        public long ProfileId { get; set; }

        // === Investments ===

        [Display(Name = "Cash In Hand")]
        [Range(0, 999999999999.99, ErrorMessage = "Invalid amount")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Max 2 decimal places allowed")]
        public decimal? CashInHand { get; set; }

        [Display(Name = "Employee Provident Fund")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? EmployeeProvidendFund { get; set; }

        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? PPF { get; set; }

        [Display(Name = "Fixed Deposits")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? FixedDeposits { get; set; }

        [Display(Name = "Mutual Funds / Shares")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? MutualFundsShares { get; set; }

        [Display(Name = "Paid-Up Insurance Policies")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? PaidUpValueOfInsurancePolicies { get; set; }

        [Display(Name = "Other Gratuity")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? OthersGratuity { get; set; }

        // === Other Assets ===

        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Home1 { get; set; }

        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Home2 { get; set; }

        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Land { get; set; }

        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Car { get; set; }

        [Display(Name = "Commercial Property")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? CommercialProperty { get; set; }

        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Jewellery { get; set; }

        [Display(Name = "Value Of Business")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? ValueOfBusiness { get; set; }

        [Display(Name = "Other Assets")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? OtherAssetsOther { get; set; }

        // === Liabilities ===

        [Display(Name = "Home 2 Loan")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? Home2Loan { get; set; }
        [Display(Name = "Home 1 Loan")]
        public decimal? Home1Loan { get; set; }


        [Display(Name = "Land Loan")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? LandLoan { get; set; }

        [Display(Name = "Commercial Property Loan")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? CommercialPropertyLoan { get; set; }

        [Display(Name = "Jewellery Loan")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? JewelleryLoan { get; set; }

        [Display(Name = "Business Loan")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? BusinessLoan { get; set; }

        [Display(Name = "Other Loan")]
        [Range(0, 999999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal? OtherLoan { get; set; }

        public DateTime? CreatedAt { get; set; }


        [Display(Name = "Car Loan")]
        public decimal? CarLoan { get; set; }
    }
    public class NewInvestmentViewModel
    {
        public int Id { get; set; }
        public int NetWorthId { get; set; }
        public long ProfileId { get; set; }

        [Required(ErrorMessage = "Investment Name is required.")]
        [Display(Name = "Investment Name")]
        public string InvestmentName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Amount must be between 0.01 and 999999999999.99.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount must be a valid number with up to 2 decimal places.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
    public class NewOtherAssetViewModel
    {
        public int Id { get; set; }
        public int NetWorthId { get; set; }
        public long ProfileId { get; set; }

        [Required(ErrorMessage = "Asset Name is required.")]
        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Amount must be between 0.01 and 999999999999.99.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount must be a valid number with up to 2 decimal places.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
    public class NewLiabilityViewModel
    {
        public int Id { get; set; }
        public int NetWorthId { get; set; }
        public long ProfileId { get; set; }

        [Required(ErrorMessage = "Liability Name is required.")]
        [Display(Name = "Liability Name")]
        public string LiabilityName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Amount must be between 0.01 and 999999999999.99.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount must be a valid number with up to 2 decimal places.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

    //step-7 Debt

    public class LoanDebtDetailViewModel
    {
        public int Id { get; set; }

        [Required]
        public long ProfileId { get; set; }



        [Display(Name = "Gold Loan")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Gold Loan must be a valid number with up to 2 decimal places.")]
        public decimal? GoldLoan { get; set; }

        [Display(Name = "Credit Card")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Credit Card must be a valid number with up to 2 decimal places.")]
        public decimal? CreditCard { get; set; }

        [Display(Name = "Personal Loan")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Personal Loan must be a valid number with up to 2 decimal places.")]
        public decimal? PersonalLoan { get; set; }

        [Display(Name = "Bad Loan (Others)")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Bad Loan (Others) must be a valid number with up to 2 decimal places.")]
        public decimal? BadLoanOthers { get; set; }

        [Display(Name = "Home Loan")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Home Loan must be a valid number with up to 2 decimal places.")]
        public decimal? HomeLoan { get; set; }

        [Display(Name = "Education Loan")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Education Loan must be a valid number with up to 2 decimal places.")]
        public decimal? EducationLoan { get; set; }

        [Display(Name = "Business Loan")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Business Loan must be a valid number with up to 2 decimal places.")]
        public decimal? BusinessLoan { get; set; }

        [Display(Name = "Good Loan (Others)")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Good Loan (Others) must be a valid number with up to 2 decimal places.")]
        public decimal? GoodLoanOthers { get; set; }

        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }

    //Step-8 Insurance
    public class LifeInsuranceViewModel
    {
        public int Id { get; set; }

        [Required]
        public long ProfileId { get; set; }

        [Required(ErrorMessage = "Insurance  Name is required.")]

        [Display(Name = "Insurance Type")]
        public string? InsuranceType { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Amount of Coverage")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only numbers allowed with up to 2 decimal places.")]
        [Range(0, 999999999999.99, ErrorMessage = "Enter a valid amount.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountOfCoverage { get; set; }

        [Display(Name = "Premium Due Date")]
        [DataType(DataType.Date)]
        public DateTime? PremiumDueDate { get; set; }

        [Display(Name = "Maturity Date")]
        [DataType(DataType.Date)]
        public DateTime? MaturityDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
    public class GeneralInsuranceViewModel
    {
        public int Id { get; set; }

        [Required]
        public long ProfileId { get; set; }

        [Required(ErrorMessage = "Insurance  Name is required.")]
        [Display(Name = "Insurance Type")]
        public string? InsuranceType { get; set; }

        [Display(Name = "Amount of Coverage")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only numbers allowed with up to 2 decimal places.")]
        [Range(0, 999999999999.99, ErrorMessage = "Enter a valid amount.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? AmountOfCoverage { get; set; }

        [Display(Name = "Premium Due Date")]
        [DataType(DataType.Date)]
        public DateTime? PremiumDueDate { get; set; }

        [Display(Name = "Maturity Date")]
        [DataType(DataType.Date)]
        public DateTime? MaturityDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
    public class ProfileDataDto
    {
        public string Name { get; set; }
        public string MaritalStatus { get; set; }
        public string SpouseName { get; set; }
    }
}
