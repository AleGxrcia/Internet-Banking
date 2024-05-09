using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetProductByAccountNumberAsync(string accountNumber)
        {
            return await _dbContext.Products
                .Where(p => p.AccountNumber == accountNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<Product?> GetPrincipalProductByUserIdAsync(string userId)
        {
            return await _dbContext.Products
                .Where(p => p.UserId == userId && p.IsPrincipal)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllProductsByUserIdAsync(string userId)
        {
            return await _dbContext.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}
