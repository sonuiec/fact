using FactFinderWeb.Models;
using FactFinderWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SystemTextJson = System.Text.Json.JsonSerializer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

//using System.Text.Json;
using stJSON= System.Text.Json;
using System.Collections.Generic;
using FactFinderWeb.ModelsView;
using FactFinderWeb.IServices;
//using NewtonsoftSerializer = Newtonsoft.Json.JsonConvert;

namespace FactFinderWeb.Utils
{
    public class JSONDataUtility
    {

        private readonly ResellerBoyinawebFactFinderWebContext _context;
        public JSONDataUtility(ResellerBoyinawebFactFinderWebContext context) => _context = context;


        List<object> selectedFormsdata = new List<object>();
        List<object> GoalsNameList = new List<object>();

        public async Task<string> GetAwareness(long profileId)
        {
            var jsonString= string.Empty;
            var profile = _context.TblffAwarenessProfileDetails.FirstOrDefault(p => p.ProfileId == profileId);
            var users = _context.TblFfRegisterUsers.FirstOrDefault(p => p.Id == profile.UserId);
            if (profile != null)
            {
                var spouse = _context.TblffAwarenessSpouses.FirstOrDefault(s => s.ProfileId == profileId);
                var children = _context.TblffAwarenessChildren.Where(c => c.ProfileId == profileId).ToList();
                var Assumptions = _context.TblffAwarenessAssumptions.Where(c => c.ProfileId == profileId).FirstOrDefault();
                string Advisor = "";
                if (users != null)
                {
                    if (users.Advisorid != null)
                    {
                        var admin = _context.TblFfAdminUsers.FirstOrDefault(p => p.Id == users.Advisorid);

                        Advisor = admin?.Name ?? "";
                    }
                }
                /* var profile = await _context.TblffAwarenessProfileDetails
                //.Include(p => p.Addresses)
                .Include(ps => _context.TblffAwarenessSpouses)//(p => p.Spouse)
                .Include(pc => _context.TblffAwarenessChildren)
                .Include(pff => _context.TblffAwarenessFamilyFinancials)
                //.Include(p => p.FamilyFinancial).FirstOrDefaultAsync(p => p.ProfileId == profileId);*/

                if (profile == null) return null;// NotFound();

                var output = new
                {
                    awareness = new AwarenessSection
                    {
                        planDetails = new PlanDetails
                        {
                            PlanType = profile.PlanType,
                            PlanYear = profile.PlanYear,
                            PlanDuration = profile.PlanDuration
                        },
                        personalDetails = new PersonalDetails
                        {
                            Name = profile.Name,
                            Advisor = Advisor,
                            Gender = profile.Gender,
                            Dob = profile.Dob,
                            Phone = profile.Phone,
                            AltPhone = profile.Altphone,
                            Aadhaar = profile.Aadhaar,
                            Email = profile.Email,
                            SecEmail = profile.SecEmail,
                            Pan = profile.Pan,
                            Occupation = profile.Occupation,
                            address = new AddressGroup
                            {
                                residential = new ResidentialAddress
                                {
                                    Address = profile.ResAddress,
                                    Country = profile.ResCountry,
                                    CountryCode= "",
                                    State = profile.ResState,
                                    StateCode  = "",
                                    City = profile.ResCity,
                                    PinCode = profile.ResPincode
                                },
                                permanent = new PermanentAddress
                                {
                                    Address = profile.PermAddress,
                                    Country = profile.PermCountry,
                                    CountryCode = "",
                                    State = profile.PermState,
                                    StateCode = "",
                                    City = profile.PermCity,
                                    PinCode = profile.PermPincode
                                },
                                office = new OfficeAddress
                                {
                                    Company=   profile.CompanyName,
                                    Address = profile.CompanyAddress,
                                    Country = profile.CompanyCountry,
                                    CountryCode = "",
                                    State = profile.CompanyState,
                                    StateCode = "",
                                    City = profile.CompanyCity,
                                    PinCode = profile.CompanyPincode
                                },
                                IsSameAddress = profile.IsSameAddress
                            }
                        },
                        familyFinancial = new FamilyFinancial
                        {
                            Stock = profile.Stock,
                            Income = profile.Income,
                            Payment = profile.Payment,
                            Holiday = profile.Holiday,
                            Shopping = profile.Shopping
                        },
                        maritalDetails = new MaritalDetails1
                        {
                           MaritalStatus= profile.MaritalStatus,
                            spouseDetails = spouse == null ? null : new SpouseDetails
                            {
                               SpouseName  = spouse.SpouseName,
                               SpouseGender  = spouse.SpouseGender,
                                SpouseDob = spouse.SpouseDob,
                                SpousePhone = spouse.SpousePhone,
                                SpouseAltPhone = spouse.SpouseAltPhone,
                                SpouseAadhaar = spouse.SpouseAadhaar,
                                SpouseEmail = spouse.SpouseEmail,
                                SpouseSecEmail = spouse.SpouseSecEmail,
                                SpousePan = spouse.SpousePan,
                                SpouseOccupation = spouse.SpouseOccupation
                            },
                            haveChildren = profile.HaveChildren,
                            childrenDetails = children.Select(pc => new ChildDetails
                            {
                                Id= pc.Id,
                                ChildName = pc.ChildName,
                                ChildGender = pc.ChildGender,
                                ChildDob = pc.ChildDob,
                                ChildPhone = pc.ChildPhone,
                                ChildAadhaar = pc.ChildAadhaar,
                                ChildEmail = pc.ChildEmail,
                               // childPan = pc.ChildPan
                            }).ToList()
                        },
                        assumptions = Assumptions == null ? null : new Assumptions
                        {
                            Equity = Assumptions.Equity,
                            Debt = Assumptions.Debt,
                            Gold = Assumptions.Gold,
                            RealEstateReturn = Assumptions.RealEstateReturn,
                            LiquidFunds = Assumptions.LiquidFunds,
                            InflationRates = Assumptions.InflationRates,
                            EducationInflation = Assumptions.EducationInflation,
                            ApplicantRetirement = Assumptions.ApplicantRetirement,
                            SpouseRetirement = Assumptions.SpouseRetirement,
                            ApplicantLifeExpectancy = Assumptions.ApplicantLifeExpectancy,
                            SpouseLifeExpectancy = Assumptions.SpouseLifeExpectancy
                        }
                    }
                };

//#if DEBUG

                var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                  jsonString = SystemTextJson.Serialize(output, options);
            return JsonConvert.SerializeObject(output, Formatting.Indented);
                //return Content(jsonString, "application/json");
//#else
//                return Json(output);
//#endif

            }
                return jsonString;

        }

