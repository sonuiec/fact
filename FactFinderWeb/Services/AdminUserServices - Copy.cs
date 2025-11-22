using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;

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
					.Where(o => o.Email == userlogin.Email)
					.Select(o => new MVAdminProfile
                    {
						Id = o.Id,
						AdminUserEmail = o.Email,
						AdminUserFullName = o.Name,
						AdminUserRole = o.AdminRole,
                        Userptx = o.Password
					}
					).FirstOrDefault();
            if(user != null)
            {                
                 bool isValid = UtilityHelperServices.PasswordVerify(user.Userptx, userlogin.Password);
                 if (isValid)
                 {
                    user.Userptx = null; // Clear the password from the object
                }
                else
                {
                    user = null;
                }
            }
            return user;
		}

		public string checkEmailExist(string email)
		{
			string ExistsUsername = _context.Set<TblFfAdminUser>()
					.Where(o => o.Email == email)
					.Select(o => o.Email).FirstOrDefault();

			return ExistsUsername;
		}



        public async Task<List<MVADUserDetails>> GetuserList()
        {
            //var userList = await _context.TblFfRegisterUsers
            //.LeftJoin(_context.TblffAwarenessProfileDetails, p => p.UserId, u => u.Id, (p, u) => new { p, u })
           // MVADUserDetails userList = new MVADUserDetails();
        var userList = await ( from ruser in _context.TblFfRegisterUsers
                        join user in _context.TblffAwarenessProfileDetails on ruser.Id equals user.Profileid
                        orderby ruser.Createddate descending
                        select new MVADUserDetails
                        { 
                            Name = ruser.Name,
                            planType = ruser.Plantype,
                            planYear = user.PlanYear.ToString(),
                            email = ruser.Email,
                            mobile = ruser.Mobile,
                            activestatus = ruser.Activestatus == "1" ? "Active" : "Deactive",
                            createddate = ruser.Createddate,
                            userFile = ruser.Ptx, //UserFile
                            ProfileId = ruser.Id,
                            Id = user.Id         
                        }).ToListAsync();
                    return userList;
 
        }


        public async Task<Int64> AdminUserAdd(TblFfAdminUser adminUser)
        {
            var user = new TblFfAdminUser();
            string Passwordhashed = UtilityHelperServices.PasswordHash(adminUser.Password);
            adminUser.Password = Passwordhashed;
            _context.TblFfAdminUsers.Add(adminUser);

            int resultCount= await _context.SaveChangesAsync();

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
    }
}
