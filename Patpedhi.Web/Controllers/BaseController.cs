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
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly IUserProfileService _userProfileService;

        public BaseController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserProfileService userProfileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userProfileService = userProfileService;
        }
    }
}