using FactFinderWeb.BLL;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Drawing.Drawing2D;
using System.Globalization;
//Comprehensive
namespace FactFinderWeb.Controllers
{
    ////[Route("plan")]
    //[Route("Comprehensive/[action]/{id?}")]
    //[Route("Wealth/[action]/{id?}")]
    //[Route("Basic/[action]/{id?}")]
    //[Route("Zero2one/[action]/{id?}")]
    [Route("{planType}/[action]/{pid?}")]
    public class ComprehensiveController : Controller
    {
        private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly AwarenessServices _AwarenessServices;
        private readonly WingsServices _WingsServices;
        private readonly KnowledgeThatMattersServices _KnowledgeThatMattersServices;
         private readonly long _userID;
        private readonly long _profileId;
        private readonly HttpContext _httpContext;
        private readonly ExecutionServices _executionServices;
        private readonly InvestServices _investServices;
        private readonly AlertnessMappingService _alertnessMappingService;
        private readonly UtilityHelperServices _utilService;
        private readonly IWebHostEnvironment _env;
        private readonly string _planType;
        int updateRows = 0;

        public ComprehensiveController(ResellerBoyinawebFactFinderWebContext context, AwarenessServices awarenessServices, IHttpContextAccessor httpContextAccessor, WingsServices wingsServices, KnowledgeThatMattersServices knowledgeThatMattersServices, ExecutionServices executionServices, InvestServices investServices, AlertnessMappingService alertnessMappingService, JSONDataUtility jSONDataUtility, UtilityHelperServices utilityHelperServices, IWebHostEnvironment env)
        { 
            _context = context;
            _AwarenessServices = awarenessServices;
            _WingsServices = wingsServices;
            _KnowledgeThatMattersServices = knowledgeThatMattersServices;

           _httpContext = httpContextAccessor.HttpContext;
           // var userIdStr = _httpContext.Session.GetString("UserId");

          //  _planType = _httpContext.Session.GetString("UserPlan");
           // _userID = Convert.ToInt64(userIdStr);
            _executionServices = executionServices;
            _investServices = investServices;
            _alertnessMappingService = alertnessMappingService;
            _utilService = utilityHelperServices;
            _env = env;

            // ✅ Use _httpContext safely
            //_httpContext.Session.SetString("UserFullName", "ajay");
            //_httpContext.Session.SetString("Useremail", "sss@g.com");
            //_httpContext.Session.SetString("UserStep", "S1");
            //_httpContext.Session.SetString("UserPlan", "Wealth");
            //_httpContext.Session.SetString("UserId", "110");

            var userIdStr = _httpContext.Session.GetString("UserId");
            _planType = _httpContext.Session.GetString("UserPlan");
            
             _userID = Convert.ToInt64(userIdStr);
            var planFromRoute = _httpContext.Request.RouteValues["planType"]?.ToString();

            if (!string.IsNullOrEmpty(planFromRoute))
            {
                _planType = planFromRoute;  // ex: "wealth" from /wealth/awareness/...
                _httpContext.Session.SetString("UserPlan", _planType); // keep session in sync
            }
            else
            {
                // fallback to session if route has nothing
                _planType = _httpContext.Session.GetString("UserPlan") ?? "Comprehensive";
            }
            var _profileId1 = _httpContext.Request.RouteValues["pid"]?.ToString() ?? _httpContext.Request.Query["pid"].ToString();

            var idFromQuery = _httpContext.Request.RouteValues["pid"]?.ToString();

          

            if (!string.IsNullOrEmpty(idFromQuery))
            {
                try
                {
                    var decrypted = CryptoHelper.Decrypt(idFromQuery);
                    _profileId = Convert.ToInt64(decrypted);

                    // store in session for later use
                     _httpContext.Session.SetString("profileId", _profileId.ToString());
                    _profileId = Convert.ToInt64(_httpContext.Session.GetString("profileId"));
                }
                catch
                {
                    // invalid id - ignore or log
                }
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(_userID.ToString()) || _userID == 0)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                { "controller", "User" },
                { "action", "Login" }
            });
            }

            base.OnActionExecuting(context);
        }


        //[Route("Awareness")]
		[HttpGet]
        public async Task<IActionResult> Awareness(string id)
        {
            //string checkEmail = _AwarenessServices.checkEmailExistProfileTbl(id);
            //if (checkEmail == null)
            //{ 
            //return Redirect("/login");
            //}
            //string decryptedId;
            //try
            //{
            //    decryptedId = CryptoHelper.Decrypt(id);
            //}
            //catch (FormatException)
            //{
            //    return BadRequest("Invalid ID format (not Base64)");
            //}
            HttpContext.Session.SetString("PageIndex", "1");
            var awareb = await _AwarenessServices.AwarenessProfileDetail(_profileId);
                ViewData["errorcheck"] = ""; 
            //var jsondata= await _jsonData.GetAwarenessJSON(_userID);
            return View(awareb);
        }

        [HttpPost]
        public async Task<IActionResult> Awareness(AwarenessViewModel awarenessViewModel, string btnSubmit)
        {
            ModelState.Remove("btnSubmit");

            var awareb = await _AwarenessServices.AwarenessProfileDetail(_profileId);
            bool checkPAN =  _AwarenessServices.checkPANExist(awarenessViewModel.ProfileDetail.PAN, _profileId);
                ViewData["errorcheck"] = "";// "PAN number exists.";
            if (checkPAN)
            {
                ViewData["errorcheck"] = "PAN number exists.";// "";//
                return View(awarenessViewModel);
            }

            if (awarenessViewModel.ProfileDetail.MaritalStatus?.ToLower() != "married")
            {
                // Skip validation for these fields if not married
                ModelState.Remove("ProfileDetail.SpouseDetails");
                foreach (var key in ModelState.Keys.Where(k => k.StartsWith("SpouseDetails")))
                {
                    ModelState.Remove(key);
                }
                ModelState.Remove("Assumptions.SpouseRetirement");
                ModelState.Remove("Assumptions.SpouseLifeExpectancy");

            }
            //if (awarenessViewModel.ProfileDetail.HaveChildren.ToLower() == "no")
                ModelState.Remove("ChildrenDetail");
                foreach (var key in ModelState.Keys.Where(k => k.Contains("ChildrenDetail")))
                {
                    ModelState.Remove(key);
                }
            if (awarenessViewModel.SpouseDetails.SpouseOccupation == "Service" && awarenessViewModel.SpouseDetails.SpouseOccupation == "Self Employed" && awarenessViewModel.SpouseDetails.SpouseOccupation == "Professional")
            {
                ModelState.Remove("Assumptions.SpouseRetirement");
                ModelState.Remove("Assumptions.SpouseLifeExpectancy");
            }
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key; // The property name
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Property: {key}, Error: {error.ErrorMessage}");
                    }
                }

                return View(awarenessViewModel);
            }

            // Process the form data here

            long result = await _AwarenessServices.AwarenessAddProfileDatail(awarenessViewModel.ProfileDetail, _profileId);
            long resultb = await _AwarenessServices.AwarenessAddProfileAssumptions(awarenessViewModel.Assumptions, _profileId);

            if (awarenessViewModel.ProfileDetail.MaritalStatus.ToLower() == "married")
            {
                long resulta = await _AwarenessServices.AwarenessAddProfileSpouseDatail(awarenessViewModel.SpouseDetails, _profileId);
            }


            if (btnSubmit == "Save")
            {
                ViewData["msg"] = "Saved successfully.";
                return View(awarenessViewModel);
            }
             


            //return RedirectToAction("wings", _planType, new { id = CryptoHelper.Encrypt(_profileId.ToString()) });

            return RedirectToRoute(new
            {
                planType = _planType,   // taken from session
                controller = "Comprehensive",
                action = "Wings",
                pid = CryptoHelper.Encrypt(_profileId)
            });


        }

		

		[HttpGet]
        public async Task<IActionResult> Wings()
        {
            HttpContext.Session.SetString("PageIndex", "2");
            var wingsViewModel = new WingsViewModel();
            wingsViewModel = await _WingsServices.WingsList();
            wingsViewModel.GoalOptions = await _WingsServices.WingsBindSelect(_planType);
            wingsViewModel.ChildrenLists = await _WingsServices.WingsChildrenList();

            var dboMain = (from a in _context.TblffAwarenessProfileDetails
                           where a.ProfileId == _profileId
                           select new
                           {
                               a.Dob
                           }).FirstOrDefault();

            var dbospouse = (from a in _context.TblffAwarenessSpouses
                             where a.ProfileId == _profileId
                             select new
                             {
                                 a.SpouseDob
                             }).FirstOrDefault();

            var lifeExpectancy = (from a in _context.TblffAwarenessAssumptions
                                  where a.ProfileId == _profileId
                                  select new
                                  {
                                      a.ApplicantLifeExpectancy,
                                      a.SpouseLifeExpectancy
                                  }).FirstOrDefault();

            // Default goalEndYear
            int goalEndYear = 0;

            // Try to parse DOB strings safely
            DateTime applicantDob, spouseDob;
            bool applicantDobParsed = DateTime.TryParse(dboMain?.Dob, out applicantDob);
            bool spouseDobParsed = DateTime.TryParse(dbospouse?.SpouseDob, out spouseDob);

            if (lifeExpectancy != null && (applicantDobParsed || spouseDobParsed))
            {
                int applicantLifeExp = lifeExpectancy.ApplicantLifeExpectancy ?? 0;
                int spouseLifeExp = lifeExpectancy.SpouseLifeExpectancy ?? 0;

                int applicantEndYear = applicantDobParsed ? applicantDob.Year + applicantLifeExp : 0;
                int spouseEndYear = spouseDobParsed ? spouseDob.Year + spouseLifeExp : 0;

                goalEndYear = Math.Max(applicantEndYear, spouseEndYear);
            }

            Console.WriteLine($"Goal End Year: {goalEndYear}");

 
                wingsViewModel.GoalEndYear = goalEndYear;
           
            wingsViewModel.ApplicantDataDto = await _WingsServices.GetApplicantDataAsync(_profileId);
            //wingsViewModel = await _WingsServices.WingsBindSelect();
            //wingsViewModel.goalList = wingsGetData.goalList;
            //wingsViewModel.GoalOptions = wingsChildrenData.GoalOptions;
            return View(wingsViewModel);
        }

        [HttpPost]
        public IActionResult Wings(WingsViewModel wingsViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(wingsViewModel);
            }
            //int result = _AwarenessServices.AwarenessAddProfileDatile(awarenessViewModel.PersonalDetails);
            return View();
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGoals([FromBody] List<UserWingsUI> submittedGoals) //UserWingsUI
        {
            if (submittedGoals == null || !submittedGoals.Any())
                return BadRequest("No data received.");

           int i = await _WingsServices.WingsAddDeleteGoal(submittedGoals);



            if (i <= 0)
            {
                return Ok(new { success = true, message = "no new goals to save." });
                //return BadRequest("Failed to save goals. Please Try after sometime.");
            } else
            {
                //return ok("wings", "Comprehensive");
                return Ok(new { success = true, message = "Goals Saved successfully." });
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGoalsOnLink([FromBody] List<UserWingsUI> submittedGoals) //UserWingsUI
        {
            if (submittedGoals == null || !submittedGoals.Any())
                return BadRequest("No data received.");

            int i = await _WingsServices.WingsAddDeleteGoal(submittedGoals);

                ViewData["msg"] = "Saved successfully.";

            if (i <= 0)
            {
                return Ok(new { success = true, message = "no new goals to save." });
                //return BadRequest("Failed to save goals. Please Try after sometime.");
            }
            else
            {
                //return ok("wings", "Comprehensive");
                return Ok(new { success = true, message = "Goals Saved successfully." });
            }

        }


        #region ***********************************For x By Manoj**************************************************
        public async Task<IActionResult> Alertness()
        {
            HttpContext.Session.SetString("PageIndex", "3");

            long? profileId = _profileId;
            if (profileId == null)
            {
                return RedirectToAction("Login");
            }
            var viewModel = new AlertnessViewModel();



            //Income
            var incomeDetail = await _context.TblffAlertnesIncomeDetails
                                             .Where(x => x.ProfileId == profileId)
                                             .OrderByDescending(x => x.CreatedDate) // Use your actual datetime column
                                             .FirstOrDefaultAsync();

            List<TblffAlertnesIncomeExtra> incomeExtras = new();
            if (incomeDetail != null)
            {
                // Now fetch spouse extras based on IncomeDetailId
                incomeExtras = await _context.TblffAlertnesIncomeExtras
                                               .Where(x => x.IncomeDetailId == incomeDetail.Id)
                                               .ToListAsync();
            }


            List<TblffAlertnesSpouseIncomeExtra> spouseExtras = new();
            if (incomeDetail != null)
            {
                // Now fetch spouse extras based on IncomeDetailId
                spouseExtras = await _context.TblffAlertnesSpouseIncomeExtras
                                             .Where(x => x.IncomeDetailId == incomeDetail.Id)
                                             .ToListAsync();
            }

            // Expense Detail
            var expenseDetail = await _context.TblffAlertnesExpenses
                                              .Where(x => x.ProfileId == profileId)
                                              .OrderByDescending(x => x.CreatedDate)
                                              .FirstOrDefaultAsync();


            List<TblffAlertnesExpenseNew> expenseNew = new();
            if (expenseDetail != null)
            {
                expenseNew = await _context.TblffAlertnesExpenseNews
                                            .Where(x => x.ExpenseId == expenseDetail.Id)
                                            .ToListAsync();
            }



            //3 Savings Detail
            var savings = await _context.TblffAlertnesSavings
                                        .Include(s => s.TblffAlertnesCommittedSavings)
                                        //.Include(s => s.TblffAlertnesNewSavings)
                                        .FirstOrDefaultAsync(s => s.ProfileId == profileId);



            if (savings == null)
            {
                savings = new TblffAlertnesSaving
                {
                    ProfileId = profileId,
                    TotalCommittedSavings = 0,
                    CreatedAt = DateTime.Now

                };



            }

            var committedSavings = await _context.TblffAlertnesCommittedSavings
                                                 .Where(x => x.SavingsId == savings.Id)
                                                 .FirstOrDefaultAsync();


            List<TblffAlertnesNewSaving> newSavings = new();
            newSavings = await _context.TblffAlertnesNewSavings
                                                 .Where(x => x.SavingsId == savings.Id)
                                                 .ToListAsync();


            //4EMI
            List<TblffAlertnesEmiDetail> emiDetail = new();
            emiDetail = await _context.TblffAlertnesEmiDetails
                                            .Where(x => x.ProfileId == profileId)
                                    .ToListAsync();


            var emiOneTimeLoanRepayment = await _context.TblffAlertnesOneTimeLoanRepayments
                                                            .Where(x => x.ProfileId == profileId)
                                                              .FirstOrDefaultAsync();

            //Step-5 PostIncomeDetailsViewModel
            var postIncomeDetailsData = await _context.TblAffAlertnesPostIncomeDetails
                                                      .Where(x => x.ProfileId == profileId)
                                                      .FirstOrDefaultAsync();

            //Step-6 
            /*
            Master Net Worth TblFF_Alertnes_NetWorth
            New Investments TblFF_Alertnes_NewInvestments
            New Other Assets    TblFF_Alertnes_NewOtherAssets
            New Liabilities TblFF_Alertnes_NewLiabilities
            */

            var alertnesNetWorths = await _context.TblFfAlertnesNetWorths
                                                  .Where(x => x.ProfileId == profileId).FirstOrDefaultAsync();
            List<TblFfAlertnesNewInvestment> newInvestments = new();
            List<TblFfAlertnesNewOtherAsset> newOtherAssets = new();
            List<TblFfAlertnesNewLiability> newLiabilities = new();

            if (alertnesNetWorths != null)
            {
                newInvestments = await _context.TblFfAlertnesNewInvestments
                                                 .Where(x => x.ProfileId == profileId).ToListAsync();

                newOtherAssets = await _context.TblFfAlertnesNewOtherAssets
                                                 .Where(x => x.ProfileId == profileId).ToListAsync();

                newLiabilities = await _context.TblFfAlertnesNewLiabilities
                                                 .Where(x => x.ProfileId == profileId).ToListAsync();
            }

            //Step-7 LoanDetailViewModel
            var loanDetailData = await _context.TblffAlertnesDebts
                                                     .Where(x => x.ProfileId == profileId)
                                                     .FirstOrDefaultAsync();

            //Step-8 Insurance
            List<TblFfAlertnesGeneralInsurance> generalInsurances = new();
            List<TblFfAlertnesLifeInsurance> lifeInsurances = new();

            generalInsurances = await _context.TblFfAlertnesGeneralInsurances
                                             .Where(x => x.ProfileId == profileId).ToListAsync();
            lifeInsurances = await _context.TblFfAlertnesLifeInsurances
                                          .Where(x => x.ProfileId == profileId).ToListAsync();


            //// Fetch maritalStatus safely  from 1st screen awerness and save in Viewbag
            // Normalize to string (avoid null reference in view) for EMI table and null Manage
            var profileData = await _alertnessMappingService.GetProfileData((long)profileId);
            var safeProfileData = new ProfileDataDto
            {
                Name = profileData?.Name ?? "NA",
                MaritalStatus = profileData?.MaritalStatus ?? string.Empty,
                SpouseName = profileData?.SpouseName ?? "NA"
            };
            ViewBag.ProfileData = safeProfileData;

            // ✅ Dropdown binding
            ViewBag.NameOptions = new List<SelectListItem>
             {
                 new SelectListItem { Text = safeProfileData.Name, Value = safeProfileData.Name },
                 new SelectListItem { Text = safeProfileData.SpouseName, Value = safeProfileData.SpouseName }
             };




            if (incomeDetail != null)
            {

                viewModel = new AlertnessViewModel
                {
                    ProfileId = incomeDetail.ProfileId,
                    Id = incomeDetail.Id,
                    Basic = incomeDetail.Basic,
                    TotalExpense = incomeDetail.TotalExpense,
                    Hra = incomeDetail.Hra,
                    EducationAllowance = incomeDetail.EducationAllowance,
                    MedicalAllowance = incomeDetail.MedicalAllowance,
                    LTA = incomeDetail.Lta,
                    Conveyance = incomeDetail.Conveyance,
                    OtherAllowance = incomeDetail.OtherAllowance,
                    PF = incomeDetail.Pf,
                    Gratuity = incomeDetail.Gratuity,
                    Reimbursement = incomeDetail.Reimbursement,
                    BusinessIncome = incomeDetail.BusinessIncome,
                    FoodCoupon = incomeDetail.FoodCoupon,
                    MonthlyPension = incomeDetail.MonthlyPension,
                    InterestIncome = incomeDetail.InterestIncome,
                    AnnualBonus = incomeDetail.AnnualBonus,
                    PerformanceLinked = incomeDetail.PerformanceLinked,

                    AnnualTotalIncome = incomeDetail.AnnualTotalIncome,
                    OverallMonthlyIncome = incomeDetail.MonthlyIncome,
                    PostITIncomeOld = incomeDetail.PostItincomeOld,
                    PostITIncomeNew = incomeDetail.PostItincomeNew,
                    //--spouse
                    SpouseBasic = incomeDetail.SpouseBasic,
                    SpouseHRA = incomeDetail.SpouseHra,
                    SpouseEducationAllowance = incomeDetail.SpouseEducationAllowance,
                    SpouseMedicalAllowance = incomeDetail.SpouseMedicalAllowance,
                    SpouseLTA = incomeDetail.SpouseLta,
                    SpouseConveyance = incomeDetail.SpouseConveyance,
                    SpouseOtherAllowance = incomeDetail.SpouseOtherAllowance,
                    SpousePF = incomeDetail.SpousePf,
                    SpouseGratuity = incomeDetail.SpouseGratuity,
                    SpouseReimbursement = incomeDetail.SpouseReimbursement,
                    SpouseBusinessIncome = incomeDetail.SpouseBusinessIncome,
                    SpouseFoodCoupon = incomeDetail.SpouseFoodCoupon,
                    SpouseMonthlyPension = incomeDetail.SpouseMonthlyPension,
                    SpouseInterestIncome = incomeDetail.SpouseInterestIncome,
                    SpouseMonthlyTotalIncome = incomeDetail.SpouseMonthlyTotalIncome,
                    SpouseConsolidatedIncome = incomeDetail.SpouseConsolidatedIncome,
                    SpouseAnnualBonus = incomeDetail.SpouseAnnualBonus,
                    SpousePerformanceLinked = incomeDetail.SpousePerformanceLinked,
                    SpouseAnnualTotalIncome = incomeDetail.SpouseAnnualTotalIncome,
                    SpouseOverallMonthlyIncome = incomeDetail.SpouseOverallMonthlyIncome,
                    SpousePostITIncomeOld = incomeDetail.SpousePostItincomeOld,
                    SpousePostITIncomeNew = incomeDetail.SpousePostItincomeNew,

                    IsActive = incomeDetail.IsActive,
                    CreatedDate = incomeDetail.CreatedDate,
                    ModifiedDate = incomeDetail.ModifiedDate,

                    IncomeExtras = incomeExtras.Select(x => new TblffAlertnesIncomeExtra
                    {
                        FieldName = x.FieldName,
                        FieldValue = x.FieldValue,
                        Type = x.Type
                    }).ToList(),



                    SpouseExtras = spouseExtras.Select(x => new TblffAlertnesSpouseIncomeExtra
                    {
                        FieldName = x.FieldName,
                        FieldValue = x.FieldValue,
                        Type = x.Type
                    }).ToList(),

                    ExpenseNew = expenseNew.Select(x => new TblffAlertnesExpenseNew
                    {
                        FieldName = x.FieldName,
                        FieldValue = x.FieldValue,
                        Section = x.Section,
                        Type = x.Type
                    }).ToList(),



                    //Saving
                    Savings = savings,
                    NewSavings = newSavings,

                    //step-4 EMI
                    EmiDetail = emiDetail,

                    //Step-6
                    NewInvestments = newInvestments,
                    NewOtherAssets = newOtherAssets,
                    NewLiabilities = newLiabilities,

                    //step-8  fill record in viewmodel

                    //step-8 Insurense fill    
                    GeneralInsuranceViewModel = generalInsurances,
                    LifeInsuranceViewModel = lifeInsurances,


                };




                //expense
                // Expense
                if (expenseDetail != null)
                {
                    viewModel.ExpenseDetail = new ExpenseViewModel
                    {
                        Id = expenseDetail.Id,
                        GroceryProvisionMilk = ParseNullableDecimal(expenseDetail.GroceryProvisionMilk),
                        DomesticHelp = ParseNullableDecimal(expenseDetail.DomesticHelp),
                        IronLaundry = ParseNullableDecimal(expenseDetail.IronLaundry),
                        Driver = ParseNullableDecimal(expenseDetail.Driver),
                        Fuel = ParseNullableDecimal(expenseDetail.Fuel),
                        CarCleaning = ParseNullableDecimal(expenseDetail.CarCleaning),
                        Maintenance = ParseNullableDecimal(expenseDetail.Maintenance),
                        TaxiPublicTransport = ParseNullableDecimal(expenseDetail.TaxiPublicTransport),
                        AirTrainTravel = ParseNullableDecimal(expenseDetail.AirTrainTravel),
                        Mobile = ParseNullableDecimal(expenseDetail.Mobile),
                        LandlineBroadband = ParseNullableDecimal(expenseDetail.LandlineBroadband),
                        DataCard = ParseNullableDecimal(expenseDetail.DataCard),
                        Electricity = ParseNullableDecimal(expenseDetail.Electricity),
                        HouseTax = ParseNullableDecimal(expenseDetail.HouseTax),
                        SocietyCharge = ParseNullableDecimal(expenseDetail.SocietyCharge),
                        Rents = ParseNullableDecimal(expenseDetail.Rents),
                        Cable = ParseNullableDecimal(expenseDetail.Cable),
                        Lpg = ParseNullableDecimal(expenseDetail.Lpg),
                        WaterBill = ParseNullableDecimal(expenseDetail.WaterBill),
                        NewsPaper = ParseNullableDecimal(expenseDetail.NewsPaper),
                        SchoolFees = ParseNullableDecimal(expenseDetail.SchoolFees),
                        Tuitions = ParseNullableDecimal(expenseDetail.Tuitions),
                        UniformsAccessories = ParseNullableDecimal(expenseDetail.UniformsAccessories),
                        BooksStationery = ParseNullableDecimal(expenseDetail.BooksStationery),
                        PicnicsActivities = ParseNullableDecimal(expenseDetail.PicnicsActivities),
                        MoviesTheatre = ParseNullableDecimal(expenseDetail.MoviesTheatre),
                        DiningOut = ParseNullableDecimal(expenseDetail.DiningOut),
                        ClubhouseExpenses = ParseNullableDecimal(expenseDetail.ClubhouseExpenses),
                        PartiesAtHome = ParseNullableDecimal(expenseDetail.PartiesAtHome),
                        ClothingGrooming = ParseNullableDecimal(expenseDetail.ClothingGrooming),
                        VacationTravel = ParseNullableDecimal(expenseDetail.VacationTravel),
                        Festivals = ParseNullableDecimal(expenseDetail.Festivals),
                        KidsBirthdays = ParseNullableDecimal(expenseDetail.KidsBirthdays),
                        FamilyFunctions = ParseNullableDecimal(expenseDetail.FamilyFunctions),
                        Medical = ParseNullableDecimal(expenseDetail.Medical),
                        VehicleServicing = ParseNullableDecimal(expenseDetail.VehicleServicing),
                        HomeRepair = ParseNullableDecimal(expenseDetail.HomeRepair),
                        NewHomeAppliances = ParseNullableDecimal(expenseDetail.NewHomeAppliances),
                        LifeInsurance = ParseNullableDecimal(expenseDetail.LifeInsurance),
                        HomeInsurance = ParseNullableDecimal(expenseDetail.HomeInsurance),
                        MedicalInsurance = ParseNullableDecimal(expenseDetail.MedicalInsurance),
                        CarInsurance = ParseNullableDecimal(expenseDetail.CarInsurance),
                        FeeToCa = ParseNullableDecimal(expenseDetail.FeeToCa),
                        OtherConsultant = ParseNullableDecimal(expenseDetail.OtherConsultant),
                        Donations = ParseNullableDecimal(expenseDetail.Donations)
                    };
                }

                //saving
                if (committedSavings != null)
                {
                    viewModel.CommittedSaving = new CommittedSavingViewModel
                    {
                        Name = committedSavings.Name,
                        CurrentValue = committedSavings.CurrentValue,
                        MonthlyContribution = committedSavings.MonthlyContribution,
                        TillWhen = committedSavings.TillWhen?.ToString("dd/MM/yyyy"),
                        FollowUp = committedSavings.FollowUp
                    };

                }

                //EMI

                if (emiOneTimeLoanRepayment != null)
                {
                    viewModel.EmiOneTimeLoanRepayment = new EmiOneTimeLoanRepayment
                    {
                        ProfileId = emiOneTimeLoanRepayment.ProfileId,
                        TotalEmi = emiOneTimeLoanRepayment?.TotalEmi,
                        NetCashflows = emiOneTimeLoanRepayment?.NetCashflows,
                        OneTimeLoanRepayment = emiOneTimeLoanRepayment?.OneTimeLoanRepayment,

                    };
                }

                //Step-5

                if (postIncomeDetailsData != null)
                {

                    viewModel.postIncomeDetailsViewModel = new PostIncomeDetailsViewModel
                    {
                        Id = postIncomeDetailsData.Id,
                        ProfileId = postIncomeDetailsData?.ProfileId ?? 0,
                        PostIncome = postIncomeDetailsData?.PostIncome,
                        MustHaveExpensesPercent = postIncomeDetailsData?.MustHaveExpensesPercent,
                        OptionalExpensesPercent = postIncomeDetailsData?.OptionalExpensesPercent,
                        SavingsPercent = postIncomeDetailsData?.SavingsPercent,
                        ProjectedGrowthRateNext5Years = postIncomeDetailsData?.ProjectedGrowthRateNext5Years,
                        ProjectedGrowthRate6To10Years = postIncomeDetailsData?.ProjectedGrowthRate6To10Years,
                        InflationRate = postIncomeDetailsData?.InflationRate,
                    };

                }

                //Step-6
                if (alertnesNetWorths != null)
                {
                    viewModel.netWorthViewModel = new NetWorthViewModel
                    {
                        NetWorthId = alertnesNetWorths.NetWorthId,
                        ProfileId = alertnesNetWorths.ProfileId,

                        // Investments
                        CashInHand = alertnesNetWorths.CashInHand ?? 0,
                        EmployeeProvidendFund = alertnesNetWorths.EmployeeProvidendFund ?? 0,
                        PPF = alertnesNetWorths.Ppf ?? 0,
                        FixedDeposits = alertnesNetWorths.FixedDeposits ?? 0,
                        MutualFundsShares = alertnesNetWorths.MutualFundsShares ?? 0,
                        PaidUpValueOfInsurancePolicies = alertnesNetWorths.PaidUpValueOfInsurancePolicies ?? 0,
                        OthersGratuity = alertnesNetWorths.OthersGratuity ?? 0,

                        // Other Assets
                        Home1 = alertnesNetWorths.Home1 ?? 0,
                        Home2 = alertnesNetWorths.Home2 ?? 0,
                        Land = alertnesNetWorths.Land ?? 0,
                        Car = alertnesNetWorths.Car ?? 0,
                        CommercialProperty = alertnesNetWorths.CommercialProperty ?? 0,
                        Jewellery = alertnesNetWorths.Jewellery ?? 0,
                        ValueOfBusiness = alertnesNetWorths.ValueOfBusiness ?? 0,
                        OtherAssetsOther = alertnesNetWorths.OtherAssetsOther ?? 0,

                        // Liabilities
                        Home2Loan = alertnesNetWorths.Home2Loan ?? 0,
                        Home1Loan =alertnesNetWorths.Home1Loan ?? 0,
                        LandLoan = alertnesNetWorths.LandLoan ?? 0,
                        CommercialPropertyLoan = alertnesNetWorths.CommercialPropertyLoan ?? 0,
                        JewelleryLoan = alertnesNetWorths.JewelleryLoan ?? 0,
                        BusinessLoan = alertnesNetWorths.BusinessLoan ?? 0,
                        OtherLoan = alertnesNetWorths.OtherLoan ?? 0,
                        CarLoan = alertnesNetWorths.CarLoan ?? 0,
                        

                        CreatedAt = alertnesNetWorths.CreatedAt ?? DateTime.Now
                    };
                }

                //Step-7
                if (loanDetailData != null)
                {
                    viewModel.loanDebtDetailViewModel = new LoanDebtDetailViewModel
                    {
                        Id = (int)loanDetailData.Id,
                        ProfileId = loanDetailData?.ProfileId ?? 0,
                        GoldLoan = loanDetailData?.GoldLoan,
                        CreditCard = loanDetailData?.CreditCard,
                        PersonalLoan = loanDetailData?.PersonalLoan,
                        BadLoanOthers = loanDetailData?.BadLoanOthers,
                        HomeLoan = loanDetailData?.HomeLoan,
                        EducationLoan = loanDetailData?.EducationLoan,
                        BusinessLoan = loanDetailData?.BusinessLoan,
                        GoodLoanOthers = loanDetailData?.GoodLoanOthers,
                    };
                }





            }
            else
            {

                //Income
                viewModel.ProfileId = profileId.Value;

                //defalut 5 entry for emi
                if (emiDetail == null || emiDetail.Count == 0)
                {

                    //Add blank Entry one time 
                    await _alertnessMappingService.AddEmiDetailsAsync((long)profileId);//emi
                    emiDetail = await _context.TblffAlertnesEmiDetails
                                            .Where(x => x.ProfileId == profileId)
                                    .ToListAsync();

                }




                viewModel.SpouseExtras = new List<TblffAlertnesSpouseIncomeExtra>();
                viewModel.IncomeExtras = new List<TblffAlertnesIncomeExtra>();
                //expense
                viewModel.ExpenseDetail = new ExpenseViewModel();
                viewModel.ExpenseNew = new List<TblffAlertnesExpenseNew>();
                //Saving
                viewModel.Savings = savings;
                viewModel.CommittedSaving = new CommittedSavingViewModel();
                viewModel.NewSavings = new List<TblffAlertnesNewSaving>();

                //EMI
                viewModel.EmiOneTimeLoanRepayment = new EmiOneTimeLoanRepayment();
                viewModel.EmiDetail = emiDetail;
                //viewModel.EmiDetail = new List<TblffAlertnesEmiDetail>();

                //Step-5
                viewModel.postIncomeDetailsViewModel = new PostIncomeDetailsViewModel();

                //Step-6
                viewModel.netWorthViewModel = new NetWorthViewModel();
                viewModel.NewInvestments = new List<TblFfAlertnesNewInvestment>();
                viewModel.NewOtherAssets = new List<TblFfAlertnesNewOtherAsset>();
                viewModel.NewLiabilities = new List<TblFfAlertnesNewLiability>();


                //Step-7 PostIncomeDetailsViewModel
                viewModel.loanDebtDetailViewModel = new LoanDebtDetailViewModel();

                //Step-8 Insurance
                viewModel.GeneralInsuranceViewModel = new List<TblFfAlertnesGeneralInsurance>();
                viewModel.LifeInsuranceViewModel = new List<TblFfAlertnesLifeInsurance>();




            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alertness(AlertnessViewModel model)
        {
       
            if (_planType?.ToLower() == "zero2one" || _planType?.ToLower() == "basic")
            {
                ModelState.Remove("Basic");
            }
            //model.ProfileId = _profileId;

            if (!ModelState.IsValid)
                {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        field = x.Key,
                        errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    });
                string ds1 = string.Join(" | ",
    ModelState.Values
              .SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage));

                string ds = "";
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        ds += error.ErrorMessage + " ";  // or add a comma/pipe separator
                    }
                }
                return Json(new { success = false, message = "Validation failed.", validationErrors = errors });
            }

            DateTime tillWhen;
            if (model.CommittedSaving.TillWhenDateParsed.HasValue)
            {
                tillWhen = model.CommittedSaving.TillWhenDateParsed.Value;
            }
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblffAlertnesIncomeDetail incomeDetail;
                TblffAlertnesExpense newExpense = null;
                TblffAlertnesSaving newSaving = null;
                TblffAlertnesCommittedSaving newCommittedSaving = null;
                TblffAlertnesEmiDetail newEmiDetail = null;
                TblAffAlertnesPostIncomeDetail alertnesPostIncomeDetail = null;
                TblffAlertnesDebt tblflertnesDebt = null;
                int savingId = 0;

                incomeDetail = _context.TblffAlertnesIncomeDetails.FirstOrDefault(x => x.ProfileId == model.ProfileId);
                if (incomeDetail !=null)
                {
                    // UPDATE
                    //incomeDetail = _context.TblffAlertnesIncomeDetails.FirstOrDefault(x => x.Id == model.Id);
                    if (incomeDetail == null)
                    {
                        return Json(new { success = false, message = "Record not found for update." });
                    }

                    // Update income details
                  
                    //incomeDetail.Id = model.Id;
                    incomeDetail.ProfileId = model.ProfileId;
                    incomeDetail.TotalExpense = model.TotalExpense;
                    incomeDetail.Basic = model.Basic;
                    incomeDetail.Hra = model.Hra;
                    incomeDetail.EducationAllowance = model.EducationAllowance;
                    incomeDetail.MedicalAllowance = model.MedicalAllowance;
                    incomeDetail.Lta = model.LTA;
                    incomeDetail.Conveyance = model.Conveyance;
                    incomeDetail.OtherAllowance = model.OtherAllowance;
                    incomeDetail.Pf = model.PF;
                    incomeDetail.Gratuity = model.Gratuity;
                    incomeDetail.Reimbursement = model.Reimbursement;
                    incomeDetail.BusinessIncome = model.BusinessIncome;
                    incomeDetail.FoodCoupon = model.FoodCoupon;
                    incomeDetail.MonthlyPension = model.MonthlyPension;
                    incomeDetail.InterestIncome = model.InterestIncome;
                    incomeDetail.AnnualBonus = model.AnnualBonus;
                    incomeDetail.PerformanceLinked = model.PerformanceLinked;

                    incomeDetail.AnnualTotalIncome = model.AnnualTotalIncome;
                    incomeDetail.MonthlyIncome = model.OverallMonthlyIncome;
                    incomeDetail.PostItincomeOld = model.PostITIncomeOld;
                    incomeDetail.PostItincomeNew = model.PostITIncomeNew;



                    incomeDetail.SpouseBasic = model.SpouseBasic;
                    incomeDetail.SpouseHra = model.SpouseHRA;
                    incomeDetail.SpouseEducationAllowance = model.SpouseEducationAllowance;
                    incomeDetail.SpouseMedicalAllowance = model.SpouseMedicalAllowance;
                    incomeDetail.SpouseLta = model.SpouseLTA;
                    incomeDetail.SpouseConveyance = model.SpouseConveyance;
                    incomeDetail.SpouseOtherAllowance = model.SpouseOtherAllowance;
                    incomeDetail.SpousePf = model.SpousePF;
                    incomeDetail.SpouseGratuity = model.SpouseGratuity;
                    incomeDetail.SpouseReimbursement = model.SpouseReimbursement;
                    incomeDetail.SpouseBusinessIncome = model.SpouseBusinessIncome;
                    incomeDetail.SpouseFoodCoupon = model.SpouseFoodCoupon;
                    incomeDetail.MonthlyPension = model.MonthlyPension;
                    incomeDetail.SpouseInterestIncome = model.SpouseInterestIncome;
                    incomeDetail.SpouseMonthlyTotalIncome = model.SpouseMonthlyTotalIncome;
                    incomeDetail.SpouseConsolidatedIncome = model.SpouseConsolidatedIncome;
                    incomeDetail.SpouseAnnualBonus = model.SpouseAnnualBonus;
                    incomeDetail.SpousePerformanceLinked = model.SpousePerformanceLinked;
                    incomeDetail.SpouseAnnualTotalIncome = model.SpouseAnnualTotalIncome;
                    incomeDetail.SpouseOverallMonthlyIncome = model.SpouseOverallMonthlyIncome;
                    incomeDetail.SpousePostItincomeOld = model.SpousePostITIncomeOld;
                    incomeDetail.SpousePostItincomeNew = model.SpousePostITIncomeNew;

                    incomeDetail.IsActive = model.IsActive ?? true;
                    incomeDetail.ModifiedDate = DateTime.UtcNow;



                    // Remove old extras
                    _context.TblffAlertnesIncomeExtras.RemoveRange(
                                                       _context.TblffAlertnesIncomeExtras.Where(x => x.IncomeDetailId == incomeDetail.Id));

                    _context.TblffAlertnesSpouseIncomeExtras.RemoveRange(
                                                             _context.TblffAlertnesSpouseIncomeExtras.Where(x => x.IncomeDetailId == incomeDetail.Id));

                    // Update expense
                    if (model.ExpenseDetail != null)
                    {
                        var existingExpense = _context.TblffAlertnesExpenses.OrderByDescending(x => x.CreatedDate)
                                                       .FirstOrDefault(x => x.ProfileId == incomeDetail.ProfileId);

                        if (existingExpense != null)
                        {

                            existingExpense.ProfileId = incomeDetail.ProfileId;
                            existingExpense.GroceryProvisionMilk = Convert.ToString(model.ExpenseDetail.GroceryProvisionMilk);
                            existingExpense.DomesticHelp = Convert.ToString(model.ExpenseDetail.DomesticHelp);
                            existingExpense.IronLaundry = Convert.ToString(model.ExpenseDetail.IronLaundry);
                            existingExpense.Driver = Convert.ToString(model.ExpenseDetail.Driver);
                            existingExpense.Fuel = Convert.ToString(model.ExpenseDetail.Fuel);
                            existingExpense.CarCleaning = Convert.ToString(model.ExpenseDetail.CarCleaning);
                            existingExpense.Maintenance = Convert.ToString(model.ExpenseDetail.Maintenance);
                            existingExpense.TaxiPublicTransport = Convert.ToString(model.ExpenseDetail.TaxiPublicTransport);
                            existingExpense.AirTrainTravel = Convert.ToString(model.ExpenseDetail.AirTrainTravel);
                            existingExpense.Mobile = Convert.ToString(model.ExpenseDetail.Mobile);
                            existingExpense.LandlineBroadband = Convert.ToString(model.ExpenseDetail.LandlineBroadband);
                            existingExpense.DataCard = Convert.ToString(model.ExpenseDetail.DataCard);
                            existingExpense.Electricity = Convert.ToString(model.ExpenseDetail.Electricity);
                            existingExpense.HouseTax = Convert.ToString(model.ExpenseDetail.HouseTax);
                            existingExpense.SocietyCharge = Convert.ToString(model.ExpenseDetail.SocietyCharge);
                            existingExpense.Rents = Convert.ToString(model.ExpenseDetail.Rents);
                            existingExpense.Cable = Convert.ToString(model.ExpenseDetail.Cable);
                            existingExpense.Lpg = Convert.ToString(model.ExpenseDetail.Lpg);
                            existingExpense.WaterBill = Convert.ToString(model.ExpenseDetail.WaterBill);
                            existingExpense.NewsPaper = Convert.ToString(model.ExpenseDetail.NewsPaper);
                            existingExpense.SchoolFees = Convert.ToString(model.ExpenseDetail.SchoolFees);
                            existingExpense.Tuitions = Convert.ToString(model.ExpenseDetail.Tuitions);
                            existingExpense.UniformsAccessories = Convert.ToString(model.ExpenseDetail.UniformsAccessories);
                            existingExpense.BooksStationery = Convert.ToString(model.ExpenseDetail.BooksStationery);
                            existingExpense.PicnicsActivities = Convert.ToString(model.ExpenseDetail.PicnicsActivities);
                            existingExpense.MoviesTheatre = Convert.ToString(model.ExpenseDetail.MoviesTheatre);
                            existingExpense.DiningOut = Convert.ToString(model.ExpenseDetail.DiningOut);
                            existingExpense.ClubhouseExpenses = Convert.ToString(model.ExpenseDetail.ClubhouseExpenses);
                            existingExpense.PartiesAtHome = Convert.ToString(model.ExpenseDetail.PartiesAtHome);
                            existingExpense.ClothingGrooming = Convert.ToString(model.ExpenseDetail.ClothingGrooming);
                            existingExpense.VacationTravel = Convert.ToString(model.ExpenseDetail.VacationTravel);
                            existingExpense.Festivals = Convert.ToString(model.ExpenseDetail.Festivals);
                            existingExpense.KidsBirthdays = Convert.ToString(model.ExpenseDetail.KidsBirthdays);
                            existingExpense.FamilyFunctions = Convert.ToString(model.ExpenseDetail.FamilyFunctions);
                            existingExpense.Medical = Convert.ToString(model.ExpenseDetail.Medical);
                            existingExpense.VehicleServicing = Convert.ToString(model.ExpenseDetail.VehicleServicing);
                            existingExpense.HomeRepair = Convert.ToString(model.ExpenseDetail.HomeRepair);
                            existingExpense.NewHomeAppliances = Convert.ToString(model.ExpenseDetail.NewHomeAppliances);
                            existingExpense.LifeInsurance = Convert.ToString(model.ExpenseDetail.LifeInsurance);
                            existingExpense.HomeInsurance = Convert.ToString(model.ExpenseDetail.HomeInsurance);
                            existingExpense.MedicalInsurance = Convert.ToString(model.ExpenseDetail.MedicalInsurance);
                            existingExpense.CarInsurance = Convert.ToString(model.ExpenseDetail.CarInsurance);
                            existingExpense.FeeToCa = Convert.ToString(model.ExpenseDetail.FeeToCa);
                            existingExpense.OtherConsultant = Convert.ToString(model.ExpenseDetail.OtherConsultant);
                            existingExpense.Donations = Convert.ToString(model.ExpenseDetail.Donations);
                            existingExpense.ModifiedDate = DateTime.UtcNow;
                        }
                        _context.TblffAlertnesExpenses.Update(existingExpense);
                        await _context.SaveChangesAsync();
                        // Remove old other expenses
                        _context.TblffAlertnesExpenseNews.RemoveRange(
                                                         _context.TblffAlertnesExpenseNews.Where(x => x.ExpenseId == existingExpense.Id));

                    }
                    //Use Servises on Below  From Step-3
                    //update Saving and TblffAlertnesCommittedSavings and remove TblffAlertnesNewSavings
                    savingId = await _alertnessMappingService.UpdateSavingsAndCommittedAsync(model, incomeDetail);


                    //Update Step-4 (tblff_Alertnes_OneTimeLoanRepayment--single records, tblff_Alertnes_EMI_Details----multiple records)
                    if (model.EmiOneTimeLoanRepayment != null)
                    {
                        int emiId = await _alertnessMappingService.UpdateEMIDetilsAsync(model, incomeDetail);
                    }

                    //Step-5 Add new Investment or update
                    if (model.postIncomeDetailsViewModel != null)
                    {
                        if (incomeDetail.ProfileId > 0)
                        {
                            await _alertnessMappingService.SavePostIncomeDetailAsync(model.postIncomeDetailsViewModel, (long)incomeDetail.ProfileId);
                        }

                    }

                    if (incomeDetail.ProfileId > 0)
                    {
                        //Step-6 
                        _context.TblFfAlertnesNewInvestments.RemoveRange(
                                                         _context.TblFfAlertnesNewInvestments.Where(x => x.ProfileId == incomeDetail.ProfileId));
                        _context.TblFfAlertnesNewOtherAssets.RemoveRange(
                                                   _context.TblFfAlertnesNewOtherAssets.Where(x => x.ProfileId == incomeDetail.ProfileId));
                        _context.TblFfAlertnesNewLiabilities.RemoveRange(
                                                   _context.TblFfAlertnesNewLiabilities.Where(x => x.ProfileId == incomeDetail.ProfileId));
                        await _alertnessMappingService.SavePostTblFfAlertnesNetWorthsAsync(model.netWorthViewModel, (long)incomeDetail.ProfileId);


                        //Step-7 Add new TblffAlertnesDebt or update
                        await _alertnessMappingService.SavePostTblffAlertnesDebtAsync(model.loanDebtDetailViewModel, (long)incomeDetail.ProfileId);


                        //Step-8 Add or Update Insurance
                        // Remove old Insurance
                        _context.TblFfAlertnesLifeInsurances.RemoveRange(
                                                         _context.TblFfAlertnesLifeInsurances.Where(x => x.ProfileId == incomeDetail.ProfileId));
                        // Remove old Insurance
                        _context.TblFfAlertnesGeneralInsurances.RemoveRange(
                                                         _context.TblFfAlertnesGeneralInsurances.Where(x => x.ProfileId == incomeDetail.ProfileId));
                    }



                }
                else
                {
                    // INSERT
                    incomeDetail = new TblffAlertnesIncomeDetail
                    {
                        ProfileId = model.ProfileId,

                        // Main Income
                        Basic = model.Basic,
                        Hra = model.Hra,
                        TotalExpense = model.TotalExpense,
                        EducationAllowance = model.EducationAllowance,
                        MedicalAllowance = model.MedicalAllowance,
                        Lta = model.LTA,
                        Conveyance = model.Conveyance,
                        OtherAllowance = model.OtherAllowance,
                        Pf = model.PF,
                        Gratuity = model.Gratuity,
                        Reimbursement = model.Reimbursement,
                        BusinessIncome = model.BusinessIncome,
                        FoodCoupon = model.FoodCoupon,
                        MonthlyPension = model.MonthlyPension,
                        InterestIncome = model.InterestIncome,
                        AnnualBonus = model.AnnualBonus,
                        PerformanceLinked = model.PerformanceLinked,

                        AnnualTotalIncome = model.AnnualTotalIncome,
                        MonthlyIncome = model.OverallMonthlyIncome,
                        PostItincomeOld = model.PostITIncomeOld,
                        PostItincomeNew = model.PostITIncomeNew,

                        // Spouse Income
                        SpouseBasic = model.SpouseBasic,
                        SpouseHra = model.SpouseHRA,
                        SpouseEducationAllowance = model.SpouseEducationAllowance,
                        SpouseMedicalAllowance = model.SpouseMedicalAllowance,
                        SpouseLta = model.SpouseLTA,
                        SpouseConveyance = model.SpouseConveyance,
                        SpouseOtherAllowance = model.SpouseOtherAllowance,
                        SpousePf = model.SpousePF,
                        SpouseGratuity = model.SpouseGratuity,
                        SpouseReimbursement = model.SpouseReimbursement,
                        SpouseBusinessIncome = model.SpouseBusinessIncome,
                        SpouseFoodCoupon = model.SpouseFoodCoupon,
                        SpouseMonthlyPension = model.SpouseMonthlyPension,
                        SpouseInterestIncome = model.SpouseInterestIncome,
                        SpouseMonthlyTotalIncome = model.SpouseMonthlyTotalIncome,
                        SpouseAnnualBonus = model.SpouseAnnualBonus,
                        SpousePerformanceLinked = model.SpousePerformanceLinked,
                        SpouseConsolidatedIncome = model.SpouseConsolidatedIncome,
                        SpouseAnnualTotalIncome = model.SpouseAnnualTotalIncome,
                        SpouseOverallMonthlyIncome = model.SpouseOverallMonthlyIncome,
                        SpousePostItincomeOld = model.SpousePostITIncomeOld,
                        SpousePostItincomeNew = model.SpousePostITIncomeNew,

                        // Meta
                        IsActive = model.IsActive ?? true,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    _context.TblffAlertnesIncomeDetails.Add(incomeDetail);
                    _context.SaveChanges(); // Get Id for IncomeExtras

                    // Insert expense if provided
                    if (model.ExpenseDetail != null)
                    {
                        newExpense = new TblffAlertnesExpense
                        {
                            ProfileId = model.ProfileId,
                            GroceryProvisionMilk = Convert.ToString(model.ExpenseDetail.GroceryProvisionMilk),
                            DomesticHelp = Convert.ToString(model.ExpenseDetail.DomesticHelp),
                            IronLaundry = Convert.ToString(model.ExpenseDetail.IronLaundry),
                            Driver = Convert.ToString(model.ExpenseDetail.Driver),
                            Fuel = Convert.ToString(model.ExpenseDetail.Fuel),
                            CarCleaning = Convert.ToString(model.ExpenseDetail.CarCleaning),
                            Maintenance = Convert.ToString(model.ExpenseDetail.Maintenance),
                            TaxiPublicTransport = Convert.ToString(model.ExpenseDetail.TaxiPublicTransport),
                            AirTrainTravel = Convert.ToString(model.ExpenseDetail.AirTrainTravel),
                            Mobile = Convert.ToString(model.ExpenseDetail.Mobile),
                            LandlineBroadband = Convert.ToString(model.ExpenseDetail.LandlineBroadband),
                            DataCard = Convert.ToString(model.ExpenseDetail.DataCard),
                            Electricity = Convert.ToString(model.ExpenseDetail.Electricity),
                            HouseTax = Convert.ToString(model.ExpenseDetail.HouseTax),
                            SocietyCharge = Convert.ToString(model.ExpenseDetail.SocietyCharge),
                            Rents = Convert.ToString(model.ExpenseDetail.Rents),
                            Cable = Convert.ToString(model.ExpenseDetail.Cable),
                            Lpg = Convert.ToString(model.ExpenseDetail.Lpg),
                            WaterBill = Convert.ToString(model.ExpenseDetail.WaterBill),
                            NewsPaper = Convert.ToString(model.ExpenseDetail.NewsPaper),
                            SchoolFees = Convert.ToString(model.ExpenseDetail.SchoolFees),
                            Tuitions = Convert.ToString(model.ExpenseDetail.Tuitions),
                            UniformsAccessories = Convert.ToString(model.ExpenseDetail.UniformsAccessories),
                            BooksStationery = Convert.ToString(model.ExpenseDetail.BooksStationery),
                            PicnicsActivities = Convert.ToString(model.ExpenseDetail.PicnicsActivities),
                            MoviesTheatre = Convert.ToString(model.ExpenseDetail.MoviesTheatre),
                            DiningOut = Convert.ToString(model.ExpenseDetail.DiningOut),
                            ClubhouseExpenses = Convert.ToString(model.ExpenseDetail.ClubhouseExpenses),
                            PartiesAtHome = Convert.ToString(model.ExpenseDetail.PartiesAtHome),
                            ClothingGrooming = Convert.ToString(model.ExpenseDetail.ClothingGrooming),
                            VacationTravel = Convert.ToString(model.ExpenseDetail.VacationTravel),
                            Festivals = Convert.ToString(model.ExpenseDetail.Festivals),
                            KidsBirthdays = Convert.ToString(model.ExpenseDetail.KidsBirthdays),
                            FamilyFunctions = Convert.ToString(model.ExpenseDetail.FamilyFunctions),
                            Medical = Convert.ToString(model.ExpenseDetail.Medical),
                            VehicleServicing = Convert.ToString(model.ExpenseDetail.VehicleServicing),
                            HomeRepair = Convert.ToString(model.ExpenseDetail.HomeRepair),
                            NewHomeAppliances = Convert.ToString(model.ExpenseDetail.NewHomeAppliances),
                            LifeInsurance = Convert.ToString(model.ExpenseDetail.LifeInsurance),
                            HomeInsurance = Convert.ToString(model.ExpenseDetail.HomeInsurance),
                            MedicalInsurance = Convert.ToString(model.ExpenseDetail.MedicalInsurance),
                            CarInsurance = Convert.ToString(model.ExpenseDetail.CarInsurance),
                            FeeToCa = Convert.ToString(model.ExpenseDetail.FeeToCa),
                            OtherConsultant = Convert.ToString(model.ExpenseDetail.OtherConsultant),
                            Donations = Convert.ToString(model.ExpenseDetail.Donations),

                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        };

                        _context.TblffAlertnesExpenses.Add(newExpense);
                        _context.SaveChanges(); // Get Id for ExpenseNew
                    }


                    if (model.Savings != null) //Add new Saving
                    {

                        savingId = await _alertnessMappingService.SaveSavingsAsync(model);
                        //Add new CommittedSaving
                        if (model.CommittedSaving != null)
                        {
                            await _alertnessMappingService.SaveCommittedSavingAsync(model.CommittedSaving, savingId);
                        }
                    }

                    //Step-5 Add new Investment or update
                    if (model.postIncomeDetailsViewModel != null)
                    {
                        if (incomeDetail.ProfileId > 0)
                        {
                            await _alertnessMappingService.SavePostIncomeDetailAsync(model.postIncomeDetailsViewModel, (long)incomeDetail.ProfileId);
                        }

                    }

                    //Step-6 

                    //Step-7 Add new TblffAlertnesDebt or update
                    if (model.loanDebtDetailViewModel != null)
                    {
                        if (incomeDetail.ProfileId > 0)
                        {
                            await _alertnessMappingService.SavePostTblffAlertnesDebtAsync(model.loanDebtDetailViewModel, (long)incomeDetail.ProfileId);
                        }
                    }

                }


                //Range Data
                // Add IncomeExtras
                if (model.IncomeExtras != null)
                {
                    _context.TblffAlertnesIncomeExtras.AddRange(
                        model.IncomeExtras.Select(extra => new TblffAlertnesIncomeExtra
                        {
                            IncomeDetailId = incomeDetail.Id,
                            FieldName = extra.FieldName,
                            FieldValue = extra.FieldValue,
                            Type = extra.Type,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        }));
                }

                // Add SpouseExtras
                if (model.SpouseExtras != null)
                {
                    _context.TblffAlertnesSpouseIncomeExtras.AddRange(
                        model.SpouseExtras.Select(extra => new TblffAlertnesSpouseIncomeExtra
                        {
                            IncomeDetailId = incomeDetail.Id,
                            FieldName = extra.FieldName,
                            FieldValue = extra.FieldValue,
                            Type = extra.Type,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        }));
                }

                // Add ExpenseNew
                if (model.ExpenseNew != null)
                {
                    //long expenseId = model.ExpenseDetail?.Id ?? newExpense?.Id ?? 0;
                    long expenseId = (model.ExpenseDetail?.Id > 0 ? model.ExpenseDetail.Id : newExpense?.Id) ?? 0;
                    if (expenseId > 0)
                    {
                        _context.TblffAlertnesExpenseNews.AddRange(
                            model.ExpenseNew.Select(expense => new TblffAlertnesExpenseNew
                            {
                                ExpenseId = expenseId,
                                FieldName = expense.FieldName,
                                FieldValue = expense.FieldValue,
                                Section = expense.Section,
                                Type = expense.Type,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow,
                                ModifiedDate = DateTime.UtcNow
                            }));
                    }
                }


                //Add new Saving
                if (model.NewSavings != null)
                {
                    if (savingId > 0)
                    {
                        await _alertnessMappingService.SaveNewSavingsAsync(model.NewSavings, savingId);
                    }
                }

                //step-6 Add New Investment
                if (model.netWorthViewModel != null)
                {
                    if (incomeDetail.ProfileId > 0)
                    {
                        await _alertnessMappingService.SavePostTblFfAlertnesNetWorthsAsync(model.netWorthViewModel, (long)incomeDetail.ProfileId);
                    }
                }

                if (model.NewInvestments != null)
                {
                    if (incomeDetail.ProfileId > 0)
                    {
                        await _alertnessMappingService.SaveNewInvestmentsAsync(model.NewInvestments, (int)incomeDetail.ProfileId);


                    }
                }
                if (model.NewOtherAssets != null)
                {
                    if (incomeDetail.ProfileId > 0)
                    {
                        await _alertnessMappingService.SaveNewOtherAssetsAsync(model.NewOtherAssets, (int)incomeDetail.ProfileId);


                    }
                }
                if (model.NewLiabilities != null)
                {
                    if (incomeDetail.ProfileId > 0)
                    {
                        await _alertnessMappingService.SaveNewLiabilitiesAsync(model.NewLiabilities, (int)incomeDetail.ProfileId);


                    }
                }




                //Add EMI
                if (model.EmiDetail != null)
                {
                    // Remove existing EMI details
                    var oldEmiDetails = await _context.TblffAlertnesEmiDetails
                                                      .Where(x => x.ProfileId == incomeDetail.ProfileId)
                                                      .ToListAsync();

                    if (oldEmiDetails.Any())
                    {
                        _context.TblffAlertnesEmiDetails.RemoveRange(oldEmiDetails);
                    }
                    _context.SaveChanges();
                    if (incomeDetail.ProfileId > 0)
                    {
                        await _alertnessMappingService.AddEmiDetailsAsync(model.EmiDetail, (int)incomeDetail.ProfileId);
                    }

                }






                //Step-8 
                await _alertnessMappingService.AddLifeInsuranceViewModelAsync(model.LifeInsuranceViewModel, (int)incomeDetail.ProfileId);
                await _alertnessMappingService.AddGenLifeInsuranceViewModelAsync(model.GeneralInsuranceViewModel, (int)incomeDetail.ProfileId);


                _context.SaveChanges();
                transaction.Commit();

                return Json(new
                {
                    success = true,
                    message = model.Id > 0 ? "Updated successfully!" : "Saved successfully!"
                });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        private decimal? ParseNullableDecimal(string? value)
        {
            return decimal.TryParse(value, out var result) ? result : null;
        }

        public DateTime? ParseDateDdMmYyyy(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime result))
            {
                return result;
            }

            return null;
        }


        #endregion ***********************************End Alertness ********************************************************





        [HttpGet]
        public async Task<IActionResult> KnowledgeThatMatters()
        {

            HttpContext.Session.SetString("PageIndex", "4");
            var viewknowledge = new MVKnowledgeRisk();

            var knowledgeData = await _KnowledgeThatMattersServices.KnowledgeThatMattersView();

            if (knowledgeData != null)
            {
                return View(knowledgeData);
            }
            else
            {
                return View(viewknowledge);
            }
        }

        [HttpPost]
        public async Task<IActionResult> KnowledgeThatMatters(MVKnowledgeRisk mvKnowledgeRisk, string btnSubmit)
        {


            ModelState.Remove("btnSubmit");
            //if (_planType?.ToLower() == "zero2one")
            //{
            //    if(btnSubmit == "Save")
            //    {
            //        return View(mvKnowledgeRisk);
            //    }
            //    return RedirectToRoute(new
            //    {
            //        planType = _planType,   // taken from session
            //        controller = "Comprehensive",
            //        action = "ExecutionWithPrecision",
            //        pid = CryptoHelper.Encrypt(_profileId)
            //    });
            //}
            if (!ModelState.IsValid)
            {
                // Collect all error messages
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();

                foreach (var error in errors)
                {
                    Console.WriteLine($"Field: {error.Field}");
                    foreach (var msg in error.Errors)
                    {
                        Console.WriteLine($"  Error: {msg}");
                        }
                }


                return View(mvKnowledgeRisk);
            }

            var knowledgedata = await _KnowledgeThatMattersServices.KnowledgeThatMattersAddUpdate(mvKnowledgeRisk);
            
            if (btnSubmit == "Save")
            {
                ViewData["msg"] = "Saved successfully.";
                return View(mvKnowledgeRisk);
            }
            else {
               
                //return RedirectToAction("ExecutionWithPrecision", _planType, new { id = CryptoHelper.Encrypt(_profileId.ToString()) });
                return RedirectToRoute(new
                {
                    planType = _planType,   // taken from session
                    controller = "Comprehensive",
                    action = "ExecutionWithPrecision",
                    pid = CryptoHelper.Encrypt(_profileId)
                });
            }

        }

		[HttpGet]
        public async Task<IActionResult> ExecutionWithPrecision()
        {

            HttpContext.Session.SetString("PageIndex", "5");
            ///select child details, Any other Investments/Assets meant for this goal --for ddl,TblFF_Alertnes_NewOtherAssets
            var executionViewModel = new ExecutionWithPrecisionModelView();
            int i = await _executionServices.InvestCheckDataThenUpdate(_profileId, _planType);
            executionViewModel = await _executionServices.GetExecutionData(_profileId);
            var dynamicAssets = await _context.TblFfAlertnesNewOtherAssets
                               .Where(x => x.ProfileId == _profileId).Select(x => x.AssetName).ToListAsync();

            var staticOptions = new List<string>  {
                                "Home1", "Home2", "Land", "Car", "Commercial Property", "Jewellery", "Value Of Business"
                            };

           var OtherAssetsallOptions = staticOptions.Concat(dynamicAssets).ToList();

            ViewBag.OtherAssetsOptions = OtherAssetsallOptions;

            return View(executionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ExecutionWithPrecision(ExecutionWithPrecisionModelView executionViewModel, string btnSubmit)
        {
            ModelState.Remove("btnSubmit");


            var dynamicAssets = await _context.TblFfAlertnesNewOtherAssets
                               .Where(x => x.ProfileId == _profileId).Select(x => x.AssetName).ToListAsync();

            var staticOptions = new List<string>  {
                                "Home1", "Home2", "Land", "Car", "Commercial Property", "Jewellery", "Value Of Business"
                            };

            var OtherAssetsallOptions = staticOptions.Concat(dynamicAssets).ToList();

            ViewBag.OtherAssetsOptions = OtherAssetsallOptions;

            updateRows = await _executionServices.ExecutionUpdateToTableSubmit(executionViewModel);

            if(updateRows > 0)
            {
                if (btnSubmit == "Save")
                {
                    ViewData["msg"] = "Saved successfully.";
                    executionViewModel = await _executionServices.GetExecutionData(_profileId);

                    return View(executionViewModel);
                }

                TempData["SuccessMessage"] = "Execution data updated successfully.";
          
                //return RedirectToAction("Invest", _planType, new { id = CryptoHelper.Encrypt(_profileId.ToString()) });
                return RedirectToRoute(new
                {
                    planType = _planType,   // taken from session
                    controller = "Comprehensive",
                    action = "Invest",
                    pid = CryptoHelper.Encrypt(_profileId)
                });
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update execution data.";
                return View(executionViewModel);
            }
        }

		[HttpGet]
        public async Task<IActionResult> Invest()
        {
            HttpContext.Session.SetString("PageIndex", "6");
            var investViewModel = new InvestViewModel();
             await _investServices.InvestCheckDataThenUpdate(_profileId);
            //investViewModel.InvestMVList = _investServices.GetInvestData(_userID).Result.InvestMVList;
            investViewModel =await _investServices.GetInvestData(_profileId);
            return View(investViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Invest(InvestViewModel investViewModel, string btnSubmit)
        {
            ModelState.Remove("btnSubmit");
            ModelState.Remove("btnSubmitCompleted");
            TblFfAdminUser admin = new TblFfAdminUser();
            var userProfileData = await _context.TblffAwarenessProfileDetails.FirstOrDefaultAsync(u => u.ProfileId == _profileId);
            if (userProfileData == null)
            {
                userProfileData = new TblffAwarenessProfileDetail();
            }
            var assingUsers = await _context.TblFfRegisterUsers.FirstOrDefaultAsync(u => u.Id == userProfileData.UserId);
            if (assingUsers != null)
            {
                admin = await _context.TblFfAdminUsers.FirstOrDefaultAsync(u => u.Id == assingUsers.Advisorid);
            }
            if (admin == null)
            {
                admin = new TblFfAdminUser();
            }
            //var createduser = await _context.TblFfRegisterUsers.FirstOrDefaultAsync(u => u.Id == userProfileData.UserId);

            
            
            
            updateRows =  await _investServices.WingsUpdateInvestDataForWings(investViewModel);

            if (updateRows > 0)
            {
                if (btnSubmit == "Completed" || btnSubmit ==null)
                {
                    string? userEmail = HttpContext.Session.GetString("Useremail") ?? "agile1021@gmail.com";
                    string? UserName = HttpContext.Session.GetString("UserFullName") ?? "";

                    await _utilService.SendEmailAsync(
                        toEmail: userEmail,
                        subject: "Form Submit Successfully - FactFinder",
                        templatePath: Path.Combine(_env.WebRootPath, "emailtemplates", "FormSubmitSuccessTemplate.html"),
                        placeholders: new Dictionary<string, string>
                        {
                            { "UserName", UserName},
                            { "FormTitle", "Form Submit Successfully - FactFinder" }
                        });
                }

                if (btnSubmit == "Save")
                {
                    ViewData["msg"] = "Saved successfully.";
                    return View(investViewModel);
                }

                ViewBag.ShowThankYou = true;
                TempData["SuccessMessage"] = "Invest data updated successfully.";



                if (btnSubmit == "Completed")
                {

                    if (userProfileData == null)
                    {
                        return NotFound();
                    }
                    userProfileData.Advisorid = Convert.ToInt32(admin.Id);
                    userProfileData.AdvisorName = admin?.Name;
                    userProfileData.ProfileStatus = "Completed";
                    userProfileData.Awakenstatus = "Awaken Export Ready";
                    _context.TblffAwarenessProfileDetails.Update(userProfileData);
                    await _context.SaveChangesAsync();
                }
                else if (btnSubmit == null) //submit
                {

                    if (updateRows > 0)
                    {
                        if (userProfileData != null)
                        {
                            if (admin != null)
                            {
                                userProfileData.ProfileStatus = "Assign"; // Data pending for approval once user saved 6 forms
                                userProfileData.Awakenstatus = "";
                                userProfileData.Advisorid = Convert.ToInt32(admin.Id);
                                userProfileData.AdvisorName = admin?.Name;
                            }
                            else
                            {
                                userProfileData.Awakenstatus = "";
                                userProfileData.ProfileStatus = "Pending"; // Data pending for approval once user saved 6 forms
                                userProfileData.Awakenstatus = "";

                            }

                            _context.TblffAwarenessProfileDetails.Update(userProfileData);
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                return View("Invest", investViewModel);
                //return RedirectToAction("dashboard", "user");




            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Invest data.";
                return View(investViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AlertnessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = "Validation failed.",
                    validationErrors = errors
                });
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblffAlertnesIncomeDetail incomeDetail;
                TblffAlertnesExpense newExpense = null;

                if (model.Id > 0)
                {
                    // UPDATE
                    incomeDetail = _context.TblffAlertnesIncomeDetails
                        .FirstOrDefault(x => x.Id == model.Id);

                    if (incomeDetail == null)
                    {
                        return Json(new { success = false, message = "Record not found for update." });
                    }

                    // Update income details
                    incomeDetail.Basic = model.Basic;
                    incomeDetail.Hra = model.Hra;
                    incomeDetail.EducationAllowance = model.EducationAllowance;
                    incomeDetail.MedicalAllowance = model.MedicalAllowance;
                    incomeDetail.Lta = model.LTA;
                    incomeDetail.SpouseBasic = model.SpouseBasic;
                    incomeDetail.SpouseHra = model.SpouseHRA;
                    incomeDetail.SpouseEducationAllowance = model.SpouseEducationAllowance;
                    incomeDetail.ModifiedDate = DateTime.UtcNow;

                    // Remove old extras
                    _context.TblffAlertnesIncomeExtras.RemoveRange(
                        _context.TblffAlertnesIncomeExtras.Where(x => x.IncomeDetailId == incomeDetail.Id));

                    _context.TblffAlertnesSpouseIncomeExtras.RemoveRange(
                        _context.TblffAlertnesSpouseIncomeExtras.Where(x => x.IncomeDetailId == incomeDetail.Id));

                    // Update expense
                    if (model.ExpenseDetail != null)
                    {
                        var existingExpense = _context.TblffAlertnesExpenses
                            .FirstOrDefault(x => x.ProfileId == incomeDetail.ProfileId);

                        if (existingExpense != null)
                        {
                            existingExpense.GroceryProvisionMilk = Convert.ToString(model.ExpenseDetail.GroceryProvisionMilk);
                            existingExpense.DomesticHelp = Convert.ToString(model.ExpenseDetail.DomesticHelp);
                            existingExpense.IronLaundry = Convert.ToString(model.ExpenseDetail.IronLaundry);
                            // Add other fields as needed...
                            existingExpense.ModifiedDate = DateTime.UtcNow;

                            // Remove old other expenses
                            _context.TblffAlertnesExpenseNews.RemoveRange(
                                _context.TblffAlertnesExpenseNews.Where(x => x.ExpenseId == existingExpense.Id));
                        }
                    }
                }
                else
                {
                    // INSERT
                    incomeDetail = new TblffAlertnesIncomeDetail
                    {
                        ProfileId = model.ProfileId,
                        Basic = model.Basic,
                        Hra = model.Hra,
                        EducationAllowance = model.EducationAllowance,
                        MedicalAllowance = model.MedicalAllowance,
                        Lta = model.LTA,
                        SpouseBasic = model.SpouseBasic,
                        SpouseHra = model.SpouseHRA,
                        SpouseEducationAllowance = model.SpouseEducationAllowance,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    _context.TblffAlertnesIncomeDetails.Add(incomeDetail);
                    _context.SaveChanges(); // Get Id for IncomeExtras

                    // Insert expense if provided
                    if (model.ExpenseDetail != null)
                    {
                        newExpense = new TblffAlertnesExpense
                        {
                            ProfileId = model.ProfileId,
                            GroceryProvisionMilk = Convert.ToString(model.ExpenseDetail.GroceryProvisionMilk),
                            DomesticHelp = Convert.ToString(model.ExpenseDetail.DomesticHelp),
                            IronLaundry = Convert.ToString(model.ExpenseDetail.IronLaundry),
                            // Add other fields as needed...
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        };

                        _context.TblffAlertnesExpenses.Add(newExpense);
                        _context.SaveChanges(); // Get Id for ExpenseNew
                    }
                }

                // Add IncomeExtras
                if (model.IncomeExtras != null)
                {
                    _context.TblffAlertnesIncomeExtras.AddRange(
                        model.IncomeExtras.Select(extra => new TblffAlertnesIncomeExtra
                        {
                            IncomeDetailId = incomeDetail.Id,
                            FieldName = extra.FieldName,
                            FieldValue = extra.FieldValue,
                            Type = extra.Type,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        }));
                }

                // Add SpouseExtras
                if (model.SpouseExtras != null)
                {
                    _context.TblffAlertnesSpouseIncomeExtras.AddRange(
                        model.SpouseExtras.Select(extra => new TblffAlertnesSpouseIncomeExtra
                        {
                            IncomeDetailId = incomeDetail.Id,
                            FieldName = extra.FieldName,
                            FieldValue = extra.FieldValue,
                            Type = extra.Type,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedDate = DateTime.UtcNow
                        }));
                }

                // Add ExpenseNew
                if (model.ExpenseNew != null)
                {
                    long expenseId = model.ExpenseDetail?.Id ?? newExpense?.Id ?? 0;

                    if (expenseId > 0)
                    {
                        _context.TblffAlertnesExpenseNews.AddRange(
                            model.ExpenseNew.Select(expense => new TblffAlertnesExpenseNew
                            {
                                ExpenseId = expenseId,
                                FieldName = expense.FieldName,
                                FieldValue = expense.FieldValue,
                                Section = expense.Section,
                                Type = expense.Type,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow,
                                ModifiedDate = DateTime.UtcNow
                            }));
                    }
                }

                _context.SaveChanges();
                transaction.Commit();

                return Json(new
                {
                    success = true,
                    message = model.Id > 0 ? "Updated successfully!" : "Saved successfully!"
                });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }



        //expense A


        /*

https://ff.mainstream.co.in/Comprehensive/Awareness
https://ff.mainstream.co.in/Comprehensive/wings
https://ff.mainstream.co.in/Comprehensive/Alertness
https://ff.mainstream.co.in/Comprehensive/KnowledgeThatMatters
https://ff.mainstream.co.in/Comprehensive/ExecutionWithPrecision
https://ff.mainstream.co.in/Comprehensive/invest
        */
    }
}
/*  ////db code
 

    public DbSet<AlertnessJsonResult> AlertnessJsonResult { get; set; }
    public DbSet<ApplicantDataDto> ApplicantData { get; set; }

        modelBuilder.Entity<AlertnessJsonResult>().HasNoKey();
        modelBuilder.Entity<ApplicantDataDto>().HasNoKey();




*/

/*
 
DBCC CHECKIDENT ('YourTableName', NORESEED);
DBCC CHECKIDENT ('YourTableName', RESEED, 0);


error on registration due to UK pan number
SqlException: Violation of UNIQUE KEY constraint 'IX_tblff_awareness_profileDetails_PAN_UQ'. Cannot insert duplicate key in object 'dbo.tblff_awareness_profileDetails'. The duplicate key value is (<NULL>).


Awareness
	Applicant's Details
	Spouse Details
	Children Details
	Residential Address
	Assumtions



Wings
	Priority   - Select Your Priority
	Goal   - Select Your Goals
	Goal Start Year
	Goal End Year  


Alertness With Money & Time
	Applicant's Details (Monthly)
	Spouse Details (Monthly)
		Expenses (Monthly)
			Home	
			Conveyance
			Communication
			Utilities
			Educational Expenses
			Entertainment/Recreation
			Medical Expenses
			New/Replacement Items
			Insurance
			Other Expenses

			Committed Savings
			EMIs
			Projected Future Income

		Networth
			Investment
			Other Assets
		Liabilities
		Debt

		Applicant's Life Insurance
		Spouse's Life Insurance
		General Insurance






Knowledge That Matters
	Risk Tolerance 
	Risk Capacity 
	Risk Requirement


Execution With Precision



Invest In The Now
Total available Savings for Investments
Action plan for Financial Goals

*/