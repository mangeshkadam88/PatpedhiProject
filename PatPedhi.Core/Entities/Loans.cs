using System;
using System.Collections.Generic;
using System.Text;

namespace PatPedhi.Core.Entities
{
    public class Loans : BaseEntity
    {
        public Guid user_id { get; set; }
        public decimal amount { get; set; }
        public decimal interest_rate { get; set; }
        public decimal monthly_emi { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool is_active { get; set; }
        public bool is_approved { get; set; }
        public Guid added_by { get; set; }
        public DateTime added_on { get; set; }
        public string description { get; set; }
        public Guid? approved_by { get; set; }
        public DateTime? approved_on { get; set; }
        public int no_of_months { get; set; }
    }
}
