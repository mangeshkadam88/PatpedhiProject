using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.DataTable;

namespace Patpedhi.Web.Controllers
{
    [Authorize(Roles = "ClientDaily,ClientNormal,ClientFD,ClientLoan")]
    public class ClientController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ISavingsService _savingsService;
        public ClientController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService,
            RoleManager<ApplicationRole> roleManager,
            ISavingsService savingsService) : base(userManager, signInManager, userProfileService)
        {
            this._roleManager = roleManager;
            this._savingsService = savingsService;
        }

        [Route("UsersSavingsHistory/")]
        public async Task<IActionResult> UsersSavingsHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrentUsersSavings(Guid selected_user_id)
        {
            List<SavingsDataModel> savings = await _savingsService.GetAllSavingsForSelectedUser(selected_user_id);
            return Json(new
            {
                iTotalRecords = savings.Count,
                iTotalDisplayRecords = savings.Count,
                aaData = savings
            });
        }
    }
}