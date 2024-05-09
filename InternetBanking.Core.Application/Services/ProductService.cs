using AutoMapper;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IUserService userService, IMapper mapper) : base(productRepository, mapper)
        {
            _productRepository = productRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task<SaveProductViewModel> Add(SaveProductViewModel vm)
        {
            while (true)
            {
                var uniqueId = UniqueIdGenerator.GenerateUniqueId();
                var existUniqueId = await _productRepository.GetProductByAccountNumberAsync(uniqueId);

                if (existUniqueId == null)
                {
                    vm.AccountNumber = uniqueId;
                    break;
                }
            }

            var product = await base.Add(vm);

            if (product != null && vm.ProductType == ProductType.Loan)
            {
                await TransferAmountToPrincipal(vm.UserId, product.Amount);

                product.Debt += product.Amount;
                product.Amount -= product.Amount;
                await Update(product, product.Id.Value);
            }

            return product;
        }

        public async Task TransferAmountToPrincipal(string userId, double amount)
        {
            var principalProduct = await _productRepository.GetPrincipalProductByUserIdAsync(userId);
            if (principalProduct != null)
            {
                principalProduct.Balance += amount;
                await _productRepository.UpdateAsync(principalProduct, principalProduct.Id);
            }
        }

        public override async Task Update(SaveProductViewModel vm, int id)
        {
            await base.Update(vm, id);
        }

        public override async Task Delete(int id)
        {
            var product = await base.GetByIdSaveViewModel(id);
            if (product.ProductType == ProductType.SavingAccount &&  product.Amount > 0)
            {
                await TransferAmountToPrincipal(product.UserId, product.Amount);
            }

            await base.Delete(id);
        }

        public async Task<ProductViewModel> GetProductByAccountNumber(string accountNumber)
        {
            var product = await _productRepository.GetProductByAccountNumberAsync(accountNumber);
            var productVm = _mapper.Map<ProductViewModel>(product);
            if (productVm != null)
            {
                SaveUserViewModel user = await _userService.GetByIdAsync(product.UserId);
                productVm.UserId = user.Id;
                productVm.UserName = user.UserName;
                productVm.FullName = $"{user.FirstName} {user.LastName}";
            }

            return productVm;
        }

        public async Task<List<ProductViewModel>> GetProductsByUserViewModel(string userId)
        {
            var products = await _productRepository.GetAllProductsByUserIdAsync(userId);

            SaveUserViewModel user = await _userService.GetByIdAsync(userId);

            return products
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = user.UserName,
                    FullName = $"{user.FirstName} {user.LastName}",
                    AccountNumber = p.AccountNumber,
                    Balance = p.Balance,
                    Debt = p.Debt,
                    IsPrincipal = p.IsPrincipal,
                    ProductType = (ProductType)p.ProductTypeId,
                }).ToList();
        }

        public async Task<int> GetProductsCreatedAllTheTime()
        {
            var products = await _productRepository.GetAllAsync();

            int productsCreatedAllTheTime = products.Count();

            return productsCreatedAllTheTime;
        }

    }
}
