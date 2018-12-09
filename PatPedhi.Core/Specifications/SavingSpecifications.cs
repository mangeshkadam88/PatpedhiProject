using PatPedhi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Specifications
{
    public class SavingSpecifications : BaseSpecification<Savings>
    {
        public SavingSpecifications(Guid user_id) : base(i => i.user_id == user_id)
        {

        }
    }
}
