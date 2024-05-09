using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternetBanking.Core.Application.Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IUserService userService, IProductService productService,
            IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(beneficiaryRepository, mapper)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _userService = userService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        public override async Task<SaveBeneficiaryViewModel> Add(SaveBeneficiaryViewModel vm)
        {
            var product = await _productService.GetProductByAccountNumber(vm.AccountNumberBeneficiary);
            if (product == null)
            {
                vm.HasError = true;
                vm.Error = "Account number not found";
                return vm;
            }

            var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            if (products.Any(p => p.AccountNumber == vm.AccountNumberBeneficiary))
            {
                vm.HasError = true;
                vm.Error = "You can't add yourself to your beneficiary list.";
                return vm;
            }

            vm.UserOwnerId = userViewModel.Id;
            return await base.Add(vm);
        }

        public override async Task Update(SaveBeneficiaryViewModel vm, int id)
        {
            //vm.UserId = userViewModel.Id;
            await base.Update(vm, id);
        }

        public async Task<List<BeneficiaryViewModel>> GetAllBeneficiaryViewModel()
        {
            var beneficiaries = await _beneficiaryRepository.GetAllAsync();

            var beneficiaryVms = beneficiaries
                .Where(b => b.UserOwnerId == userViewModel.Id)
                .Select(async b =>
                {
                    var beneficiaryProduct = await _productService.GetProductByAccountNumber(b.AccountNumberBeneficiary);
                    var userId = beneficiaryProduct.UserId;
                    var user = await _userService.GetByIdAsync(userId);

                    return new BeneficiaryViewModel
                    {
                        Id = b.Id,
                        FullName = beneficiaryProduct.FullName,
                        AccountNumber = beneficiaryProduct.AccountNumber
                    };
                });

            return (await Task.WhenAll(beneficiaryVms)).ToList();
        }

    }
}
