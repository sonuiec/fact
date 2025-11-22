using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FactFinderWeb.Services
{
    public class AlertnessMappingService
    {
        private ResellerBoyinawebFactFinderWebContext _context;
        public AlertnessMappingService(ResellerBoyinawebFactFinderWebContext context)
        {
            _context = context;
        }



        public async Task<int> UpdateSavingsAndCommittedAsync(AlertnessViewModel model, TblffAlertnesIncomeDetail incomeDetail)
        {
            if (model.Savings == null)
                return 0;

            var existingSaving = _context.TblffAlertnesSavings
                                         .FirstOrDefault(x => x.ProfileId == incomeDetail.ProfileId);

            if (existingSaving != null)
            {
                existingSaving.ProfileId = incomeDetail.ProfileId;
                existingSaving.Id = model.Savings.Id;
                existingSaving.TotalCommittedSavings = Convert.ToDecimal(model.Savings.TotalCommittedSavings ?? 0);

                // Update existing committed saving
                var existingCommittedSaving = _context.TblffAlertnesCommittedSavings
                                                      .FirstOrDefault(x => x.SavingsId == existingSaving.Id);

                if (existingCommittedSaving != null && model.CommittedSaving != null)
                {
                    existingCommittedSaving.SavingsId = existingSaving.Id;
                    existingCommittedSaving.Name = model.CommittedSaving.Name ?? string.Empty;
                    existingCommittedSaving.CurrentValue = Convert.ToDecimal(model.CommittedSaving.CurrentValue ?? 0);
                    existingCommittedSaving.MonthlyContribution = Convert.ToDecimal(model.CommittedSaving.MonthlyContribution ?? 0);
                    existingCommittedSaving.FollowUp = model.CommittedSaving.FollowUp;
                    existingCommittedSaving.TillWhen = ParseDateDdMmYyyy(model.CommittedSaving.TillWhen);
                }

                // Remove existing NewSavings
                var oldNewSavings = _context.TblffAlertnesNewSavings
                                            .Where(x => x.SavingsId == existingSaving.Id)
                                            .ToList();

                if (oldNewSavings.Any())
                {
                    _context.TblffAlertnesNewSavings.RemoveRange(oldNewSavings);
                }



                await _context.SaveChangesAsync();
            }
            return existingSaving.Id;
        }



        public async Task<int> SaveSavingsAsync(AlertnessViewModel model)
        {
            if (model.Savings == null || model.ProfileId == null)
                throw new ArgumentException("Invalid savings or profile ID");

            var newSaving = new TblffAlertnesSaving
            {
                ProfileId = model.ProfileId.Value,
                TotalCommittedSavings = model.Savings.TotalCommittedSavings
            };

            _context.TblffAlertnesSavings.Add(newSaving);
            await _context.SaveChangesAsync();

            return newSaving.Id;
        }


        public async Task SaveCommittedSavingAsync(CommittedSavingViewModel? committedSavingModel, int savingId)
        {
            if (committedSavingModel == null)
                return;

            var newCommittedSaving = new TblffAlertnesCommittedSaving
            {
                SavingsId = savingId,
                Name = committedSavingModel.Name ?? "",
                CurrentValue = committedSavingModel.CurrentValue ?? 0,
                MonthlyContribution = committedSavingModel.MonthlyContribution ?? 0,
                TillWhen = ParseDateDdMmYyyy(committedSavingModel.TillWhen)
            };

            _context.TblffAlertnesCommittedSavings.Add(newCommittedSaving);
            await _context.SaveChangesAsync();
        }



        public async Task SaveNewSavingsAsync(List<TblffAlertnesNewSaving>? newSavings, int savingId)
        {
            if (newSavings == null || !newSavings.Any())
                return; // Nothing to save

            // Set SavingsId to associate with parent saving record
            foreach (var saving in newSavings)
            {
                saving.SavingsId = savingId;
                saving.Name = saving.Name;
                saving.CurrentValue = saving.CurrentValue;
                saving.MonthlyContribution = saving.MonthlyContribution;
                saving.TillWhen = saving.TillWhen;
                saving.FollowUp = saving.FollowUp;
                saving.CreatedAt = DateTime.UtcNow;

            }

            await _context.TblffAlertnesNewSavings.AddRangeAsync(newSavings);
            await _context.SaveChangesAsync();
        }


        //Stpe 4 EMI
        public async Task<int> UpdateEMIDetilsAsync(AlertnessViewModel model, TblffAlertnesIncomeDetail incomeDetail)
        {
            if (model.EmiOneTimeLoanRepayment == null)
                return 0;

            var existingEmiOneTimeLoanRepayment = await _context.TblffAlertnesOneTimeLoanRepayments
                                                                .FirstOrDefaultAsync(x => x.ProfileId == incomeDetail.ProfileId);

            if (existingEmiOneTimeLoanRepayment != null)
            {
                existingEmiOneTimeLoanRepayment.TotalEmi = model.EmiOneTimeLoanRepayment.TotalEmi;
                existingEmiOneTimeLoanRepayment.NetCashflows = model.EmiOneTimeLoanRepayment.NetCashflows;
                existingEmiOneTimeLoanRepayment.OneTimeLoanRepayment = model.EmiOneTimeLoanRepayment.OneTimeLoanRepayment;

                // Remove existing EMI details
                var oldEmiDetails = await _context.TblffAlertnesEmiDetails
                                                  .Where(x => x.ProfileId == incomeDetail.ProfileId)
                                                  .ToListAsync();

                if (oldEmiDetails.Any())
                {
                    _context.TblffAlertnesEmiDetails.RemoveRange(oldEmiDetails);
                }



                await _context.SaveChangesAsync();
                return existingEmiOneTimeLoanRepayment.Id;
            }
            return 0;
        }

        public async Task AddEmiDetailsAsync(List<TblffAlertnesEmiDetail>? emiDetails, long profileId)
        {
            if (emiDetails == null || !emiDetails.Any())
                return;

            foreach (var emiDetail in emiDetails)
            {
                emiDetail.ProfileId = profileId;
                emiDetail.Name = emiDetail.Name;
                emiDetail.Outstanding = emiDetail.Outstanding;
                emiDetail.Interest = emiDetail.Interest;
                emiDetail.Principal = emiDetail.Principal;
                emiDetail.FollowUp = emiDetail.FollowUp;
                emiDetail.Monthly = emiDetail.Monthly;
                emiDetail.IsNew = false;
                emiDetail.Till = emiDetail.Till;

            }

            await _context.TblffAlertnesEmiDetails.AddRangeAsync(emiDetails);
            await _context.SaveChangesAsync();
        }

        //Add defalut entry for balnk case
        public async Task AddEmiDetailsAsync(long profileId)
        {
            // 1. Add one-time loan repayment
            var oneTimeLoanRepayment = new TblffAlertnesOneTimeLoanRepayment
            {
                ProfileId = profileId,
                TotalEmi = 0,
                NetCashflows = 0,
                OneTimeLoanRepayment = 0
            };
            _context.TblffAlertnesOneTimeLoanRepayments.Add(oneTimeLoanRepayment);
            await _context.SaveChangesAsync(); // <- use async version

            // 2. Add default EMI list



            var emiList = new List<TblffAlertnesEmiDetail>
    {
        new TblffAlertnesEmiDetail
        {
            ProfileId = profileId,
            Name = "Home Loan",
            Outstanding = 0.00m,
            Interest = 0.0m,
            Principal = 0.00m,
            Monthly = 0.00m,
            Till = new DateOnly(1900, 10, 1),
            FollowUp = false,
            IsNew = true
        },
        new TblffAlertnesEmiDetail
        {
            ProfileId = profileId,
            Name = "Car Loan",
             Outstanding = 0.00m,
            Interest = 0.0m,
            Principal = 0.00m,
            Monthly = 0.00m,
            Till = new DateOnly(1900, 10, 1),
            FollowUp = false,
            IsNew = true
        },
        new TblffAlertnesEmiDetail
        {
            ProfileId = profileId,
            Name = "Educational Loan",
             Outstanding = 0.00m,
            Interest = 0.0m,
            Principal = 0.00m,
            Monthly = 0.00m,
            Till = new DateOnly(1900, 10, 1),
            FollowUp = false,
            IsNew = true
        },
        new TblffAlertnesEmiDetail
        {
            ProfileId = profileId,
            Name = "Business Loan in case of Sole Proprietor",
            Outstanding = 0.00m,
            Interest = 0.0m,
            Principal = 0.00m,
            Monthly = 0.00m,
            Till = new DateOnly(1900, 10, 1),
            FollowUp = false,
            IsNew = true
        },
        new TblffAlertnesEmiDetail
        {
            ProfileId = profileId,
            Name = "Other EMIs",
            Outstanding = 0.00m,
            Interest = 0.0m,
            Principal = 0.00m,
            Monthly = 0.00m,
           Till = new DateOnly(1900, 10, 1),
            FollowUp = false,
            IsNew = true
        }
    };

            _context.TblffAlertnesEmiDetails.AddRange(emiList);
            await _context.SaveChangesAsync();
        }


        //step-5
        public async Task SavePostIncomeDetailAsync(PostIncomeDetailsViewModel postIncomeDetailsViewModel, long incomeDetailProfileId)
        {
            if (postIncomeDetailsViewModel == null)
                return;

            var postIncome = postIncomeDetailsViewModel.PostIncome;
            if (postIncome != null && Decimal.Round((decimal)postIncome, 2) != postIncome)
            {
                throw new ArgumentException("Post Income must have at most two decimal places.");
            }


            var existingDetail = await _context.TblAffAlertnesPostIncomeDetails
                .FirstOrDefaultAsync(x => x.ProfileId == incomeDetailProfileId);

            if (existingDetail != null)
            {
                // ✅ Update existing record
                existingDetail.PostIncome = postIncome;
                existingDetail.MustHaveExpensesPercent = postIncomeDetailsViewModel.MustHaveExpensesPercent;
                existingDetail.OptionalExpensesPercent = postIncomeDetailsViewModel.OptionalExpensesPercent;
                existingDetail.SavingsPercent = postIncomeDetailsViewModel.SavingsPercent;
                existingDetail.ProjectedGrowthRateNext5Years = postIncomeDetailsViewModel.ProjectedGrowthRateNext5Years;
                existingDetail.ProjectedGrowthRate6To10Years = postIncomeDetailsViewModel.ProjectedGrowthRate6To10Years;
                existingDetail.InflationRate = postIncomeDetailsViewModel.InflationRate;

                _context.TblAffAlertnesPostIncomeDetails.Update(existingDetail);
            }
            else
            {
                // ❗Optional: Insert new if not found (or skip based on your logic)
                var newDetail = new TblAffAlertnesPostIncomeDetail
                {
                    ProfileId = incomeDetailProfileId,
                    PostIncome = postIncome,
                    MustHaveExpensesPercent = postIncomeDetailsViewModel.MustHaveExpensesPercent,
                    OptionalExpensesPercent = postIncomeDetailsViewModel.OptionalExpensesPercent,
                    SavingsPercent = postIncomeDetailsViewModel.SavingsPercent,
                    ProjectedGrowthRateNext5Years = postIncomeDetailsViewModel.ProjectedGrowthRateNext5Years,
                    ProjectedGrowthRate6To10Years = postIncomeDetailsViewModel.ProjectedGrowthRate6To10Years,
                    InflationRate = postIncomeDetailsViewModel.InflationRate
                };

                _context.TblAffAlertnesPostIncomeDetails.Add(newDetail);
            }

            await _context.SaveChangesAsync();
        }

        //step-6
        public async Task SavePostTblFfAlertnesNetWorthsAsync(NetWorthViewModel netWorthViewModel, long incomeDetailProfileId)
        {
            if (netWorthViewModel == null)
                return;

            var existingDetail = await _context.TblFfAlertnesNetWorths
                                               .FirstOrDefaultAsync(x => x.ProfileId == incomeDetailProfileId);

            if (existingDetail != null)
            {
                existingDetail.ProfileId = incomeDetailProfileId;
                existingDetail.CashInHand = netWorthViewModel.CashInHand;
                existingDetail.EmployeeProvidendFund = netWorthViewModel.EmployeeProvidendFund;
                existingDetail.Ppf = netWorthViewModel.PPF;
                existingDetail.FixedDeposits = netWorthViewModel.FixedDeposits;
                existingDetail.MutualFundsShares = netWorthViewModel.MutualFundsShares;
                existingDetail.PaidUpValueOfInsurancePolicies = netWorthViewModel.PaidUpValueOfInsurancePolicies;
                existingDetail.OthersGratuity = netWorthViewModel.OthersGratuity;

                // Assets
                existingDetail.Home1 = netWorthViewModel.Home1;
                existingDetail.Home2 = netWorthViewModel.Home2;
                existingDetail.Land = netWorthViewModel.Land;
                existingDetail.Car = netWorthViewModel.Car;
                existingDetail.CommercialProperty = netWorthViewModel.CommercialProperty;
                existingDetail.Jewellery = netWorthViewModel.Jewellery;
                existingDetail.ValueOfBusiness = netWorthViewModel.ValueOfBusiness;
                existingDetail.OtherAssetsOther = netWorthViewModel.OtherAssetsOther;

                // Liabilities
                existingDetail.Home2Loan = netWorthViewModel.Home2Loan;
                existingDetail.LandLoan = netWorthViewModel.LandLoan;
                existingDetail.Home1Loan = netWorthViewModel.Home1Loan;
                existingDetail.CarLoan = netWorthViewModel.CarLoan;
                existingDetail.CommercialPropertyLoan = netWorthViewModel.CommercialPropertyLoan;
                existingDetail.JewelleryLoan = netWorthViewModel.JewelleryLoan;
                existingDetail.BusinessLoan = netWorthViewModel.BusinessLoan;
                existingDetail.OtherLoan = netWorthViewModel.OtherLoan;
                _context.TblFfAlertnesNetWorths.Update(existingDetail);
            }
            else
            {
                // ❗Optional: Insert new if not found (or skip based on your logic)              
                var newNetWorth = new TblFfAlertnesNetWorth
                {
                    ProfileId = incomeDetailProfileId,
                    CashInHand = netWorthViewModel?.CashInHand,
                    EmployeeProvidendFund = netWorthViewModel?.EmployeeProvidendFund,
                    Ppf = netWorthViewModel?.PPF,
                    FixedDeposits = netWorthViewModel?.FixedDeposits,
                    MutualFundsShares = netWorthViewModel?.MutualFundsShares,
                    PaidUpValueOfInsurancePolicies = netWorthViewModel?.PaidUpValueOfInsurancePolicies,
                    OthersGratuity = netWorthViewModel?.OthersGratuity,

                    // Assets
                    Home1 = netWorthViewModel?.Home1,
                    Home2 = netWorthViewModel?.Home2,
                    Land = netWorthViewModel?.Land,
                    Car = netWorthViewModel?.Car,
                    CommercialProperty = netWorthViewModel?.CommercialProperty,
                    Jewellery = netWorthViewModel?.Jewellery,
                    ValueOfBusiness = netWorthViewModel?.ValueOfBusiness,
                    OtherAssetsOther = netWorthViewModel?.OtherAssetsOther,

                    // Liabilities
                    Home2Loan = netWorthViewModel?.Home2Loan,
                    LandLoan = netWorthViewModel?.LandLoan,
                    CommercialPropertyLoan = netWorthViewModel?.CommercialPropertyLoan,
                    JewelleryLoan = netWorthViewModel?.JewelleryLoan,
                    BusinessLoan = netWorthViewModel?.BusinessLoan,
                    OtherLoan = netWorthViewModel?.OtherLoan,

                    // Optional: override CreatedAt (usually auto-filled by SQL Server)
                    CreatedAt = DateTime.Now
                };

                _context.TblFfAlertnesNetWorths.Add(newNetWorth);

            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveNewInvestmentsAsync(List<TblFfAlertnesNewInvestment>? newSavings, int savingId)
        {
            if (newSavings == null || !newSavings.Any())
                return; // Nothing to save

            // Set SavingsId to associate with parent saving record
            foreach (var saving in newSavings)
            {
                saving.ProfileId = savingId;
                saving.InvestmentName = saving.InvestmentName;
                saving.Amount = saving.Amount;

            }
            await _context.TblFfAlertnesNewInvestments.AddRangeAsync(newSavings);
            await _context.SaveChangesAsync();
        }
        public async Task SaveNewOtherAssetsAsync(List<TblFfAlertnesNewOtherAsset>? newSavings, int savingId)
        {
            if (newSavings == null || !newSavings.Any())
                return; // Nothing to save

            // Set SavingsId to associate with parent saving record
            foreach (var saving in newSavings)
            {
                saving.ProfileId = savingId;
                saving.AssetName = saving.AssetName;
                saving.Amount = saving.Amount;

            }
            await _context.TblFfAlertnesNewOtherAssets.AddRangeAsync(newSavings);
            await _context.SaveChangesAsync();
        }
        public async Task SaveNewLiabilitiesAsync(List<TblFfAlertnesNewLiability>? newSavings, int savingId)
        {
            if (newSavings == null || !newSavings.Any())
                return; // Nothing to save

            // Set SavingsId to associate with parent saving record
            foreach (var saving in newSavings)
            {
                saving.ProfileId = savingId;
                saving.LiabilityName = saving.LiabilityName;
                saving.Amount = saving.Amount;

            }
            await _context.TblFfAlertnesNewLiabilities.AddRangeAsync(newSavings);
            await _context.SaveChangesAsync();
        }

        //step-7
        public async Task SavePostTblffAlertnesDebtAsync(LoanDebtDetailViewModel loanDetailViewModel, long incomeDetailProfileId)
        {
            if (loanDetailViewModel == null)
                return;

            var existingDetail = await _context.TblffAlertnesDebts
                .FirstOrDefaultAsync(x => x.ProfileId == incomeDetailProfileId);

            if (existingDetail != null)
            {
                // ✅ Update existing record
                existingDetail.ProfileId = incomeDetailProfileId;
                existingDetail.GoldLoan = loanDetailViewModel.GoldLoan;
                existingDetail.CreditCard = loanDetailViewModel.CreditCard;
                existingDetail.PersonalLoan = loanDetailViewModel.PersonalLoan;
                existingDetail.BadLoanOthers = loanDetailViewModel.BadLoanOthers;
                existingDetail.EducationLoan = loanDetailViewModel.EducationLoan;
                existingDetail.HomeLoan = loanDetailViewModel.HomeLoan;
                existingDetail.BusinessLoan = loanDetailViewModel.BusinessLoan;
                existingDetail.GoodLoanOthers = loanDetailViewModel.GoodLoanOthers;

                _context.TblffAlertnesDebts.Update(existingDetail);
            }
            else
            {
                // ❗Optional: Insert new if not found (or skip based on your logic)              
                var newDetail = new TblffAlertnesDebt
                {
                    ProfileId = incomeDetailProfileId,
                    GoldLoan = loanDetailViewModel?.GoldLoan,
                    CreditCard = loanDetailViewModel?.CreditCard,
                    PersonalLoan = loanDetailViewModel?.PersonalLoan,
                    BadLoanOthers = loanDetailViewModel?.BadLoanOthers,
                    HomeLoan = loanDetailViewModel?.HomeLoan,
                    EducationLoan = loanDetailViewModel?.EducationLoan,
                    BusinessLoan = loanDetailViewModel?.BusinessLoan,
                    GoodLoanOthers = loanDetailViewModel?.GoodLoanOthers
                };

                _context.TblffAlertnesDebts.Add(newDetail);

            }

            await _context.SaveChangesAsync();
        }
        //step-8 LifeInsurance

        public async Task AddLifeInsuranceViewModelAsync(List<TblFfAlertnesLifeInsurance> models, int profileId)
        {
            if (models == null || !models.Any())
            {
                //var defaultEntry = new TblFfAlertnesLifeInsurance
                //{
                //    ProfileId = profileId,
                //    InsuranceType = "Term Insurance",
                //    Name = "Default",
                //    AmountOfCoverage = 0,
                //    PremiumDueDate = null,
                //    MaturityDate = null,
                //    CreatedDate = DateTime.Now
                //};

                //_context.TblFfAlertnesLifeInsurances.Add(defaultEntry);
                //await _context.SaveChangesAsync();
                return;
            }
            foreach (var item in models)
            {
                item.ProfileId = profileId;
                item.CreatedDate = item.CreatedDate == default ? DateTime.Now : item.CreatedDate;
                if (item.MaturityDate != null && item.MaturityDate.Value == default)
                {
                    item.MaturityDate = null;
                }
                if (item.PremiumDueDate != null && item.PremiumDueDate.Value == default)
                {
                    item.PremiumDueDate = null;
                }
                // Handle missing InsuranceType
                if (string.IsNullOrWhiteSpace(item.InsuranceType))
                    item.InsuranceType = "Term Insurance";

                // Handle missing InsuranceType
                if (string.IsNullOrWhiteSpace(item.Name))
                    item.Name = "Default";


                // Handle null or invalid AmountOfCoverage
                if (item.AmountOfCoverage == null || item.AmountOfCoverage < 0)
                    item.AmountOfCoverage = 0;

            }
            _context.TblFfAlertnesLifeInsurances.AddRange(models);
            await _context.SaveChangesAsync();
        }
        public async Task AddGenLifeInsuranceViewModelAsync(List<TblFfAlertnesGeneralInsurance> models, int profileId)
        {
            if (models == null || !models.Any())
            {
                //var defaultEntry = new TblFfAlertnesGeneralInsurance
                //{
                //    ProfileId = profileId,
                //    InsuranceType = "Individual Mediclaim",
                //    AmountOfCoverage = 0, // Optional: ensure default amount
                //    PremiumDueDate = null,
                //    MaturityDate = null,
                //    CreatedDate = DateTime.Now
                //};

                //_context.TblFfAlertnesGeneralInsurances.Add(defaultEntry);
                //await _context.SaveChangesAsync();
                return;
            }

            foreach (var item in models)
            {

                item.ProfileId = profileId;
                item.CreatedDate = item.CreatedDate == default ? DateTime.Now : item.CreatedDate;
                if (item.MaturityDate is { } maturity && maturity == default)
                    item.MaturityDate = null;

                if (item.PremiumDueDate is { } premiumDue && premiumDue == default)
                    item.PremiumDueDate = null;

                // Handle missing InsuranceType
                if (string.IsNullOrWhiteSpace(item.InsuranceType))
                    item.InsuranceType = "Individual Mediclaim";

                // Handle null or invalid AmountOfCoverage
                if (item.AmountOfCoverage == null || item.AmountOfCoverage < 0)
                    item.AmountOfCoverage = 0;


            }
            _context.TblFfAlertnesGeneralInsurances.AddRange(models);
            await _context.SaveChangesAsync();
        }

        private DateTime? ParseDateDdMmYyyy(string dateStr)
        {
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                return dt;

            return null;
        }
        public async Task<ProfileDataDto?> GetProfileData(long profileID)
        {
            if (profileID <= 0)
                throw new ArgumentException("Invalid profile ID");

            var profileData = await (
                from profile in _context.TblffAwarenessProfileDetails
                where profile.ProfileId == profileID
                join spouse in _context.TblffAwarenessSpouses
                    on profile.ProfileId equals spouse.ProfileId into spouseGroup
                from spouse in spouseGroup.DefaultIfEmpty()
                select new ProfileDataDto
                {
                    Name = profile.Name,
                    MaritalStatus = profile.MaritalStatus,
                    SpouseName = spouse != null ? spouse.SpouseName : "NA"
                }
            ).FirstOrDefaultAsync();
            return profileData;
        }
    }

}

