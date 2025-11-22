using FactFinderWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using Microsoft.Data.SqlClient;
using System.Net;


using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Security.Cryptography;



namespace FactFinderWeb.Services
{
	public class UtilityHelperServices
	{

		private readonly ResellerBoyinawebFactFinderWebContext _context;
		private readonly long _userID;
		private readonly HttpContext _httpContext;
        private readonly IConfiguration _config;
        private static readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
		public UtilityHelperServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor, IConfiguration config)
		{
			_context = context;
			_httpContext = httpContextAccessor.HttpContext;
			var userIdStr = _httpContext.Session.GetString("UserId");
			_userID = Convert.ToInt64(userIdStr);
            _config = config;
        }

		public async Task<string> GetUserPlanType()
		{
			var userPlanType = await _context.TblFfRegisterUsers
				.Where(u => u.Id == _userID)
				.Select(u => u.Plantype)
				.FirstOrDefaultAsync();

			return userPlanType;
		}

		public async Task<string> emailOnForgotPassword()
		{
			var userEmail = await _context.TblFfRegisterUsers
				.Where(u => u.Id == _userID)
				.Select(u => u.Email)
				.FirstOrDefaultAsync();

			return userEmail;
		}

		public async Task<string> emailOnSubmitForm()
		{
			var userEmail = await _context.TblFfRegisterUsers
				.Where(u => u.Id == _userID)
				.Select(u => u.Email)
				.FirstOrDefaultAsync();

			return userEmail;
		}

		public async Task<string> emailWelcomeOnSignUp()
		{
			var userEmail = await _context.TblFfRegisterUsers
				.Where(u => u.Id == _userID)
				.Select(u => u.Email)
				.FirstOrDefaultAsync();

			return userEmail;
		}


