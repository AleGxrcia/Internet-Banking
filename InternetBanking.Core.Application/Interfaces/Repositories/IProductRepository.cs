using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllProductsByUserIdAsync(string userId);
        Task<Product?> GetPrincipalProductByUserIdAsync(string userId);
        Task<Product?> GetProductByAccountNumberAsync(string accountNumber);
    }
}
