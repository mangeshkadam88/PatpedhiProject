using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.DataTable;
using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Patpedhi.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserProfileController : BaseController
    {
        public UserProfileController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService) : base(userManager, signInManager, userProfileService)
        {

        }

        [Route("UserProfiles/")]        
        public async Task<IActionResult> UserProfiles()
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUsers(string role)
        {
            List<UsersDataModel> users = await _userProfileService.GetAllUsersForCurrentUser(role, _userManager);
            return Json(new
            {
                iTotalRecords = users.Count,
                iTotalDisplayRecords = users.Count,
                aaData = users
            });
        }
    }
}