using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
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
      int pageSize = 10, string search = "")
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
                    where profile.UserId == user.Id
                    orderby profile.CreateDate descending
                    select new UserProfileViewModel
                    {

                        UserFullName = profile.Name,
                        UserPlan = profile.PlanType,
                        UserPlanYear = profile.PlanYear,
                        UserEmail = profile.Email,
                        UserMobile = profile.Phone,
                        UserRegisterDate = profile.CreateDate,
                        ProfileStatus = profile.ProfileStatus, //user submitted =pending or admin locked = locked
                        ProfileId = profile.ProfileId,
                        UId = profile.Uid,
                        Awakenstatus = profile.Awakenstatus
                    }
                ).ToListAsync();

                user.ProfileLst= userProfiles;
            }


            return (users, totalRecords, totalPages);
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
            //string Passwordhashedold = UtilityHelperServices.PasswordHash(oldPwd);

            var adminuserData = await _context.TblFfAdminUsers.Where(u => u.Id == adminID).FirstOrDefaultAsync();
            // && u.Password == Passwordhashedold
            if (adminuserData == null)
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
            user.Advisorid = registerid;
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
    }
}
