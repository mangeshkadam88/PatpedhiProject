using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;

namespace Patpedhi.Web.Controllers
{
    public class LoanController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ISavingsService _savingsService;
        public LoanController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService,
            RoleManager<ApplicationRole> roleManager,
            ISavingsService savingsService) : base(userManager, signInManager, userProfileService)
        {
            this._roleManager = roleManager;
            this._savingsService = savingsService;
        }

        [Route("Loan/CalculateLoan")]
        public async Task<IActionResult> CalculateLoan()
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";
            return View();
        }
    }
}