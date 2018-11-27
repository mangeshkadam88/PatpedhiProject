using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.Account;
using PatPedhi.Core.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace Patpedhi.Web.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService) : base(userManager, signInManager, userProfileService)
        {

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(returnUrl))
            {
                ViewData["ReturnUrl"] = "/UserProfiles/";
            }
            ViewData["AccountAlias"] = "login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model, string returnUrl = null)
        {
            ViewData["AccountAlias"] = "login";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
            //}
            if (result.Succeeded)
            {                
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(AccountController.SignIn));
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewData["AccountAlias"] = "register";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "ClientDaily");

                    UserProfile user_profile = await _userProfileService.CreateUserProfile(model, user.Id);
                    user.UserProfile = user_profile;
                    await _userManager.UpdateAsync(user);

                    ////await _signInManager.SignInAsync(user, isPersistent: false);
                    ////return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
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