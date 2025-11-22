using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace FactFinderWeb.ModelsView.AdminMV
{
    public class AdminViewModel
    {

    }

    public class MVLoginAdmin
    {
        [Required(ErrorMessage = "Please Enter Email.")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }
    }
    public class AdminRegViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$",
   ErrorMessage = "Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, and a number. Spaces are not allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required, Phone]
        public string Mobile { get; set; }

        [Required]
        public string AdminRole { get; set; }

        [Required]
        public string Department { get; set; }
    }



    //public class AdminSecurityPageVm
    //{
    //    public FactFinderWeb.ModelsView.TblFfAdminUser Profile { get; set; }
    //    public ChangePasswordVm ChangePassword { get; set; }
    //}


    public class AdminchangepwdViewModel
    {


        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$",
   ErrorMessage = "Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, and a number. Spaces are not allowed.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$",
   ErrorMessage = "Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, and a number. Spaces are not allowed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }


    public class MVAdminProfile
    {
        public long Id { get; set; }
        public string? AdminUserEmail { get; set; }
        public string? AdminUserFullName { get; set; }
        public string? AdminUserRole { get; set; }

        public string? Mobile { get; set; }

        public string? Adminuserid { get; set; }

        public string? AdminRole { get; set; }

        public string? Department { get; set; }

        public string? AccountStatus { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Accesskey { get; set; }
    }

    public class MVUserList
    {
        public List<MVADUserDetails> UserListView { get; set; }
    }

    public class MVADUserDetails
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public string? ProfileStatus { get; set; }
        public string? Name { get; set; }
        public string? planType { get; set; }
        public string? planYear { get; set; }
        public string? email { get; set; }
        public string? mobile { get; set; }
        public string? activestatus { get; set; }
        public DateTime? createddate { get; set; }
        public string? userFile { get; set; }
        public int? advisorid { get; set; }

        public long? UserId { get; set; }

        public List<UserProfileViewModel> ProfileLst { get; set; }

        //Name planType planYear email mobile activestatus createddate
    }
}
