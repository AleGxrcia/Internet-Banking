using InternetBanking.Core.Application.ViewModels.Product;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
    public class SaveCreditCardPaymentViewModel : PaymentViewModel
    {
        public List<ProductViewModel>? CreditCardsProducts { get; set; }
    }
}
