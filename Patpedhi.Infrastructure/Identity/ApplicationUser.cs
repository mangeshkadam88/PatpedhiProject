using Microsoft.AspNetCore.Identity;
using PatPedhi.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patpedhi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}
