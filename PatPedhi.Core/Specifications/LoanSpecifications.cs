using PatPedhi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Specifications
{
    public class LoanSpecifications : BaseSpecification<Loans>
    {
        public LoanSpecifications(Guid user_id) : base(i => i.user_id == user_id)
        {

        }
    }
}
