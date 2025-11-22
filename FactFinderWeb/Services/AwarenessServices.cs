using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Drawing.Drawing2D;


namespace FactFinderWeb.Services
{
    public class AwarenessServices : IAwareness
    {
        private ResellerBoyinawebFactFinderWebContext _context;
        //private readonly long _userID;
        private readonly long _profileId;
        private readonly HttpContext _httpContext;
        int updateRows = 0;
        int AddorUpdate = 0;

        public AwarenessServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("UserId");
            //_userID = Convert.ToInt64(userIdStr);
            _profileId = Convert.ToInt64(_httpContext.Session.GetString("profileId"));
        }
        public async Task<AwarenessViewModel> AwarenessProfileDetail(long pid)
        {
            var awarenessData = await _context.TblffAwarenessProfileDetails
                            .Where(p => p.ProfileId == pid)
                            .Select(p => new AwarenessViewModel
                            {
                                ProfileDetail = new ProfileDetail
                                {
                                    PlanType = p.PlanType,
                                    PlanYear = p.PlanYear.ToString(),
                                    PlanDuration = p.PlanDuration,
                                    Name = p.Name,
                                    Email = p.Email,
                                    Gender = p.Gender,
                                    Dob = p.Dob,
                                    Aadhaar = p.Aadhaar,
                                    PAN = p.Pan,
                                    Phone = p.Phone,
                                    AltPhone = p.Altphone,
                                    SecEmail = p.SecEmail,
                                    EduDetails = p.Education,
                                    Hobbies = p.Hobbies,
                                    Occupation = p.Occupation,
                                    HaveChildren = p.HaveChildren,
                                    MaritalStatus = p.MaritalStatus,
                                    CompanyAddress = p.CompanyAddress,
                                    CompanyCity = p.CompanyCity,
                                    CompanyCountry = p.CompanyCountry,
                                    CompanyName = p.CompanyName,
                                    CompanyPINcode = p.CompanyPincode,
                                    CompanyState = p.CompanyState,

                                    IsSameAddress = p.IsSameAddress??false,

                                    ResAddress = p.ResAddress,
                                    ResCity = p.ResCity,
                                    ResCountry = p.ResCountry,
                                    ResPincode = p.ResPincode,
                                    ResState = p.ResState,

                                    PermAddress = p.PermAddress,
                                    PermCity = p.PermCity,
                                    PermCountry = p.PermCountry,
                                    PermPincode = p.PermPincode,
                                    PermState = p.PermState,

                                    Stock = p.Stock,
                                    Income = p.Income,
                                    Payment = p.Payment,
                                    Holiday = p.Holiday,
                                    Shopping = p.Shopping
                                },

                                SpouseDetails = p.MaritalStatus.ToLower() != "married" ? new SpouseDetails() : _context.TblffAwarenessSpouses.Where(m => m.ProfileId == p.ProfileId)
                                    .Select(m => new SpouseDetails
                                    {
                                        SpouseName = m.SpouseName,
                                        SpouseGender = m.SpouseGender,
                                        SpouseDob = m.SpouseDob,
                                        SpousePhone = m.SpousePhone,
                                        SpouseAltPhone = m.SpouseAltPhone,
                                        SpouseAadhaar = m.SpouseAadhaar,
                                        SpouseEmail = m.SpouseEmail,
                                        SpouseSecEmail = m.SpouseSecEmail,
                                        SpousePan = m.SpousePan,
                                        SpouseOccupation = m.SpouseOccupation,
                                        SpouseCompanyAddress = m.SpouseCompanyAddress,
                                        SpouseCompanyCity = m.SpouseCompanyCity,
                                        SpouseCompanyCountry = m.SpouseCompanyCountry,
                                        SpouseCompanyName = m.SpouseCompanyName,
                                        SpouseCompanyPINcode = m.SpouseCompanyPincode,
                                        SpouseCompanyState = m.SpouseCompanyState
                                    }).FirstOrDefault(),

                                Assumptions = _context.TblffAwarenessAssumptions.Where(m => m.ProfileId == p.ProfileId)
                                    .Select(m => new Assumptions
                                    {
                                        Equity = m.Equity,
                                        Debt = m.Debt,
                                        Gold = m.Gold,
                                        RealEstateReturn = m.RealEstateReturn,
                                        LiquidFunds = m.LiquidFunds,
                                        InflationRates = m.InflationRates,
                                        EducationInflation = m.EducationInflation,
                                        ApplicantRetirement = m.ApplicantRetirement,
                                        SpouseRetirement = m.SpouseRetirement,
                                        ApplicantLifeExpectancy = m.ApplicantLifeExpectancy,
                                        SpouseLifeExpectancy = m.SpouseLifeExpectancy
                                    }).FirstOrDefault() ?? new Assumptions(),

                                ChildrenDetails = p.HaveChildren.ToLower() == "no" ? null : _context.TblffAwarenessChildren.Where(c => c.ProfileId == p.ProfileId)
                                    .Select(c => new ChildDetails
                                    {
                                        Id = c.Id,
                                        ChildName = c.ChildName,
                                        ChildGender = c.ChildGender,
                                        ChildDob = c.ChildDob,
                                        ChildPhone = c.ChildPhone,
                                        ChildAadhaar = c.ChildAadhaar,
                                        ChildEmail = c.ChildEmail,
                                        ChildPan = c.ChildPan
                                    }).ToList()
                            }).FirstOrDefaultAsync();
            var assumeData = _context.TblffAwarenessAssumptions.FirstOrDefault(m => m.ProfileId == pid);
            if (assumeData == null) {
                awarenessData.Assumptions.Equity = 12;
                awarenessData.Assumptions.Debt = 7;
                awarenessData.Assumptions.Gold = 8;
                awarenessData.Assumptions.RealEstateReturn = 10;
                awarenessData.Assumptions.LiquidFunds = 6;
                awarenessData.Assumptions.InflationRates = 7;
                awarenessData.Assumptions.EducationInflation = 8;
                awarenessData.Assumptions.ApplicantRetirement = null;
                awarenessData.Assumptions.ProfileId = _profileId;
            }

            return awarenessData;

        }

