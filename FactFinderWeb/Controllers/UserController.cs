using FactFinderWeb.BLL;
using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text.Json.Nodes;

namespace FactFinderWeb.Controllers
{
    public class UserController : Controller
	{
		private readonly ResellerBoyinawebFactFinderWebContext _context;
		private readonly UserServices _UserServices;
        private readonly UtilityHelperServices _utilService;
        private readonly JSONDataUtility _jsonData;
        private readonly IWebHostEnvironment _env;
        public UserController(ResellerBoyinawebFactFinderWebContext context, UserServices userServices, UtilityHelperServices utilityHelperServices, IWebHostEnvironment env, JSONDataUtility jSONDataUtility)
        {
            _context = context;
            _UserServices = userServices;
            _utilService = utilityHelperServices;
            _env = env;
            _jsonData = jSONDataUtility;
        }

		[HttpGet]
        [Route("register")]  // Custom Route: /register
        public IActionResult register()
        {
			ViewData["Error"] = "";
			//return View();

			var model = new MVLoginRegister();
			return View(model);
		}

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> register(MVLoginRegister mVLogin)
        {
			if (!ModelState.IsValid)
			{
				return View(mVLogin);
			}
			string UserEmail =  _UserServices.checkEmailExist(mVLogin.Email);

			if (UserEmail == null)
			{
				var newRegister = new TblFfRegisterUser
				{
					Name = mVLogin.Name,
					Email = mVLogin.Email,
					Mobile = mVLogin.Mobile,
                    Password = mVLogin.Password, // Hash the password in a real application
					Plantype = mVLogin.PlanType,
                    Createddate = DateTime.Now,
					Updatedate = DateTime.Now
				};

                TblffAwarenessProfileDetail userProfile = await _UserServices.UserAdd(newRegister);
				if (userProfile.UserId > 0)
				{
                    string token = _context.TblFfRegisterUsers.Where(u => u.Id == userProfile.UserId)
                                    .Select(u => u.Emailverified).FirstOrDefault();
                    if (string.IsNullOrEmpty(token))
                    {
                        ViewData["success"] = "SignUp Successfuly. Please contact to admin for email verification.";
                    }
                    else
                    {
                        ViewData["success"] = "SignUp Successfuly. Please verify your email before login link has already been sent.";
                    }
                    
					string weburl = _utilService.webAppURL();
						await _utilService.SendEmailAsync(
						toEmail: mVLogin.Email,// "user@example.com",
						subject: "SignUp Successfully - FactFinder",
						templatePath: Path.Combine(_env.WebRootPath, "emailtemplates", "SignupSuccessTemplate.html"),
						placeholders: new Dictionary<string, string>
						{
								{ "UserName", mVLogin.Name},
                              
                                 { "Email", mVLogin.Email},
                                 { "Password", mVLogin.Password},

                                { "LoginUrl", weburl+"/emailverification/"+token  }
						});
                    //<p>Please click this link to open the fact finder-</p>
                    mVLogin = new MVLoginRegister();
							//return Ok(new { message = "Success" });
                    //return RedirectToAction("login");
                }
				else
				{
					string error = "<b>We're sorry!...</b><br/>Errors occurred. Please correct them and try again.<br/><br/>";
					ViewData["Error"] = error;
                    return BadRequest(new { message = error });
				}

				//if (mVLogin.Email == "admin" && mVLogin.Password == "password") // Dummy validation
				//{
				//	HttpContext.Session.SetString("Username", mVLogin.Email);
				//	return RedirectToAction("Dashboard");
				//}
			}
			else
            {
                ViewData["Error"] = "Email already exists.";
                return View(mVLogin);
            }
            return View(mVLogin);
        }