        public async Task<RootAwareness> GetAwakenSection(long profileId)
        {
            var awakenSection = new AwakenSection
            {
                Awareness = BuildAwarenessSection(profileId),
                Wings = BuildWingsSection(profileId),
                Alertness = BuildAlertnessSection(profileId),
                Knowledge = BuildKnowledgeSection(profileId),
                ExecutePlan = BuildExecutePlanSection(profileId),
                Invest = BuildInvestSection(profileId),
                ProfileId = profileId
            };

            var result = new RootAwareness
            {
                Awaken = awakenSection
            };

            return await Task.FromResult(result); // keeps async signature
        }


        private AwarenessSection BuildAwarenessSection(long profileId)
        {
            var profile = _context.TblffAwarenessProfileDetails.FirstOrDefault(p => p.ProfileId == profileId);
            var spouse = _context.TblffAwarenessSpouses.FirstOrDefault(s => s.ProfileId == profileId);
            var children = _context.TblffAwarenessChildren.Where(c => c.ProfileId == profileId).ToList();
            var Assumptions = _context.TblffAwarenessAssumptions.Where(c => c.ProfileId == profileId).FirstOrDefault();
            //var familyFinancial = _context.TblffAwarenessFamilyFinancials.FirstOrDefault(f => f.ProfileId == profileId);
            string Advisor = "";
            var users = _context.TblFfRegisterUsers.FirstOrDefault(p => p.Id == profile.UserId);

            if (users != null)
            {
                if (users.Advisorid != null)
                {
                    var admin = _context.TblFfAdminUsers.FirstOrDefault(p => p.Id == users.Advisorid);

                    Advisor = admin?.Name ?? "";
                }
            }

            return new AwarenessSection
            {
                Advisor = Advisor,
                planDetails = new PlanDetails
                {
                    PlanType = profile?.PlanType ?? string.Empty,
                    PlanYear = profile?.PlanYear??0,
                    PlanDuration = profile?.PlanDuration ?? string.Empty
                },
                personalDetails = new PersonalDetails
                {
                    Name = profile?.Name ?? string.Empty,
                    Gender = profile?.Gender ?? string.Empty,
                    Dob = profile?.Dob ?? string.Empty,
                    Phone = profile?.Phone ?? string.Empty,
                    AltPhone = profile?.Altphone ?? string.Empty,
                    Aadhaar = profile?.Aadhaar ?? string.Empty,
                    Email = profile?.Email ?? string.Empty,
                    SecEmail = profile?.SecEmail ?? string.Empty,
                    Pan = profile?.Pan ?? string.Empty,
                    Occupation = profile?.Occupation ?? string.Empty,
                    address = new AddressGroup
                    {
                        residential = new ResidentialAddress
                        {
                            Address = profile?.ResAddress ?? string.Empty,
                            Country = profile?.ResCountry ?? string.Empty,
                            CountryCode = profile?.ResCountry ?? string.Empty,
                            State = profile?.ResState ?? string.Empty,
                            StateCode = profile?.ResState ?? string.Empty,
                            City = profile?.ResCity ?? string.Empty,
                            PinCode = profile?.ResPincode ?? string.Empty
                        },
                        permanent  = new PermanentAddress
                        {
                            Address = profile?.PermAddress ?? string.Empty,
                            Country = profile?.PermCountry ?? string.Empty,
                            CountryCode = profile?.PermCountry ?? string.Empty,
                            State = profile?.PermState ?? string.Empty,
                            StateCode = profile?.PermState ?? string.Empty,
                            City = profile?.PermCity ?? string.Empty,
                            PinCode = profile?.PermPincode ?? string.Empty
                        },
                        office = new OfficeAddress
                        {
                            Company = profile?.CompanyName ?? string.Empty,
                            Address = profile?.CompanyAddress ?? string.Empty,
                            Country = profile?.CompanyCountry ?? string.Empty,
                            CountryCode = profile?.CompanyCountry ?? string.Empty,
                            State = profile?.CompanyState ?? string.Empty,
                            StateCode = profile?.CompanyState ?? string.Empty,
                            City = profile?.CompanyCity ?? string.Empty,
                            PinCode = profile?.CompanyPincode ?? string.Empty
                        },
                        IsSameAddress = profile?.IsSameAddress ?? false
                    }
                },
                familyFinancial = new FamilyFinancial
                {
                    Stock = profile?.Stock ?? string.Empty,
                    Income = profile?.Income ?? string.Empty,
                    Payment = profile?.Payment ?? string.Empty,
                    Holiday = profile?.Holiday ?? string.Empty,
                    Shopping = profile?.Shopping ?? string.Empty
                },
                maritalDetails = new MaritalDetails1
                {
                    MaritalStatus = profile?.MaritalStatus ?? string.Empty,
                    spouseDetails = spouse == null ? null : new SpouseDetails
                    {
                        SpouseName = spouse?.SpouseName ?? string.Empty,
                        SpouseGender = spouse?.SpouseGender ?? string.Empty,
                        SpouseDob = spouse?.SpouseDob ?? string.Empty,
                        SpousePhone = spouse?.SpousePhone ?? string.Empty,
                        SpouseAltPhone = spouse?.SpouseAltPhone ?? string.Empty,
                        SpouseAadhaar = spouse?.SpouseAadhaar ?? string.Empty,
                        SpouseEmail = spouse?.SpouseEmail ?? string.Empty,
                        SpouseSecEmail = spouse?.SpouseSecEmail ?? string.Empty,
                        SpousePan = spouse?.SpousePan ?? string.Empty,
                        SpouseOccupation = spouse?.SpouseOccupation ?? string.Empty
                    },
                    haveChildren = (children != null && children.Any()) ? "yes" : "no",
                    childrenDetails = children?.Select(c => new ChildDetails
                    {
                        Id = c.Id,
                        ChildName = c.ChildName ?? string.Empty,
                        ChildGender = c.ChildGender ?? string.Empty,
                        ChildDob = c.ChildDob ?? string.Empty,
                        ChildPhone = c.ChildPhone ?? string.Empty,
                        ChildAadhaar = c.ChildAadhaar ?? string.Empty,
                        ChildEmail = c.ChildEmail ?? string.Empty,
                        ChildPan = c.ChildPan ?? string.Empty
                    }).ToList() ?? new List<ChildDetails>()
                },
                assumptions = new Assumptions
                {
                    Equity = Assumptions?.Equity ?? 0,
                    Debt = Assumptions?.Debt ?? 0,
                    Gold = Assumptions?.Gold ?? 0,
                    RealEstateReturn = Assumptions?.RealEstateReturn ?? 0,
                    LiquidFunds = Assumptions?.LiquidFunds ?? 0,
                    InflationRates = Assumptions?.InflationRates ?? 0,
                    EducationInflation = Assumptions?.EducationInflation ?? 0,
                    ApplicantRetirement = Assumptions?.ApplicantRetirement ?? 0,
                    SpouseRetirement = Assumptions?.SpouseRetirement ?? 0,
                    ApplicantLifeExpectancy = Assumptions?.ApplicantLifeExpectancy ?? 0,
                    SpouseLifeExpectancy = Assumptions?.SpouseLifeExpectancy ?? 0
                }
            };

        }
        private WingsSection BuildWingsSection(long profileId)
        {
            // Fetch all goal records for this profile
            var wingsData = _context.TblffWings
                .Where(p => p.ProfileId == profileId)
                .OrderBy(u => u.GoalPriority)
                .Select(u => new
                {
                    u.Id,
                    u.GoalPriority,
                    u.GoalName,
                    u.GoalStartYear,
                    u.GoalPlanYear,
                    u.GoalEndYear,
                    u.TimeHorizon,
                    u.NewGoals
                })
                .ToList();

            var selectedFormsList = new List<SelectedGoal>();
            var goalNameList = new List<GoalForm>();

            foreach (var goal in wingsData)
            {
                var selectedGoal = new SelectedGoal
                {
                   
                    Priority = goal.GoalPriority,
                    Goal = goal.GoalName,
                    GoalStartYear = goal.GoalStartYear,
                    GoalPlanYear = goal.GoalPlanYear,
                    GoalEndYear = goal.GoalEndYear,
                    TimeHorizon = goal.TimeHorizon,
                    //NewGoals = goal.NewGoals
                };

                var goalForm = new GoalForm
                {
                   // Id = goal.Id,
                    Goal = goal.GoalName
                };

                goalNameList.Add(goalForm);
                selectedFormsList.Add(selectedGoal);
            }

            return new WingsSection
            {
                Forms = new List<GoalForm>
        {
            new GoalForm { Id = 1, Goal = "Emergency Fund" },
            new GoalForm { Id = 2, Goal = "Retirement Accumulation" },
            new GoalForm { Id = 3, Goal = "World Tour" },
            new GoalForm { Id = 4, Goal = "Purchase of Dream Car" },
            new GoalForm { Id = 5, Goal = "Purchase of Dream House" },
            new GoalForm { Id = 6, Goal = "Seed Capital for Business" },
            new GoalForm { Id = 7, Goal = "Charity" }
        },
                SelectedForms = selectedFormsList
            };
        }