        public async Task<int> AwarenessAddProfileDatail(ProfileDetail awarenessProfileDetail, long profileID)
        {
            try
            {
                AddorUpdate = 0;
                TblffAwarenessProfileDetail awareness = await _context.TblffAwarenessProfileDetails.FirstOrDefaultAsync(x => x.ProfileId == profileID);
                if (awareness == null)
                {
                    AddorUpdate = 1; // Add new record
                    TblffAwarenessProfileDetail tblffAwarenessProfileDetail = new TblffAwarenessProfileDetail();
                    awareness = tblffAwarenessProfileDetail;
                    //awareness.UserId = _userid;
                }
                //awareness.id =  awarenessProfileDetail.asdf;
                
                awareness.PlanType = awarenessProfileDetail.PlanType;
                awareness.PlanYear = Convert.ToInt32(awarenessProfileDetail.PlanYear);
                awareness.PlanDuration = awarenessProfileDetail.PlanDuration;
                awareness.Name = awarenessProfileDetail.Name;
                awareness.Gender = awarenessProfileDetail.Gender;
                awareness.Dob = awarenessProfileDetail.Dob;
                awareness.Phone = awarenessProfileDetail.Phone;
                awareness.Altphone = awarenessProfileDetail.AltPhone;
                awareness.Aadhaar = awarenessProfileDetail.Aadhaar;
                awareness.Email = awarenessProfileDetail.Email;
                awareness.SecEmail = awarenessProfileDetail.SecEmail;
                awareness.Pan = awarenessProfileDetail.PAN;
                awareness.Education = awarenessProfileDetail.EduDetails;
                awareness.Hobbies = awarenessProfileDetail.Hobbies;
                awareness.Occupation = awarenessProfileDetail.Occupation;
                awareness.MaritalStatus = awarenessProfileDetail.MaritalStatus;
                awareness.HaveChildren = string.IsNullOrWhiteSpace(awarenessProfileDetail.HaveChildren) ? "No" : awarenessProfileDetail.HaveChildren.Trim();
                awareness.CompanyAddress = awarenessProfileDetail.CompanyAddress;
                awareness.CompanyCity = awarenessProfileDetail.CompanyCity;
                awareness.CompanyCountry = awarenessProfileDetail.CompanyCountry;
                awareness.CompanyName = awarenessProfileDetail.CompanyName;
                awareness.CompanyPincode = awarenessProfileDetail.CompanyPINcode;
                awareness.CompanyState = awarenessProfileDetail.CompanyState;

                awareness.IsSameAddress = awarenessProfileDetail.IsSameAddress;
                awareness.ResAddress = awarenessProfileDetail.ResAddress;
                awareness.ResCity = awarenessProfileDetail.ResCity;
                awareness.ResCountry = awarenessProfileDetail.ResCountry;
                awareness.ResPincode = awarenessProfileDetail.ResPincode;
                awareness.ResState = awarenessProfileDetail.ResState;

                awareness.PermAddress = awarenessProfileDetail.PermAddress;
                awareness.PermCity = awarenessProfileDetail.PermCity;
                awareness.PermCountry = awarenessProfileDetail.PermCountry;
                awareness.PermPincode = awarenessProfileDetail.PermPincode;
                awareness.PermState = awarenessProfileDetail.PermState;

                awareness.Stock = awarenessProfileDetail.Stock;
                awareness.Income = awarenessProfileDetail.Income;
                awareness.Payment = awarenessProfileDetail.Payment;
                awareness.Holiday = awarenessProfileDetail.Holiday;
                awareness.Shopping = awarenessProfileDetail.Shopping;

                //if (awarenessProfileDetail.MaritalStatus.ToLower() != "married")
                if (!awarenessProfileDetail.MaritalStatus.Equals("married", StringComparison.CurrentCultureIgnoreCase))
                {
                    TblffAwarenessSpouse SpouseDetail =await _context.TblffAwarenessSpouses.FirstOrDefaultAsync(x => x.ProfileId == profileID);
                    if (SpouseDetail != null)
                    {
                        _context.TblffAwarenessSpouses.Remove(SpouseDetail);
                    }
                    
                    if (awarenessProfileDetail.MaritalStatus == "Un-Married")
                    {
                        //if (awarenessProfileDetail.HaveChildren == "Yes")
                        //{
                        List<TblffAwarenessChild> childDetails = await _context.TblffAwarenessChildren
                                                                .Where(x => x.ProfileId == profileID).ToListAsync();
                        if (childDetails.Any())
                        {
                            _context.TblffAwarenessChildren.RemoveRange(childDetails);
                        }
                        //}
                    }
                }


                if (AddorUpdate == 1)
                {
                    awareness.CreateDate = DateTime.Now;
                    awareness.UpdateDate = DateTime.Now;
                    _context.TblffAwarenessProfileDetails.Add(awareness);
                }
                else
                {
                    awareness.UpdateDate = DateTime.Now;
                    _context.TblffAwarenessProfileDetails.Update(awareness);
                }

                return updateRows = await _context.SaveChangesAsync();// awarenessProfileDetail.Id;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                Console.WriteLine($"Error occurred while adding profile detail: {ex.Message}");
                throw; // rethrow or handle as appropriate
            }
        }

