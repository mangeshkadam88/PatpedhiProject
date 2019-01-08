using Microsoft.AspNetCore.Identity;
using Patpedhi.Infrastructure.Identity;
using Patpedhi.Web.ViewModels;
using Patpedhi.Web.ViewModels.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.Interfaces
{
    public interface ISavingsService
    {
        Task<List<SavingsDataModel>> GetAllSavingsForSelectedUser(string filter, Guid selected_user);
        Task<List<SavingsDataModel>> GetAllSavingsForSelectedUser(Guid selected_user);
        Task<SavingsModel> GetModelById(Guid id);
        Task Save(SavingsModel model, string mode, string current_user_role, Guid current_user_id, Guid selected_user_id, string selected_user_role);

    }
}