        public static string GenerateSecureToken(int length = 32)
        {
            var tokenBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes)
                         .Replace("+", "")
                         .Replace("/", "")
                         .Replace("=", ""); // optional: remove URL-unsafe chars
        }

        public static string PasswordHash(string plainTextPassword)
		{
			return _passwordHasher.HashPassword(null, plainTextPassword);
			//for use string hashed = UtilHelperServices.HashPassword("MySecurePassword123");
		}

		public static bool PasswordVerify(string hashedPassword, string plainTextPassword)
		{
			var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainTextPassword);
			return result == PasswordVerificationResult.Success;
			// verify bool isValid = UtilHelperServices.VerifyPassword(storedHashedPassword, enteredPassword);
		}

		public static string GenerateRandomPassword(int length)
		{
			const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			char[] chars = new char[length];
			Random random = new Random();
			for (int i = 0; i < length; i++)
			{
				chars[i] = validChars[random.Next(validChars.Length)];
			}
			return new string(chars);
		}

        public  string webAppURL()
        {            
			//string appURL = _config.GetConnectionString();
            //return new string(chars);
 				string appURL= _config["AppSettings:WebAppURL"];
			return appURL;
        }


        public string GenerateJSONfile(string fileBytes, string nmmm, string mobbb)
		{
			string ftpFolder = "ftp://fact-finder.mainstream.co.in/httpdocs/memprf/";
			string ftpFolderx = "https://fact-finder.mainstream.co.in/memprf/";
			byte[] bytes = Convert.FromBase64String(fileBytes);
			try
			{
				//Create FTP Request.
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFolder + nmmm + "data" + ".xlsx");
				request.Method = WebRequestMethods.Ftp.UploadFile;

				//Enter FTP Server credentials.
				request.Credentials = new NetworkCredential("mkt198", "yk95lP66@XfgcmkNo");
				request.ContentLength = bytes.Length;
				request.UsePassive = true;
				request.UseBinary = true;
				request.ServicePoint.ConnectionLimit = bytes.Length;
				request.EnableSsl = false;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();

				//Page page = HttpContext.Current.Handler as Page;
				//if (page != null)
				//{
				//    string error = "Payments made Successfully !!!"; error.Replace("'", "\'");
				//    ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", true);

				//}
				response.Close();
				SqlConnection con1 = new SqlConnection();


				/*
				DATABASE DB = new DATABASE();

				string sql = "update  tbl_register set  ptx = '" + ftpFolderx + nmmm + "data" + ".xlsx" + "' where mobile ='" + mobbb + "'";
				con1 = DB.getCon();
				SqlCommand cmmds = new SqlCommand(sql, con1);
				DB.ExecQry(cmmds);
				*/
			}
			catch (WebException ex)
			{
				throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
			}
			return "Email sent successfully.";
		}

		public string GenerateExcelFile()
		{
			return "Email sent successfully.";

		}




        public async Task SendEmailAsync(string toEmail, string subject, string templatePath, Dictionary<string, string> placeholders)
        {
            string body = await File.ReadAllTextAsync(templatePath);

            foreach (var item in placeholders)
            {
                body = body.Replace($"{{{{{item.Key}}}}}", item.Value);  // Replace {{Key}} in HTML
            }

            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Smtp:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }



        public string SendEmail(string mailTo, string mSubject, string mbody)
		{

			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
			string recipientEmail = "supportx@fact-finder.mainstream.co.in";
			try
			{
				MailMessage mail = new MailMessage(); // Create the email message
				//  mail.From = new MailAddress("interim@fact-finder.mainstream.co.in");
				mail.From = new MailAddress("interim@fact-finder.mainstream.co.in");
				mail.Subject = mSubject; //"Pact-finder Fiametric data relevant to Mr. " + nmmm + " " + "Contact No :" + mobbb;
				if (mSubject == null)
				{
					mail.Subject = "Fact Finder";
				}
				if (mailTo == null)
				{
					mail.To.Add("advisor@mainstream.co.in");
				}
				else
				{
					mail.To.Add(mailTo);
				}

				mail.Body = mbody;       // "Please find the attached Excel file.";

				// Set up the SMTP client // Replace with your SMTP server MaInSeRv^343
				SmtpClient smtpClient = new SmtpClient("webmail.fact-finder.mainstream.co.in");
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtpClient.Port = 587; // Replace with the port number //  smtpClient.UseDefaultCredentials=false;

				smtpClient.Credentials = new NetworkCredential("interim@fact-finder.mainstream.co.in", "Simple@123#123");
				// Replace with your SMTP credentials
				smtpClient.EnableSsl = false;
				smtpClient.Send(mail);

				return "Email sent successfully.";
			}
			catch (Exception ex)
			{
				// Log the exception and return an error message
				return "Error: " + ex.Message;
			}
		}



		public string SendEmailWithAttachment(string fileBytes, string mailTo, string mSubject, string mbody)
		{

			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
			//string recipientEmail = "supportx@fact-finder.mainstream.co.in";
			byte[] bytes = Convert.FromBase64String(fileBytes);

			try
			{
				// Decode the base64 string to byte array
				// byte[] bytes = Convert.FromBase64String(fileBytes);

				// Create a memory stream from the byte array
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					// Create the email message
					MailMessage mail = new MailMessage();
					//  mail.From = new MailAddress("interim@fact-finder.mainstream.co.in");
					mail.From = new MailAddress("interim@fact-finder.mainstream.co.in");
					mail.Subject = mSubject; //"Pact-finder Fiametric data relevant to Mr. " + nmmm + " " + "Contact No :" + mobbb;
					if (mSubject == null)
					{
						mail.Subject = "Fact Finder";
					}
					if (mailTo == null)
					{
						mail.To.Add("advisor@mainstream.co.in");
					}
					else
					{
						mail.To.Add(mailTo);
					}
					mail.Body = mbody;       // "Please find the attached Excel file.";

					// Create the attachment
					Attachment attachment = new Attachment(memoryStream, "data-pact.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
					mail.Attachments.Add(attachment);

					// Set up the SMTP client
					SmtpClient smtpClient = new SmtpClient("webmail.fact-finder.mainstream.co.in"); // Replace with your SMTP server MaInSeRv^343
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtpClient.Port = 587; // Replace with the port number
										   //  smtpClient.UseDefaultCredentials=false;
					smtpClient.Credentials = new NetworkCredential("interim@fact-finder.mainstream.co.in", "Simple@123#123");
					// Replace with your SMTP credentials
					smtpClient.EnableSsl = false;
					smtpClient.Send(mail);
					// Send the email
					//smtpClient.Send(mail);
				}

				// Return a success message
				return "Email sent successfully.";
			}
			catch (Exception ex)
			{
				// Log the exception and return an error message
				return "Error: " + ex.Message;
			}
		}


		public string SendEmailWithAttachment(string fileBytes, string nmmm, string mobbb)
		{
			string ftpFolder = "ftp://fact-finder.mainstream.co.in/httpdocs/memprf/";
			string ftpFolderx = "https://fact-finder.mainstream.co.in/memprf/";
			byte[] bytes = Convert.FromBase64String(fileBytes);
			/*
			try
			{
				
				//Create FTP Request.
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFolder + nmmm + "data" + ".xlsx");
				request.Method = WebRequestMethods.Ftp.UploadFile;

				//Enter FTP Server credentials.
				request.Credentials = new NetworkCredential("mkt198", "yk95lP66@XfgcmkNo");
				request.ContentLength = bytes.Length;
				request.UsePassive = true;
				request.UseBinary = true;
				request.ServicePoint.ConnectionLimit = bytes.Length;
				request.EnableSsl = false;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();

				//Page page = HttpContext.Current.Handler as Page;
				//if (page != null)
				//{
				//    string error = "Payments made Successfully !!!"; error.Replace("'", "\'");
				//    ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + error + "');", true);

				//}
				response.Close();
				SqlConnection con1 = new SqlConnection();

				DATABASE DB = new DATABASE();

				string sql = "update  tbl_register set  ptx = '" + ftpFolderx + nmmm + "data" + ".xlsx" + "' where mobile ='" + mobbb + "'";
				con1 = DB.getCon();
				SqlCommand cmmds = new SqlCommand(sql, con1);
				DB.ExecQry(cmmds);
				
			}
			catch (WebException ex)
			{
				throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
			}
				*/
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
			string recipientEmail = "supportx@fact-finder.mainstream.co.in";
			try
			{
				// Decode the base64 string to byte array
				// byte[] bytes = Convert.FromBase64String(fileBytes);

				// Create a memory stream from the byte array
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					// Create the email message
					MailMessage mail = new MailMessage();
					//  mail.From = new MailAddress("interim@fact-finder.mainstream.co.in");
					mail.From = new MailAddress("interim@fact-finder.mainstream.co.in");
					mail.To.Add("advisor@mainstream.co.in");
					mail.Subject = "Pact-finder Fiametric data relevant to Mr. " + nmmm + " " + "Contact No :" + mobbb;
					mail.Body = "Please find the attached Excel file.";

					// Create the attachment
					Attachment attachment = new Attachment(memoryStream, "data-pact.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
					mail.Attachments.Add(attachment);

					// Set up the SMTP client
					SmtpClient smtpClient = new SmtpClient("webmail.fact-finder.mainstream.co.in"); // Replace with your SMTP server MaInSeRv^343
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtpClient.Port = 587; // Replace with the port number
										   //  smtpClient.UseDefaultCredentials=false;
										   ////////smtpClient.Credentials = new NetworkCredential("interim@fact-finder.mainstream.co.in", "Simple@123#123"); // Replace with your SMTP credentials
					smtpClient.EnableSsl = false;
					smtpClient.Send(mail);
					// Send the email
					//smtpClient.Send(mail);
				}

				// Return a success message
				return "Email sent successfully.";
			}
			catch (Exception ex)
			{
				// Log the exception and return an error message
				return "Error: " + ex.Message;
			}
		}


	}




}
public class AppSettings
{
    public string WebAppURL { get; set; }
}