		[HttpGet]
		[Route("login")]  // Custom Route: /login
		public IActionResult Login()
		{
			var model= new MVLogin();
			return View(model);
		}

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(MVLogin mVLogin)
        {
            var plantype = "";

            if (!ModelState.IsValid)
            {
                return View(mVLogin);
            }
            try
            {
                MVUserProfile user = await _UserServices.UserLogin(mVLogin);

                if (user != null)
                {
                    if (user.Emailverified == "yesverified")
                    {
                        var existingProfile = _context.TblffAwarenessProfileDetails
                                           .FirstOrDefault(a => a.UserId == user.UserId && a.Registerid == user.UserId && a.ProfileStatus == "Draft");
                        // Store user in session
                        HttpContext.Session.SetString("UserFullName", user.UserFullName);
                        HttpContext.Session.SetString("Useremail", user.UserEmail);
                        HttpContext.Session.SetString("UserStep", "S1");
                        HttpContext.Session.SetString("UserPlan", user.UserPlan);
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                       
                        
                        //long userId = Convert.ToInt64(HttpContext.Session.TryGetValue["Username"]);
                        var userIdStr = HttpContext.Session.GetString("UserId");
                        long userId = userIdStr == null ? 1 : Convert.ToInt64(userIdStr);
                        plantype = user.UserPlan;

                        if (existingProfile == null)
                            return RedirectToAction("dashboard", "User");//"Comprehensive"
                        else
                        {
                            HttpContext.Session.SetString("profileId", existingProfile.ProfileId.ToString());
                            return RedirectToAction("Awareness", plantype, new { id = CryptoHelper.Encrypt(existingProfile.ProfileId) });
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Please verify your email before login. Link has already been sent to your email.";
                        return View(mVLogin);
                    }
                }
                ViewData["Error"] = "Invalid username or password.";
                return View(mVLogin);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Login error: {ex}");
                ViewData["Error"] = "An unexpected error occurred while logging in." + plantype;
                return View(mVLogin);
            }
        }

		[HttpGet]
		[Route("dashboard")]
        public async Task<IActionResult> Dashboard()
        {

            string? username = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            List<DashboardViewModel> dashboardViewModel = new List<DashboardViewModel>();

            dashboardViewModel = await  _UserServices.UserDashboard();

			// ViewData["Username"] = username;
			return View(dashboardViewModel);
        }

        //        [HttpGet]
        //        [Route("userprofile/data/{profileId}")]
        //        public async Task<IActionResult> GetUserProfileData(int profileId)
        //        {
        //            long? UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
        //            if (string.IsNullOrEmpty(Convert.ToString(UserId)))
        //            {
        //                return Unauthorized(new { message = "User not logged in." });
        //            }

        //            var user = _context.TblffAwarenessProfileDetails.Where(u =>  u.ProfileId== profileId).FirstOrDefault();
        //            if(user == null)
        //            {
        //                return Unauthorized(new { message = "User not logged in." });

        //            }

        //            var jsonContent = await _jsonData.UserGetAwarenessJSON(Convert.ToInt64(profileId));

        //            try
        //            {
        ////                var jsonObject = JsonConvert.DeserializeObject<object>(jsonContent);
        //                var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent);
        //                return Ok(jsonContent);
        //                //return Ok(jsonObject); // returns JSON properly
        //            }
        //            catch (JsonException)
        //            {
        //                return StatusCode(500, new { message = "Invalid JSON format returned by service." });
        //            }
        //        }
        [HttpGet]
        [Route("/User/userprofile/{profileId}")]
        [Route("userprofile/{profileId}")]
        public async Task<IActionResult> UserProfile(long profileId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            //if (string.IsNullOrEmpty(userIdString))
            //    return Unauthorized(new { message = "User not logged in." });

            long userId = Convert.ToInt64(userIdString);

            var user = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(u => u.ProfileId == profileId);

            if (user == null)
                return NotFound(new { message = "Profile not found." });

            // ✅ Get full structured data (class, not JSON string)
            var awakenData = await _jsonData.GetAwakenSection(profileId);

            // ✅ ASP.NET Core automatically serializes it to JSON in response
            return View(awakenData);
        }


        [HttpGet]
        [Route("/User/userprofile1/{profileId}")]
        [Route("userprofile1/{profileId}")]
        public IActionResult UserProfile1(int profileId)
        {
            string? UserId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(UserId))
            {
                return RedirectToAction("Login");
            }
            var user = _context.TblffAwarenessProfileDetails.Where(u => u.ProfileId == profileId).FirstOrDefault();
            ViewBag.profileId = profileId;
            ViewData["UserId"] = UserId;
            ViewBag.planType = user.PlanType?.ToLower(); ;

            return View(); // loads Razor view
        }

