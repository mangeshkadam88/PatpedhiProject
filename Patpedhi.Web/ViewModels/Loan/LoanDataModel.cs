using System;
using System.Collections.Generic;
using System.Linq;

namespace Patpedhi.Web.ViewModels.Loan
{
    public class AmortPayment
    {
        public int row_number { get; set; }
        public DateTime date { get; set; }
        public double monthly_installment { get; set; }
        public double initial_balance { get; set; }
        public double principal { get; set; }
        public double interest { get; set; }
        public double new_balance { get; set; }
    }

    public class LoanPaymentModel
    {
        public List<AmortPayment> ampts { get; set; }
        public double monthly_installments { get; set; }
        public double total_repayment { get; set; }
        public double total_interest { get; set; }

        public double loan_amount { get; set; }
        public string rate_of_interest_p_a { get; set; } //%
        public double rate_of_interest_p_m { get; set; }
        public int year_of_loan { get; set; }
        public int no_of_payment_per_year { get; set; } // months / 12

        public Guid? loan_id { get; set; }
        public Guid user_id { get; set; }
        public string loan_amount_string { get; set; }
        public string modified_date_string { get; set; }
        public string is_approved_string { get; set; }
        public string approved_by { get; set; }
        public string approved_on { get; set; }
        public string is_active_string { get; set; }
    }
}
