using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Entities
{
    public class LoansHistory : BaseEntity
    {
        public Guid loan_id { get; set; }
        public decimal emi_paid { get; set; }
        public DateTime date_paid { get; set; }
        public bool is_active { get; set; }
        public bool is_approved { get; set; }
        public Guid added_by { get; set; }
        public DateTime added_on { get; set; }
        public string description { get; set; }
        public Guid? approved_by { get; set; }
        public DateTime? approved_on { get; set; }
    }
}