        private AlertnessRoot BuildAlertnessSection(long profileId)
        {
            var profileIdParam = new SqlParameter("@ProfileId", profileId);

            // Call stored procedure
            var jsonResult = _context.AlertnessJsonResult
                .FromSqlRaw("EXEC Sp_AlertnessJshon_new @ProfileId", profileIdParam)
                .AsEnumerable()
                .FirstOrDefault();

            if (jsonResult == null || string.IsNullOrEmpty(jsonResult.AlertnessJson))
                return new AlertnessRoot (); // return empty object if SP fails

            // Parse raw JSON string to dynamic object
            var alertnessData = JsonConvert.DeserializeObject<AlertnessRoot>(jsonResult.AlertnessJson);
            return alertnessData;
        }

        private KnowledgeSection BuildKnowledgeSection(long profileId)
        {
            var kData = _context.TblffKnowledgeRisks
                .FirstOrDefault(p => p.ProfileId == profileId);
            if (kData == null)
            {
                new TblffKnowledgeRisk();
            }
            return new KnowledgeSection
            {
                Knowledge = new KnowledgeData
                {
                    RiskCapacity = kData?.RiskCapacity ?? string.Empty,
                    RiskRequirement = kData?.RiskRequirement ?? string.Empty,
                    TotalRiskProfileScore = kData?.TotalRiskProfileScore ?? string.Empty,
                    PlannerAssessmentOnRiskProfile = kData?.PlannerAssessmentOnRiskProfile ?? string.Empty,
                    RiskTolerance = kData?.RiskTolerance ?? string.Empty
                }
            };
        }

