using System;
using System.Collections.Generic;
using FactFinderWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FactFinderWeb.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAffAlertnesPostIncomeDetail> TblAffAlertnesPostIncomeDetails { get; set; }

    public virtual DbSet<TblFfAdminUser> TblFfAdminUsers { get; set; }

    public virtual DbSet<TblFfAlertnesGeneralInsurance> TblFfAlertnesGeneralInsurances { get; set; }

    public virtual DbSet<TblFfAlertnesLifeInsurance> TblFfAlertnesLifeInsurances { get; set; }

    public virtual DbSet<TblFfAlertnesNetWorth> TblFfAlertnesNetWorths { get; set; }

    public virtual DbSet<TblFfAlertnesNewInvestment> TblFfAlertnesNewInvestments { get; set; }

    public virtual DbSet<TblFfAlertnesNewLiability> TblFfAlertnesNewLiabilities { get; set; }

    public virtual DbSet<TblFfAlertnesNewOtherAsset> TblFfAlertnesNewOtherAssets { get; set; }

    public virtual DbSet<TblFfRegisterUser> TblFfRegisterUsers { get; set; }

    public virtual DbSet<TblffAlertnesCommittedSaving> TblffAlertnesCommittedSavings { get; set; }

    public virtual DbSet<TblffAlertnesDebt> TblffAlertnesDebts { get; set; }

    public virtual DbSet<TblffAlertnesEmiDetail> TblffAlertnesEmiDetails { get; set; }

    public virtual DbSet<TblffAlertnesExpense> TblffAlertnesExpenses { get; set; }

    public virtual DbSet<TblffAlertnesExpenseNew> TblffAlertnesExpenseNews { get; set; }

    public virtual DbSet<TblffAlertnesIncomeDetail> TblffAlertnesIncomeDetails { get; set; }

    public virtual DbSet<TblffAlertnesIncomeExtra> TblffAlertnesIncomeExtras { get; set; }

    public virtual DbSet<TblffAlertnesNewSaving> TblffAlertnesNewSavings { get; set; }

    public virtual DbSet<TblffAlertnesOneTimeLoanRepayment> TblffAlertnesOneTimeLoanRepayments { get; set; }

    public virtual DbSet<TblffAlertnesSaving> TblffAlertnesSavings { get; set; }

    public virtual DbSet<TblffAlertnesSpouseIncomeExtra> TblffAlertnesSpouseIncomeExtras { get; set; }

    public virtual DbSet<TblffAwarenessAddress> TblffAwarenessAddresses { get; set; }

    public virtual DbSet<TblffAwarenessAssumption> TblffAwarenessAssumptions { get; set; }

    public virtual DbSet<TblffAwarenessChild> TblffAwarenessChildren { get; set; }

    public virtual DbSet<TblffAwarenessFamilyFinancial> TblffAwarenessFamilyFinancials { get; set; }

    public virtual DbSet<TblffAwarenessProfileDetail> TblffAwarenessProfileDetails { get; set; }

    public virtual DbSet<TblffAwarenessProfileDetailsBak20250903> TblffAwarenessProfileDetailsBak20250903s { get; set; }

    public virtual DbSet<TblffAwarenessSpouse> TblffAwarenessSpouses { get; set; }

    public virtual DbSet<TblffInvestWingsGoal> TblffInvestWingsGoals { get; set; }

    public virtual DbSet<TblffInvestWingsGoalMaster> TblffInvestWingsGoalMasters { get; set; }

    public virtual DbSet<TblffKnowledgeRisk> TblffKnowledgeRisks { get; set; }

    public virtual DbSet<TblffPasswordResetRequest> TblffPasswordResetRequests { get; set; }

    public virtual DbSet<TblffWing> TblffWings { get; set; }

    public virtual DbSet<TblffWingsGoalMaster> TblffWingsGoalMasters { get; set; }

    public virtual DbSet<TblffWingsGoalStep5ExecutionDatum> TblffWingsGoalStep5ExecutionData { get; set; }

    public virtual DbSet<TblffWingsGoalStep5ExecutionMaster> TblffWingsGoalStep5ExecutionMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=103.205.142.104,1444;Database=Reseller_Boyinaweb_FactFinderWeb;User=Reseller_Boyinaweb_factfinderWebDB;Password=8wmKwX?25xomtVai2;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Reseller_Boyinaweb_factfinderWebDB");

        modelBuilder.Entity<TblAffAlertnesPostIncomeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblAff_A__3214EC0792E3789B");

            entity.ToTable("TblAff_Alertnes_PostIncomeDetails", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblAff_Alertnes_PostIncomeDetails_ProfileId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InflationRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MustHaveExpensesPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.OptionalExpensesPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PostIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProjectedGrowthRate6To10Years).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ProjectedGrowthRateNext5Years).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.SavingsPercent).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<TblFfAdminUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_admin_user");

            entity.ToTable("tblFF_admin_user", "dbo");

            entity.HasIndex(e => e.Email, "tblFF_admin_user_emailuqkey").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accesskey)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("accesskey");
            entity.Property(e => e.AccountStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AdminRole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("adminRole");
            entity.Property(e => e.Adminuserid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("adminuserid");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("mobile");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblFfAlertnesGeneralInsurance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblFF_Al__3214EC07E478E1C0");

            entity.ToTable("TblFF_Alertnes_GeneralInsurance", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblFF_Alertnes_GeneralInsurance_ProfileId");

            entity.Property(e => e.AmountOfCoverage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InsuranceType).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblFfAlertnesLifeInsurance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblFF_Al__3214EC07E8B0435A");

            entity.ToTable("TblFF_Alertnes_LifeInsurance", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblFF_Alertnes_LifeInsurance_ProfileId");

            entity.Property(e => e.AmountOfCoverage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InsuranceType).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<TblFfAlertnesNetWorth>(entity =>
        {
            entity.HasKey(e => e.NetWorthId).HasName("PK__TblFF_Al__C6FF9ACC189DA904");

            entity.ToTable("TblFF_Alertnes_NetWorth", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblFF_Alertnes_NetWorth_ProfileId");

            entity.Property(e => e.BusinessLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Car).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CarLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CashInHand).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CommercialProperty).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CommercialPropertyLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeProvidendFund).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FixedDeposits).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Home1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Home1Loan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Home2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Home2Loan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Jewellery).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.JewelleryLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Land).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LandLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MutualFundsShares).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OtherAssetsOther).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OtherLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OthersGratuity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidUpValueOfInsurancePolicies).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ppf)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PPF");
            entity.Property(e => e.ValueOfBusiness).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblFfAlertnesNewInvestment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblFF_Al__3214EC0721C39CD6");

            entity.ToTable("TblFF_Alertnes_NewInvestments", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblFF_Alertnes_NewInvestments_ProfileId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InvestmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblFfAlertnesNewLiability>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblFF_Al__3214EC07FE660C9F");

            entity.ToTable("TblFF_Alertnes_NewLiabilities", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblFF_Alertnes_NewLiabilities_ProfileId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LiabilityName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblFfAlertnesNewOtherAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblFF_Al__3214EC07DD372323");

            entity.ToTable("TblFF_Alertnes_NewOtherAssets", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_TblFF_Alertnes_NewOtherAssets_ProfileId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AssetName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TblFfRegisterUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_registerUser");

            entity.ToTable("tblFF_registerUser", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activestatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((1))")
                .HasColumnName("activestatus");
            entity.Property(e => e.AdvisorName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("advisorName");
            entity.Property(e => e.Advisorid).HasColumnName("advisorid");
            entity.Property(e => e.Approvedbyadmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("approvedbyadmin");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Emailverified)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emailverified");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("mobile");
            entity.Property(e => e.Mobileverified)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("mobileverified");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Plantype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("plantype");
            entity.Property(e => e.Ptx)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ptx");
            entity.Property(e => e.Updatedate)
                .HasColumnType("datetime")
                .HasColumnName("updatedate");
        });

        modelBuilder.Entity<TblffAlertnesCommittedSaving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC0726AEF2AB");

            entity.ToTable("tblff_Alertnes_CommittedSavings", "dbo");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MonthlyContribution).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TillWhen).HasColumnType("datetime");

            entity.HasOne(d => d.Savings).WithMany(p => p.TblffAlertnesCommittedSavings)
                .HasForeignKey(d => d.SavingsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommittedSavings_Savings");
        });

        modelBuilder.Entity<TblffAlertnesDebt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC070BC810D9");

            entity.ToTable("tblff_Alertnes_Debt", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_Alertnes_Debt_ProfileId");

            entity.Property(e => e.BadLoanOthers).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BusinessLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditCard).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EducationLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GoldLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GoodLoanOthers).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HomeLoan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PersonalLoan).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblffAlertnesEmiDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC074F8D2926");

            entity.ToTable("tblff_Alertnes_EMI_Details", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_Alertnes_EMI_Details_ProfileId");

            entity.Property(e => e.Interest).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsNew).HasDefaultValue(false);
            entity.Property(e => e.Monthly).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Outstanding).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Principal).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblffAlertnesExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC07CD195383");

            entity.ToTable("tblff_Alertnes_Expenses", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_Alertnes_Expenses_ProfileId");

            entity.Property(e => e.AirTrainTravel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BooksStationery)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Cable)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CarCleaning)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CarInsurance)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClothingGrooming)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClubhouseExpenses)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataCard)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DiningOut)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DomesticHelp)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Donations)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Driver)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Electricity)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FamilyFunctions)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FeeToCa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FeeToCA");
            entity.Property(e => e.Festivals)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fuel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GroceryProvisionMilk)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HomeInsurance)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HomeRepair)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HouseTax)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IronLaundry)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.KidsBirthdays)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LandlineBroadband)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LifeInsurance)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Lpg)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LPG");
            entity.Property(e => e.Maintenance)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Medical)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MedicalInsurance)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MoviesTheatre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NewHomeAppliances)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NewsPaper)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OtherConsultant)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PartiesAtHome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PicnicsActivities)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProfileId).HasColumnName("profileId");
            entity.Property(e => e.Rents)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SchoolFees)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SocietyCharge)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TaxiPublicTransport)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tuitions)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UniformsAccessories)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VacationTravel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VehicleServicing)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WaterBill)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblffAlertnesExpenseNew>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC079942092A");

            entity.ToTable("tblff_Alertnes_Expense_New", "dbo");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FieldName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FieldValue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Section)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Expense).WithMany(p => p.TblffAlertnesExpenseNews)
                .HasForeignKey(d => d.ExpenseId)
                .HasConstraintName("FK__tblff_Ale__Expen__03BB8E22");
        });

        modelBuilder.Entity<TblffAlertnesIncomeDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC07DDFB0E86");

            entity.ToTable("tblff_Alertnes_IncomeDetails", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_Alertnes_IncomeDetails_ProfileId");

            entity.Property(e => e.AnnualBonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AnnualTotalIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Basic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BusinessIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Conveyance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EducationAllowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FoodCoupon).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Gratuity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Hra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("HRA");
            entity.Property(e => e.InterestIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Lta)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("LTA");
            entity.Property(e => e.MedicalAllowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MonthlyIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MonthlyPension).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OtherAllowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PerformanceLinked).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Pf)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PF");
            entity.Property(e => e.PostItincomeNew)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PostITIncomeNew");
            entity.Property(e => e.PostItincomeOld)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PostITIncomeOld");
            entity.Property(e => e.Reimbursement).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseAnnualBonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseAnnualTotalIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseBasic).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseBusinessIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseConsolidatedIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseConveyance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseEducationAllowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseFoodCoupon).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseGratuity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseHra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SpouseHRA");
            entity.Property(e => e.SpouseInterestIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseLta)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SpouseLTA");
            entity.Property(e => e.SpouseMedicalAllowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseMonthlyPension).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseMonthlyTotalIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseOtherAllowance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpouseOverallMonthlyIncome).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpousePerformanceLinked).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpousePf)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SpousePF");
            entity.Property(e => e.SpousePostItincomeNew)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SpousePostITIncomeNew");
            entity.Property(e => e.SpousePostItincomeOld)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SpousePostITIncomeOld");
            entity.Property(e => e.SpouseReimbursement).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalExpense).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblffAlertnesIncomeExtra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC07A54711E8");

            entity.ToTable("tblff_Alertnes_IncomeExtras", "dbo");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FieldName).HasMaxLength(100);
            entity.Property(e => e.FieldValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<TblffAlertnesNewSaving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC072D88109D");

            entity.ToTable("tblff_Alertnes_NewSavings", "dbo");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CurrentValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MonthlyContribution).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TillWhen).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblffAlertnesOneTimeLoanRepayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC0711EAC352");

            entity.ToTable("tblff_Alertnes_OneTimeLoanRepayment", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_Alertnes_OneTimeLoanRepayment_ProfileId");

            entity.Property(e => e.NetCashflows).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OneTimeLoanRepayment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalEmi).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblffAlertnesSaving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC07B947ED12");

            entity.ToTable("tblff_Alertnes_Savings", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_Alertnes_Savings_ProfileId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalCommittedSavings).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<TblffAlertnesSpouseIncomeExtra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Al__3214EC0775690F1B");

            entity.ToTable("tblff_Alertnes_SpouseIncomeExtras", "dbo");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FieldName).HasMaxLength(100);
            entity.Property(e => e.FieldValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<TblffAwarenessAddress>(entity =>
        {
            entity.ToTable("tblff_awareness_address", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_awareness_address_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.AddressType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("addressType");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Company)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.CountryCode).HasColumnName("countryCode");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.PinCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pinCode");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.StateCode).HasColumnName("stateCode");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffAwarenessAssumption>(entity =>
        {
            entity.ToTable("tblff_awareness_assumptions", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_awareness_assumptions_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicantLifeExpectancy).HasColumnName("applicantLifeExpectancy");
            entity.Property(e => e.ApplicantRetirement).HasColumnName("applicantRetirement");
            entity.Property(e => e.Debt).HasColumnName("debt");
            entity.Property(e => e.EducationInflation).HasColumnName("educationInflation");
            entity.Property(e => e.Equity).HasColumnName("equity");
            entity.Property(e => e.Gold).HasColumnName("gold");
            entity.Property(e => e.InflationRates).HasColumnName("inflationRates");
            entity.Property(e => e.LiquidFunds).HasColumnName("liquidFunds");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.RealEstateReturn).HasColumnName("realEstateReturn");
            entity.Property(e => e.SpouseLifeExpectancy).HasColumnName("spouseLifeExpectancy");
            entity.Property(e => e.SpouseRetirement).HasColumnName("spouseRetirement");
        });

        modelBuilder.Entity<TblffAwarenessChild>(entity =>
        {
            entity.ToTable("tblff_awareness_children", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_awareness_children_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChildAadhaar)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("childAadhaar");
            entity.Property(e => e.ChildDob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("childDob");
            entity.Property(e => e.ChildEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("childEmail");
            entity.Property(e => e.ChildGender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("childGender");
            entity.Property(e => e.ChildName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("childName");
            entity.Property(e => e.ChildPan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("childPan");
            entity.Property(e => e.ChildPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("childPhone");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffAwarenessFamilyFinancial>(entity =>
        {
            entity.ToTable("tblff_awareness_FamilyFinancial", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_awareness_FamilyFinancial_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addby)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("addby");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Holiday)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("holiday");
            entity.Property(e => e.Income)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("income");
            entity.Property(e => e.Payment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.Shopping)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shopping");
            entity.Property(e => e.Stock)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stock");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffAwarenessProfileDetail>(entity =>
        {
            entity.HasKey(e => e.ProfileId);

            entity.ToTable("tblff_awareness_profileDetails", "dbo", tb => tb.HasTrigger("trg_SetProfileUId"));

            entity.Property(e => e.Aadhaar)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("aadhaar");
            entity.Property(e => e.Addby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("addby");
            entity.Property(e => e.Addbyid).HasColumnName("addbyid");
            entity.Property(e => e.AdvisorName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Altphone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("altphone");
            entity.Property(e => e.Awakenid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("awakenid");
            entity.Property(e => e.CompanyAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("companyAddress");
            entity.Property(e => e.CompanyCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyCity");
            entity.Property(e => e.CompanyCountry)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyCountry");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("companyName");
            entity.Property(e => e.CompanyPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyPINcode");
            entity.Property(e => e.CompanyState)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyState");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Dob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dob");
            entity.Property(e => e.Education)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("education");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.HaveChildren)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("haveChildren");
            entity.Property(e => e.Hobbies)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Holiday)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("holiday");
            entity.Property(e => e.Income)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("income");
            entity.Property(e => e.IsSameAddress).HasDefaultValue(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maritalStatus");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Occupation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("occupation");
            entity.Property(e => e.Pan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pan");
            entity.Property(e => e.Payment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment");
            entity.Property(e => e.PermAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PermCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermCountry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PermPINcode");
            entity.Property(e => e.PermState)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PlanDuration)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("planDuration");
            entity.Property(e => e.PlanType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("planType");
            entity.Property(e => e.PlanYear).HasColumnName("planYear");
            entity.Property(e => e.ProfileStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Registerid).HasColumnName("registerid");
            entity.Property(e => e.ResAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ResCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResCountry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ResPINcode");
            entity.Property(e => e.ResState)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SecEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("secEmail");
            entity.Property(e => e.Shopping)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shopping");
            entity.Property(e => e.Stock)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stock");
            entity.Property(e => e.Uid)
                .HasMaxLength(100)
                .HasColumnName("UId");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffAwarenessProfileDetailsBak20250903>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblff_awareness_profileDetails_bak_20250903", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_awareness_profileDetails_bak_20250903_ProfileId");

            entity.Property(e => e.Aadhaar)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("aadhaar");
            entity.Property(e => e.Addby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("addby");
            entity.Property(e => e.Addbyid).HasColumnName("addbyid");
            entity.Property(e => e.AdvisorName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Altphone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("altphone");
            entity.Property(e => e.Awakenid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("awakenid");
            entity.Property(e => e.CompanyAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("companyAddress");
            entity.Property(e => e.CompanyCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyCity");
            entity.Property(e => e.CompanyCountry)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyCountry");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("companyName");
            entity.Property(e => e.CompanyPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyPINcode");
            entity.Property(e => e.CompanyState)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("companyState");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Dob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dob");
            entity.Property(e => e.Education)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("education");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.HaveChildren)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("haveChildren");
            entity.Property(e => e.Hobbies)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Holiday)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("holiday");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Income)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("income");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("maritalStatus");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Occupation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("occupation");
            entity.Property(e => e.Pan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pan");
            entity.Property(e => e.Payment)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment");
            entity.Property(e => e.PermAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PermCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermCountry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PermPINcode");
            entity.Property(e => e.PermState)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PlanDuration)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("planDuration");
            entity.Property(e => e.PlanType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("planType");
            entity.Property(e => e.PlanYear).HasColumnName("planYear");
            entity.Property(e => e.ProfileStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.ResAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ResCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResCountry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ResPINcode");
            entity.Property(e => e.ResState)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SecEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("secEmail");
            entity.Property(e => e.Shopping)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shopping");
            entity.Property(e => e.Stock)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stock");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffAwarenessSpouse>(entity =>
        {
            entity.ToTable("tblff_awareness_spouse", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_awareness_spouse_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.SpouseAadhaar)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("spouseAadhaar");
            entity.Property(e => e.SpouseAltPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("spouseAltPhone");
            entity.Property(e => e.SpouseCompanyAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpouseCompanyCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpouseCompanyCountry)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpouseCompanyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpouseCompanyPincode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SpouseCompanyPINcode");
            entity.Property(e => e.SpouseCompanyState)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SpouseDob)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("spouseDob");
            entity.Property(e => e.SpouseEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("spouseEmail");
            entity.Property(e => e.SpouseGender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("spouseGender");
            entity.Property(e => e.SpouseName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("spouseName");
            entity.Property(e => e.SpouseOccupation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("spouseOccupation");
            entity.Property(e => e.SpousePan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("spousePan");
            entity.Property(e => e.SpousePhone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("spousePhone");
            entity.Property(e => e.SpouseSecEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("spouseSecEmail");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffInvestWingsGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_wingsGoalStep6Invest");

            entity.ToTable("tblff_InvestWingsGoal", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_InvestWingsGoal_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.GoalName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalName");
            entity.Property(e => e.Goalid).HasColumnName("goalid");
            entity.Property(e => e.LumpsumAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.Sipamount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SIPAmount");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffInvestWingsGoalMaster>(entity =>
        {
            entity.ToTable("tblff_InvestWingsGoalMaster", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_InvestWingsGoalMaster_ProfileId");

            entity.HasIndex(e => e.ProfileId, "profileid_uqkey").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addby).HasColumnName("addby");
            entity.Property(e => e.AvailableLumpsum)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.IntendedSipmonthly)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("IntendedSIPmonthly");
            entity.Property(e => e.MonthlySavings)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffKnowledgeRisk>(entity =>
        {
            entity.ToTable("tblff_KnowledgeRisk", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_KnowledgeRisk_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("addBy");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.PlannerAssessmentOnRiskProfile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("plannerAssessmentOnRiskProfile");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.RiskCapacity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("riskCapacity");
            entity.Property(e => e.RiskRequirement)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("riskRequirement");
            entity.Property(e => e.RiskTolerance)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("riskTolerance");
            entity.Property(e => e.TotalRiskProfileScore)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("totalRiskProfileScore");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffPasswordResetRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblff_Pa__3214EC076447E1CD");

            entity.ToTable("tblff_PasswordResetRequests", "dbo");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.IsUsed).HasDefaultValue(false);
            entity.Property(e => e.Token).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<TblffWing>(entity =>
        {
            entity.ToTable("tblff_wings", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_wings_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.GoalEndYear).HasColumnName("goalEndYear");
            entity.Property(e => e.GoalName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalName");
            entity.Property(e => e.GoalPlanYear).HasColumnName("goalPlanYear");
            entity.Property(e => e.GoalPriority).HasColumnName("goalPriority");
            entity.Property(e => e.GoalStartYear).HasColumnName("goalStartYear");
            entity.Property(e => e.GoalType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalType");
            entity.Property(e => e.Goalid).HasColumnName("goalid");
            entity.Property(e => e.NewGoals).HasColumnName("newGoals");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.TimeHorizon).HasColumnName("timeHorizon");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffWingsGoalMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_wingsGoalMaster");

            entity.ToTable("tblff_wingsGoalMaster", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.GoalName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalName");
            entity.Property(e => e.GoalPriority).HasColumnName("goalPriority");
            entity.Property(e => e.GoalSequence).HasColumnName("goalSequence");
            entity.Property(e => e.GoalType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalType");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        modelBuilder.Entity<TblffWingsGoalStep5ExecutionDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_wingsGoalStep5ExecutionData");

            entity.ToTable("tblff_wingsGoalStep5ExecutionData", "dbo");

            entity.HasIndex(e => e.ProfileId, "IX_tblff_wingsGoalStep5ExecutionData_ProfileId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.ExecutionDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("executionDescription");
            entity.Property(e => e.ExecutionValue)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionValue");
            entity.Property(e => e.ExecutionValueType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionValueType");
            entity.Property(e => e.GoalName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalName");
            entity.Property(e => e.Goalid).HasColumnName("goalid");
            entity.Property(e => e.ProfileId).HasColumnName("profileid");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.Wingid).HasColumnName("wingid");
        });

        modelBuilder.Entity<TblffWingsGoalStep5ExecutionMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_wingsGoalStep5ExecutionMaster");

            entity.ToTable("tblff_wingsGoalStep5ExecutionMaster", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.DealType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("dealtype");
            entity.Property(e => e.ExecutionDescription)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("executionDescription");
            entity.Property(e => e.ExecutionDescriptionName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("executionDescriptionName");
            entity.Property(e => e.ExecutionDescriptionOrder).HasColumnName("executionDescriptionOrder");
            entity.Property(e => e.ExecutionValueType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("executionValueType");
            entity.Property(e => e.GoalName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalName");
            entity.Property(e => e.GoalType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("goalType");
            entity.Property(e => e.Goalid).HasColumnName("goalid");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
