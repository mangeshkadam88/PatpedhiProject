using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.ViewModels.Account;
using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateUserProfile(RegisterViewModel userProfileModel);
    }
}
