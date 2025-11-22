namespace FactFinderWeb.ModelsView
{
    public class RootAwareness
    {
        public AwakenSection Awaken { get; set; } = new AwakenSection();
    }

    public class AwakenSection
    {
        public AwarenessSection Awareness { get; set; } = new AwarenessSection();
        public WingsSection Wings { get; set; } = new WingsSection();
        public AlertnessRoot Alertness { get; set; } = new AlertnessRoot();
        public KnowledgeSection Knowledge { get; set; } = new KnowledgeSection();
        public ExecutePlanSection ExecutePlan { get; set; } = new ExecutePlanSection();
        public InvestSection Invest { get; set; } = new InvestSection();

        public long ProfileId { get; set; }
    }
    public class AwarenessSection
        {

        
             public string Advisor { get; set; }
        public PlanDetails planDetails { get; set; }
            public PersonalDetails personalDetails { get; set; }
            public FamilyFinancial familyFinancial { get; set; }
            public MaritalDetails1 maritalDetails { get; set; }
            public Assumptions assumptions { get; set; }
        }

        #region Plan Details
        public class PlanDetails
        {
            public string PlanType { get; set; }
            public int PlanYear { get; set; }
            public string PlanDuration { get; set; }
        }
        #endregion

        #region Personal Details
        public class PersonalDetails
        {
            public string Name { get; set; }
        public string Advisor { get; set; }
        
            public string Gender { get; set; }
            public string Dob { get; set; }
            public string Phone { get; set; }
            public string AltPhone { get; set; }
            public string Aadhaar { get; set; }
            public string Email { get; set; }
            public string SecEmail { get; set; }
            public string Pan { get; set; }
            public string Occupation { get; set; }

            public AddressGroup address { get; set; }
        }
        #endregion

        #region Address Group
        public class AddressGroup
        {
            public ResidentialAddress residential { get; set; }
            public OfficeAddress office { get; set; }
            public PermanentAddress permanent { get; set; }
            public bool? IsSameAddress { get; set; }
        }

        public class ResidentialAddress
        {
            public string Address { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }
            public string City { get; set; }
            public string PinCode { get; set; }
        }
    public class WingsSection
    {
        public List<GoalForm> Forms { get; set; }
        public List<SelectedGoal> SelectedForms { get; set; }
        public ExpectedReturns ExpectedReturns { get; set; } = new ExpectedReturns();
    }

    public class GoalForm
    {
        public int Id { get; set; }
        public string Goal { get; set; }
    }

    public class SelectedGoal
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Goal { get; set; }
        public int? GoalStartYear { get; set; }
        public int? GoalPlanYear { get; set; }
        public int? GoalEndYear { get; set; }
        public int? TimeHorizon { get; set; }
        public bool ? NewGoals { get; set; }
    }
    public class OfficeAddress
        {
            public string Company { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }
            public string City { get; set; }
            public string PinCode { get; set; }
        }

        public class PermanentAddress
        {
            public string Address { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }
            public string City { get; set; }
            public string PinCode { get; set; }
        }
    #endregion
    public class MaritalDetails1
    {
        public string MaritalStatus { get; set; }
        public SpouseDetails spouseDetails { get; set; }
        public string haveChildren { get; set; }
        public List<ChildDetails> childrenDetails { get; set; }
    }
    public class AlertnessRoot
    {
        public Alertness alertness { get; set; }
    }

    public class Alertness
    {
        public IncomeDetail incomeDetail { get; set; }
        public Expenses expenses { get; set; }
        public Savings savings { get; set; }
        public Emi emi { get; set; }
        public PostIncomeDetails postIncomeDetails { get; set; }
        public NetWorth netWorth { get; set; }
        public Debt debt { get; set; }
        public LifeInsurance lifeInsurance { get; set; }
        public GeneralInsurance generalInsurance { get; set; }
    }
  

 

    public class IncomeDetail
    {
        public Income income { get; set; }
        public SpouseIncome spouseIncome { get; set; }
    }

    public class Income
    {
        public double? PostITIncomeOld { get; set; }
        public double? TotalExpense { get; set; }
        public double?  basic { get; set; }
        public double?  HRA { get; set; }
        public double?  educationAllowance { get; set; }
        public double?  medicalAllowance { get; set; }
        public double?  LTA { get; set; }
        public double?  conveyance { get; set; }
        public double?  otherAllowance { get; set; }
        public double?  PF { get; set; }
        public double?  gratuity { get; set; }
        public double?  reimbursement { get; set; }
        public double?  businessIncome { get; set; }
        public double?  foodCoupon { get; set; }
        public double?  monthlyPension { get; set; }
        public double?  interestIncome { get; set; }
        public double?  annualBonus { get; set; }
        public double?  performanceLinked { get; set; }
        public List<Dictionary<string, double?>> newIncomes { get; set; }
        public List<Dictionary<string, double?>> annualIncome { get; set; }

    }

    public class NewIncome
    {
        public string Name { get; set; }
        public decimal? Value { get; set; }
    }

    public class AnnualIncome
    {
        public string Name { get; set; }
        public decimal? Value { get; set; }
    }

    public class SpouseIncome
    {
        public double?  spouseBasic { get; set; }
        public double?  spouseHRA { get; set; }
        public double? SpousePostITIncomeOld { get; set; }
        public double?  spouseEducationAllowance { get; set; }
        public double?  spouseMedicalAllowance { get; set; }
        public double?  spouseLTA { get; set; }
        public double?  spouseConveyance { get; set; }
        public double?  spouseOtherAllowance { get; set; }
        public double?  spousePF { get; set; }
        public double?  spouseGratuity { get; set; }
        public double?  spouseReimbursement { get; set; }
        public double?  spouseBusinessIncome { get; set; }
        public double?  spouseFoodCoupon { get; set; }
        public double?  spouseInterestIncome { get; set; }
        public double? spouseMonthlyPension { get; set; }
        public double?  spouseMonthlyTotalIncome { get; set; }
        public double? spouseConsolidatedIncome { get; set; }
        public double?  spouseAnnualBonus { get; set; }
        public double?  spousePerformanceLinked { get; set; }
        public double? spouseAnnualTotalIncome { get; set; }
        public double? spouseOverallMonthlyIncome { get; set; }
        public double? spousePostITIncomeOld { get; set; }
        public double? spousePostITIncomeNew { get; set; }

        public List<Dictionary<string, double?>> annualSpouseIncome { get; set; }
        public List<Dictionary<string, double?>> newSpouseIncomes { get; set; }
    }

    public class NewSpouseIncome
    {
        public double?  vv { get; set; }
    }

    public class Expenses
    {
        public HomeExpenses homeExpenses { get; set; } = new HomeExpenses();
        public Conveyance conveyance { get; set; } = new Conveyance();
        public Communication communication { get; set; } = new Communication();
        public Utilities utilities { get; set; } = new Utilities();
        public EducationalExpenses educationalExpenses { get; set; } = new EducationalExpenses();
        public Medical medical { get; set; } = new Medical();
        public EntertainmentRecreation entertainmentRecreation { get; set; } = new EntertainmentRecreation();
        public NewReplacementItems newReplacementItems { get; set; } = new NewReplacementItems();
        public InsuranceExpenses insurance { get; set; } = new InsuranceExpenses();
        public OtherExpenses otherExpenses { get; set; } = new OtherExpenses();
    }
    public class EntertainmentRecreation
    {
        public double?  MoviesTheatre { get; set; }
        public double?  DiningOut { get; set; }
        public double?  ClubhouseExpenses { get; set; }
        public double?  PartiesAtHome { get; set; }
        public double?  ClothingGrooming { get; set; }
        public double?  VacationTravel { get; set; }
        public double?  Festivals { get; set; }
        public double?  KidsBirthdays { get; set; }
        public double?  FamilyFunctions { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

    public class NewReplacementItems
    {
        public double?  VehicleServicing { get; set; }
        public double?  HomeRepair { get; set; }
        public double?  NewHomeAppliances { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

    public class InsuranceExpenses
    {
        public double?  LifeInsurance { get; set; }
        public double?  HomeInsurance { get; set; }
        public double?  MedicalInsurance { get; set; }
        public double?  CarInsurance { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

    public class OtherExpenses
    {
        public double?  feeToCA { get; set; }
        public double?  otherConsultant { get; set; }
        public double?  donations { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }
    public class HomeExpenses
    {
        public double?  groceryProvisionMilk { get; set; }
        public double?  domesticHelp { get; set; }
        public double?  ironLaundry { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }

    }

public class Conveyance
{
    public double?  driver { get; set; }
    public double?  fuel { get; set; }
    public double?  carCleaning { get; set; }
    public double?  maintenance { get; set; }
    public double?  taxiPublicTransport { get; set; }
    public double?  airTrainTravel { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

public class Communication
{
    public double?  mobile { get; set; }
    public double?  landlineBroadband { get; set; }
    public double?  dataCard { get; set; }
       public List<Dictionary<string, double?>> newAdd { get; set; }
    }

public class Utilities
{
    public double?  electricity { get; set; }
    public double?  houseTax { get; set; }
    public double?  societyCharge { get; set; }
    public double?  rents { get; set; }
    public double?  cable { get; set; }
    public double?  LPG { get; set; }
    public double?  waterBill { get; set; }
    public double?  newsPaper { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

public class EducationalExpenses
{
    public double?  schoolFees { get; set; }
    public double?  tuitions { get; set; }
    public double?  uniformsAccessories { get; set; }
    public double?  booksStationery { get; set; }
    public double?  picnicsActivities { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

public class Medical
{
    public double?  medical { get; set; }
        public List<Dictionary<string, double?>> newAdd { get; set; }
    }

public class NewExpense
{
    public int a { get; set; }
    public int b { get; set; }
    public int c { get; set; }
    public int d { get; set; }
    public int e { get; set; }
}

public class Savings
{
    public List<CommittedSaving> committedSavings { get; set; }
        public List<CommittedSaving> newAdd { get; set; }
        public double? totalCommittedSavings { get; set; }
}

public class CommittedSaving
{
    public string name { get; set; }
    public double? currentValue { get; set; }
    public double? monthlyContribution { get; set; }
    public string tillWhen { get; set; }
    public string followUp { get; set; }
}

public class Emi
{
    public List<EmiDetail1> details { get; set; }
    public double? totalEMI { get; set; }
    public double? oneTimeLoanRepay { get; set; }
}

public class EmiDetail1
{
    public string Name { get; set; }
    public double? Outstanding { get; set; }
    public double? Interest { get; set; }
    public double? Principal { get; set; }
    public double? Monthly { get; set; }
    public string Till { get; set; }
    public string FollowUp { get; set; }
}

public class PostIncomeDetails
{
    public double? PostIncome { get; set; }
    public double?  MustHaveExpensesPercent { get; set; }
    public double?  OptionalExpensesPercent { get; set; }
    public double?  SavingsPercent { get; set; }
    public double?  ProjectedGrowthRateNext5Years { get; set; }
    public double?  ProjectedGrowthRate6To10Years { get; set; }
    public double?  InflationRate { get; set; }
}

public class NetWorth
{
    public Investments investments { get; set; }
    public List<NewInvestment> newInvestments { get; set; }
    public OtherAssets otherAssets { get; set; }
    public List<NewOtherAsset> newOtherAssets { get; set; }
    public Liabilities liabilities { get; set; }
    public List<NewLiability> newLiabilities { get; set; }
}
    public class Investments
    {
        public double?  CashInHand { get; set; }
        public double?  EmployeeProvidendFund { get; set; }
        public double?  PPF { get; set; }
        public double?  FixedDeposits { get; set; }
        public double?  MutualFundsShares { get; set; }
        public double?  PaidUpValueOfInsurancePolicies { get; set; }
        public double?  OthersGratuity { get; set; }
    }

    public class NewInvestment
    {
        public string InvestmentName { get; set; }
        public double? Amount { get; set; }
    }

    public class OtherAssets
    {
        public double?  Home1 { get; set; }
        public double?  Home2 { get; set; }
        public double?  Land { get; set; }
        public double?  Car { get; set; }
        public double?  CommercialProperty { get; set; }
        public double?  Jewellery { get; set; }
        public double?  ValueOfBusiness { get; set; }
        public double? Other { get; set; }
    }

    public class NewOtherAsset
    {
        public string AssetName { get; set; }
        public double?  Amount { get; set; }
    }

    public class Liabilities
    {
        public double?  Home1Loan { get; set; }
        public double?  Home2Loan { get; set; }
        public double?  CarLoan { get; set; }
        public double?  LandLoan { get; set; }
        public double?  CommercialPropertyLoan { get; set; }
        public double?  JewelleryLoan { get; set; }
        public double?  BusinessLoan { get; set; }
        public double?  OtherLoan { get; set; }
    }

    public class NewLiability
    {
        public string LiabilityName { get; set; }
        public double?  Amount { get; set; }
    }

    public class Debt
    {
        public BadLoans BadLoans { get; set; }
        public GoodLoans GoodLoans { get; set; }
    }

    public class BadLoans
    {
        public double?  GoldLoan { get; set; }
        public double?  CreditCard { get; set; }
        public double?  PersonalLoan { get; set; }
        public double?  BadLoanOthers { get; set; }
    }


    public class GoodLoans
{
    public double?  HomeLoan { get; set; }
    public double?  educationLoan { get; set; }
    public double?  businessLoan { get; set; }
    public double?  goodLoanOthers { get; set; }
}

public class LifeInsurance
{
    public List<Insurance> insurance { get; set; }
}

public class Insurance
{
    public string insuranceType { get; set; }
    public string name { get; set; }
    public double?  amountOfCoverage { get; set; }
    public string premiumDueDate { get; set; }
    public string maturityDate { get; set; }
}

public class GeneralInsurance
{
    public List<Insurance> insurance { get; set; }
}
    public class KnowledgeSection
    {
        public KnowledgeData Knowledge { get; set; } = new KnowledgeData();
    }

    public class KnowledgeData
    {
        public string RiskCapacity { get; set; } = string.Empty;
        public string RiskRequirement { get; set; } = string.Empty;
        public string TotalRiskProfileScore { get; set; } = string.Empty;
        public string PlannerAssessmentOnRiskProfile { get; set; } = string.Empty;
        public string RiskTolerance { get; set; } = string.Empty;
    }
    public class ExecutePlanSection
    {
        public FormData FormData { get; set; } = new FormData();
    }

    public class FormData
    {
        public LifeInsuranceData LifeInsurance { get; set; } = new LifeInsuranceData();
        public SpouseExecutionData SpouseDetails { get; set; } = new SpouseExecutionData();
        public GoalNameData GoalNames { get; set; } = new GoalNameData();
        public List<GoalData> GoalData { get; set; } = new List<GoalData>();
    }

    public class LifeInsuranceData
    {
        public string IncomeUsedForFamily { get; set; } = string.Empty;
        public string FundsReturnRate { get; set; } = string.Empty;
        public string InflationRate { get; set; } = string.Empty;
    }

    public class SpouseExecutionData
    {
        public string SLiabilities { get; set; } = string.Empty;
    }

    public class GoalNameData
    {
        public List<string> GoalsNameList { get; set; } = new List<string>();
    }

    public class GoalData
    {
        public string Name { get; set; } = string.Empty;
        public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
    }


    public class InvestSection
    {
        public WingsSection Wings { get; set; } = new WingsSection();
        public AlertnessSectionForInvest Alertness { get; set; } = new AlertnessSectionForInvest();
        public KnowledgeSectionForInvest Knowledge { get; set; } = new KnowledgeSectionForInvest();
        public ExecuteWithPrecision ExecuteWithPrecision { get; set; } = new ExecuteWithPrecision();
        public ActNowSection ActNow { get; set; } = new ActNowSection();

        public ExpectedReturns ExpectedReturns { get; set; } = new ExpectedReturns();
    }


    public class ExpectedReturns
    {
        public List<string> GoalsNameList { get; set; } = new List<string>();
    }

    public class AlertnessSectionForInvest
    {
        public string FamilyMonthlyIncomeTotal { get; set; } = string.Empty;
        public string FamilyMonthlyExpensesTotal { get; set; } = string.Empty;
    }

    public class KnowledgeSectionForInvest
    {
        public string SelectedRiskProfile { get; set; } = string.Empty;
    }

    public class ExecuteWithPrecision
    {
        public HofInsurance HofInsurance { get; set; } = new HofInsurance();
    }

    public class HofInsurance
    {
        public InsuranceCoverage HomeInsurance { get; set; } = new InsuranceCoverage();
        public InsuranceCoverage CarInsurance { get; set; } = new InsuranceCoverage();
    }

    public class InsuranceCoverage
    {
        public string CurrentCoverage { get; set; } = string.Empty;
        public string RequiredCoverage { get; set; } = string.Empty;
        public string Shortfall { get; set; } = string.Empty;
    }

    public class ActNowSection
    {
        public SavingsForInvestments SavingsForInvestments { get; set; } = new SavingsForInvestments();
        public List<InsuranceRequirement> LifeInsuranceReq { get; set; } = new List<InsuranceRequirement>();
        public List<InsuranceRequirement> GeneralInsuranceReq { get; set; } = new List<InsuranceRequirement>();
        public ActionPlanFinancialGoals ActionPlanFinancialGoals { get; set; } = new ActionPlanFinancialGoals();
        public GoalNames GoalStatusReport { get; set; } = new GoalNames();
        public TotalGoalStatusReport TotalGoalStatusReport { get; set; } = new TotalGoalStatusReport();
        public GoalNames GoalsOptions { get; set; } = new GoalNames();
        public List<InvestGoal> Earmarked { get; set; } = new List<InvestGoal>();
    }

    public class SavingsForInvestments
    {
        public decimal IntendedSipMonthly { get; set; }
        public decimal AvailableLumpsum { get; set; }
        public decimal MonthlySavings { get; set; }
        
    }

    public class InsuranceRequirement
    {
        public long Id { get; set; }
        public string GoalName { get; set; } = string.Empty;
        public decimal LumpsumAmount { get; set; }
        public decimal SipAmount { get; set; }
    }

    public class InvestGoal
    {
        public long Id { get; set; }
        public string EarmarkedForGoal { get; set; } = string.Empty;
        public List<string> GoalOptions { get; set; } = new List<string>();
        public string TypeOfFund { get; set; } = string.Empty;
        public string SelectedFunds { get; set; } = string.Empty;
        public decimal LumpSum { get; set; }
        public decimal SIP { get; set; }
        public List<string> SelectedFundsOptions { get; set; } = new List<string>();
    }

    public class ActionPlanFinancialGoals
    {
        public GoalNames Goals { get; set; } = new GoalNames();
        public GoalTotals Total { get; set; } = new GoalTotals();
    }

    public class GoalNames
    {
        public List<string> GoalsNameList { get; set; } = new List<string>();
    }

    public class GoalTotals
    {
        public string TotalLumpSum { get; set; } = string.Empty;
        public string TotalFixOrSipAmt { get; set; } = string.Empty;
        public string TotalSipAmt { get; set; } = string.Empty;
        public string TotalGapBtnSip { get; set; } = string.Empty;
    }

    public class TotalGoalStatusReport
    {
        public string TotalFutureValueCurrentCorpus { get; set; } = string.Empty;
        public string TotalFutureValueLumpSum { get; set; } = string.Empty;
        public string TotalFutureValueSip { get; set; } = string.Empty;
        public string TotalTotalCorpus { get; set; } = string.Empty;
        public string TotalReqCorpus { get; set; } = string.Empty;
    }
}