        private ExecutePlanSection BuildExecutePlanSection(long profileId)
        {
            var executionData = _context.TblffWingsGoalStep5ExecutionData
                .Where(p => p.ProfileId == profileId)
                .Select(u => new { u.GoalName, u.ExecutionDescription, u.ExecutionValue })
                .ToList();

            var groupedGoals = executionData
                .GroupBy(e => e.GoalName)
                .Select(group =>
                {
                    var goal = new GoalData
                    {
                        Name = group.Key
                    };

                    foreach (var item in group)
                    {
                        var key = item.ExecutionDescription;
                        goal.Values[key] = item.ExecutionValue ?? string.Empty;
                    }

                    return goal;
                })
                .ToList();

            return new ExecutePlanSection
            {
                FormData = new FormData
                {
                    LifeInsurance = new LifeInsuranceData
                    {
                        IncomeUsedForFamily = string.Empty,
                        FundsReturnRate = string.Empty,
                        InflationRate = string.Empty
                    },
                    SpouseDetails = new SpouseExecutionData
                    {
                        SLiabilities = string.Empty
                    },
                    GoalNames = new GoalNameData
                    {
                        GoalsNameList = new List<string>() // you can fill this if needed
                    },
                    GoalData = groupedGoals
                }
            };
        }

        private InvestSection BuildInvestSection(long profileId)
        {
            // Master Data
            var investMasterDataUser = _context.TblffInvestWingsGoalMasters
                .Where(p => p.ProfileId == profileId)
                .Select(u => new { u.Id, u.IntendedSipmonthly, u.AvailableLumpsum,u.MonthlySavings })
                .FirstOrDefault();
          
            // Earmarked Investments (Goals)
            var investDataUser = _context.TblffInvestWingsGoals
                .Where(p => p.ProfileId == profileId)
                .Select(u => new InvestGoal
                {
                    Id = u.Id,
                    EarmarkedForGoal = u.GoalName ?? string.Empty,
                    TypeOfFund = string.Empty,
                    SelectedFunds = string.Empty,
                    LumpSum = u.LumpsumAmount ?? 0,
                    SIP = u.Sipamount ?? 0,
                    GoalOptions = new List<string>(),
                    SelectedFundsOptions = new List<string>()
                })
                .ToList();

            // Life Insurance Data
            var lifeInsuranceDataUser = _context.TblffInvestWingsGoals
                .Where(p => p.ProfileId == profileId)
                .Select(u => new InsuranceRequirement
                {
                    Id = u.Id,
                    GoalName = u.GoalName ?? string.Empty,
                    LumpsumAmount = u.LumpsumAmount ?? 0,
                    SipAmount = u.Sipamount ?? 0
                })
                .ToList();

            // General Insurance Data
            var generalInsuranceDataUser = _context.TblffInvestWingsGoals
                .Where(p => p.ProfileId == profileId)
                .Select(u => new InsuranceRequirement
                {
                    Id = u.Id,
                    GoalName = u.GoalName ?? string.Empty,
                    LumpsumAmount = u.LumpsumAmount ?? 0,
                    SipAmount = u.Sipamount ?? 0
                })
                .ToList();

            // Return fully typed model
            return new InvestSection
            {
                Wings = new WingsSection
                {
                    ExpectedReturns = new ExpectedReturns
                    {
                        GoalsNameList = new List<string>() // can populate distinct goal names if needed
                    }
                },
                Alertness = new AlertnessSectionForInvest
                {
                    FamilyMonthlyIncomeTotal = string.Empty,
                    FamilyMonthlyExpensesTotal = string.Empty
                },
                Knowledge = new KnowledgeSectionForInvest
                {
                    SelectedRiskProfile = string.Empty
                },
                ExecuteWithPrecision = new ExecuteWithPrecision
                {
                    HofInsurance = new HofInsurance
                    {
                        HomeInsurance = new InsuranceCoverage(),
                        CarInsurance = new InsuranceCoverage()
                    }
                },
                ActNow = new ActNowSection
                {
                    SavingsForInvestments = new SavingsForInvestments
                    {
                        IntendedSipMonthly = investMasterDataUser?.IntendedSipmonthly ?? 0,
                        AvailableLumpsum = investMasterDataUser?.AvailableLumpsum ?? 0,
                        MonthlySavings = investMasterDataUser?.MonthlySavings ?? 0
                    },
                    LifeInsuranceReq = lifeInsuranceDataUser,
                    GeneralInsuranceReq = generalInsuranceDataUser,
                    ActionPlanFinancialGoals = new ActionPlanFinancialGoals
                    {
                        Goals = new GoalNames { GoalsNameList = new List<string>() },
                        Total = new GoalTotals()
                    },
                    GoalStatusReport = new GoalNames { GoalsNameList = new List<string>() },
                    TotalGoalStatusReport = new TotalGoalStatusReport(),
                    GoalsOptions = new GoalNames { GoalsNameList = new List<string>() },
                    Earmarked = investDataUser
                }
            };
        }


