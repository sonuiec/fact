using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FactFinderWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiController : ControllerBase
    {
        private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly AwarenessServices _AwarenessServices;
       private readonly long _userID;
        private readonly HttpContext _httpContext;
        private readonly JSONDataUtility _jsonData;
        private readonly long _profileId;

        int updateRows = 0;


        public apiController(ResellerBoyinawebFactFinderWebContext context, AwarenessServices awarenessServices, JSONDataUtility jSONDataUtility, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _AwarenessServices = awarenessServices;
            _jsonData = jSONDataUtility;
            
            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("UserId");
            _profileId = Convert.ToInt64(_httpContext.Session.GetString("profileId"));
            _userID = Convert.ToInt64(userIdStr);
        }

        //[Authorize]
        [HttpPost("child/addchildren")]
        public async Task<IActionResult> SaveChild([FromForm] ChildDetails ChildData)
        {

            if (string.IsNullOrEmpty(_userID.ToString()) || _userID == 0)
            {
                return Unauthorized(); // manually block if not logged in
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

             var updatedRow = await _AwarenessServices.AwarenessAddProfileChild(ChildData);
            if (updatedRow >= 0)
            {
                if (ChildData.Id == 0)
                {
                    return Ok(new { message = "Child saved successfully" });
                }
                else
                {
                    return Ok(new { message = "Child updated successfully" });
                }
            }
            else
            {
                return Ok(new { message = "Error, please contact admin." });
            }
        }

        [HttpGet("child/list")]
        public async Task<IActionResult> AwarenessListAllChild()
        {
            if (string.IsNullOrEmpty(_userID.ToString()) ||  _userID == 0)
            {
                return Unauthorized(); // manually block if not logged in
            }
            var awarenessList = await _context.TblffAwarenessChildren.Where(c => c.ProfileId == _profileId).ToListAsync();
            return Ok(awarenessList); // Automatically serialized as JSON
        }

        [HttpGet("child/get/{id}")]
        public async Task<IActionResult> GetChildById(long id)
        {
            if (string.IsNullOrEmpty(_userID.ToString()) || _userID == 0)
            {
                return Unauthorized(); // manually block if not logged in
            }
            var child = await _context.TblffAwarenessChildren.FirstOrDefaultAsync(ChildDetails => ChildDetails.Id == id && ChildDetails.ProfileId == _profileId);

            if (child == null)
            {
                return NotFound(new { message = "Child not found" });
            }
            return Ok(child); // Automatically serialized as JSON
        }

        [HttpPost("child/delete/{id}")]
        public async Task<IActionResult> DeleteChild(long id)
        {
            if (string.IsNullOrEmpty(_userID.ToString()) || _userID == 0)
            {
                return Unauthorized(); // manually block if not logged in
            }
            var child = await _context.TblffAwarenessChildren
                .FirstOrDefaultAsync(c => c.Id == id && c.ProfileId == _profileId);

            if (child == null)
            {
                return NotFound(new { message = "Child not found or unauthorized access" });
            }

            _context.TblffAwarenessChildren.Remove(child);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Child deleted successfully" });
        }


//        [HttpGet("user/{id}")]
//        public async Task<IActionResult> GetUserProfile(string id)
//        {
//            // Ensure user is authorized (replace _userID with actual session/user logic)
//            //if (string.IsNullOrEmpty(_userID.ToString()) || _userID == 0)
//            //{
//            //    return Unauthorized(new { message = "Unauthorized access" });
//            //}

//            if (string.IsNullOrWhiteSpace(id))
//            {
//                return BadRequest(new { message = "User ID is required." });
//            }
//long profileId;
//            if(id == "")
//            { 
//                return NotFound(new { message = "User not found." });
//            }
//            if (!long.TryParse(id, out profileId))
//            {
//                return BadRequest(new { message = "Invalid user ID format." });
//            }
//            var user = await _context.TblFfRegisterUsers.FindAsync(profileId);
//            if (user == null)
//            {
//                return NotFound(new { message = "User not found." });
//            }
//            if (profileId != 2)
//            {
//                return NotFound(new { message = "Unauthorized access / User not found." });
//            }



//          //  var jsonContent = await _jsonData.GetAwarenessJSON(profileId);
//         //   Console.WriteLine(jsonContent);
         
//        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API is alive!");
        }
    }
}

/*

[ApiController]
[Route("api/child")]
[HttpPost("save")]
public IActionResult SaveChild([FromForm] ChildDetails ChildData)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    if (model.Id == 0)
    {
        var awareb = await _AwarenessServices.AwarenessAddProfileChild(ChildData);
        _childService.AddChild(model);
    }
    else
    {
        var awareb = await _AwarenessServices.AwarenessAddProfileChild(ChildData);
        _childService.UpdateChild(model);
    }

    return Ok(new { message = "Success" });
}*/