        //[HttpGet]
        //[Route("userprofile")]
        //public async Task<IActionResult> UserProfile()
        //{
        //    string? UserId = HttpContext.Session.GetString("UserId");
        //    if (string.IsNullOrEmpty(UserId))
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var jsonContent = await _jsonData.GetAwarenessJSON(Convert.ToInt64(UserId));
        //    Console.WriteLine(jsonContent);
        //    try
        //    {
        //        return new ContentResult
        //        {
        //            Content = jsonContent,
        //            ContentType = "application/json",
        //            StatusCode = 200
        //        };
        //        var jsonObject = JsonConvert.DeserializeObject<object>(jsonContent);
        //        //return new JsonResult(jsonObject);
        //    }
        //    catch (JsonException)
        //    {
        //        return StatusCode(500, new { message = "Invalid JSON format returned by service." });
        //    }



        //    ViewData["UserId"] = UserId;

        //    // ViewData["Username"] = username;
        //    return View();
        //}


        //[HttpPost]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserFullName");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserStep");
            HttpContext.Session.Remove("UserPlan");
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Login");
        }
        
		[HttpGet]
        [Route("forgotpassword")]  // Custom Route: /forgotpassword
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[Route("forgotpassword")]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			if (!ModelState.IsValid) {	
				return View();
			}
			string result = await _UserServices.ForgotPassword(email);
            string weburl = _utilService.webAppURL();
            string resetLink = "";
            //string templatePath = Path.Combine(_env.WebRootPath, "emailtemplates", "ForgotPasswordTemplate.html");
            if (result != "")
			{
                string? UserName = HttpContext.Session.GetString("UserFullName") ?? "";

                await _utilService.SendEmailAsync(
                    toEmail: email,
                    subject: "Reset Password - FactFinder",
                    templatePath: Path.Combine(_env.WebRootPath, "emailtemplates", "ForgotPasswordTemplate.html"),
                    placeholders: new Dictionary<string, string>
                    {
                            { "UserName", UserName },
                            { "ResetLink", weburl+"/resetpassword?email="+email+"&token="+result },
                            { "FormTitle", "Reset Password - FactFinder" }
                    });

                ViewData["msg"] = "An email has been sent. Please check your inbox to reset your password.";
            }
			else
			{
                ViewData["msg"] = "No account found with that email address.";
            }

