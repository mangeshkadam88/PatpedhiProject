using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels;
using Patpedhi.Web.ViewModels.DataTable;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patpedhi.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
    public class SavingsController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ISavingsService _savingsService;
        public SavingsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService,
            RoleManager<ApplicationRole> roleManager,
            ISavingsService savingsService) : base(userManager, signInManager, userProfileService)
        {
            this._roleManager = roleManager;
            this._savingsService = savingsService;
        }

        [Route("UsersSavings/")]
        public async Task<IActionResult> UsersSavings()
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersByRole(string role, string filter, string user_type)
        {
            List<UsersDataModel> users = await _userProfileService.GetAllUsersForCurrentUser(role, _userManager, filter, user_type);
            return Json(new
            {
                iTotalRecords = users.Count,
                iTotalDisplayRecords = users.Count,
                aaData = users
            });
        }

        [Route("Savings/List/{user_id}")]
        public async Task<IActionResult> List(Guid user_id)
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";

            var selected_user = await _userManager.FindByIdAsync(user_id.ToString());
            selected_user.UserProfile = await _userProfileService.GetUserProfileById(user_id);
            ViewBag.selecteduser = selected_user;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersSavings(string filter, Guid selected_user_id)
        {
            List<SavingsDataModel> savings = await _savingsService.GetAllSavingsForSelectedUser(filter, selected_user_id);
            return Json(new
            {
                iTotalRecords = savings.Count,
                iTotalDisplayRecords = savings.Count,
                aaData = savings
            });
        }

        [Route("Savings/Save/{selected_user_id}/{saving_id?}")]
        public async Task<IActionResult> Save(Guid selected_user_id, Guid? saving_id)
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";

            var selected_user = await _userManager.FindByIdAsync(selected_user_id.ToString());
            selected_user.UserProfile = await _userProfileService.GetUserProfileById(selected_user_id);
            ViewBag.selecteduser = selected_user;
            var selected_user_roles = await _userManager.GetRolesAsync(selected_user);
            ViewBag.selecteduserroles = selected_user_roles.Count > 0 ? selected_user_roles[0] : "Role Not Found!";


            SavingsModel saving_to_save = new SavingsModel();
            string mode = "insert";
            if (saving_id.HasValue)
            {
                saving_to_save = await _savingsService.GetModelById(saving_id.Value);
                mode = "update";
            }
            else
            {
                saving_to_save.SavingId = Guid.NewGuid();
                saving_to_save.UserId = selected_user_id;
                saving_to_save.IsApproved = false;
                saving_to_save.IsActive = true;
            }

            ViewBag.Mode = mode;
            return View(saving_to_save);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Savings/Save/{selected_user_id}/{saving_id?}")]
        public async Task<IActionResult> Save(SavingsModel model, string mode, string current_user_role, Guid current_user_id, string selected_user_role, Guid selected_user_id)
        {
            if (ModelState.IsValid)
            {
                await _savingsService.Save(model, mode, current_user_role, current_user_id, selected_user_id, selected_user_role);
                TempData["ShowNotification"] = "User data has been saved successfully!";
                return Redirect("/Savings/List/" + selected_user_id.ToString());
            }

            // If we got this far, something failed, redisplay form

            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(current_user_id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";

            var selected_user = await _userManager.FindByIdAsync(selected_user_id.ToString());
            selected_user.UserProfile = await _userProfileService.GetUserProfileById(selected_user_id);
            ViewBag.selecteduser = selected_user;
            var selected_user_roles = await _userManager.GetRolesAsync(selected_user);
            ViewBag.selecteduserroles = selected_user_roles.Count > 0 ? selected_user_roles[0] : "Role Not Found!";

            ViewBag.Mode = mode;
            return View(model);
        }

        
    }
}