using Microsoft.AspNetCore.Identity;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.Account;
using Patpedhi.Web.ViewModels.DataTable;
using PatPedhi.Core.Entities.Identity;
using PatPedhi.Core.Interfaces;
using PatPedhi.Core.Specifications;
using System;
using System.Collections.Generic;
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

        public async Task<UserProfile> CreateUserProfile(RegisterViewModel model, Guid user_id)
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
                is_approved = model.IsApproved,
                profile_photo_url = string.IsNullOrEmpty(model.ProfilePhotoURL) ? "" : model.ProfilePhotoURL,
                signature_photo_url = string.IsNullOrEmpty(model.SignaturePhotoURL) ? "" : model.SignaturePhotoURL,
                account_no = Convert.ToInt64(model.AccountNo),
                user_id = user_id
            };
            await _userProfileRepository.AddAsync(user_profile);

            return user_profile;
        }

        public async Task<UserProfile> UpdateUserProfile(RegisterViewModel model, Guid user_id)
        {
            var user_profile = await GetUserProfileById(user_id);

            user_profile.first_name = model.FirstName;
            user_profile.middle_name = model.MiddleName;
            user_profile.last_name = model.LastName;
            user_profile.date_of_birth = DateTime.ParseExact(model.DateofBirth, "dd/MM/yyyy", null);
            user_profile.modified_date = DateTime.Now;
            user_profile.is_active = model.IsActive;
            user_profile.is_approved = model.IsApproved;
            user_profile.profile_photo_url = string.IsNullOrEmpty(model.ProfilePhotoURL) ? user_profile.profile_photo_url : model.ProfilePhotoURL;
            user_profile.signature_photo_url = string.IsNullOrEmpty(model.SignaturePhotoURL) ? user_profile.signature_photo_url : model.SignaturePhotoURL;
            user_profile.account_no = Convert.ToInt64(model.AccountNo);
            await _userProfileRepository.UpdateAsync(user_profile);

            return user_profile;
        }

        public async Task<UserProfile> GetUserProfileById(Guid Id)
        {
            var filterSpecification = new UserProfileSpecifications(Id);
            var user_profile = await _userProfileRepository.GetSingleBySpecAsync(filterSpecification);
            return user_profile;
        }

        public async Task<List<UsersDataModel>> GetAllUsersForCurrentUser(string role, UserManager<ApplicationUser> _userManager, string filter, string user_type)
        {
            List<UsersDataModel> user_data_model_list = new List<UsersDataModel>();
            List<ApplicationUser> usersInRole = new List<ApplicationUser>();
            if (!string.IsNullOrEmpty(user_type))
                usersInRole.AddRange(await _userManager.GetUsersInRoleAsync(user_type));
            else
            {
                if (role == "SuperAdmin")
                    usersInRole.AddRange(_userManager.Users);
                else if (role == "Admin")
                {
                    usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("Employee"));
                    usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientDaily"));
                    usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientNormal"));
                    usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientFD"));
                    usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientLoan"));
                }
            }
            bool match_with_filter = false;
            foreach (ApplicationUser user in usersInRole)
            {
                user.UserProfile = await GetUserProfileById(user.Id);
                if (filter == "active")
                    match_with_filter = user.UserProfile.is_active;
                else if (filter == "inactive")
                    match_with_filter = !user.UserProfile.is_active;
                else
                    match_with_filter = !user.UserProfile.is_approved;

                if (match_with_filter)
                {
                    UsersDataModel udm = new UsersDataModel();
                    udm.user_id = user.Id;
                    udm.first_name = user.UserProfile.first_name;
                    udm.middle_name = user.UserProfile.middle_name;
                    udm.last_name = user.UserProfile.last_name;
                    udm.account_number = Convert.ToString(user.UserProfile.account_no);
                    udm.date_of_birth_string = user.UserProfile.date_of_birth.ToString("dd/MM/yyyy");
                    
                    var roles = await _userManager.GetRolesAsync(user);
                    string role_name = roles.Count > 0 ? roles[0] : "Role Not Found!";
                    udm.role_string = role_name;

                    udm.is_approved_string = user.UserProfile.is_approved.ToString();
                    udm.is_active_string = user.UserProfile.is_active.ToString();
                    user_data_model_list.Add(udm);
                }
            }

            return user_data_model_list;
        }

        public async Task<List<UsersDataModel>> GetAllClientsForCurrentUser(string role, UserManager<ApplicationUser> _userManager, string filter, bool loan_users)
        {
            List<UsersDataModel> user_data_model_list = new List<UsersDataModel>();
            List<ApplicationUser> usersInRole = new List<ApplicationUser>();
            if (loan_users)
                usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientLoan"));
            else
            {
                usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientDaily"));
                usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientNormal"));
                usersInRole.AddRange(await _userManager.GetUsersInRoleAsync("ClientFD"));
            }
            bool match_with_filter = false;
            foreach (ApplicationUser user in usersInRole)
            {
                user.UserProfile = await GetUserProfileById(user.Id);
                if (filter == "active")
                    match_with_filter = user.UserProfile.is_active;
                else if (filter == "inactive")
                    match_with_filter = !user.UserProfile.is_active;
                else
                    match_with_filter = !user.UserProfile.is_approved;

                if (match_with_filter)
                {
                    UsersDataModel udm = new UsersDataModel();
                    udm.user_id = user.Id;
                    udm.first_name = user.UserProfile.first_name;
                    udm.middle_name = user.UserProfile.middle_name;
                    udm.last_name = user.UserProfile.last_name;
                    udm.account_number = Convert.ToString(user.UserProfile.account_no);
                    udm.date_of_birth_string = user.UserProfile.date_of_birth.ToString("dd/MM/yyyy");

                    var roles = await _userManager.GetRolesAsync(user);
                    string role_name = roles.Count > 0 ? roles[0] : "Role Not Found!";
                    udm.role_string = role_name;

                    udm.is_approved_string = user.UserProfile.is_approved.ToString();
                    udm.is_active_string = user.UserProfile.is_active.ToString();
                    user_data_model_list.Add(udm);
                }
            }

            return user_data_model_list;
        }

        public async Task SetActiveUserProfile(Guid user_id, bool is_active)
        {
            var user_profile = await GetUserProfileById(user_id);
            user_profile.is_active = is_active;
            await _userProfileRepository.UpdateAsync(user_profile);
        }

        
    }
}
