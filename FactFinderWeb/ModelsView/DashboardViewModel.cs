using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FactFinderWeb.ModelsView
{
	public class DashboardViewModel
	{

		public string UId { get; set; }

		public string UserEmail { get; set; } = null!;

		public string? UserFullName { get; set; }
		public string? UserMobile { get; set; }
		public string? UserPlan { get; set; }
		public long UserPlanYear { get; set; }
		public string? UserEmailVerification { get; set; }
		public string? UserActiveStatus{ get; set; }

		public string? Userptx { get; set; }

		public DateTime? UserRegisterDate { get; set; }
		public string? AdvisorName { get; set; }
		public long? Advisorid { get; set; }

        public DateTime? PlanCreatedDate { get; set; }
        public DateTime? PlanUpdatedDate { get; set; }

        public long? ProfileId { get; set; }

        public string? ProfileStatus { get; set; }
    }

    public class DashboardViewModelNew
    {
        public DashboardViewModel dashboardViewModel { get; set; }
        public List<UserProfileViewModel> userProfileViewModel { get; set; }
    }
    public class RenewalListViewModel
    {
        public List<UserProfileViewModel> Users { get; set; } = new();
        public RenewalStatusCounts StatusCounts { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public string? Search { get; set; }
        public string Status { get; set; }
        public string PlanType { get; set; }
    }
    public class RenewalStatusCounts
    {
        public int Total { get; set; }
        public int Overdue { get; set; }
        public int DueSoon { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
        public int RenewalSent { get; set; }   // 👈 add this
    }
    public class StatusCountItem
    {
        public string Status { get; set; } = "";
        public int Count { get; set; }
    }

    public class UserProfileViewModel
    {
        public string? UIdText { get; set; }
        public string? UId { get; set; }
        public string? PdfPath { get; set; }
        
        public long? Id { get; set; }

        public long? ProfileId { get; set; }
        public string? Awakenstatus { get; set; }
        public string UserEmail { get; set; } = null!;

        public string? UserFullName { get; set; }
        public string? UserMobile { get; set; }
        public string? UserPlan { get; set; }
        public long UserPlanYear { get; set; }
        public string? UserEmailVerification { get; set; }

      //  [Required(ErrorMessage = "Please select account status.")]
        public string? UserActiveStatus { get; set; }

        public string? Userptx { get; set; }   //user submitted =pending or admin locked = locked

        public DateTime? UserRegisterDate { get; set; }
        
       // [Required(ErrorMessage = "Please select profile status.")]
        public string? ProfileStatus { get; set; }
        public string? AdvisorName { get; set; }
      //  [Required(ErrorMessage = "Please select advisor name.")]
        public int? Advisorid { get; set; }
        public List<AdvisorList> AdvisorListSelect { get; set; } = new();
        public List<SelectListItem> AdvisorListOptions { get; set; } = new List<SelectListItem>();

        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public string? PlanStatus { get; set; }
        public int? PlanYear { get; set; }

        public string? PlanDuration { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? DOB { get; set; }

        public int? DaysUntilRenewal { get; set; }

        public string? RenewalStatus { get; set; }
        public bool? RenewalSent { get; set; }

        public string? Status { get; set; }

        public int? RevisionNumber { get; set; }

        public bool? IsRenewal { get; set; }

    }

    public class AdvisorList
    {
        public string? AdvisorId { get; set; }
        public string? AdvisorName { get; set; }
    }
}
