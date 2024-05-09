using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
        Task<ProductViewModel> GetProductByAccountNumber(string accountNumber);
        Task<List<ProductViewModel>> GetProductsByUserViewModel(string userId);
        Task<int> GetProductsCreatedAllTheTime();
        Task TransferAmountToPrincipal(string userId, double amount);
    }
}
