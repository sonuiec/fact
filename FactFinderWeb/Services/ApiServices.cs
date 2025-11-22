using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using Microsoft.AspNetCore.Mvc;

namespace FactFinderWeb.Services
{
    public class ApiServices
    {

        private ResellerBoyinawebFactFinderWebContext _context;

        public ApiServices(ResellerBoyinawebFactFinderWebContext context)
        {
            _context = context;
        }

        public async Task<List<UserProfileDto>> GetUserListData()
        {
            var UserListData = (from user in _context.TblFfRegisterUsers
                        join profile in _context.TblffAwarenessProfileDetails
                            on user.Id equals profile.ProfileId
                        select new UserProfileDto
                        {
                            UserId = user.Id,
                            FullName = profile.Name,
                            Email = profile.Email,
                            Activestatus = user.Activestatus,
                            Approvedbyadmin = user.Approvedbyadmin,
                           LockedProfile=user.Ptx // locked profile
                        }).ToList();

            return UserListData;
        }
    }
    public class UserProfileDto
    {
        public long UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Activestatus { get; set; }
        public string? Approvedbyadmin { get; set; }
        public string? LockedProfile { get; set; }
    }
}
