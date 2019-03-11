using Patpedhi.Web.Interfaces;
using Patpedhi.Web.ViewModels.DataTable;
using Patpedhi.Web.ViewModels.Loan;
using PatPedhi.Core.Entities;
using PatPedhi.Core.Entities.Identity;
using PatPedhi.Core.Interfaces;
using PatPedhi.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.Services
{
    public class LoansService : ILoanService
    {
        private readonly IAsyncRepository<Loans> _loansRepository;
        protected readonly IUserProfileService _userProfileService;
        public LoansService(IAsyncRepository<Loans> loansRepository,
            IUserProfileService userProfileService)
        {
            _loansRepository = loansRepository;
            _userProfileService = userProfileService;
        }

        private async Task<List<Loans>> GetUserLoansById(Guid selected_user_id)
        {
            var filterSpecification = new LoanSpecifications(selected_user_id);
            var savings = await _loansRepository.ListAsync(filterSpecification);
            List<Loans> loansList = new List<Loans>(savings);
            return loansList.OrderByDescending(x => x.modified_date).ThenByDescending(x => x.added_date).ToList();
        }

        public async Task<List<LoanPaymentModel>> GetAllLoansForSelectedUser(string filter, Guid selected_user)
        {
            List<Loans> loans = await GetUserLoansById(selected_user);
            List<LoanPaymentModel> loans_data_model = new List<LoanPaymentModel>();
            CultureInfo hindi = new CultureInfo("hi-IN");
            bool match_with_filter = false;
            foreach (Loans item in loans)
            {
                if (filter == "active")
                    match_with_filter = item.is_active;
                else if (filter == "inactive")
                    match_with_filter = !item.is_active;
                else
                    match_with_filter = !item.is_approved;

                if (match_with_filter)
                {
                    LoanPaymentModel dm = new LoanPaymentModel();
                    dm.loan_id = item.Id;
                    dm.user_id = item.user_id;
                    dm.loan_amount_string = string.Format(hindi, "{0:c}", item.amount);
                    dm.modified_date_string = item.modified_date.Value.ToString("dd/MM/yyyy hh:mm tt");
                    dm.is_approved_string = item.is_approved.ToString();
                    if (item.approved_by.HasValue)
                    {
                        UserProfile user_profile = await _userProfileService.GetUserProfileById(item.user_id);

                        dm.approved_by = user_profile.full_name;
                        dm.approved_on = item.approved_on.HasValue ? item.approved_on.Value.ToString("dd/MM/yyyy hh:mm tt") : "";
                    }
                    dm.is_active_string = item.is_active.ToString();
                    loans_data_model.Add(dm);
                }
            }

            return loans_data_model;
        }

        public async Task<LoanPaymentModel> GetModelById(Guid id)
        {
            var loan = await _loansRepository.GetByIdAsync(id);            
            var loan_detail = await CalculateLoan(loan.StartDate.ToString("dd/MM/yyyy"), loan.amount.ToString(), loan.interest_rate.ToString(), loan.no_of_months.ToString(), loan.user_id, loan);
            return loan_detail;
        }

        public async Task<LoanPaymentModel> CalculateLoan(string _startDateString, string _principal, string _interestRate, string _period, Guid user_id, Loans loan)
        {
            // Make sure we use types that hold decimal places   
            DateTime _startDate = new DateTime(Convert.ToInt32(_startDateString.Split("/")[2]), Convert.ToInt32(_startDateString.Split("/")[1]), Convert.ToInt32(_startDateString.Split("/")[0]));
            DateTime payDate = _startDate;//DateTime.ParseExact(_startDate.Value, "yyyyMMdd", null);
            double interestRate = 0;
            double monthlyInterest = 0;
            double loanAmount;
            short amortizationTerm = 0;
            double currentBalance;
            double cummulativeInterest = 0;
            double monthlyPrincipal = 0;
            double cummulativePrincipal = 0;

            loanAmount = double.Parse(_principal);
            double initialBalance = loanAmount;

            currentBalance = loanAmount;
            interestRate = double.Parse(_interestRate) * 0.01;
            amortizationTerm = short.Parse(_period);

            // Calculate the monthly payment and round it to 2 decimal places           
            var monthlyPayment = ((interestRate / 12) / (1 - (Math.Pow((1 + (interestRate / 12)), -(amortizationTerm))))) * loanAmount;
            monthlyPayment = Math.Round(monthlyPayment, 2);

            // Storage List
            List<AmortPayment> amortPaymentList = new List<AmortPayment>();

            // Loop for amortization term (number of monthly payments)
            for (int j = 0; j < amortizationTerm; j++)
            {
                // Calculate monthly cycle
                ////monthlyInterest = currentBalance * interestRate;
                monthlyInterest = currentBalance * interestRate / 12;
                monthlyPrincipal = monthlyPayment - monthlyInterest;
                currentBalance = currentBalance - monthlyPrincipal;

                if (j == amortizationTerm - 1 && currentBalance != monthlyPayment)
                {
                    // Adjust the last payment to make sure the final balance is 0
                    monthlyPayment += currentBalance;
                    currentBalance = 0;
                }

                // Reset Date
                payDate = payDate.AddMonths(1);
                // Add to cummulative totals
                cummulativeInterest += monthlyInterest;
                cummulativePrincipal += monthlyPrincipal;

                amortPaymentList.Add
                    (new AmortPayment
                    {
                        row_number = j + 1,
                        date = payDate,
                        initial_balance = Math.Round(initialBalance, 2),
                        monthly_installment = Math.Round(monthlyPayment, 2),
                        principal = Math.Round(Math.Round(monthlyPayment, 2) - Math.Round(monthlyInterest, 2), 2),
                        interest = Math.Round(monthlyInterest, 2),
                        new_balance = Math.Round(currentBalance, 2)
                    });
                initialBalance = currentBalance;
            }

            CultureInfo hindi = new CultureInfo("hi-IN");

            LoanPaymentModel loan_payment_detail = new LoanPaymentModel();
            loan_payment_detail.ampts = amortPaymentList;
            loan_payment_detail.monthly_installments = Math.Round(amortPaymentList.First().monthly_installment, 2);
            loan_payment_detail.total_repayment = Math.Round(loan_payment_detail.monthly_installments * amortizationTerm, 2);
            loan_payment_detail.total_interest = Math.Round(loan_payment_detail.total_repayment - loanAmount, 2);
            loan_payment_detail.loan_amount = loanAmount;
            loan_payment_detail.rate_of_interest_p_a = _interestRate + "%";
            loan_payment_detail.rate_of_interest_p_m = Math.Round(interestRate / 12, 3);
            loan_payment_detail.year_of_loan = amortizationTerm / 12;
            loan_payment_detail.no_of_payment_per_year = 12;

            loan_payment_detail.user_id = user_id;
            loan_payment_detail.loan_amount_string = string.Format(hindi, "{0:c}", loanAmount);
            if (loan != null)
            {
                loan_payment_detail.loan_id = loan.Id;
                loan_payment_detail.modified_date_string = loan.modified_date.HasValue ? loan.modified_date.Value.ToString("dd/MM/yyyy hh:mm tt") : "";
                
                loan_payment_detail.is_approved_string = loan.is_approved.ToString();
                if (loan.approved_by.HasValue)
                {
                    UserProfile user_profile = await _userProfileService.GetUserProfileById(loan.user_id);

                    loan_payment_detail.approved_by = user_profile.full_name;
                    loan_payment_detail.approved_on = loan.approved_on.HasValue ? loan.approved_on.Value.ToString("dd/MM/yyyy hh:mm tt") : "";
                }
                loan_payment_detail.is_active_string = loan.is_active.ToString();
            }

            return loan_payment_detail;
        }
    }
}