        public string GenerateGoalJson(string goalName, string goalDescription, string goalValue)
        {
            // Create an anonymous object that represents the goal data

            var goalData = new
            {
                goalName = goalName,
                goalDescription = goalDescription,
                goalValue = goalValue
            };


            var goalJson = new object();
            /*
            switch (goalName)
            {
                case "Emergency Fund":
                    goalJson = new
                    {
                        name = goalName,
                        monthsRequired = goal.MonthsRequired ?? "",
                        availableEmergencyFund = goal.AvailableEmergencyFund ?? "",
                        annualAmountForFund = goal.AnnualAmountForFund ?? "",
                        expectedAnnualIncrease = goal.ExpectedAnnualIncrease ?? "",
                        monthlySipTopup = goal.MonthlySipTopup ?? ""
                    };
                    break;

                case "Retirement Accumulation":
                    goalJson = new
                    {
                        name = goal.Name,
                        monthlyRetirementExpenses = goal.MonthlyRetirementExpenses ?? "",
                        expectedReturnOnPortfolio = goal.ExpectedReturnOnPortfolio ?? "",
                        expectedInflationPostRetirement = goal.ExpectedInflationPostRetirement ?? "",
                        existingInvestments = goal.ExistingInvestments ?? "",
                        annualContribution = goal.AnnualContribution ?? "",
                        expectedAvgReturnInvestments = goal.ExpectedAvgReturnInvestments ?? "",
                        expectedReturnOnProperty = goal.ExpectedReturnOnProperty ?? "",
                        valueOfSharesMFSForRetirement = goal.ValueOfSharesMFSForRetirement ?? "",
                        expectedAnnualIncreaseInvestment = goal.ExpectedAnnualIncreaseInvestment ?? "",
                        monthlySIPAmountTopup = goal.MonthlySIPAmountTopup ?? ""
                    };
                    break;

                case "Child1 higher education":
                    goalJson = new
                    {
                        name = goal.Name,
                        durationHigherEducation = goal.DurationHigherEducation ?? "",
                        ageTimeFundingMF = goal.AgeTimeFundingMF ?? "",
                        expectedInflation = goal.ExpectedInflation ?? "",
                        yearlyExpenseHigherEducation = goal.YearlyExpenseHigherEducation ?? "",
                        presentValueFundsEarmarked = goal.PresentValueFundsEarmarked ?? "",
                        expectedReturnFundsEarmarked = goal.ExpectedReturnFundsEarmarked ?? "",
                        expectedReturnPropertyAsset = goal.ExpectedReturnPropertyAsset ?? "",
                        expectedAnnualIncreaseInvestmentPercentage = goal.ExpectedAnnualIncreaseInvestmentPercentage ?? "",
                        monthlySIPAmountWithTopup10PercentPerAnnum = goal.MonthlySIPAmountWithTopup10PercentPerAnnum ?? "",
                        childId = goal.ChildId ?? ""
                    };
                    break;

                case "Purchase of Dream Car":
                    goalJson = new
                    {
                        name = goal.Name,
                        anticipatedInflation = goal.AnticipatedInflation ?? "",
                        cost = goal.Cost ?? "",
                        fundsEarmarked = goal.FundsEarmarked ?? "",
                        expectedReturnFundsEarmarked = goal.ExpectedReturnFundsEarmarked ?? "",
                        expectedAnnualIncreaseInvestment = goal.ExpectedAnnualIncreaseInvestment ?? "",
                        monthlySIPTopup = goal.MonthlySipTopup ?? ""
                    };
                    break;

                // Add more cases here for other goal types...

                default:
                    // Handle default case or unknown goal names
                    goalJson = new
                    {
                        name = goal.Name,
                        description = "No specific description available.",
                        value = "N/A"
                    };
                    break;
            }
            */
            // Return the JSON result
            //return Json(goalData, JsonRequestBehavior.AllowGet);
            return "";
        }

        public static string GetJsonKey(string label)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "No of Months for which Emergency Fund may be Required", "monthsRequired" },
                { "Amount availble for Emergency Fund", "availableEmergencyFund" },
                { "Anunal Amount Going Towards Creating Emergency Fund", "annualAmountForFund" }, 
                { "Available Funds meant for this goal", "availableFundsMeantForThisGoal" }, 
                { "Monthly Retirement Expenses @ Current Cost", "monthlyRetirementExpenses" }, 
                { "Existing Investments in EPF, Supper Annuation, PPF, NPS etc.", "ExistingInvestmentsEPFSupperAnnuationPPFNPS" }, //
                { "Annual Contribution", "expectedAnnualIncrease" }, 
                { "Expected Average Return in EPF, Supper Annuation, PPF, NPS etc (%)", "expectedReturnOnPortfolio" }, 
                { "Value of Shares and MFs meant for retirement", "valueOfSharesMFSForRetirement" }, 
                { "Any other Investments / Assets meant for this goal", "existingInvestments" }, 
                { "Children Name", "name" }, 
                { "Duration of Higher Education", "durationHigherEducation" }, 
                { "Age of  at the start Higher Education", "ageOfAtTheStartHigherEducation" }, 
                { "Yearly Expense for Higher Education @ Current Cost", "yearlyExpenseHigherEducation" }, 
                { "Present Value of Funds Earmarked for this Goal", "presentValueFundsEarmarked" }, 
                { "Expected Return on Funds Earmarked for this Goal", "expectedReturnFundsEarmarked" }, 
                { "Cost of Dream Car @ Current Cost", "cost" }, 
                { "Year of World Travel", "YearofWorldTravel" }, //
                { "Repeat After Every(Years)", "repeatYears" }, 
                { "Cost of World Tour  @ Current Cost", "cost" }, 
                { "Child's Name", "childName" }, 
                { "Expected Age for Marriage", "expectedAgeForMarriage" }, 
                { "Period left for Marriage", "periodleftforMarriage" }, 
                { "Peiod left for Marriage", "periodleftforMarriage" }, 
                { "Expense for Marriage @ Current Cost", "expenseForMarriageAtCurrentCost" }, 
                { "Cost of Dream Home @ Current Cost", "seedCost" }, 
                { "Cost of Seed Capital @ Current Cost", "cost" }, 
                { "Cost of Charity @ Current Cost", "cost" }, 
                { "Cost of Custom Goal @ Current Cost", "cost" }

