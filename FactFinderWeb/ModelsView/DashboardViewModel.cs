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


    public class UserProfileViewModel
    {

        public string? UId { get; set; }

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

    }

    public class AdvisorList
    {
        public string? AdvisorId { get; set; }
        public string? AdvisorName { get; set; }
    }
}