        public async Task<int> AwarenessAddProfileSpouseDatail(SpouseDetails awarenessProfileSpouseDetail, long profileID)
        {
            AddorUpdate = 0;
            TblffAwarenessSpouse awarenessSpouseDetail = await _context.TblffAwarenessSpouses.FirstOrDefaultAsync(x => x.ProfileId == profileID);
            if (awarenessSpouseDetail == null)
            {
                AddorUpdate = 1; 
                TblffAwarenessSpouse TblffAwarenessSpousevar = new TblffAwarenessSpouse();
                awarenessSpouseDetail = TblffAwarenessSpousevar;
            }
            //awareness.id =  awarenessProfileDetail.asdf;
            //awareness.ProfileId =  awarenessProfileDetail.asdf;
            awarenessSpouseDetail.ProfileId = profileID;
            awarenessSpouseDetail.SpouseName = awarenessProfileSpouseDetail.SpouseName;
            awarenessSpouseDetail.SpouseGender = awarenessProfileSpouseDetail.SpouseGender;
            awarenessSpouseDetail.SpouseDob = awarenessProfileSpouseDetail.SpouseDob;
            awarenessSpouseDetail.SpousePhone = awarenessProfileSpouseDetail.SpousePhone;
            awarenessSpouseDetail.SpouseAltPhone = awarenessProfileSpouseDetail.SpouseAltPhone;
            awarenessSpouseDetail.SpouseAadhaar = awarenessProfileSpouseDetail.SpouseAadhaar;
            awarenessSpouseDetail.SpouseEmail = awarenessProfileSpouseDetail.SpouseEmail;
            awarenessSpouseDetail.SpouseSecEmail = awarenessProfileSpouseDetail.SpouseSecEmail;
            awarenessSpouseDetail.SpousePan = awarenessProfileSpouseDetail.SpousePan;
            awarenessSpouseDetail.SpouseOccupation = awarenessProfileSpouseDetail.SpouseOccupation;
            awarenessSpouseDetail.SpouseCompanyAddress = awarenessProfileSpouseDetail.SpouseCompanyAddress;
            awarenessSpouseDetail.SpouseCompanyCity = awarenessProfileSpouseDetail.SpouseCompanyCity;
            awarenessSpouseDetail.SpouseCompanyCountry = awarenessProfileSpouseDetail.SpouseCompanyCountry;
            awarenessSpouseDetail.SpouseCompanyName = awarenessProfileSpouseDetail.SpouseCompanyName;
            awarenessSpouseDetail.SpouseCompanyPincode = awarenessProfileSpouseDetail.SpouseCompanyPINcode;
            awarenessSpouseDetail.SpouseCompanyState = awarenessProfileSpouseDetail.SpouseCompanyState;


            if (AddorUpdate == 1)
            {
                awarenessSpouseDetail.CreateDate = DateTime.Now;
                awarenessSpouseDetail.UpdateDate = DateTime.Now;
                _context.TblffAwarenessSpouses.Add(awarenessSpouseDetail);
            }
            else
            {
                awarenessSpouseDetail.UpdateDate = DateTime.Now;
                _context.TblffAwarenessSpouses.Update(awarenessSpouseDetail);
            }

            return updateRows = await _context.SaveChangesAsync();// awarenessProfileDetail.Id;
        }

