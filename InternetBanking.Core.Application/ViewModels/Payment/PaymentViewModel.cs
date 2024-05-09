using InternetBanking.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Cuenta origen")]
        public string SourceAccountNumber { get; set; }

        [Display(Name = "Cuenta destino")]
        public string DestinationAccountNumber { get; set; }

        [Display(Name = "Monto")]
        public double Amount { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
