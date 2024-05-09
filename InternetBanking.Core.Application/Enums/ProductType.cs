using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.Enums
{
    public enum ProductType
    {
        [Display(Name = "Cuenta de ahorro")]
         SavingAccount = 1,
        [Display(Name = "Tarjeta de crédito")]
         CreditCard,
        [Display(Name = "Préstamo")]
         Loan
    }
}
