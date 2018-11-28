using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.Account;
using Patpedhi.Web.ViewModels.DataTable;
using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Patpedhi.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserProfileController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserProfileController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService,
            RoleManager<ApplicationRole> roleManager) : base(userManager, signInManager, userProfileService)
        {
            this._roleManager = roleManager;
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
        public async Task<IActionResult> GetUsers(string role, string filter)
        {
            List<UsersDataModel> users = await _userProfileService.GetAllUsersForCurrentUser(role, _userManager, filter);
            return Json(new
            {
                iTotalRecords = users.Count,
                iTotalDisplayRecords = users.Count,
                aaData = users
            });
        }

        [HttpGet]
        [Route("UserProfiles/Save/{user_id?}")]
        public async Task<IActionResult> Save(Guid? user_id)
        {
            var current_user = await _userManager.GetUserAsync(User);
            current_user.UserProfile = await _userProfileService.GetUserProfileById(current_user.Id);
            ViewBag.loggedinuser = current_user;
            var current_user_roles = await _userManager.GetRolesAsync(current_user);
            ViewBag.loggedinuserroles = current_user_roles.Count > 0 ? current_user_roles[0] : "Role Not Found!";

            RegisterViewModel user_to_save = new RegisterViewModel();
            string mode = "insert";
            string current_role = "";
            if (user_id.HasValue)
            {
                var app_user = await _userManager.FindByIdAsync(user_id.Value.ToString());
                app_user.UserProfile = await _userProfileService.GetUserProfileById(app_user.Id);
                user_to_save.UserId = app_user.Id;
                user_to_save.Email = app_user.UserName;
                user_to_save.FirstName = app_user.UserProfile.first_name;
                user_to_save.MiddleName = app_user.UserProfile.middle_name;
                user_to_save.LastName = app_user.UserProfile.last_name;
                user_to_save.DateofBirth = app_user.UserProfile.date_of_birth.ToString("dd/MM/yyyy");
                user_to_save.AccountNo = app_user.UserProfile.account_no.HasValue ? Convert.ToInt64(app_user.UserProfile.account_no.Value) : 0;
                user_to_save.IsActive = app_user.UserProfile.is_active;
                user_to_save.IsApproved = app_user.UserProfile.is_approved;

                var app_user_roles = await _userManager.GetRolesAsync(app_user);
                user_to_save.RoleName = app_user_roles.Count > 0 ? app_user_roles[0] : "CLIENTDAILY";
                current_role = app_user_roles.Count > 0 ? app_user_roles[0] : "";

                user_to_save.ProfilePhotoURL = app_user.UserProfile.profile_photo_url;
                user_to_save.SignaturePhotoURL = app_user.UserProfile.signature_photo_url;
                mode = "update";
            }
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach (var item in _roleManager.Roles)
            {
                SelectListItem list_item = new SelectListItem();
                list_item.Text = item.Name;
                list_item.Value = item.Name;
                roles.Add(list_item);
            }

            ViewBag.CurrentRole = current_role;
            ViewBag.Roles = roles;
            ViewBag.UserToSave = user_to_save;
            ViewBag.Mode = mode;
            return View(user_to_save);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UserProfiles/Save/{user_id?}")]
        public async Task<IActionResult> Save(RegisterViewModel model, IFormFile file_profile, IFormFile file_signature, string mode, string current_role)
        {
            if (ModelState.IsValid)
            {
                bool file_profile_found = true;
                if (file_profile == null || file_profile.Length == 0) file_profile_found = false;

                bool file_signature_found = true;
                if (file_signature == null || file_signature.Length == 0) file_signature_found = false;
                                
                if (file_profile_found)
                {
                    string extension = "." + file_profile.FileName.Split('.')[file_profile.FileName.Split('.').Length - 1];
                    string fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\user_images",
                        fileName);
                    model.ProfilePhotoURL = "wwwroot\\user_images\\" + fileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file_profile.CopyToAsync(stream);
                    }
                }
                                
                if (file_signature_found)
                {
                    string extension = "." + file_signature.FileName.Split('.')[file_signature.FileName.Split('.').Length - 1];
                    string fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\user_signatures",
                        fileName);
                    model.SignaturePhotoURL = "wwwroot\\user_signatures\\" + fileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file_signature.CopyToAsync(stream);
                    }
                }

                ApplicationUser user_to_save;
                
                if (mode == "update")
                {
                    user_to_save = await _userManager.FindByIdAsync(model.UserId.ToString());
                    if (current_role != model.RoleName)
                    {
                        // it seems user has modified the role
                        // step 1 -delete existing role for that user
                        await _userManager.RemoveFromRoleAsync(user_to_save, current_role);
                        // step 2 - assign new role
                        await _userManager.AddToRoleAsync(user_to_save, model.RoleName);
                    }

                    // update existing profile
                    
                    UserProfile user_profile = await _userProfileService.UpdateUserProfile(model, user_to_save.Id);

                    TempData["SavedSuccessfully"] = "true";
                    return RedirectToAction("UserProfiles");
                }
                else
                {
                    user_to_save = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user_to_save, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user_to_save, model.RoleName);

                        UserProfile user_profile = await _userProfileService.CreateUserProfile(model, user_to_save.Id);
                        user_to_save.UserProfile = user_profile;
                        await _userManager.UpdateAsync(user_to_save);

                        TempData["SavedSuccessfully"] = "true";
                        return RedirectToAction("UserProfiles");
                    }

                    AddErrors(result);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}