            return View();

		}

        [HttpGet]
        [Route("resetpassword")]  // Custom Route: /forgotpassword
        public IActionResult ResetPassword(string email = null, string token = null)
        {
            if (email == null || token == null)
            {
                return BadRequest("A code and email must be supplied for password reset.");
            }
			var varMVResetPassword = new MVResetPassword
			{
				code = token,
				Email = email
            };
            return View(varMVResetPassword);
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(MVResetPassword mVResetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
			//string UserEmail = _UserServices.checkEmailExist(mVResetPassword.Email);
			//if (UserEmail == null)
			//{
			//	throw new Exception("User not found");
			//}

			ViewData["msg"] = await _UserServices.RequestPasswordReset(mVResetPassword);

            return View();
        }


        [HttpGet]
        [Route("emailverification/{verificationKey}")]  // Custom Route: /forgotpassword
        public IActionResult emailverification(string verificationKey = null)
        {
            if (verificationKey == null)
            {
                return BadRequest("Token is expired, Please contact to admin.");
            }
            var varMVResetPassword = new MVResetPassword
            {
                Email = verificationKey
            };


            var user= _context.TblFfRegisterUsers.Where(u => u.Emailverified == verificationKey).FirstOrDefault();

            if (user != null)
            {
                if (user == null)
                {
                    TempData["msg"] = "Invalid or expired link.";
                    return RedirectToAction("login");
                }

                if (user.Emailverified == "yesverified")
                {
                    TempData["msg"] = "Your email is already verified. You can now login.";
                    return RedirectToAction("login");
                }
                if (user.Updatedate != null && user.Updatedate.Value.AddDays(7) < DateTime.UtcNow)
                {
                    TempData["msg"] = "Verification link has expired. Please request a new one.";
                    return RedirectToAction("login");
                }
                user.Emailverified = "yesverified";
                user.Activestatus = "1";
                user.Updatedate = DateTime.Now;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["msg"] = "Your email has been verified successfully. You can now login.";
            }
            else
            {
                TempData["msg"] = "Invalid token or email already verified.";
            }

           return RedirectToAction("login");

        }
        //https://login.teamviewer.com/Cmd/ActivateAccount?lng=en&token=7c169489-e705-4808-853f-51412a15ea12

        [HttpGet]
        [Route("ResendVerificationEmail")]   
        public IActionResult ResendVerificationEmail()//string email = null
        {
            //if (email == null)
            //{
            //    return BadRequest("A code and email must be supplied for password reset.");
            //}
            //var varMVResetPassword = new MVResetPassword
            //{
            //    Email = email
            //};
            return View();
        }

        [HttpPost]
        [Route("ResendVerificationEmail")]
        public async Task<IActionResult> ResendVerificationEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //string UserEmail = _UserServices.checkEmailExist(mVResetPassword.Email);
            //if (UserEmail == null)
            //{
            //	throw new Exception("User not found");
            //}

            string token = await _UserServices.RequestResendVerificationLink(email);
            if (string.IsNullOrEmpty(token))
            {
                ViewData["msg"] = "No account found with that email address.";
                return View();
            }

            string weburl = _utilService.webAppURL();
            await _utilService.SendEmailAsync(
            toEmail: email,// "user@example.com",
            subject: "Verification Link Resend - FactFinder",
            templatePath: Path.Combine(_env.WebRootPath, "emailtemplates", "ResendEmailVerification.html"),
            placeholders: new Dictionary<string, string>
            {
                                { "LoginUrl", weburl+"/emailverification/"+token  }
            });

            ViewData["msg"] = "An email has been sent. Please check your inbox to verify your email.";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SetPlan(string planType)
        {
            if (!string.IsNullOrEmpty(planType))
            {
                HttpContext.Session.SetString("UserPlan", planType);
                long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

                // mark existing draft as deleted
             

                // create new draft
                var newProfile = new TblffAwarenessProfileDetail
                {
                    UserId = userId,
                    PlanType = planType,
                    CreateDate = DateTime.Now,
                    //UpdateDate = DateTime.Now,
                    Name = "",
                    Phone="",
                    PlanYear =DateTime.Now.Year,
                    ProfileStatus = "Draft"
                };

                _context.TblffAwarenessProfileDetails.Add(newProfile);
                await _context.SaveChangesAsync(); // ✅ MUST await

                return RedirectToRoute(new
                {
                    planType = planType,   // taken from session
                    controller = "Comprehensive",
                    action = "Awareness",
                    pid = CryptoHelper.Encrypt(newProfile.ProfileId)
                });

                // redirect to awareness with new profile id
               // return RedirectToAction("Awareness", planType,new { pid = CryptoHelper.Encrypt(newProfile.ProfileId) });
            }

            return RedirectToAction("Dashboard");
        }

    }

}
