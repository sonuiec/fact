using Microsoft.AspNetCore.Mvc.Rendering;

namespace FactFinderWeb.ModelsView
{

    public class WingsViewModel
    {
        public UserWings UserWings { get; set; } = new(); 
        public ApplicantDataDto ApplicantDataDto { get; set; } = new(); 
        public List<ChildrenList> ChildrenLists { get; set; } = new(); 

        public List<UserWings> UserWingsList { get; set; } = new();
        public List<UserWingsLocalStorageGoalDto> goalList { get; set; } = new();

        public List<WingsGoalSelect> WingsGoalSelect { get; set; } = new();
        public List<SelectListItem> GoalOptions { get; set; } = new List<SelectListItem>();

         public int? GoalEndYear { get; set; }
    }
    public class WingsGoalSelect
    {
        public string? GoalValue { get; set; }
        public string? GoalName { get; set; }
    }

    public class ChildrenList
    {
        public string? ChildName { get; set; }
        public string? ChildAge { get; set; }
        public string? ChildGender { get; set; }
    }


    public class ApplicantDataDto
    {
        public long profileid { get; set; }
        public DateTime dob { get; set; }
        public DateTime spouseDob { get; set; }
        public int applicantRetirement { get; set; }
        public int spouseRetirement { get; set; }
        public int applicantLifeExpectancy { get; set; }
        public int spouseLifeExpectancy { get; set; }
        public int applicantLifeExpectancyYear { get; set; }
        public int spouseLifeExpectancyYear { get; set; }
        public int goalStartYear { get; set; }
        public int goalEndYear { get; set; }
    }
    

    public class UserWings
    {
        public long Id { get; set; }

        public long ProfileId { get; set; }

        public string? GoalType { get; set; }

        public int? GoalPriority { get; set; }

        public string? GoalName { get; set; }

        public int? GoalPlanYear { get; set; }

        public int? GoalStartYear { get; set; }

        public int? GoalEndYear { get; set; }

        public int? TimeHorizon { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public byte? NewGoals { get; set; }
    }

    public class UserWingsLocalStorageGoalDto
    {
        public int? priority { get; set; }
        public string? goal { get; set; }
        public int? planYear { get; set; }
        public int? startYear { get; set; }
        public int? goalEndYear { get; set; }
        public int? timeHorizon { get; set; }
        public byte? NewGoals { get; set; }
    }
    
    public class UserWingsUI
    {
        public long Id { get; set; }
        //public long ProfileId { get; set; }
        //public int? Goalid { get; set; }

        public int priority { get; set; }
        public string? goal { get; set; }
        public int? planYear { get; set; }
        public int? startYear { get; set; }
        public int? goalEndYear { get; set; }
        public int? timeHorizon { get; set; } 
        public byte? NewGoals { get; set; }


        public long? goalId { get; set; }
    }
}