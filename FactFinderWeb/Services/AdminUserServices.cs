using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactFinderWeb.Services
{
    public class AdminUserServices
    {

        private ResellerBoyinawebFactFinderWebContext _context;
        private readonly long _userID;
        private readonly HttpContext _httpContext;

        public AdminUserServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("AdminUserId");
            _userID = Convert.ToInt64(userIdStr);
        }

        public async Task<MVAdminProfile> AdminUserLogin(MVLoginAdmin userlogin)
        {
            //string Passwordhashed = UtilityHelperServices.PasswordHash(userlogin.Password);

            var user = _context.Set<TblFfAdminUser>()
                    .Where(o => o.Email == userlogin.Email && o.AccountStatus.ToLower() == "active")
                    .Select(o => new MVAdminProfile
                    {
                        Id = o.Id,
                        AdminUserEmail = o.Email,
                        AdminUserFullName = o.Name,
                        AdminUserRole = o.AdminRole,
                        Accesskey = o.Password
                    }
                    ).FirstOrDefault();
            if (user != null)
            {
                bool isValid = UtilityHelperServices.PasswordVerify(user.Accesskey, userlogin.Password);
                if (isValid)
                {
                    user.Accesskey = null; // Clear the password from the object
                }
                else
                {
                    user = null;
                }
            }
            return user;
        }



        public async Task<List<MVAdminProfile>> GetAdminList()
        { 
            var userList = await (from ruser in _context.TblFfAdminUsers 
                                  orderby ruser.Adminuserid descending
                                  select new MVAdminProfile
                                  {
                                      Id = ruser.Id,
                                      AdminUserFullName = ruser.Name,
                                      AdminUserEmail = ruser.Email,
                                      Mobile = ruser.Mobile,
                                        Adminuserid = ruser.Adminuserid,
                                        AdminRole = ruser.AdminRole,
                                        Department = ruser.Department,
                                        AccountStatus = ruser.AccountStatus,
                                        CreateDate = ruser.CreateDate,
                                        Accesskey = ruser.Accesskey
                                  }).ToListAsync();
                                  //}).Take(100).ToListAsync();
            return userList;
        }


        public async Task<List<SelectListItem>> GetAdvisorList()
        {
            var advisorListdata = await (from ruser in _context.TblFfAdminUsers
                                  where ruser.AccountStatus.ToLower() =="active" && ruser.AdminRole== "admin"
                                         orderby ruser.Adminuserid descending
                                  select new AdvisorList
                                  {
                                      AdvisorId = ruser.Id.ToString(),
                                      AdvisorName = ruser.Name
                                      //AdminRole = ruser.AdminRole,
                                  }).ToListAsync();
            //}).Take(100).ToListAsync();

            var advisorLists = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Select" },
                };

            foreach (var advisor in advisorListdata) // 2. Dynamically add "Child Education" if needed
            {
                advisorLists.Add(new SelectListItem
                {
                    Value = advisor.AdvisorId,
                    Text = advisor.AdvisorName
                });
            }

            return advisorLists;
        }



        public string checkEmailExist(string email)
        {
            string ExistsUsername = _context.Set<TblFfAdminUser>()
                    .Where(o => o.Email == email)
                    .Select(o => o.Email).FirstOrDefault();

            return ExistsUsername;
        }



 public async Task<(List<MVADUserDetails> Users, int TotalRecords, int TotalPages)> GetUserListAsync(
      string adminRole,
      int advisorID,
      int pageNumber = 1,
      int pageSize = 10, string search = "", string? plantype="")
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Base query
            var query = _context.TblFfRegisterUsers.AsQueryable();

            // Filter if admin
            if (adminRole == "admin")
            {
                query = query.Where(ruser => ruser.Advisorid == advisorID);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(ruser =>
                    ruser.Name.ToLower().StartsWith(search) ||
                    ruser.Email.ToLower().StartsWith(search));
            }
            if (!string.IsNullOrWhiteSpace(plantype))
            {
                plantype = plantype.ToLower();

                query = query.Where(ruser =>
                    _context.TblffAwarenessProfileDetails.Any(p =>
                        p.UserId == ruser.Id &&
                        p.PlanType != null &&
                        p.PlanType.ToLower() == plantype
                    )
                );
            }

            // Get total records before paging
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Fetch paged data
            var users = await query
                .OrderByDescending(r => r.Createddate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(ruser => new MVADUserDetails
                {
                    Name = ruser.Name,
                    email = ruser.Email,
                    mobile = ruser.Mobile,
                    activestatus = ruser.Activestatus == "1" ? "Active" : "Deactive",
                    createddate = ruser.Createddate,
                    userFile = ruser.Ptx,
                    advisorid = ruser.Advisorid,
                    Id = ruser.Id,
                   

                })
                .ToListAsync();

         
            foreach (var user in users)
            {
                var userProfiles = await (
                    from profile in _context.TblffAwarenessProfileDetails
                    where profile.UserId == user.Id && profile.PlanStatus != "Expired"
                    orderby profile.CreateDate descending
                    select new UserProfileViewModel
                    {

                        UserFullName = profile.Name,
                        UserPlan = profile.PlanType.ToLower() == "basic" ? "Basic" : profile.PlanType.ToLower() == "comprehensive" ? "Comprehensive" : profile.PlanType.ToLower() == "zero2one" ? "Zero2One" : profile.PlanType.ToLower() == "wealth" ? "Wealth" : profile.PlanType,
                        UserPlanYear = profile.PlanYear,
                        UserEmail = profile.Email,
                        UserMobile = profile.Phone,
                        UserRegisterDate = profile.CreateDate,
                        ProfileStatus = profile.ProfileStatus, //user submitted =pending or admin locked = locked
                        ProfileId = profile.ProfileId,
                        UId = profile.Uid,
                        Awakenstatus = profile.Awakenstatus,
                        PlanStatus = profile.PlanStatus,
                        PlanStartDate = profile.PlanStartDate,
                        PlanEndDate = profile.PlanEndDate,
                        PlanYear = profile.PlanYear,
                        PlanDuration = profile.PlanDuration,
                        RevisionNumber = profile.RevisionNumber,
                        PdfPath = profile.PdfPath,
                        UIdText = profile.RevisionNumber > 0 ? profile.Uid + "-R" + profile.RevisionNumber : profile.Uid,
                    }
                ).ToListAsync();

                user.ProfileLst= userProfiles;
                user.advisorName = await _context.TblFfAdminUsers.Where(a => a.Id == user.advisorid).Select(a => a.Name).FirstOrDefaultAsync();
            }
            

                return (users, totalRecords, totalPages);
        }
        public async Task<List<UserProfileViewModel>> GetclientsProfilebbyIdAsyncold(string? UID = "")
        {

            var userProfiles = await (
                from profile in _context.TblffAwarenessProfileDetails
                where profile.Uid == UID && profile.PlanStatus == "Expired"
                orderby profile.CreateDate descending
                select new UserProfileViewModel
                {

                    UserFullName = profile.Name,
                    UserPlan = profile.PlanType.ToLower()== "basic"? "Basic": profile.PlanType.ToLower() == "comprehensive" ? "Comprehensive" : profile.PlanType.ToLower() == "zero2one" ? "Zero2One" : profile.PlanType.ToLower() == "wealth" ? "Wealth" : profile.PlanType,
                    UserPlanYear = profile.PlanYear,
                    UserEmail = profile.Email,
                    UserMobile = profile.Phone,
                    UserRegisterDate = profile.CreateDate,
                    ProfileStatus = profile.ProfileStatus, //user submitted =pending or admin locked = locked
                    ProfileId = profile.ProfileId,
                    UIdText = profile.RevisionNumber > 0 ? profile.Uid + "-R" + profile.RevisionNumber : profile.Uid,
                    Awakenstatus = profile.Awakenstatus,
                    UId = profile.Uid,
                    PlanStatus = profile.PlanStatus,
                    PlanStartDate = profile.PlanStartDate,
                    PlanEndDate = profile.PlanEndDate,
                    PlanYear = profile.PlanYear,
                    PlanDuration = profile.PlanDuration,
                    RevisionNumber = profile.RevisionNumber
                }
            ).ToListAsync();

            return userProfiles;
        }
        public async Task<List<UserProfileViewModel>> GetclientsProfilebbyIdAsync(string? UID = "")
        {
            var today = DateTime.Today;
            var userProfiles = await (
                from profile in _context.TblffAwarenessProfileDetails
                where profile.Uid == UID && profile.PlanStatus == "Expired"
                orderby profile.CreateDate descending
                select new UserProfileViewModel
                {

                    UserFullName = profile.Name,
                    UserPlan = profile.PlanType.ToLower() == "basic" ? "Basic" : profile.PlanType.ToLower() == "comprehensive" ? "Comprehensive" : profile.PlanType.ToLower() == "zero2one" ? "Zero2One" : profile.PlanType.ToLower() == "wealth" ? "Wealth" : profile.PlanType,
                    UserPlanYear = profile.PlanYear,
                    UserEmail = profile.Email,
                    UserMobile = profile.Phone,
                    UserRegisterDate = profile.CreateDate,
                    ProfileStatus = profile.ProfileStatus, //user submitted =pending or admin locked = locked
                    ProfileId = profile.ProfileId,
                    UIdText = profile.RevisionNumber > 0 ? profile.Uid + "-R" + profile.RevisionNumber : profile.Uid,
                    Awakenstatus = profile.Awakenstatus,
                    UId = profile.Uid,
                    PlanStatus = profile.PlanStatus,
                    PlanStartDate = profile.PlanStartDate,
                    PlanEndDate = profile.PlanEndDate,
                    PlanYear = profile.PlanYear,
                    PlanDuration = profile.PlanDuration,
                    RevisionNumber = profile.RevisionNumber,
                    CreateDate = profile.CreateDate,
                    Advisorid = profile.Advisorid,
                    PdfPath = profile.PdfPath
                }
            ).ToListAsync();
            foreach (var item in userProfiles)
            {
                item.AdvisorName = await _context.TblFfAdminUsers.Where(a => a.Id == item.Advisorid).Select(a => a.Name).FirstOrDefaultAsync();
                item.DaysUntilRenewal = item.PlanEndDate != null
            ? (item.PlanEndDate.Value - today).Days
            : null;
            }

            return userProfiles;
        }

        public async Task<(List<UserProfileViewModel> Users, int TotalRecords, int TotalPages, RenewalStatusCounts StatusCounts)>
     GetReNewUserListAsync(
        string adminRole,
        int advisorID,
        int pageNumber = 1,
        int pageSize = 10,
        string search = "",
        string status = "",
        string plantype = "",
        int assignedto = 0)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.TblffAwarenessProfileDetails.AsQueryable();
            query = query.Where(x => x.PlanStatus != "Expired");
         //   query = query.Where(x => x.PlanStartDate != null);
            // Admin filter
            if (adminRole == "admin")
            {
                query = query.Where(x => x.Advisorid == advisorID);
            }

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(x =>
                    (x.Name ?? "").ToLower().StartsWith(search) ||
                      (x.Uid ?? "").ToLower().StartsWith(search) ||
                    (x.Email ?? "").ToLower().StartsWith(search) ||
                    (x.PlanType ?? "").ToLower().StartsWith(search) ||
                    (x.PlanDuration ?? "").ToLower().StartsWith(search));
            }

            // PlanType
            if (!string.IsNullOrWhiteSpace(plantype))
            {
                plantype = plantype.ToLower();
                query = query.Where(x =>
                    x.PlanType != null &&
                    x.PlanType.ToLower().StartsWith(plantype));
            }
            var today = DateTime.Today;
            // Status
            if (!string.IsNullOrWhiteSpace(status))
            {
             

                switch (status.ToLower())
                {
                    case "overdue":
                        query = query.Where(x =>
                            x.PlanEndDate != null &&
                            x.PlanEndDate < today);
                        break;

                    case "due-soon":
                        query = query.Where(x =>
                            x.PlanEndDate != null &&
                            x.PlanEndDate >= today &&
                            x.PlanEndDate <= today.AddDays(30));
                        break;

                    case "active":
                        query = query.Where(x =>
                            x.PlanStartDate != null &&
                            x.PlanEndDate != null &&
                            today >= x.PlanStartDate &&
                            today <= x.PlanEndDate);
                        break;

                    case "renewed":
                        query = query.Where(x => x.RenewalSent == true);
                        break;
                }
            }

            // Total count
            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var statusCounts = await query
    .GroupBy(x => 1)
    .Select(g => new RenewalStatusCounts
    {
        Total = g.Count(),

        Overdue = g.Count(x =>
            x.PlanEndDate != null && x.PlanEndDate < today),

        DueSoon = g.Count(x =>
            x.PlanEndDate != null &&
            x.PlanEndDate >= today &&
            x.PlanEndDate <= today.AddDays(30)),

        Active = g.Count(x =>
            x.PlanStartDate != null &&
            x.PlanEndDate != null &&
            today >= x.PlanStartDate &&
            today <= x.PlanEndDate),

        Inactive = g.Count(x =>
            x.PlanStartDate == null ||
            x.PlanEndDate == null ||
            today < x.PlanStartDate ||
            today > x.PlanEndDate),
        RenewalSent = g.Count(x => x.RenewalSent == true)   // ✅ added
    })
    .FirstOrDefaultAsync();

            // Paging + Projection
            var users = await query
                .OrderByDescending(x => x.CreateDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(profile => new UserProfileViewModel
                {
                    UIdText = profile.RevisionNumber > 0 ? profile.Uid + "-R" + profile.RevisionNumber : profile.Uid,
                    UserFullName = profile.Name,
                    UserPlan  = profile.PlanType.ToLower() == "basic" ? "Basic" : profile.PlanType.ToLower() == "comprehensive" ? "Comprehensive" : profile.PlanType.ToLower() == "zero2one" ? "Zero2One" : profile.PlanType.ToLower() == "wealth" ? "Wealth" : profile.PlanType,
                    UserPlanYear = profile.PlanYear,
                    UserEmail = profile.Email,
                    UserMobile = profile.Phone,
                    UserRegisterDate = profile.CreateDate,
                    ProfileStatus = profile.ProfileStatus,
                    ProfileId = profile.ProfileId,
                    UId = profile.Uid,
                    Awakenstatus = profile.Awakenstatus,
                    PlanStatus = profile.PlanStatus,
                    PlanStartDate = profile.PlanStartDate,
                    PlanEndDate = profile.PlanEndDate,
                    RevisionNumber = profile.RevisionNumber,
                    PlanYear = profile.PlanYear,
                    PlanDuration = profile.PlanDuration,
                    CreateDate = profile.CreateDate,
                    Advisorid = profile.Advisorid,
                    DOB = Convert.ToDateTime(profile.Dob).ToString("MM/dd/yyyy"),
                    RenewalSent = profile.RenewalSent ?? false,
                    RenewalStatus =
                                  profile.RenewalSent == true
                                ? "renewed"
                                : profile.PlanEndDate != null && profile.PlanEndDate < today
                                    ? "overdue"
                                    : profile.PlanEndDate != null &&
                                      profile.PlanEndDate >= today &&
                                      profile.PlanEndDate <= today.AddDays(30)
                                        ? "due-soon"
                                        : profile.PlanStartDate != null &&
                                          profile.PlanEndDate != null &&
                                          today >= profile.PlanStartDate &&
                                          today <= profile.PlanEndDate
                                            ? "active"
                                            : "inactive",
                    Status =
                                  profile.PlanEndDate != null && profile.PlanEndDate < today
                                    ? "overdue"
                                    : profile.PlanEndDate != null &&
                                      profile.PlanEndDate >= today &&
                                      profile.PlanEndDate <= today.AddDays(30)
                                        ? "due-soon"
                                        : profile.PlanStartDate != null &&
                                          profile.PlanEndDate != null &&
                                          today >= profile.PlanStartDate &&
                                          today <= profile.PlanEndDate
                                            ? "active"
                                            : "inactive"
                })
                .ToListAsync();
            foreach (var item in users)
            {
                item.AdvisorName = await _context.TblFfAdminUsers.Where(a => a.Id == item.Advisorid).Select(a => a.Name).FirstOrDefaultAsync();
                item.DaysUntilRenewal = item.PlanEndDate != null
            ? (item.PlanEndDate.Value - today).Days
            : null;
            }
            return (users, totalRecords, totalPages, statusCounts);
        }




        public async Task<UserProfileViewModel> GetUserDetail(long id)
        {
            //var userList = await _context.TblFfRegisterUsers
            //.LeftJoin(_context.TblffAwarenessProfileDetails, p => p.UserId, u => u.Id, (p, u) => new { p, u })
            // MVADUserDetails userList = new MVADUserDetails();
            var userList = await (from ruser in _context.TblFfRegisterUsers where ruser.Id == id
                                  orderby ruser.Createddate descending
                                  select new UserProfileViewModel
                                  {
                                      AdvisorName = ruser.AdvisorName,
                                      Advisorid = ruser.Advisorid,
                                      UserFullName = ruser.Name,
                                      UserPlan = ruser.Plantype,
                                     // UserPlanYear = ruser.PlanYear,
                                      UserEmail = ruser.Email,
                                      UserEmailVerification = ruser.Emailverified,
                                      UserMobile = ruser.Mobile,
                                      UserActiveStatus = ruser.Activestatus,// == "1" ? "Active" : "Deactive"
                                      UserRegisterDate = ruser.Createddate,
                                      Userptx = ruser.Ptx, //user submitted =1 or admin locked = 2
                                                           // ProfileStatus = ruser.ProfileStatus, //user submitted =pending or admin locked = locked
                                      Id = ruser.Id
                                  }).FirstOrDefaultAsync();
            return userList;
        }



        public async Task<int> UserUpdate(UserProfileViewModel userProfileViewModel)
        {


            TblFfRegisterUser user = await _context.TblFfRegisterUsers.Where(x => x.Id == userProfileViewModel.Id).FirstOrDefaultAsync();

            if(user == null)
            {
                return 0; // User not found
            }
 
            user.Advisorid = userProfileViewModel.Advisorid;
            user.Activestatus = userProfileViewModel.UserActiveStatus;
            _context.TblFfRegisterUsers.Update(user);

            await _context.SaveChangesAsync();

            var profile = await _context.TblffAwarenessProfileDetails.Where(u => u.UserId == user.Id).ToListAsync();

            foreach (var item in profile)
            {
                item.Advisorid = userProfileViewModel.Advisorid;
                item.ProfileStatus = "Assign";
                _context.TblffAwarenessProfileDetails.Update(item);
            }

            int resultCount = await _context.SaveChangesAsync();
           
            return resultCount;
        }


        public async Task<Int64> AdminUserAdd(TblFfAdminUser adminUser)
        {
            var user = new TblFfAdminUser();
            string Passwordhashed = UtilityHelperServices.PasswordHash(adminUser.Password);
            adminUser.Password = Passwordhashed;
            _context.TblFfAdminUsers.Add(adminUser);

            int resultCount = await _context.SaveChangesAsync();

            return resultCount;
        }

        public async Task<bool> AddAdminUserAsync(AdminRegViewModel newUser)
        {
            try
            {
                bool emailExists = await _context.TblFfAdminUsers.AnyAsync(u => u.Email == newUser.Email);

                if (emailExists)
                {
                    return false; // Or throw/return error if needed
                }

                string hashedPassword = UtilityHelperServices.PasswordHash(newUser.Password);

                var entity = new TblFfAdminUser
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Password = hashedPassword,
                    AdminRole = newUser.AdminRole,
                    Department = newUser.Department, // Add if exists
                    Mobile = newUser.Mobile          // Add if exists
                };

                _context.TblFfAdminUsers.Add(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<TblFfAdminUser> GetAdminUserDetail(long Userid)
        {
            var adminuserList = await (from ruser in _context.TblFfAdminUsers
                                       where ruser.Id == Userid 
                                  orderby ruser.CreateDate descending
                                  select new TblFfAdminUser
                                  {
                                      Id = ruser.Id,
                                      Name = ruser.Name,
                                      Email = ruser.Email,
                                      AdminRole = ruser.AdminRole,
                                      Department = ruser.Department,
                                      AccountStatus = ruser.AccountStatus,
                                      CreateDate = ruser.CreateDate,
                                      UpdateDate = ruser.UpdateDate
                                  }).FirstOrDefaultAsync();
            return adminuserList;
        }


        public async Task<int> UpdateAdminUserDetail(TblFfAdminUser adminuser)
        {
            var adminuserList = await _context.TblFfAdminUsers.Where(u => u.Id == adminuser.Id)
                                       .FirstOrDefaultAsync();

            if (adminuserList == null)
            {
                return 0; // User not found
            }

            adminuserList.AccountStatus = adminuser.AccountStatus;  
            adminuserList.AdminRole = adminuser.AdminRole;  
            adminuserList.UpdateDate = DateTime.UtcNow;
            _context.TblFfAdminUsers.Update(adminuserList);

            int resultCount = await _context.SaveChangesAsync();
            return resultCount;
        }


        public async Task<Int32> AdminChangepwd(long adminID, string oldPwd, string newPwd)
        {
        
            var adminuserData = await _context.TblFfAdminUsers.Where(u => u.Id == adminID ).FirstOrDefaultAsync();
            bool isValid = UtilityHelperServices.PasswordVerify(adminuserData.Password, oldPwd);
            if (!isValid)
            {
                return 0; // User not found
            }

            string Passwordhashed = UtilityHelperServices.PasswordHash(newPwd);
            adminuserData.Password = Passwordhashed;
            adminuserData.UpdateDate = DateTime.Now;
            _context.TblFfAdminUsers.Update(adminuserData);
            int resultCount = await _context.SaveChangesAsync();
            return resultCount;
        }
        public string checkUseEmailExist(string email)
        {
            string ExistsUsername = _context.Set<TblFfRegisterUser>()
                    .Where(o => o.Email == email)
                    .Select(o => o.Email).FirstOrDefault();

            return ExistsUsername;
        }
        public async Task<TblffAwarenessProfileDetail> UserAdds(TblFfRegisterUser user, int registerid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            string Passwordhashed = UtilityHelperServices.PasswordHash(user.Password);
            string emailVerifyToken = UtilityHelperServices.GenerateSecureToken(24);

            user.Updatedate = DateTime.Now;
            user.Mobile = user.Mobile;
            user.Password = Passwordhashed; //user.Password = CommonUtillity.EncryptData(user.Password);
            user.Emailverified = "yesverified";
            user.Createddate = DateTime.Now;
          //  user.Advisorid = registerid;
            _context.TblFfRegisterUsers.Add(user);
            await _context.SaveChangesAsync();

            /// Add user profile details
			TblffAwarenessProfileDetail userProfile = new TblffAwarenessProfileDetail();
            userProfile.UserId = user.Id;
            userProfile.Name = user.Name;
            userProfile.Email = user.Email;
            userProfile.Phone = user.Mobile;
            userProfile.PlanType = user.Plantype;
            userProfile.PlanYear = DateTime.Now.Year;
            userProfile.CreateDate = DateTime.Now;
            userProfile.UpdateDate = DateTime.Now;
            userProfile.ProfileStatus = "Draft";
            userProfile.Registerid = registerid;

            _context.TblffAwarenessProfileDetails.Add(userProfile);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return userProfile;
        }
        public async Task<string> GeneratePdf(long ProfileId, JSONDataUtility _jsonData, PdfService _pdfService, IViewRenderService _viewRenderService, IWebHostEnvironment _env)
        {
            var userProfile = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(p => p.ProfileId == ProfileId);
            string PdfPath = "";
            if (userProfile != null)
            {

                var awakenData = await _jsonData.GetAwakenSection(userProfile.ProfileId);

                string html = await _viewRenderService.RenderToStringAsync(
                    "Schedule/assignedpdf",
                    awakenData
                );

                byte[] pdfBytes = _pdfService.GeneratePdf(html,  _env);

                var uniqueId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDFs");
                var fileName = "FF_" + userProfile.PlanType + "_" + userProfile.Name.Replace(" ", "").Replace("  ", "").Replace("  ", "") + "_" + userProfile.ProfileId + "_" + uniqueId + ".pdf";
                var savePath = Path.Combine(baseFolder, fileName);
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                    Console.WriteLine($"🗑️ Existing file deleted: {savePath}");
                }
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath); // delete the old file
                    Console.WriteLine($"🗑️ Existing file replaced: {savePath}");
                }

                await System.IO.File.WriteAllBytesAsync(savePath, pdfBytes);
                PdfPath = baseFolder + "/" + fileName;
                userProfile.PdfPath = "/PDFs/" + fileName;
                userProfile.PdfGeneratedOn = DateTime.Now;
                if (userProfile.ProfileStatus == "Assign")
                {
                    userProfile.Addby = "pdf generted";
                }
                await _context.SaveChangesAsync();

            }
            return PdfPath;
        }
    }
}
