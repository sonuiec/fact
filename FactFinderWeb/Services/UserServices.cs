using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace FactFinderWeb.Services 
{
	public class UserServices  
{

	private ResellerBoyinawebFactFinderWebContext _context;

	private readonly long _userID;
	private readonly HttpContext _httpContext;

		public UserServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContext = httpContextAccessor.HttpContext;
			var userIdStr = _httpContext.Session.GetString("UserId");
			_userID = Convert.ToInt64(userIdStr);
		}

		public async Task<TblffAwarenessProfileDetail> UserAdd(TblFfRegisterUser user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            string Passwordhashed = UtilityHelperServices.PasswordHash(user.Password);
			string emailVerifyToken = UtilityHelperServices.GenerateSecureToken(24);

            user.Updatedate = DateTime.Now;
            user.Mobile = user.Mobile; 
            user.Password = Passwordhashed; //user.Password = CommonUtillity.EncryptData(user.Password);
			user.Emailverified = emailVerifyToken;
            user.Createddate = DateTime.Now;
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
            userProfile.Registerid = user.Id;
            _context.TblffAwarenessProfileDetails.Add(userProfile);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return userProfile;
        }

        public async void UserAddProfileDetail(TblFfRegisterUser user)
        {
			TblffAwarenessProfileDetail userProfile = new TblffAwarenessProfileDetail();
            userProfile.ProfileId = user.Id;
			userProfile.Name = user.Name;
			userProfile.Email = user.Email;
			userProfile.Phone = user.Mobile;
			userProfile.PlanType = user.Plantype;
			userProfile.PlanYear = DateTime.Now.Year;
			userProfile.CreateDate = DateTime.Now;
			userProfile.UpdateDate = DateTime.Now;
            _context.TblffAwarenessProfileDetails.Add(userProfile);
            await _context.SaveChangesAsync();
        }
        
		public async Task<Int64> UserUpdate(TblFfRegisterUser user)
		{
			user.Updatedate = DateTime.Now;

			_context.TblFfRegisterUsers.Update(user);

			await _context.SaveChangesAsync();

			return user.Id;
		}

		public async Task<MVUserProfile> UserLogin(MVLogin userlogin)
		{
			var user = _context.Set<TblFfRegisterUser>()
					.Where(o => o.Email == userlogin.Email )
					.Select(o => new MVUserProfile
					{
						UserId = o.Id,
						UserEmail = o.Email,
						UserFullName = o.Name,
                        Emailverified = o.Emailverified,
                        Userptx = o.Password,
						UserPlan = o.Plantype
					}
					).FirstOrDefault();
			if (user != null)
			{
				bool isValid = UtilityHelperServices.PasswordVerify(user.Userptx, userlogin.Password);
				if (isValid)
				{
					user.Userptx = null; // Clear the password from the object
										 //_httpContext.Session.SetString("UserId", user.UserId.ToString());
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
			string ExistsUsername = _context.Set<TblFfRegisterUser>()
					.Where(o => o.Email == email)
					.Select(o => o.Email).FirstOrDefault();

			return ExistsUsername;
		}

		public async Task<TblFfRegisterUser> GetOrganizationDetails(int Id)
		{
			// Fetch the organization from the database
			var user = await _context.TblFfRegisterUsers
											.Where(o => o.Id == Id)
											.FirstOrDefaultAsync();

			return user;
		}


        public async Task<List<UserProfileViewModel>>GetDashBoardUserListAsync(long? _userID)
        {
           
            var query = _context.TblffAwarenessProfileDetails.AsQueryable();
             query = query.Where(x => x.PlanStatus != "Expired");
            // Admin filter
                query = query.Where(x => x.UserId == _userID);

			// Searchvar 
			var today = DateTime.Today;

            

            // Paging + Projection
            var users = await query
                .OrderByDescending(x => x.CreateDate)
                .Select(profile => new UserProfileViewModel
                {
                    UserFullName = profile.Name,
                    UserPlan = profile.PlanType.ToLower() == "basic" ? "Basic" : profile.PlanType.ToLower() == "comprehensive" ? "Comprehensive" : profile.PlanType.ToLower() == "zero2one" ? "Zero2One" : profile.PlanType.ToLower() == "wealth" ? "Wealth" : profile.PlanType,
                    UserPlanYear = profile.PlanYear,
                    RevisionNumber = profile.RevisionNumber,
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
                    UIdText = profile.RevisionNumber > 0 ? profile.Uid + "-R" + profile.RevisionNumber : profile.Uid,
                    PlanYear = profile.PlanYear,
                    PlanDuration = profile.PlanDuration,
                    CreateDate = profile.CreateDate,
                    Advisorid = profile.Advisorid,
                    PdfPath = profile.PdfPath,
                    DOB = Convert.ToDateTime(profile.Dob).ToString("MM/dd/yyyy"),
                    RenewalSent = profile.RenewalSent ?? false,
                    IsRenewal = profile.RenewedFromProfileId > 0,
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
			return users;
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

        public async Task<List<DashboardViewModel>> UserDashboard()
		{


			var user =await (from o in _context.TblFfRegisterUsers
						join p in _context.TblffAwarenessProfileDetails
						on o.Id equals p.UserId
                        join ad in _context.TblFfAdminUsers
						on (long)p.Advisorid equals ad.Id into advisorJoin
                        //join ad in _context.TblFfAdminUsers
                        //  on o.Advisorid equals ad.Id into advisorJoin
                        from ad in advisorJoin.DefaultIfEmpty()

						where o.Id == _userID
						select new DashboardViewModel
						{
							UId = p.Uid,
							UserEmail = o.Email,
							UserFullName = p.Name,
							UserMobile = p.Phone,
							UserPlan = p.PlanType,
							UserPlanYear = p.PlanYear,
							UserRegisterDate = o.Createddate,
							UserActiveStatus = o.Activestatus,
							UserEmailVerification = o.Emailverified,
							Userptx = o.Password,
							AdvisorName = ad.Name,
                            PlanCreatedDate =p.CreateDate,
							PlanUpdatedDate= p.UpdateDate,
							ProfileId= p.ProfileId,
							ProfileStatus = p.ProfileStatus

                        }).ToListAsync();

			return user;
		}


		public async Task<string> RequestPasswordReset(MVResetPassword mVResetPassword)
		{
            var user = await _context.TblFfRegisterUsers.FirstOrDefaultAsync(u => u.Email == mVResetPassword.Email);
			if (user == null) return "user not exist";

			var confirmData = await _context.TblffPasswordResetRequests.FirstOrDefaultAsync(u => u.UserId == user.Id && u.Token == mVResetPassword.code);
			if (confirmData != null)
			{
				confirmData.CreatedAt = DateTime.Now;
				confirmData.IsUsed = true;
				_context.TblffPasswordResetRequests.Update(confirmData);

				string Passwordhashed = UtilityHelperServices.PasswordHash(mVResetPassword.Password);
				user.Password = Passwordhashed;
				_context.TblFfRegisterUsers.Update(user);
			}

			await _context.SaveChangesAsync();

			return "Password updated successfully. Please log in with your new password.";
		}
        public async Task<string> RequestResendVerificationLink(string email)
        {
            var user = await _context.TblFfRegisterUsers
                                     .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return "";

            // Generate new verification token
            string emailVerifyToken = UtilityHelperServices.GenerateSecureToken(24);

            // Update the existing user  
            user.Emailverified = emailVerifyToken;
            user.Updatedate = DateTime.Now;

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                // Return token for sending in email
                return emailVerifyToken;
            }

            return "";
        }


        public async Task<string> ForgotPassword(string email)
		{
			var user = await _context.TblFfRegisterUsers.FirstOrDefaultAsync(u => u.Email == email);
			if (user == null) return "";

			var token = Guid.NewGuid().ToString(); // Secure random token
			var expiration = DateTime.UtcNow.AddDays(7);
			
			_context.TblffPasswordResetRequests.Add(new TblffPasswordResetRequest
			{
				UserId = user.Id,
				Token = token,
				Expiration = expiration
			});

			//await _emailService.SendResetPasswordEmail(user.Email, resetLink);
	
            int i = await _context.SaveChangesAsync();
			if(i > 0)
			{
				
				//string resetLink = $"https://yourapp.com/reset-password?token={token}";
                return token;
            }
			else
			{
				return "";
			}
			
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

/*
	public void CreateOrUpdate(MVLogin login)
	{
		try
		{
			//var passwordhash = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(userView.PasswordHash)).Select(s => s.ToString("x2")));


			if (login.txtemail != null)
			{
				if (login.UserId != 0)
				{
					User a = _context.TblFfRegisterUsers.Where(x => x.Email == login.txtemail).FirstOrDefault();
					if (a != null)
					{
						a.FirstName = login.txtfullname;
						a.UpdatedAt = DateTime.Now;

						_context.Users.Update(a);
						_context.SaveChanges();

					}
				}
				else
				{
					var passwordhash = CommonUtillity.EncryptData(userView.PasswordHash);
					User a = new User();
					a.FirstName = userView.FirstName;
					a.LastName = userView.LastName;
					a.Email = userView.Email;
					a.UserName = userView.Username;
					a.PasswordHash = passwordhash;
					a.RoleId = userView.RoleId;
					a.IsActive = userView.IsActive;
					a.CreatedAt = DateTime.Now;
					a.UpdatedAt = null;

					_context.Users.Add(a);
					_context.SaveChanges();

				}

			}
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	public async Task<PaginatedResult<UserView>> GetAll(
			string? searchTerm = null,     // Optional search term
			string sortBy = "Name",       // Default sort by "Name"
			bool sortDescending = false,  // Default ascending order
			int pageNumber = 1,           // Default page number
			int pageSize = 10             // Default page size
		)
	{
		// Ensure page number and size are valid
		if (pageNumber <= 0) pageNumber = 1;
		if (pageSize <= 0) pageSize = 10;

		var query = from u in _context.Users
					join v in _context.Roles
					   on u.RoleId equals v.Id
					select new UserView
					{
						UserId = u.UserId,
						FirstName = u.FirstName,
						LastName = u.LastName,
						Username = u.UserName,
						PasswordHash = u.PasswordHash,
						Email = u.Email,
						IsActive = u.IsActive,
						RoleName = v.RoleName,
						RoleId = u.RoleId,
						UpdatedAt = u.UpdatedAt,
						CreatedAt = u.CreatedAt,
					};

		if (!string.IsNullOrWhiteSpace(searchTerm))
		{
			query = query.Where(x => x.FirstName.Contains(searchTerm));
		}

		// Apply sorting
		query = sortBy.ToLower() switch
		{
			"name" => sortDescending ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName),
			"createdat" => sortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
			"updatedat" => sortDescending ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
			_ => sortDescending ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName) // Default sorting
		};

		// Get total item count for pagination
		int totalItems = await query.CountAsync();

		// Calculate total pages
		int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

		// Apply paging
		query = query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize);

		// Project to LayoutView and execute the query
		var items = await query.Select(u => new UserView
		{
			UserId = u.UserId,
			FirstName = u.FirstName,
			LastName = u.LastName,
			PasswordHash = u.PasswordHash,
			Username = u.Username,
			Email = u.Email,
			IsActive = u.IsActive,
			RoleName = u.RoleName,
			RoleId = u.RoleId,
			UpdatedAt = u.UpdatedAt,
			CreatedAt = u.CreatedAt
		}).ToListAsync();

		// Return paginated result
		return new PaginatedResult<UserView>
		{
			Items = items,
			TotalItems = totalItems,
			TotalPages = totalPages
		};

	}

	public async Task<UserView> GetById(int id)
	{
		var query = await (from u in _context.Users
						   join v in _context.Roles
						   on u.RoleId equals v.Id
						   where u.UserId == id
						   select new UserView
						   {
							   UserId = u.UserId,
							   FirstName = u.FirstName,
							   LastName = u.LastName,
							   Username = u.UserName,
							   Email = u.Email,
							   IsActive = u.IsActive,
							   RoleId = u.RoleId,
							   RoleName = v.RoleName,
							   UpdatedAt = u.UpdatedAt,
							   CreatedAt = u.CreatedAt,
						   }).FirstOrDefaultAsync();
		return query;
	}

	public async Task<UserView> GetByEmail(string email)
	{

		var query = await (from u in _context.Users
						   where u.Email == email
						   select new UserView
						   {
							   UserId = u.UserId,
							   FirstName = u.FirstName,
							   LastName = u.LastName,
							   Username = u.UserName,
							   Email = u.Email,
							   IsActive = u.IsActive,
							   RoleId = u.RoleId,
							   UpdatedAt = u.UpdatedAt,
							   CreatedAt = u.CreatedAt,
						   }).FirstOrDefaultAsync();
		return query;
	}

	public async Task<UserView> DeleteById(int id)
	{
		// Fetch the user from the database
		User a = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

		// Check if the user exists
		if (a == null)
		{
			// Handle user not found case (throw exception or return null)
			return null; // Or throw an exception
		}
		else
		{
			var userView = new UserView
			{
				UserId = a.UserId,
				FirstName = a.FirstName,
				LastName = a.LastName,
				Username = a.UserName,
				Email = a.Email,
				IsActive = false,
				RoleId = a.RoleId,
				UpdatedAt = a.UpdatedAt,
				CreatedAt = a.CreatedAt,
			};
			_context.Users.Update(a);
			_context.SaveChanges();
			return userView;

		}

	}

	public AuthenticateView Authenticate(UserLoginView userLoginView)
	{
		AuthenticateView authenticateView = new AuthenticateView();
		if (userLoginView != null)
		{
			var query = _context.Users.Where(x => x.Email == userLoginView.Email).FirstOrDefault();
			if (query != null)
			{
				var encrptPWD = CommonUtillity.EncryptData(userLoginView.Password);
				if (query.PasswordHash == encrptPWD)
				{
					var queryrole = _context.Roles.Where(x => x.Id == query.RoleId).FirstOrDefault();
					authenticateView.Id = query.UserId;
					authenticateView.Email = userLoginView.Email;
					authenticateView.FirstName = query.FirstName;
					authenticateView.LastName = query.LastName;
					authenticateView.UserName = query.UserName;
					authenticateView.RoleId = query.RoleId;
					authenticateView.RoleName = queryrole.RoleName;
					authenticateView.Status = "Successfully";
				}
				else
				{
					authenticateView.Email = userLoginView.Email;
					authenticateView.Status = "Password not Matched";
				}
			}
			else
			{
				authenticateView.Email = userLoginView.Email;
				authenticateView.Status = "Email Id not Exists";
			}
		}

		return authenticateView;
	}

}*/