        public async Task<int> AwarenessAddProfileAssumptions(Assumptions awarenessProfileAssumptions, long profileID)
        {
            AddorUpdate = 0;
            TblffAwarenessAssumption awarenessAssumptions = await _context.TblffAwarenessAssumptions.FirstOrDefaultAsync(x => x.ProfileId == profileID);
            if (awarenessAssumptions == null)
            {
                AddorUpdate = 1; // Add new record
                TblffAwarenessAssumption awarenessAssumptionsvar = new TblffAwarenessAssumption();
                awarenessAssumptions = awarenessAssumptionsvar;
            }

            //awareness.id =  awarenessProfileDetail.asdf;
            //awareness.ProfileId =  awarenessProfileDetail.asdf;
            awarenessAssumptions.ProfileId = profileID;
            awarenessAssumptions.Equity = awarenessProfileAssumptions.Equity;
            awarenessAssumptions.Debt = awarenessProfileAssumptions.Debt;
            awarenessAssumptions.Gold = awarenessProfileAssumptions.Gold;
            awarenessAssumptions.RealEstateReturn = awarenessProfileAssumptions.RealEstateReturn;
            awarenessAssumptions.LiquidFunds = awarenessProfileAssumptions.LiquidFunds;
            awarenessAssumptions.InflationRates = awarenessProfileAssumptions.InflationRates;
            awarenessAssumptions.EducationInflation = awarenessProfileAssumptions.EducationInflation;
            awarenessAssumptions.ApplicantRetirement = (int)awarenessProfileAssumptions.ApplicantRetirement;
            awarenessAssumptions.SpouseRetirement = awarenessProfileAssumptions.SpouseRetirement;
            awarenessAssumptions.ApplicantLifeExpectancy = awarenessProfileAssumptions.ApplicantLifeExpectancy;
            awarenessAssumptions.SpouseLifeExpectancy = awarenessProfileAssumptions.SpouseLifeExpectancy;

            if (AddorUpdate == 1)
            {
                _context.TblffAwarenessAssumptions.Add(awarenessAssumptions);
            }
            else
            {
                _context.TblffAwarenessAssumptions.Update(awarenessAssumptions);

            }
            return updateRows = await _context.SaveChangesAsync();// awarenessProfileDetail.Id;
        }


