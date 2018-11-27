using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Specifications
{
    public class UserProfileSpecifications : BaseSpecification<UserProfile>
    {
        public UserProfileSpecifications(Guid user_id) : base(i => i.user_id == user_id)
        {

        }
    }
}