                // Add more mappings here
            };

            return map.TryGetValue(label, out var key) ? key : ToCamelCase(label); // fallback
        }
        public static string ToCamelCase(string label)
        {        // Example: "Age of at the start Higher Education" → "ageOfAtTheStartHigherEducation"
            if (string.IsNullOrWhiteSpace(label))
                return label;

            var words = label.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower());

            var camel = string.Concat(words);
            return char.ToLower(camel[0]) + camel.Substring(1);
        }

        //-----for user FrontEnd


        public async Task<string> UserGetAwarenessJSON(long profileId)
        {
            var result = new
            {
                awaken = new
                {
                    awareness = UserBuildAwarenessSection(profileId),
                    wings = UserBuildWingsSection(profileId),
                    alertness = UserBuildAlertnessSection(profileId),
                    knowledge = UserBuildKnowledgeSection(profileId),
                    ExecutePlan = UserBuildExecutePlanSection(profileId),
                    Invest = UserBuildInvestSection(profileId)
                }
            };

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        private object UserBuildAwarenessSection(long profileId)
        {
            var profile = _context.TblffAwarenessProfileDetails.FirstOrDefault(p => p.ProfileId == profileId);
            var spouse = _context.TblffAwarenessSpouses.FirstOrDefault(s => s.ProfileId == profileId);
            var children = _context.TblffAwarenessChildren.Where(c => c.ProfileId == profileId).ToList();
            var Assumptions = _context.TblffAwarenessAssumptions.Where(c => c.ProfileId == profileId).FirstOrDefault();
            //var familyFinancial = _context.TblffAwarenessFamilyFinancials.FirstOrDefault(f => f.ProfileId == profileId);


            return new
            {
                planDetails = new
                {
                    planType = profile?.PlanType ?? "",
                    planYear = profile?.PlanYear.ToString() ?? "",
                    planDuration = profile?.PlanDuration ?? "",
                },
                personalDetails = new
                {
                    name = profile?.Name ?? "",
                    gender = profile?.Gender ?? "",
                    dob = profile?.Dob ?? "",
                    phone = profile?.Phone ?? "",
                    altPhone = profile?.Altphone ?? "",
                    aadhaar = profile?.Aadhaar ?? "",
                    email = profile?.Email ?? "",
                    secEmail = profile?.SecEmail ?? "",
                    pan = profile?.Pan ?? "",
                    occupation = profile?.Occupation ?? "",
                    address = new
                    {
                        residential = new
                        {
                            address = profile.ResAddress ?? "",
                            country = profile.ResCountry ?? "",
                            countryCode = profile.ResCountry ?? "",
                            state = profile.ResState ?? "",
                            stateCode = profile.ResState ?? "",
                            city = profile.ResCity ?? "",
                            pinCode = profile.ResPincode
                        },
                        permanent = new
                        {
                            address = profile.PermAddress ?? "",
                            country = profile.PermCountry ?? "",
                            countryCode = profile.PermCountry ?? "",
                            state = profile.PermState ?? "",
                            stateCode = profile.PermState ?? "",
                            city = profile.PermCity ?? "",
                            pinCode = profile.PermPincode
                        },
                        office = new
                        {
                            company = profile.CompanyName ?? "",
                            address = profile.CompanyAddress ?? "",
                            country = profile.CompanyCountry ?? "",
                            countryCode = profile.CompanyCountry ?? "",
                            state = profile.CompanyState ?? "",
                            stateCode = profile.CompanyState ?? "",
                            city = profile.CompanyCity ?? "",
                            pinCode = profile.CompanyPincode ?? ""
                        },
                        isSameAddress = profile.IsSameAddress
                    }
                },
                familyFinancial = new
                {
                    stock = profile?.Stock ?? "",
                    income = profile?.Income ?? "",
                    payment = profile?.Payment ?? "",
                    holiday = profile?.Holiday ?? "",
                    shopping = profile?.Shopping ?? ""
                },
                maritalDetails = new
                {
                    maritalStatus = profile?.MaritalStatus ?? "",
                    spouseDetails = spouse == null ? null : new
                    {
                        spouseName = spouse.SpouseName ?? "",
                        spouseGender = spouse.SpouseGender ?? "",
                        spouseDob = spouse.SpouseDob ?? "",
                        spousePhone = spouse.SpousePhone ?? "",
                        spouseAltPhone = spouse.SpouseAltPhone ?? "",
                        spouseAadhaar = spouse.SpouseAadhaar ?? "",
                        spouseEmail = spouse.SpouseEmail ?? "",
                        spouseSecEmail = spouse.SpouseSecEmail ?? "",
                        spousePan = spouse.SpousePan ?? "",
                        spouseOccupation = spouse.SpouseOccupation ?? "",

                        SpouseCmpAddress = spouse.SpouseCompanyAddress ?? "",
                        SpouseCmpCity = spouse.SpouseCompanyCity ?? "",
                        SpouseCmpCountry = spouse.SpouseCompanyCountry ?? "",
                        SpouseCmpName = spouse.SpouseCompanyName ?? "",
                        SpouseCmpPincode = spouse.SpouseCompanyPincode ?? "",
                        SpouseCmpState = spouse.SpouseCompanyState ?? ""

                    },
                    haveChildren = children.Any() ? "yes" : "no",
                    childrenDetails = children.Select(c => new
                    {
                        id = c.Id,
                        childName = c.ChildName ?? "",
                        childGender = c.ChildGender ?? "",
                        childDob = c.ChildDob ?? "",
                        childPhone = c.ChildPhone ?? "",
                        childAadhaar = c.ChildAadhaar ?? "",
                        childEmail = c.ChildEmail ?? "",
                        childPan = c.ChildPan ?? ""
                    }).ToList()
                },
                assumptions = new
                {
                    equity = Assumptions?.Equity ?? 0,
                    debt = Assumptions?.Debt ?? 0,
                    gold = Assumptions?.Gold ?? 0,
                    realEstateReturn = Assumptions?.RealEstateReturn ?? 0,
                    liquidFunds = Assumptions?.LiquidFunds ?? 0,
                    inflationRates = Assumptions?.InflationRates ?? 0,
                    educationInflation = Assumptions?.EducationInflation ?? 0,
                    applicantRetirement = Assumptions?.ApplicantRetirement ?? 0,
                    spouseRetirement = Assumptions?.SpouseRetirement ?? 0,
                    applicantLifeExpectancy = Assumptions?.ApplicantLifeExpectancy ?? 0,
                    spouseLifeExpectancy = Assumptions?.SpouseLifeExpectancy ?? 0,
                }
            };
        }
        private object UserBuildWingsSection(long profileId)
        {
            /*// {
            //     "id": 4,
            //     "priority": 3, //--- it denotes on this particular priority the goal will be
            //     "goal": "Purchase of Dream Car",
            //     "goalStartYear": 2056,
            //     "goalPlanYear": 2025, // it is non input field which will be referenced from awareness
            //     "goalEndYear" : "",
            //     "timeHorizon": 31, // it is also autopopulated field
            //     "newGoals": false // --- it tells whether it is newlyadded goal which can be added dynamically through the application
            // }*/
            var WingsData = _context.TblffWings.Where(p => p.ProfileId == profileId).OrderBy(u => u.GoalPriority)
                                   .Select(u => new { u.Id, u.GoalPriority, u.GoalName, u.GoalStartYear, u.GoalPlanYear, u.GoalEndYear, u.TimeHorizon, u.NewGoals }).ToList();

            ////var selectedFormsdata = new List<object>();

            foreach (var goal in WingsData)
            {
                var goalObj = new
                {
                    id = goal.Id,
                    priority = goal.GoalPriority,
                    goal = goal.GoalName,
                    goalStartYear = goal.GoalStartYear,
                    goalPlanYear = goal.GoalPlanYear,
                    //goalEndYear = goal.GoalEndYear ?? "",  // In case GoalEndYear is null, use an empty string
                    timeHorizon = goal.TimeHorizon,
                    newGoals = goal.NewGoals
                };
                var goalNames = new
                {
                    goal = goal.GoalName
                };
                GoalsNameList.Add(goalNames);  // Add each goal object to the selectedForms array
                selectedFormsdata.Add(goalObj);  // Add each goal object to the selectedForms array
            }

            return new
            {
                forms = new[]
                    {
                        new { id = 1, goal = "Emergency Fund" },
                        new { id = 2, goal = "Retirement Accumulation" },
                        new { id = 3, goal = "World Tour" },
                        new { id = 4, goal = "Purchase of Dream Car" },
                        new { id = 5, goal = "Purchase of Dream House" },
                        new { id = 6, goal = "Seed Capital for Business" },
                        new { id = 7, goal = "Charity" }
                    },
                selectedForms = selectedFormsdata
            };
        }

        private object UserBuildAlertnessSection(long profileId)
        {
            var profileIdParam = new SqlParameter("@ProfileId", profileId);

            // Call stored procedure
            var jsonResult = _context.AlertnessJsonResult
                .FromSqlRaw("EXEC Sp_AlertnessAPIJshonForPDF @ProfileId", profileIdParam)
                .AsEnumerable()
                .FirstOrDefault();

            if (jsonResult == null || string.IsNullOrEmpty(jsonResult.AlertnessJson))
                return new { }; // return empty object if SP fails

            // Parse raw JSON string to dynamic object
            var alertnessData = JsonConvert.DeserializeObject<object>(jsonResult.AlertnessJson);
            return alertnessData;
        }

        private object UserBuildKnowledgeSection(long profileId)
        {
            var kData = _context.TblffKnowledgeRisks.Where(p => p.ProfileId == profileId).FirstOrDefault();

            return new
            {
                knowledge = new
                {
                    riskCapacity = string.IsNullOrEmpty(kData?.RiskCapacity) ? "" : kData.RiskCapacity,
                    riskRequirement = string.IsNullOrEmpty(kData?.RiskRequirement) ? "" : kData.RiskRequirement,
                    //plannerAssessmentOnRiskProfile = string.IsNullOrEmpty(kData?.PlannerAssessmentOnRiskProfile) ? "" : kData.PlannerAssessmentOnRiskProfile,
                    riskTolerance = string.IsNullOrEmpty(kData?.RiskTolerance) ? "" : kData.RiskTolerance,
                }
            };
            //var knowledge = new Dictionary<string, object>
            //{
            //    { "Risk Capacity", string.IsNullOrEmpty(kData?.RiskCapacity) ? "" : kData.RiskCapacity },
            //    { "Risk Requirement", string.IsNullOrEmpty(kData?.RiskRequirement) ? "" : kData.RiskRequirement },
            //    // { "Total Risk Profile Score", string.IsNullOrEmpty(kData?.TotalRiskProfileScore) ? "" : kData.TotalRiskProfileScore },
            //    // { "Planner Assessment On Risk Profile", string.IsNullOrEmpty(kData?.PlannerAssessmentOnRiskProfile) ? "" : kData.PlannerAssessmentOnRiskProfile },
            //    { "Risk Tolerance", string.IsNullOrEmpty(kData?.RiskTolerance) ? "" : kData.RiskTolerance }
            //};

            //return new Dictionary<string, object>
            //{
            //    { "knowledge", knowledge }
            //};

        }

        private object UserBuildExecutePlanSection(long profileId)
        {
            var executionData = _context.TblffWingsGoalStep5ExecutionData
                            .Where(p => p.ProfileId == profileId)
                            .Select(u => new { u.GoalName, u.ExecutionDescription, u.ExecutionValue })
                            .ToList();
            //.Select(u => new { u.GoalName, u.ExecutionDescription, u.ExecutionValue })

            var groupedGoals = executionData
                    .GroupBy(e => e.GoalName)
                    .Select(group =>
                    {
                        var goalDict = new Dictionary<object, object>
                        {
                            ["Goal Name"] = group.Key
                        };

                        foreach (var item in group)
                        {
                            var keyWithSpace = item.ExecutionDescription?.Trim();  // Use label directly
                            if (!string.IsNullOrWhiteSpace(keyWithSpace) && !goalDict.ContainsKey(keyWithSpace))
                            {
                                goalDict[keyWithSpace] = item.ExecutionValue ?? "";
                            }
                        }
                        return goalDict;
                    }).ToList();


            //var groupedGoals = executionData
            //    .GroupBy(e => e.GoalName)
            //    .Select(group =>
            //    { 

            //        //var goalDict = new Dictionary<string, object>
            //        //{
            //        //    ["name"] = group.Key
            //        //};

            //        //foreach (var item in group)
            //        //{
            //        //    var key = GetJsonKey(item.ExecutionDescription);
            //        //    if (!goalDict.ContainsKey(key))
            //        //        goalDict[key] = item.ExecutionValue ?? "";
            //        //}
            //        ///***key need to space types also render in html

            //        return goalDict;
            //    }).ToList();
            //return new { goalData = groupedGoals };

            return new
            {
                formData = new
                {
                    //lifeInsurance = new { incomeUsedForFamily = "", fundsReturnRate = "", inflationRate = "" },
                    //spouseDetails = new { Sliabilities = "" },
                    //goalNames = new { GoalsNameList },
                    goalData = groupedGoals
                }
            };

        }
        private object UserBuildInvestSection(long profileId)
        {
            var InvestMasterDataUser = _context.TblffInvestWingsGoalMasters
                                    .Where(p => p.ProfileId == profileId)
                                    .Select(u => new { u.Id, u.IntendedSipmonthly, u.AvailableLumpsum, u.MonthlySavings })
                                    .FirstOrDefault();

            var InvestDataUser = _context.TblffInvestWingsGoals
                .Where(p => p.ProfileId == profileId)
                .Select(u => new
                {
                    //id = u.Id,
                    goal = u.GoalName ?? "",
                    //goalOptions = new List<object>(),
                    //typeOfFund = "",
                    //selectedFunds = "",
                    lumpSum = u.LumpsumAmount ?? 0,
                    SIP = u.Sipamount ?? 0,
                    //selectedFundsOptions = new List<object>()
                }).ToList();

            //var GeneralInsuranceDataUser = _context.TblffInvestWingsGoals
            //    .Where(p => p.ProfileId == profileId)
            //    .Select(u => new { u.Id, u.GoalName, u.LumpsumAmount, u.Sipamount })
            //    .ToList();//?? new List<object>()

            //var LifeInsuranceDataUser = _context.TblffInvestWingsGoals
            //    .Where(p => p.ProfileId == profileId)
            //    .Select(u => new { u.Id, u.GoalName, u.LumpsumAmount, u.Sipamount })
            //    .ToList();//?? new List<object>()

            return new
            {
                //wings = new { expectedReturns = new { GoalsNameList }, },
                //alertness = new { familyMonthlyIncomeTotal = "", familyMonthlyExpensesTotal = "" },
                //knowledge = new { selectedRiskProfile = "" },
                //executeWithPrecision = new
                //{
                //    hofInsurance = new
                //    {
                //        homeInsurance = new { currentCoverage = "", requiredCoverage = "", shortfall = "" },
                //        carInsurance = new { currentCoverage = "", requiredCoverage = "", shortfall = "" }
                //    }
                //},
                actNow = new
                {
                    savingsForInvestments = new { intendedSipMonthly = InvestMasterDataUser?.IntendedSipmonthly ?? 0, availableLumpsum = InvestMasterDataUser?.AvailableLumpsum ?? 0, MonthlySavings = InvestMasterDataUser?.MonthlySavings },
                    //lifeInsuranceReq = LifeInsuranceDataUser, // dynamically generated
                    //generalInsuranceReq = GeneralInsuranceDataUser, // dynamically generated
                    //actionPlanFinancialGoals = new
                    //{
                    //    goals = new { GoalsNameList },// new List<object>(),
                    //    total = new { totalLumpSum = "", totalFixorSipAmt = "", totalSipAmt = "", totalGapBtnSip = "" }
                    //},
                    //goalStatusReport = new { GoalsNameList },
                    //totalGoalStatusReport = new
                    //{
                    //    totalFutureValueCurrentCorpus = "",
                    //    totalFutureValueLumpSum = "",
                    //    totalFutureValueSip = "",
                    //    totalTotalCorpus = "",
                    //    totalReqCorpus = ""
                    //},
                    //goalsOptions = new { GoalsNameList },
                    InvestGoal = InvestDataUser // List of funds
                }
            };
        }





    }


    //public static class JsonResultExtensions
    //{
    //    public static string ToJsonString(this object obj)
    //    {
    //        var options = new JsonSerializerOptions { WriteIndented = true };
    //        return JsonSerializer.Serialize(obj, options);
    //    }
    //}
    // need to add in dbcontext
    //public DbSet<AlertnessJsonResult> AlertnessJsonResult { get; set; }
    //modelBuilder.Entity<AlertnessJsonResult>().HasNoKey();
    public class AlertnessJsonResult
    {
        public string? AlertnessJson { get; set; }
    }

}
