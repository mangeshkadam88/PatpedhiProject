using Microsoft.AspNetCore.Identity;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.Account;
using PatPedhi.Core.Entities.Identity;
using PatPedhi.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Patpedhi.Web.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IAsyncRepository<UserProfile> _userProfileRepository;
        public UserProfileService(IAsyncRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfile> CreateUserProfile(RegisterViewModel model)
        {
            var user_profile = new UserProfile()
            {
                Id = new Guid(),
                first_name = model.FirstName,
                middle_name = model.MiddleName,
                last_name = model.LastName,
                date_of_birth = DateTime.ParseExact(model.DateofBirth, "dd/MM/yyyy", null),
                added_date = DateTime.Now,
                modified_date = DateTime.Now,
                is_active = true,
                is_approved = false,
                profile_photo_url = "",
                signature_photo_url = ""
            };
            await _userProfileRepository.AddAsync(user_profile);

            return user_profile;
        }
    }
}