        public async Task<int> AwarenessAddProfileFamilyFinancial(FamilyFinancial awarenessProfileFamilyFinancial, long profileID)
        {

            AddorUpdate = 0;
            TblffAwarenessFamilyFinancial awarenessFamilyFinancial = await _context.TblffAwarenessFamilyFinancials.FirstOrDefaultAsync(x => x.ProfileId == profileID);
            if (awarenessFamilyFinancial == null)
            {
                AddorUpdate = 1; // Add new record
                TblffAwarenessFamilyFinancial awarenessFamilyFinancialvar = new TblffAwarenessFamilyFinancial();
                awarenessFamilyFinancial = awarenessFamilyFinancialvar;
            }

            awarenessFamilyFinancial.ProfileId = profileID;
            awarenessFamilyFinancial.Stock = awarenessProfileFamilyFinancial.Stock;
            awarenessFamilyFinancial.Income = awarenessProfileFamilyFinancial.Income;
            awarenessFamilyFinancial.Payment = awarenessProfileFamilyFinancial.Payment;
            awarenessFamilyFinancial.Holiday = awarenessProfileFamilyFinancial.Holiday;
            awarenessFamilyFinancial.Shopping = awarenessProfileFamilyFinancial.Shopping;
            awarenessFamilyFinancial.CreateDate = DateTime.Now;
            awarenessFamilyFinancial.UpdateDate = DateTime.Now;
            awarenessFamilyFinancial.Addby = profileID.ToString();

            if (AddorUpdate == 1)
            {
                _context.TblffAwarenessFamilyFinancials.Add(awarenessFamilyFinancial);
            }
            else
            {
                _context.TblffAwarenessFamilyFinancials.Update(awarenessFamilyFinancial);
            }
            return updateRows = await _context.SaveChangesAsync();// awarenessProfileDetail.Id;
        }


        public async Task<int> AwarenessAddProfileChild(ChildDetails childDetails)
        {
            AddorUpdate = 0;
            TblffAwarenessChild awarenessChild = await _context.TblffAwarenessChildren.FirstOrDefaultAsync(x => x.Id == childDetails.Id);
            if (awarenessChild == null)
            {
                AddorUpdate = 1; // Add new record
                TblffAwarenessChild TblffAwarenessChildrenvar = new TblffAwarenessChild();
                awarenessChild = TblffAwarenessChildrenvar;
            }

            //awareness.id =  awarenessProfileDetail.asdf;
            //awareness.ProfileId =  awarenessProfileDetail.asdf;
            awarenessChild.ProfileId = _profileId;
            awarenessChild.ChildName = childDetails.ChildName;
            awarenessChild.ChildGender = childDetails.ChildGender;
            awarenessChild.ChildDob = childDetails.ChildDob;
            awarenessChild.ChildPhone = childDetails.ChildPhone;
            awarenessChild.ChildAadhaar = childDetails.ChildAadhaar;
            awarenessChild.ChildEmail = childDetails.ChildEmail;
            awarenessChild.ChildPan = childDetails.ChildPan;
            awarenessChild.CreateDate = DateTime.Now;
            awarenessChild.UpdateDate = DateTime.Now;
            //awarenessChild.Addby = profileID.ToString();

            if (AddorUpdate == 1)
            {
                _context.TblffAwarenessChildren.Add(awarenessChild);
            }
            else
            {
                _context.TblffAwarenessChildren.Update(awarenessChild);
            }
            return updateRows = await _context.SaveChangesAsync();// awarenessProfileDetail.Id;
        }


