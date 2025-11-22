using FactFinderWeb.BLL;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace FactFinderWeb.Controllers
{
    public class AdminController : AuthenticatedController
    {
        private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly AdminUserServices _AdminUserServices;
        private readonly JSONDataUtility _jsonData;
        private readonly UtilityHelperServices _utilService;
        private readonly IWebHostEnvironment _env;

        public AdminController(ResellerBoyinawebFactFinderWebContext context, AdminUserServices AdminuserServices, JSONDataUtility jSONDataUtility, UtilityHelperServices utilityHelperServices, IWebHostEnvironment env)
        {
            _context = context;
            _AdminUserServices = AdminuserServices;
            _jsonData = jSONDataUtility;
            _utilService = utilityHelperServices;
            _env = env;
        }

        [HttpGet]
        [Route("admin/logout")]
        public IActionResult Logout()
        {
            Console.WriteLine("Hello from AdminController -> Logout()");
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("login", "admin");

        }
        [HttpPost]
        [Route("admin/logout")]
        public IActionResult Logout1()
        {
            Console.WriteLine("Hello from AdminController -> Logout()");
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("login", "admin");
        }

        [HttpGet]
        [Route("admin/login")]  // Custom Route: /login
        public IActionResult Login()
        {
            MVLoginAdmin mVLogin = new MVLoginAdmin();
            return View(mVLogin);
        }
    
        [HttpPost]
        [Route("admin/login")]
        public async Task<IActionResult> Login(MVLoginAdmin mVLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(mVLogin);
            }

            MVAdminProfile user = await _AdminUserServices.AdminUserLogin(mVLogin);

            if (user != null) // Dummy validation
            {
                // Store user in session
                HttpContext.Session.SetString("AdminUserId", user.Id.ToString());
                HttpContext.Session.SetString("AdminUserFullName", user.AdminUserFullName);
                HttpContext.Session.SetString("AdminUserRole", user.AdminUserRole  );
                HttpContext.Session.SetString("AdminUserEmail", user.AdminUserEmail);
                return RedirectToAction("Dashboard", "Admin");
            }

            ViewData["Error"] = "Invalid username or password.";
            return View();
        }
        [HttpPost("admin/UpdateAdvisor")]
        public async Task<IActionResult> UpdateAdvisor(int id, int advisorid)
        {
            var user = await _context.TblFfRegisterUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.Advisorid = advisorid;
            _context.TblFfRegisterUsers.Update(user);

            await _context.SaveChangesAsync();

            var profile = await _context.TblffAwarenessProfileDetails.Where(u => u.UserId == id).ToListAsync();

            foreach (var item in profile)
            {
                item.Advisorid = advisorid;
                item.ProfileStatus = "Assign";
                _context.TblffAwarenessProfileDetails.Update(item);
            }
            
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        [HttpPost("admin/UpdateSStatus")]
        public async Task<IActionResult> UpdateStatus(int profileid, string Status)
        {
            var user = await _context.TblffAwarenessProfileDetails.FirstOrDefaultAsync(u => u.ProfileId == profileid);
            if (user == null)
            {
                return NotFound();
            }
            user.ProfileStatus = Status;
            user.Awakenstatus = "Awaken Export Ready";
            _context.TblffAwarenessProfileDetails.Update(user);

            await _context.SaveChangesAsync();

           
            return Json(new { success = true });
        }


        [HttpGet("admin/dashboard")]
        public async Task<IActionResult> Dashboard(string? search, int page = 1, int pageSize = 20)
        {
            string? AdminUserFullName = HttpContext.Session.GetString("AdminUserFullName");
            int AdminUserId = Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0");
            var AdminUserRole = HttpContext.Session.GetString("AdminUserRole");
            AdminUserRole = string.IsNullOrEmpty(AdminUserRole) ? "advisor" : AdminUserRole;

            if (AdminUserId <= 0)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewData["userID"] = AdminUserId;
            ViewData["Username"] = AdminUserFullName;
            var (users, totalRecords, totalPages) = await _AdminUserServices.GetUserListAsync(AdminUserRole, AdminUserId,page,pageSize, search);

            // Optional: search filteringAdvisoridAdvisorid
        
          

            var adminUsers = await (from a in _context.TblFfAdminUsers where a.AdminRole == "admin" select a).ToListAsync();
                         


            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["Search"] = search;
            ViewData["AdminUsersDropdown"] = adminUsers;

            return View(users);
        }


        [HttpGet]
        [Route("admin/ProfileDashboard/")]
        public async Task<IActionResult> ProfileDashboard()
        { 
            string? AdminUserId = HttpContext.Session.GetString("AdminUserId");
            long AdminID = Convert.ToInt64(AdminUserId);
            var adminuser = await _context.TblFfAdminUsers.FindAsync(AdminID);
            if (adminuser == null)
                return NotFound("User not found.");


            TblFfAdminUser adminuserdata = await _AdminUserServices.GetAdminUserDetail(AdminID);

            return View(adminuserdata);
        }

        [HttpPost]
        [Route("admin/ProfileDashboard/")]
        public async Task<IActionResult> ProfileDashboard(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
            {
                ViewData["msg"] = "Passwords do not match.";
                return RedirectToAction("ProfileDashboard");
            }

            string? AdminUserId = HttpContext.Session.GetString("AdminUserId");
            long AdminID = Convert.ToInt64(AdminUserId);


            var adminUser = await _context.TblFfAdminUsers.FindAsync(AdminID);
            if (adminUser == null)
                return NotFound("User not found."); 

            int updaterow = await _AdminUserServices.AdminChangepwd(AdminID,currentPassword, newPassword);
            if (updaterow > 0)
            {
                ViewData["msg"] = "Password updated successfully.";
            }
            else
            {
                ViewData["msg"] = "Failed to update password.";
            }
            TblFfAdminUser adminuserdata = await _AdminUserServices.GetAdminUserDetail(AdminID);

            return View(adminuserdata);
        }




        [HttpGet]
        public IActionResult AddRegisterAdmin()
        {
            var AdminUserRole = HttpContext.Session.GetString("AdminUserRole");
            AdminUserRole = string.IsNullOrEmpty(AdminUserRole) ? "advisor" : AdminUserRole;
            if (AdminUserRole.ToLower() == "advisor")
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRegisterAdmin(AdminRegViewModel mVLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(mVLogin);
            }
            var AdminUserRole = HttpContext.Session.GetString("AdminUserRole");
            AdminUserRole = string.IsNullOrEmpty(AdminUserRole) ? "advisor" : AdminUserRole;
            if (AdminUserRole.ToLower() != "superadmin")
            {
                return RedirectToAction("Dashboard");
            }
            string UserEmail = _AdminUserServices.checkEmailExist(mVLogin.Email);

            if (UserEmail == null)
            {
                var newRegister = new TblFfAdminUser
                {
                    Name = mVLogin.Name,
                    Email = mVLogin.Email,
                    Password = mVLogin.Password, // Hash the password in a real application
                    Mobile = mVLogin.Mobile,
                    AdminRole = mVLogin.AdminRole,
                    Department = mVLogin.Department,
                    AccountStatus ="Active",
                    Adminuserid = mVLogin.Mobile,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                Int64 UserID = await _AdminUserServices.AdminUserAdd(newRegister);

                if (UserID > 0)
                {
                    ViewData["Error"] = "New Admin User Added";
                    string weburl = _utilService.webAppURL();
                    await _utilService.SendEmailAsync(toEmail: "agile1021@gmail.com",//mVLogin.Email,// "user@example.com",
                      subject: "Welcome Team",
                     templatePath: Path.Combine(_env.WebRootPath, "emailtemplates", "SignupSuccessTemplate.html"),
                      //templatePath: Path.Combine(_env.WebRootPath, "EmailTemp/ForgotPasswordTemplate.html"),
                      placeholders: new Dictionary<string, string>
                      {
                            { "UserName", mVLogin.Name}, 
                            { "ResetLink", weburl + "/admin/login" },
                                { "LoginUrl", weburl+ "/admin/login"  },
                                 { "Email", mVLogin.Email},
                                 { "Password", mVLogin.Password},

                      });
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    string error = "<b>We're sorry!...</b><br/>Errors occurred. Please correct them and try again.<br/><br/>";
                    ViewData["Error"] = error;
                    return BadRequest(new { message = error });
                }
            }
            else
            {
                ViewData["Error"] = "Email already exists.";
                return View(mVLogin);
            } 
            //return View(mVLogin);
        }


        [HttpGet]
        [Route("admin/adminTeamlist")]
        public async Task<IActionResult> adminTeamlist()
        {
            string? AdminUserFullName = HttpContext.Session.GetString("AdminUserFullName");
            string? AdminUserId = HttpContext.Session.GetString("AdminUserId");

            if (string.IsNullOrEmpty(AdminUserFullName))
            {
                return RedirectToAction("admin/Login");
            }

            var adminUserList = await _AdminUserServices.GetAdminList();
            ViewData["userID"] = AdminUserId;

            ViewData["Username"] = AdminUserFullName;
            return View(adminUserList);
        }



        [HttpGet]
        [Route("admin/adminuserdata/{id}")]
        public async Task<IActionResult> adminuserdata(long id)
        {
            if (id <= 0)
                return BadRequest("Invalid user ID.");

            var adminuser = await _context.TblFfAdminUsers.FindAsync(id);
            if (adminuser == null)
                return NotFound("User not found.");


            TblFfAdminUser adminuserdata = await _AdminUserServices.GetAdminUserDetail(id);

            return View(adminuserdata);
        }

        [HttpPost]
        [Route("admin/adminuserdata/{id}")]
        public async Task<IActionResult> adminuserdata(TblFfAdminUser userprofile)
        {
            if (!ModelState.IsValid)
            {
                return View(userprofile);
            }

            if (userprofile.Id <= 0)
                return BadRequest("Invalid user ID.");

            var user = await _context.TblFfAdminUsers.FindAsync(userprofile.Id);
            if (user == null)
                return NotFound("User not found.");


            int updaterow = await _AdminUserServices.UpdateAdminUserDetail(userprofile);
            if (updaterow > 0)
            {
                ViewData["msg"] = "User details updated successfully.";
            }
            else
            {
                ViewData["msg"] = "Failed to update user details.";
            }
            return View(userprofile);
        }



        [HttpGet]
        [Route("admin/forgotpassword")]  // Custom Route: /forgotpassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("admin/forgotpassword")]
        public IActionResult ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View();

        }


        //[HttpGet]
        //[Route("/admin/UserProfileJSON")]
        //public async Task<IActionResult> UserProfileJSON(long id)
        //{
        //    if (id <= 0)
        //        return BadRequest("Invalid user ID.");

        //    var user = await _context.TblFfRegisterUsers.FindAsync(id);
        //    if (user == null)
        //        return NotFound("User not found.");

        //    var jsonContent = await _jsonData.GetAwarenessJSON(id);

        //    var fileName = $"UserProfile_{id}.json";
        //    var fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonContent);

        //    return File(fileBytes, "application/json", fileName);
        //}

        [HttpGet]
        [Route("/admin/UserProfilePDF")]
        public async Task<IActionResult> UserProfilePDF(long id)
        {
            long? userid = id;
            int AdminUserId = Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0");
            string AdminUserRole = HttpContext.Session.GetString("AdminUserRole");
            AdminUserRole = string.IsNullOrEmpty(AdminUserRole) ? "advisor" : AdminUserRole;

            if (AdminUserId <= 0)
                return RedirectToAction("Login", "Admin");

            if (userid <= 0)
                return BadRequest("Invalid user ID.");

            var awarenessProfileDetail = _context.TblffAwarenessProfileDetails
                .FirstOrDefault(u => u.ProfileId == userid);

            if (awarenessProfileDetail == null)
                return NotFound("User not found.");
            var awarenessusers = _context.TblFfRegisterUsers
           .FirstOrDefault(u => u.Id == awarenessProfileDetail.UserId);

            HttpContext.Session.SetString("UserFullName", awarenessusers.Name);
            HttpContext.Session.SetString("Useremail", awarenessusers.Email);
            HttpContext.Session.SetString("UserStep", "S1");
            HttpContext.Session.SetString("UserPlan", awarenessProfileDetail.PlanType);
            HttpContext.Session.SetString("profileId", awarenessProfileDetail.ProfileId.ToString());
            HttpContext.Session.SetString("UserId", awarenessProfileDetail.UserId.ToString());
            HttpContext.Session.SetString("RegisterId", awarenessProfileDetail.Registerid.ToString());


            string planType = awarenessProfileDetail.PlanType;
            ViewData["msg"] = "";
            ViewData["Error"] = "";

            return RedirectToAction("userprofile", "User", new { id = awarenessProfileDetail.ProfileId });




            return RedirectToAction("Dashboard", "Admin");
        }


        [HttpGet]
        [Route("admin/editUserPlan/{ProfileId}")]
        public IActionResult editUserPlan(long ProfileId)
        {
            int AdminUserId = Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0");
            string AdminUserRole = HttpContext.Session.GetString("AdminUserRole");
            AdminUserRole = string.IsNullOrEmpty(AdminUserRole) ? "advisor" : AdminUserRole;

            if (AdminUserId <= 0)
                return RedirectToAction("Login", "Admin");

            if (ProfileId <= 0)
                return BadRequest("Invalid user ID.");

            var awarenessProfileDetail = _context.TblffAwarenessProfileDetails 
                .FirstOrDefault(u => u.ProfileId == ProfileId);


            if (awarenessProfileDetail == null)
                return NotFound("User not found.");

            var awarenessusers= _context.TblFfRegisterUsers
                .FirstOrDefault(u => u.Id == awarenessProfileDetail.UserId);

            HttpContext.Session.SetString("UserFullName", awarenessusers.Name);
            HttpContext.Session.SetString("Useremail", awarenessusers.Email);
            HttpContext.Session.SetString("UserStep", "S1");
            HttpContext.Session.SetString("UserPlan", awarenessProfileDetail.PlanType);
            HttpContext.Session.SetString("profileId", awarenessProfileDetail.ProfileId.ToString());
            HttpContext.Session.SetString("UserId", awarenessProfileDetail.UserId.ToString());
            HttpContext.Session.SetString("RegisterId", awarenessProfileDetail.Registerid.ToString());

            string planType = awarenessProfileDetail.PlanType;

            ViewData["msg"] = "";
            ViewData["Error"] = "";

           
                return RedirectToAction("Awareness", planType,  new { id = CryptoHelper.Encrypt(awarenessProfileDetail.ProfileId) });

            //if (awarenessProfileDetail.Advisorid == AdminUserId)
            //    return RedirectToAction("Awareness", planType);

            return RedirectToAction("Dashboard", "Admin");
        }
        [HttpGet]
        [Route("/admin/UserDetails/{id}")]
        public async Task<IActionResult> UserDetails(long id)
        {
            if (Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0") <= 0)
                return RedirectToAction("Login", "Admin");

            if (id <= 0)
                return BadRequest("Invalid user ID.");

            if (await _context.TblFfRegisterUsers.FindAsync(id) == null)
                return NotFound("User not found.");

            UserProfileViewModel userdata = await _AdminUserServices.GetUserDetail(id);
            userdata.AdvisorListOptions = await _AdminUserServices.GetAdvisorList();

            return View(userdata);
        }


        [HttpPost]
      
        [Route("/admin/UserDetails/{id}")]
        public async Task<IActionResult> UserDetails(UserProfileViewModel userprofile)
        {
            if (Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0") <= 0)
                return RedirectToAction("Login", "Admin");

            if (!ModelState.IsValid)
            {
             
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                
                return View(userprofile);
            }

            if (userprofile.Id <= 0)
                return BadRequest("Invalid user ID.");

            if (await _context.TblFfRegisterUsers.FindAsync(userprofile.Id) == null)
                return NotFound("User not found.");

            string AdminUserRole = HttpContext.Session.GetString("AdminUserRole");
            AdminUserRole = string.IsNullOrEmpty(AdminUserRole) ? "advisor" : AdminUserRole;

            int updaterow = await _AdminUserServices.UserUpdate(userprofile);

            if (updaterow > 0)
                ViewData["msg"] = "User details updated successfully.";
            else
                ViewData["msg"] = "Failed to update user details.";

            userprofile.AdvisorListOptions = await _AdminUserServices.GetAdvisorList();

            return View(userprofile);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ChangePassword(long id, string currentPassword, string newPassword)
        //{
        //    if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
        //    {
        //        ViewData["msg"] = "Passwords do not match.";
        //        return RedirectToAction("Dashboard", new { id });
        //    }

        //    // Optional: add server-side strength checks that mirror the pattern rule
        //    // if (!Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$")) { ... }

        //    var ok = _AdminUserServices.AdminChangepwd(id, currentPassword, newPassword);
        //    ViewData["msg"] = ok ? "Password updated successfully." : "Current password is incorrect.";
        //    return RedirectToAction("Dashboard", new { id });
        //}


        [HttpGet]
        [Route("/admin/register")]  // Custom Route: /register
        public IActionResult register()
        {
            if (Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0") <= 0)
                return RedirectToAction("Login", "Admin");

            ViewData["Error"] = "";
            //return View();

            var model = new MVLoginRegister();
            return View(model);
        }

        [HttpPost]
        [Route("/admin/register")]
        public async Task<IActionResult> register(MVLoginRegister mVLogin)
        {
            if (Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0") <= 0)
                return RedirectToAction("Login", "Admin");

            if (!ModelState.IsValid)
            {
                return View(mVLogin);
            }
            string UserEmail = _AdminUserServices.checkUseEmailExist(mVLogin.Email);
           int AdminUserId= Convert.ToInt32(HttpContext.Session.GetString("AdminUserId") ?? "0");

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

                TblffAwarenessProfileDetail userProfile = await _AdminUserServices.UserAdds(newRegister, AdminUserId);
                if (userProfile.UserId > 0)
                {
                      

                    string weburl = _utilService.webAppURL();
                    await _utilService.SendEmailAsync(
                    toEmail: mVLogin.Email,// "user@example.com",
                    subject: "SignUp Successfully - FactFinder",
                    templatePath: Path.Combine(_env.WebRootPath, "emailtemplates", "SignupSuccessTemplateAdmin.html"),
                    placeholders: new Dictionary<string, string>
                    {
                                { "UserName", mVLogin.Name},
                                { "LoginUrl", weburl+"/login/"  },
                                 { "Email", mVLogin.Email},
                                 { "Password", mVLogin.Password},

                    });
                    //<p>Please click this link to open the fact finder-</p>
                    mVLogin = new MVLoginRegister();
                    //return Ok(new { message = "Success" });
                    HttpContext.Session.SetString("UserFullName", userProfile.Name);
                    HttpContext.Session.SetString("Useremail", userProfile.Email);
                    HttpContext.Session.SetString("UserStep", "S1");
                    HttpContext.Session.SetString("UserPlan", userProfile.PlanType);
                    HttpContext.Session.SetString("profileId", userProfile.ProfileId.ToString());
                    HttpContext.Session.SetString("UserId", userProfile.UserId.ToString());
                    HttpContext.Session.SetString("RegisterId", userProfile.Registerid.ToString());

                    return RedirectToAction("Awareness", userProfile.PlanType.ToLower(), new { id = CryptoHelper.Encrypt(userProfile.ProfileId) });
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
        

       
    }
}