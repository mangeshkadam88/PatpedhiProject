﻿using Microsoft.AspNetCore.Identity;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.ViewModels.Account;
using Patpedhi.Web.ViewModels.DataTable;
using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateUserProfile(RegisterViewModel userProfileModel, Guid Id);
        Task<UserProfile> GetUserProfileById(Guid Id);
        Task<List<UsersDataModel>> GetAllUsersForCurrentUser(string role, UserManager<ApplicationUser> _userManager, string filter);

        Task<UserProfile> UpdateUserProfile(RegisterViewModel userProfileModel, Guid Id);
        Task SetActiveUserProfile(Guid user_id, bool is_active);
    }
}
