using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.Enums
{
    public enum PaymentType
    {
        [Display(Name = "Pago")]
         Payment = 1,
        [Display(Name = "Transacción")]
        Transaction
    }
}
