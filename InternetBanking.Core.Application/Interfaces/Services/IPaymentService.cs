using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {
        Task<SavePaymentViewModel> AccountTrasfer(SavePaymentViewModel vm);
        Task<SavePaymentViewModel> BeneficiaryPayment(SavePaymentViewModel vm);
        Task<SavePaymentViewModel> CashAdvances(SavePaymentViewModel vm);
        Task<SavePaymentViewModel> CreditCardPayment(SavePaymentViewModel vm);
        Task<int> GetPaymentsMadeAllTheTime();
        Task<int> GetPaymentsMadeLast24Hours();
        Task<int> GetTransactionsMadeAllTheTime();
        Task<int> GetTransactionsMadeLast24Hours();
        Task<SavePaymentViewModel> LoanPayment(SavePaymentViewModel vm);
    }
}
