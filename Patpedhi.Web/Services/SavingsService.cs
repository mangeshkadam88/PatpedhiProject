using Microsoft.AspNetCore.Identity;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels;
using Patpedhi.Web.ViewModels.DataTable;
using PatPedhi.Core.Entities;
using PatPedhi.Core.Entities.Identity;
using PatPedhi.Core.Interfaces;
using PatPedhi.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Patpedhi.Web.Services
{
    public class SavingsService : ISavingsService
    {
        private readonly IAsyncRepository<Savings> _savingsRepository;
        protected readonly IUserProfileService _userProfileService;
        public SavingsService(IAsyncRepository<Savings> savingsRepository,
            IUserProfileService userProfileService)
        {
            _savingsRepository = savingsRepository;
            _userProfileService = userProfileService;
        }

        private async Task<List<Savings>> GetUserSavingsById(Guid selected_user_id)
        {
            var filterSpecification = new SavingSpecifications(selected_user_id);
            var savings = await _savingsRepository.ListAsync(filterSpecification);
            List<Savings> savingsList = new List<Savings>(savings);
            return savingsList;
        }

        public async Task<SavingsModel> GetModelById(Guid id)
        {
            var saving = await _savingsRepository.GetByIdAsync(id);
            SavingsModel model = new SavingsModel();
            model.Amount = saving.amount.ToString();
            model.Description = saving.description;
            model.IsApproved = saving.is_approved;
            model.SavingId = saving.Id;
            model.UserId = saving.user_id;
            model.IsActive = saving.is_active;
            return model;
        }

        public async Task<List<SavingsDataModel>> GetAllSavingsForSelectedUser(string filter, Guid selected_user)
        {
            List<Savings> savings = await GetUserSavingsById(selected_user);
            List<SavingsDataModel> savings_data_model = new List<SavingsDataModel>();
            CultureInfo hindi = new CultureInfo("hi-IN");
            bool match_with_filter = false;
            foreach (Savings item in savings)
            {
                if (filter == "active")
                    match_with_filter = item.is_active;
                else if (filter == "inactive")
                    match_with_filter = !item.is_active;
                else
                    match_with_filter = !item.is_approved;

                if (match_with_filter)
                {
                    SavingsDataModel dm = new SavingsDataModel();
                    dm.saving_id = item.Id;
                    dm.user_id = item.user_id;
                    dm.amount_string = string.Format(hindi, "{0:c}", item.amount);
                    dm.modified_date_string = item.modified_date.Value.ToString("dd/MM/yyyy hh:mm tt");
                    dm.is_approved_string = item.is_approved.ToString();
                    if (item.approved_by.HasValue)
                    {
                        UserProfile user_profile = await _userProfileService.GetUserProfileById(item.user_id);

                        dm.approved_by = user_profile.full_name;
                        dm.approved_on = item.approved_on.HasValue ? item.approved_on.Value.ToString("dd/MM/yyyy hh:mm tt") : "";
                    }
                    dm.is_active_string = item.is_active.ToString();
                    savings_data_model.Add(dm);
                }
            }

            return savings_data_model;
        }

        public async Task Save(SavingsModel model, string mode, string current_user_role, Guid current_user_id, Guid selected_user_id, string selected_user_role)
        {
            Savings saving = new Savings();
            if (mode == "update")
                saving = await _savingsRepository.GetByIdAsync(model.SavingId);
            else
            {
                saving.added_by = current_user_id;
                saving.added_date = DateTime.Now;
            }
            if ((current_user_role.ToLower() == "superadmin" || current_user_role.ToLower() == "admin") && model.IsApproved)
            {
                saving.is_approved = model.IsApproved;
                saving.approved_by = current_user_id;
                saving.approved_on = DateTime.Now;
            }
            saving.modified_date = DateTime.Now;
            saving.amount = Convert.ToDecimal(model.Amount);
            saving.description = model.Description;
            saving.is_active = model.IsActive;
            saving.user_id = selected_user_id;
            saving.savings_type = selected_user_role;

            if (mode == "update")
                await _savingsRepository.UpdateAsync(saving);
            else
                await _savingsRepository.AddAsync(saving);
        }
    }
}
