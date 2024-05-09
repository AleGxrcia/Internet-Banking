using InternetBanking.Core.Application.Enums;

namespace InternetBanking.Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public double Balance { get; set; }
        public double Debt { get; set; }
        public ProductType ProductType { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
