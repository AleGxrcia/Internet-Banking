using InternetBanking.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Product
{
    public class SaveProductViewModel
    {
        public int? Id { get; set; }
        public string? AccountNumber { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Currency)]
        public double Amount { get; set; }
        public double Debt { get; set; }
        public ProductType ProductType { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
