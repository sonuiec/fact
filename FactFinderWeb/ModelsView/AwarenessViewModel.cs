using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FactFinderWeb.ModelsView
{
    public class AwarenessViewModel
    {
        public ProfileDetail ProfileDetail { get; set; } = new();

        public SpouseDetails SpouseDetails { get; set; } = new(); // only if married

        public List<ChildDetails> ChildrenDetails { get; set; } = new();
        public ChildDetails ChildrenDetail { get; set; } = new();
        public Assumptions Assumptions { get; set; } = new();
        //public MaritalDetails MaritalDetails { get; set; } = new();
        //public FamilyFinancial FamilyFinancial { get; set; } = new();
    }

    public class ProfileDetail
    {
        [Required(ErrorMessage ="Please select plan type.")]
        public string PlanType { get; set; }

        public string? PlanYear { get; set; }

        [Required(ErrorMessage = "Please select plan duration.")]        
        public string? PlanDuration { get; set; } 
    
        [Required(ErrorMessage ="Please enter name.")]
        public string? Name { get; set; }

        [Required(ErrorMessage ="Please select gender.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage ="Please select date of birth.")]
        [DataType(DataType.Date)]
        public string? Dob { get; set; }

        [Required(ErrorMessage ="Please enter phone number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid phone number.")]
        public string? Phone { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid phone number.")]
        public string? AltPhone { get; set; }

        //[Required(ErrorMessage ="Please Enter Aadhaar Number.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Enter a valid 12-digit aadhaar number.")]
        public string? Aadhaar { get; set; }

        [EmailAddress (ErrorMessage ="Please enter valid email.")]
        [Required(ErrorMessage ="Please enter email.")]
        public string? Email { get; set; }

   
        public string? SecEmail { get; set; }

        [Required(ErrorMessage = "Please enter pan.")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid pan format.")]
        public string PAN { get; set; }


        [Required(ErrorMessage = "Please select marital status.")]
        public string? MaritalStatus { get; set; } // married, unmarried, divorced, singleParent

        [ValidateNever]
        public string? HaveChildren { get; set; } // yes or no

        [Required(ErrorMessage = "Please select occupation.")]
        public string? Occupation { get; set; }

        //[Required(ErrorMessage = "Please Enter Educations Details.")]
        public string? EduDetails { get; set; }


        //[Required(ErrorMessage = "Please Enter Hobbies.")]
        public string? Hobbies { get; set; } = null;

        [RegularExpression("^[a-zA-Z0-9 .,\\-#]*$", ErrorMessage = "Please enter valid company name. Use only letters, numbers, and punctuation like .,-#")]
        public string? CompanyName { get; set; } = null;

        [RegularExpression("^[a-zA-Z0-9 .,\\-#]*$", ErrorMessage = "Use only letters, numbers, and punctuation like .,-#")]
        public string? CompanyAddress { get; set; } = null;
        public string? CompanyCity { get; set; } = null;
        public string? CompanyState { get; set; } = null;
        public string? CompanyCountry { get; set; } = null;
        [RegularExpression("^[0-9]*$", ErrorMessage = "Use only valid pin code.")]
        public string? CompanyPINcode { get; set; } = null;

        public string? ResAddress { get; set; }
        public string? ResCity { get; set; }
        public string? ResState { get; set; }
        public string? ResCountry { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Use only valid pin code.")]
        public string? ResPincode { get; set; }
        public bool IsSameAddress { get; set; }

        public string? PermAddress { get; set; }
        public string? PermCity { get; set; }
        public string? PermState { get; set; }
        public string? PermCountry { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Use only valid pin code.")]
        public string? PermPincode { get; set; }

        public string? Stock { get; set; }
        public string? Income { get; set; }
        public string? Payment { get; set; }
        public string? Holiday { get; set; }
        public string? Shopping { get; set; }
    }

    public class Address
    {
        [ValidateNever]
        public Location Residential { get; set; } = new();
        
        [ValidateNever]
        public Location Permanent { get; set; } = new();
        
        [ValidateNever]
        public bool IsSameAddress { get; set; }
    }

    public class Location
    {
        //[Required]
        public string Address { get; set; } 

        public string Country { get; set; } 
        public string CountryCode { get; set; } 
        public string State { get; set; } 
        public string StateCode { get; set; }
        public string City { get; set; } 

        //[Required]
        //[RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit PIN code")]
        public string PinCode { get; set; } 
    }

    public class OfficeLocation : Location
    {
        public string Company { get; set; }
    }

    public class FamilyFinancial
    {
        [Required(ErrorMessage = "Please select investment in stock.")]
        public string Stock { get; set; }
        
        [Required(ErrorMessage = "Please select saving from income.")]
        public string Income { get; set; }

        [Required(ErrorMessage = "Please select digital payment.")]
        public string Payment { get; set; }

        [Required(ErrorMessage = "Please select go for holiday.")]
        public string Holiday { get; set; }

        [Required(ErrorMessage = "Please select go for shopping.")]
        public string Shopping { get; set; }
    }

    public class MaritalDetails
    {
       
    }
    public class SpouseDetails
    {
        [Required(ErrorMessage = "Please enter name.")]
        public string? SpouseName { get; set; }

        [Required(ErrorMessage = "Please select gender.")]
        public string? SpouseGender { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select date of birth.")]
        public string? SpouseDob { get; set; }

        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid phone number.")]
        public string? SpousePhone { get; set; }

        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid phone number.")]
        public string? SpouseAltPhone { get; set; }

        [RegularExpression(@"^\d{12}$")]
        public string? SpouseAadhaar { get; set; }

        [EmailAddress (ErrorMessage ="Please enter valid email.")]
        public string? SpouseEmail { get; set; }

        [EmailAddress (ErrorMessage ="Please enter valid email.")]
        public string? SpouseSecEmail { get; set; }

        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid pan format.")]
        public string? SpousePan { get; set; }
        
        [Required(ErrorMessage = "Please select occupation.")]
        public string? SpouseOccupation { get; set; }

        [RegularExpression("^[a-zA-Z0-9 .,\\-#]*$", ErrorMessage = "Please enter valid company name. Use only letters, numbers, and punctuation like .,-#")]
        public string? SpouseCompanyName { get; set; } = null;

        [RegularExpression("^[a-zA-Z0-9 .,\\-#]*$", ErrorMessage = "Use only letters, numbers, and punctuation like .,-#")]
        public string? SpouseCompanyAddress { get; set; } = null;
        public string? SpouseCompanyCity { get; set; } = null;
        public string? SpouseCompanyState { get; set; } = null;
        public string? SpouseCompanyCountry { get; set; } = null;

        [RegularExpression("^[0-9]*$", ErrorMessage = "Use only valid pin code.")]
        public string? SpouseCompanyPINcode { get; set; } = null;
    }

    public class ChildDetails
    {
        public long? Id { get; set; }
        
        [Required(ErrorMessage = "Please enter name.")]
        public string? ChildName { get; set; }

        [Required(ErrorMessage = "Please select gender.")]
        public string? ChildGender { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select date of birth.")]
        public string? ChildDob { get; set; }

        public string? ChildPhone { get; set; }

        public string? ChildAadhaar { get; set; }

        public string? ChildEmail { get; set; }

        public string? ChildPan { get; set; }/**/
    }

    public class Assumptions
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }

        [Required(ErrorMessage = "Please enter assumptions in equity.")]
        public int Equity { get; set; }
        
        [Required(ErrorMessage = "Please enter assumptions in debt.")]
        public int Debt { get; set; }


        [Required(ErrorMessage = "Please enter assumptions in gold.")]
        public int Gold { get; set; }

        [Required(ErrorMessage = "Please enter assumptions in real estate.")]
        public int RealEstateReturn { get; set; }

        [Required(ErrorMessage = "Please enter assumptions in Liquid fund.")]
        public int LiquidFunds { get; set; }

        [Required(ErrorMessage = "Please enter assumptions in inflation rates.")]
        public int InflationRates { get; set; }

        [Required(ErrorMessage = "Please enter assumptions in education inflation.")]
        public int EducationInflation { get; set; }

        [Required(ErrorMessage = "Please enter applicant retirement age.")]
        public int? ApplicantRetirement { get; set; }

        [Required(ErrorMessage = "Please enter applicant life expectancy age.")]
        public int? ApplicantLifeExpectancy { get; set; }

        //[Required(ErrorMessage = "Please Enter.")]
        [Required(ErrorMessage = "Please enter spouse retirement age.")]
        public int? SpouseRetirement { get; set; }

        [Required(ErrorMessage = "Please enter spouse life expectancy age.")]
        public int? SpouseLifeExpectancy { get; set; }
    }
}
