using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PatPedhi.Core.Interfaces
{
    public interface IUserService
    {
        Task AddUserProfile(UserProfile userProfile);
    }
}
