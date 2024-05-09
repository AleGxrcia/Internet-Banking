using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Product;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
    public class SavePaymentViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Cuenta origen")]
        public string SourceAccountNumber { get; set; }

        [Display(Name = "Cuenta destino")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string DestinationAccountNumber { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public double Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
        public List<ProductViewModel>? Products { get; set; }
        public List<ProductViewModel>? CreditCardsProducts { get; set; }
        public List<ProductViewModel>? LoanProducts { get; set; }
        public List<BeneficiaryViewModel>? Beneficiaries { get; set; }
    }
}
