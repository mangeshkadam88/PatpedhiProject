using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Entities
{
    public class Savings : BaseEntity
    {
        public Guid user_id { get; set; }
        public decimal amount { get; set; }
        public bool is_active { get; set; }
        public bool is_approved { get; set; }        
        public string savings_type { get; set; }
        public Guid added_by { get; set; }
        public string description { get; set; }
        public Guid? approved_by { get; set; }
        public DateTime? approved_on { get; set; }
    }
}
