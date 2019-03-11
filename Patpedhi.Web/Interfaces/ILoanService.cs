using Patpedhi.Web.ViewModels.Loan;
using PatPedhi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patpedhi.Web.Interfaces
{
    public interface ILoanService
    {
        Task<List<LoanPaymentModel>> GetAllLoansForSelectedUser(string filter, Guid selected_user);
        Task<LoanPaymentModel> GetModelById(Guid id);
        Task<LoanPaymentModel> CalculateLoan(string _startDateString, string _principal, string _interestRate, string _period, Guid user_id, Loans loan);
    }
}
