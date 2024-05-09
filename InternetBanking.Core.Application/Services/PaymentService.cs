using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternetBanking.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel, PaymentViewModel, Payment>, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IUserService userService, IProductService productService,
            IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(paymentRepository, mapper)
        {
            _paymentRepository = paymentRepository;
            _userService = userService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        public override async Task<SavePaymentViewModel> Add(SavePaymentViewModel vm)
        {
            var destinationProduct = await _productService.GetProductByAccountNumber(vm.DestinationAccountNumber);
            if (destinationProduct == null)
            {
                vm.HasError = true;
                vm.Error = "Destination account number not found";
                return vm;
            }

            var sourceProduct = await _productService.GetProductByAccountNumber(vm.SourceAccountNumber);
            if (sourceProduct.Balance < vm.Amount)
            {
                vm.HasError = true;
                vm.Error = "Insufficient funds in the source account to complete the payment";
                return vm;
            }

            if (destinationProduct.AccountNumber == sourceProduct.AccountNumber)
            {
                vm.HasError = true;
                vm.Error = "Cannot make payment using the same account.";
                return vm;
            }

            vm.PaymentType = PaymentType.Payment;
            SavePaymentViewModel payment = await base.Add(vm);
            if (sourceProduct == null)
            {
                vm.HasError = true;
                vm.Error = "A problem occurred while trying to process your payment.";
                return vm;
            }

            if (sourceProduct.ProductType == ProductType.CreditCard)
            {
                sourceProduct.Debt += vm.Amount;
            }

            sourceProduct.Balance -= vm.Amount;
            SaveProductViewModel productSrc = _mapper.Map<SaveProductViewModel>(sourceProduct);
            await _productService.Update(productSrc, productSrc.Id.Value);

            destinationProduct.Balance += vm.Amount;
            SaveProductViewModel producDest = _mapper.Map<SaveProductViewModel>(destinationProduct);
            await _productService.Update(producDest, producDest.Id.Value);

            return payment;
        }

        public async Task<SavePaymentViewModel> CreditCardPayment(SavePaymentViewModel vm)
        {
            var destinationProduct = await _productService.GetProductByAccountNumber(vm.DestinationAccountNumber);
            if (destinationProduct == null)
            {
                vm.HasError = true;
                vm.Error = "Destination account number not found";
                return vm;
            }

            var sourceProduct = await _productService.GetProductByAccountNumber(vm.SourceAccountNumber);
            if (sourceProduct.Balance < vm.Amount)
            {
                vm.HasError = true;
                vm.Error = "Insufficient funds in the source account to complete the payment";
                return vm;
            }

            if (destinationProduct.AccountNumber == sourceProduct.AccountNumber)
            {
                vm.HasError = true;
                vm.Error = "Cannot make payment using the same account.";
                return vm;
            }

            if (vm.Amount > destinationProduct.Debt)
            {
                vm.Amount = destinationProduct.Debt;
            }

            vm.PaymentType = PaymentType.Payment;
            SavePaymentViewModel payment = await base.Add(vm);
            if (sourceProduct == null)
            {
                vm.HasError = true;
                vm.Error = "A problem occurred while trying to process your payment.";
                return vm;
            }

            sourceProduct.Balance -= vm.Amount;
            SaveProductViewModel productSrc = _mapper.Map<SaveProductViewModel>(sourceProduct);
            await _productService.Update(productSrc, productSrc.Id.Value);

            double newBalance = destinationProduct.Balance + vm.Amount;
            double newDebt = destinationProduct.Debt - vm.Amount;

            destinationProduct.Balance = newBalance;
            destinationProduct.Debt = newDebt;
            SaveProductViewModel productDest = _mapper.Map<SaveProductViewModel>(destinationProduct);
            await _productService.Update(productDest, productDest.Id.Value);

            return payment;
        }

        public async Task<SavePaymentViewModel> LoanPayment(SavePaymentViewModel vm)
        {
            var destinationProduct = await _productService.GetProductByAccountNumber(vm.DestinationAccountNumber);
            if (destinationProduct == null)
            {
                vm.HasError = true;
                vm.Error = "Destination account number not found";
                return vm;
            }

            var sourceProduct = await _productService.GetProductByAccountNumber(vm.SourceAccountNumber);
            if (sourceProduct.Balance < vm.Amount)
            {
                vm.HasError = true;
                vm.Error = "Insufficient funds in the source account to complete the payment";
                return vm;
            }

            if (vm.Amount > destinationProduct.Debt)
            {
                vm.Amount = destinationProduct.Debt;
            }

            vm.PaymentType = PaymentType.Payment;
            SavePaymentViewModel payment = await base.Add(vm);
            if (sourceProduct == null)
            {
                vm.HasError = true;
                vm.Error = "A problem occurred while trying to process your payment.";
                return vm;
            }

            if (sourceProduct.ProductType == ProductType.CreditCard)
            {
                sourceProduct.Debt += vm.Amount;
            }

            sourceProduct.Balance -= vm.Amount;
            SaveProductViewModel productSrc = _mapper.Map<SaveProductViewModel>(sourceProduct);
            await _productService.Update(productSrc, productSrc.Id.Value);

            double newDebt = destinationProduct.Debt - vm.Amount;

            destinationProduct.Debt = newDebt;
            SaveProductViewModel productDest = _mapper.Map<SaveProductViewModel>(destinationProduct);
            await _productService.Update(productDest, productDest.Id.Value);

            return payment;
        }

        public async Task<SavePaymentViewModel> BeneficiaryPayment(SavePaymentViewModel vm)
        {
            var destinationProduct = await _productService.GetProductByAccountNumber(vm.DestinationAccountNumber);
            if (destinationProduct == null)
            {
                vm.HasError = true;
                vm.Error = "Destination account number not found";
                return vm;
            }

            var sourceProduct = await _productService.GetProductByAccountNumber(vm.SourceAccountNumber);
            if (sourceProduct.Balance < vm.Amount)
            {
                vm.HasError = true;
                vm.Error = "Insufficient funds in the source account to complete the payment";
                return vm;
            }

            vm.PaymentType = PaymentType.Payment;
            SavePaymentViewModel payment = await base.Add(vm);
            if (sourceProduct == null)
            {
                vm.HasError = true;
                vm.Error = "A problem occurred while trying to process your payment.";
                return vm;
            }

            if (sourceProduct.ProductType == ProductType.CreditCard)
            {
                sourceProduct.Debt += vm.Amount;
            }

            sourceProduct.Balance -= vm.Amount;
            SaveProductViewModel productSrc = _mapper.Map<SaveProductViewModel>(sourceProduct);
            await _productService.Update(productSrc, productSrc.Id.Value);

            destinationProduct.Balance += vm.Amount;
            SaveProductViewModel producDest = _mapper.Map<SaveProductViewModel>(destinationProduct);
            await _productService.Update(producDest, producDest.Id.Value);

            return payment;
        }

        public async Task<SavePaymentViewModel> AccountTrasfer(SavePaymentViewModel vm)
        {
            var destinationProduct = await _productService.GetProductByAccountNumber(vm.DestinationAccountNumber);
            if (destinationProduct == null)
            {
                vm.HasError = true;
                vm.Error = "Destination account number not found";
                return vm;
            }

            var sourceProduct = await _productService.GetProductByAccountNumber(vm.SourceAccountNumber);
            if (sourceProduct.Balance < vm.Amount)
            {
                vm.HasError = true;
                vm.Error = "Insufficient funds in the source account to complete the payment";
                return vm;
            }

            vm.PaymentType = PaymentType.Transaction;
            SavePaymentViewModel payment = await base.Add(vm);
            if (sourceProduct == null)
            {
                vm.HasError = true;
                vm.Error = "A problem occurred while trying to process your payment.";
                return vm;
            }

            sourceProduct.Balance -= vm.Amount;
            SaveProductViewModel productSrc = _mapper.Map<SaveProductViewModel>(sourceProduct);
            await _productService.Update(productSrc, productSrc.Id.Value);

            destinationProduct.Balance += vm.Amount;
            SaveProductViewModel producDest = _mapper.Map<SaveProductViewModel>(destinationProduct);
            await _productService.Update(producDest, producDest.Id.Value);

            return payment;
        }

        public async Task<SavePaymentViewModel> CashAdvances(SavePaymentViewModel vm)
        {
            var destinationProduct = await _productService.GetProductByAccountNumber(vm.DestinationAccountNumber);
            if (destinationProduct == null)
            {
                vm.HasError = true;
                vm.Error = "Destination account number not found";
                return vm;
            }

            var sourceProduct = await _productService.GetProductByAccountNumber(vm.SourceAccountNumber);
            if (sourceProduct.Balance < vm.Amount)
            {
                vm.HasError = true;
                vm.Error = "Insufficient funds in the source account to complete the payment";
                return vm;
            }

            double totalAmount = vm.Amount * (1 + 0.0625);

            if (totalAmount > sourceProduct.Balance)
            {
                double exceedAmount = totalAmount - sourceProduct.Balance;
                string errorMessage = $"The cash advance amount exceeds the credit card limit by {exceedAmount:C2}. " +
                                      $"Total amount including interest: {totalAmount:C2}. " +
                                      $"Remaining credit limit: {sourceProduct.Balance:C2}.";

                vm.HasError = true;
                vm.Error = errorMessage;
                return vm;
            }

            vm.PaymentType = PaymentType.Transaction;
            SavePaymentViewModel payment = await base.Add(vm);
            if (sourceProduct == null)
            {
                vm.HasError = true;
                vm.Error = "A problem occurred while trying to process your payment.";
                return vm;
            }

            double newBalance = sourceProduct.Balance - totalAmount;
            double newDebt = sourceProduct.Debt + totalAmount;

            sourceProduct.Balance = newBalance;
            sourceProduct.Debt = newDebt;
            SaveProductViewModel productSrc = _mapper.Map<SaveProductViewModel>(sourceProduct);
            await _productService.Update(productSrc, productSrc.Id.Value);

            destinationProduct.Balance += vm.Amount;
            SaveProductViewModel productDest = _mapper.Map<SaveProductViewModel>(destinationProduct);
            await _productService.Update(productDest, productDest.Id.Value);

            return payment;
        }

        public async Task<int> GetPaymentsMadeLast24Hours()
        {
            var payments = await _paymentRepository.GetAllAsync();

            DateTime now = DateTime.UtcNow;
            DateTime twentyFourHoursAgo = now.AddHours(-24);

            int paymentsMadeLast24Hours = payments
                .Where(p => p.PaymentTypeId == (int)PaymentType.Payment &&
                             p.Created >= twentyFourHoursAgo && p.Created <= now)
                .Count();

            return paymentsMadeLast24Hours;
        }

        public async Task<int> GetPaymentsMadeAllTheTime()
        {
            var payments = await _paymentRepository.GetAllAsync();

            int paymentsMadeAllTheTime = payments.Where(t => t.PaymentTypeId == (int)PaymentType.Payment).Count();

            return paymentsMadeAllTheTime;
        }

        public async Task<int> GetTransactionsMadeLast24Hours()
        {
            var transactions = await _paymentRepository.GetAllAsync();
            
            DateTime now = DateTime.UtcNow;
            DateTime twentyFourHoursAgo = now.AddHours(-24);

            int transactionMadeLast24Hours = transactions
                .Where(p => p.PaymentTypeId == (int)PaymentType.Transaction &&
                             p.Created >= twentyFourHoursAgo && p.Created <= now)
                .Count();

            return transactionMadeLast24Hours;
        }

        public async Task<int> GetTransactionsMadeAllTheTime()
        {
            var transactionn = await _paymentRepository.GetAllAsync();

            int transactionsMadeAllTheTime = transactionn.Where(t => t.PaymentTypeId == (int)PaymentType.Transaction).Count();

            return transactionsMadeAllTheTime;
        }

    }
}
