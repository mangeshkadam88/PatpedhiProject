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
using Patpedhi.Web.ViewModels.Loan;

namespace Patpedhi.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,ClientLoan")]
    public class LoanController : BaseController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ISavingsService _savingsService;
        private readonly ILoanService _loanService;
        public LoanController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService,
            RoleManager<ApplicationRole> roleManager,
            ISavingsService savingsService, ILoanService loanService) : base(userManager, signInManager, userProfileService)
        {
            this._roleManager = roleManager;
            this._savingsService = savingsService;
            this._loanService = loanService;
        }

        [Route("UsersLoans/")]
        public async Task<IActionResult> UsersLoans()
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersByRole(string role, string filter)
        {
            List<UsersDataModel> users = await _userProfileService.GetAllClientsForCurrentUser(role, _userManager, filter, true);
            return Json(new
            {
                iTotalRecords = users.Count,
                iTotalDisplayRecords = users.Count,
                aaData = users
            });
        }

        [Route("UsersLoans/History/{user_id}")]
        public async Task<IActionResult> History(Guid user_id)
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
        public async Task<IActionResult> GetUsersLoans(string filter, Guid selected_user_id)
        {
            List<LoanPaymentModel> savings = await _loanService.GetAllLoansForSelectedUser(filter, selected_user_id);
            return Json(new
            {
                iTotalRecords = savings.Count,
                iTotalDisplayRecords = savings.Count,
                aaData = savings
            });
        }

        [Route("Loan/CalculateLoan/{user_id}/{loan_id?}")]
        public async Task<IActionResult> CalculateLoan(Guid user_id, Guid? loan_id)
        {
            var user = await _userManager.GetUserAsync(User);
            user.UserProfile = await _userProfileService.GetUserProfileById(user.Id);
            ViewBag.loggedinuser = user;
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.loggedinuserroles = roles.Count > 0 ? roles[0] : "Role Not Found!";

            var selected_user = await _userManager.FindByIdAsync(user_id.ToString());
            selected_user.UserProfile = await _userProfileService.GetUserProfileById(user_id);
            ViewBag.selecteduser = selected_user;

            string mode = "insert";
            var loan = new LoanPaymentModel();
            if (loan_id.HasValue)
            {
                loan = await _loanService.GetModelById(loan_id.Value);
                mode = "update";
            }
            ViewBag.LoanDetail = loan;
            ViewBag.Mode = mode;
            return View();
        }

        [HttpGet]
        [Route("Loan/AjaxCalculateLoan")]
        public async Task<IActionResult> AjaxCalculateLoan(string _startDate, string _principal, string _interestRate, string _period, Guid user_id)
        {
            LoanPaymentModel loan_detail = await _loanService.CalculateLoan(_startDate, _principal, _interestRate, _period, user_id, null);
            return Json(new
            {
                data = loan_detail
            });
        }
    }
}