        public async Task<int> AwarenessAddProfileChildrens(ChildDetails childDetails, long profileID)
        {
            AddorUpdate = 0;
            TblffAwarenessChild awarenessChild = await _context.TblffAwarenessChildren.FirstOrDefaultAsync(x => x.ProfileId == profileID);
            if (awarenessChild == null)
            {
                AddorUpdate = 1; // Add new record
                TblffAwarenessChild TblffAwarenessChildrenvar = new TblffAwarenessChild();
                awarenessChild = TblffAwarenessChildrenvar;
            }

            //awareness.id =  awarenessProfileDetail.asdf;
            //awareness.ProfileId =  awarenessProfileDetail.asdf;
            awarenessChild.ProfileId = profileID;
            awarenessChild.ChildName = childDetails.ChildName;
            awarenessChild.ChildGender = childDetails.ChildGender;
            awarenessChild.ChildDob = childDetails.ChildDob;
            awarenessChild.ChildPhone = childDetails.ChildPhone;
            awarenessChild.ChildAadhaar = childDetails.ChildAadhaar;
            awarenessChild.ChildEmail = childDetails.ChildEmail;
            awarenessChild.ChildPan = childDetails.ChildPan;
            awarenessChild.CreateDate = DateTime.Now;
            awarenessChild.UpdateDate = DateTime.Now;
            //awarenessChild.Addby = profileID.ToString();

            if (AddorUpdate == 1)
            {
                _context.TblffAwarenessChildren.Add(awarenessChild);
            }
            else
            {
                _context.TblffAwarenessChildren.Update(awarenessChild);
            }

            _context.TblffAwarenessChildren.Add(awarenessChild);
            return updateRows = await _context.SaveChangesAsync();// awarenessProfileDetail.Id;
        }


        //public async Task<ChildDetails> AwarenessListAllChild() {           
        //    var awarenessList = await _context.TblffAwarenessChildren.ToListAsync();
        //    return awarenessList;
        //}


		public async Task<int> AwarenessAdd(TblffAwarenessProfileDetail awareness)
        {
            awareness.UpdateDate = DateTime.Now;
            //user.Password  =  CommonUtillity.EncryptData(user.Password);
            //awareness.Createddate  =  DateTime.Now;
            _context.TblffAwarenessProfileDetails.Add(awareness);

           return updateRows= await _context.SaveChangesAsync(); //  awareness.Id;
        }

        public Task SaveAwarenessAsync(AwarenessViewModel model)
        {
            throw new NotImplementedException();
        }

        public string checkEmailExist(string email)
        {
            string ExistsUsername = _context.TblffAwarenessProfileDetails.Where(o => o.Email == email)
                                    .Select(o => o.Email).FirstOrDefault();
            return ExistsUsername;
        }
        public string checkEmailExistProfileTbl(long profileID)
        {
            string ExistsUsername = _context.TblffAwarenessProfileDetails.Where(o => o.ProfileId == profileID)
                                    .Select(o => o.Email).FirstOrDefault();
            return ExistsUsername;
        }
        public bool checkPANExist(string PAN, long profileID)
        {
            bool ExistsPAN = _context.TblffAwarenessProfileDetails.Any(o =>o.Pan == PAN && o.ProfileId != profileID);

            return ExistsPAN;
        }
    }
}

  