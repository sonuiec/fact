using System.ComponentModel.DataAnnotations;

namespace FactFinderWeb.ModelsView
{

    public class MVLogin
    { 
        [Required]
        [EmailAddress]
		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

		[Required]
        public string Password { get; set; } = null!;
    }

	public class MVUserProfile
	{
		public long? UserId { get; set; }
        public string UserEmail { get; set; } = null!;
        
        public string? Emailverified { get; set; }

        public string UserFullName { get; set; }  
		public string Userptx { get; set; } 
		public string UserPlan { get; set; }

	}

	public class MVLoginRegister
	{
		[Required(ErrorMessage = "Please enter name.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please enter mobile number.")]
		[RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
		public string Mobile { get; set; }

		[Required(ErrorMessage = "Please enter email address.")]
		[EmailAddress]
		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
		public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$",
    ErrorMessage = "Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, and a number. Spaces are not allowed.")]
        public string Password { get; set; } = null!;


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Please select plan type.")]
		public string PlanType { get; set; }
		= string.Empty;


	}



    public class MVResetPassword
    {
        public string Email { get; set; } = null!;
        public string code { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$",
    ErrorMessage = "Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, and a number. Spaces are not allowed.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}