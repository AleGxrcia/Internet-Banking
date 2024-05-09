using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace InternetBanking.WebApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;


        public PaymentController(IPaymentService paymentService, IProductService productService, IBeneficiaryService beneficiaryService,
            IHttpContextAccessor httpContextAccessor)
        {
            _paymentService = paymentService;
            _productService = productService;
            _beneficiaryService = beneficiaryService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        public async Task<IActionResult> ExpressPayment()
        {
            var listProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var model = new SavePaymentViewModel()
            {
                Products = listProducts
            };
            return View("ExpressPayment", model);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPayment(SavePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                return View("ExpressPayment", vm);
            }

            var paymentVm = await _paymentService.Add(vm);
            if (paymentVm.HasError == true)
            {   
                vm.HasError = paymentVm.HasError;
                vm.Error = paymentVm.Error;

                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                return View("ExpressPayment", vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> CreditCardPayment()
        {
            var listProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var listCreditCards = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var model = new SavePaymentViewModel()
            {
                Products = listProducts,
                CreditCardsProducts = listCreditCards.Where(c => c.ProductType == ProductType.CreditCard).ToList()
            };
            return View("CreditCardPayment", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreditCardPayment(SavePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.CreditCardsProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                return View("CreditCardPayment", vm);
            }

            var paymentVm = await _paymentService.CreditCardPayment(vm);
            if (paymentVm.HasError == true)
            {
                vm.HasError = paymentVm.HasError;
                vm.Error = paymentVm.Error;

                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.CreditCardsProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                return View("CreditCardPayment", vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> LoanPayment()
        {
            var listProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var listLoans = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var model = new SavePaymentViewModel()
            {
                Products = listProducts,
                LoanProducts = listLoans.Where(c => c.ProductType == ProductType.Loan).ToList()
            };
            return View("LoanPayment", model);
        }

        [HttpPost]
        public async Task<IActionResult> LoanPayment(SavePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.LoanProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                return View("LoanPayment", vm);
            }

            var paymentVm = await _paymentService.LoanPayment(vm);
            if (paymentVm.HasError == true)
            {
                vm.HasError = paymentVm.HasError;
                vm.Error = paymentVm.Error;

                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.LoanProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                return View("LoanPayment", vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> BeneficiaryPayment()
        {
            var listProducts = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var listBeneficiaries = await _beneficiaryService.GetAllBeneficiaryViewModel();
            var model = new SavePaymentViewModel()
            {
                Products = listProducts,
                Beneficiaries = listBeneficiaries
            };
            return View("BeneficiaryPayment", model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(SavePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.Beneficiaries = await _beneficiaryService.GetAllBeneficiaryViewModel();
                return View("BeneficiaryPayment", vm);
            }

            var listBeneficiaries = await _beneficiaryService.GetAllBeneficiaryViewModel();
            var beneficiary = listBeneficiaries.Where(b => b.AccountNumber == vm.DestinationAccountNumber).FirstOrDefault();

            var confirmVm = new ConfirmPaymentViewModel()
            {
                BeneficiaryName = beneficiary.FullName,
                SourceAccountNumber = vm.SourceAccountNumber,
                DestinationAccountNumber = vm.DestinationAccountNumber,
                Amount = vm.Amount,
            };

            return View("ConfirmPayment", confirmVm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(ConfirmPaymentViewModel confirmVm)
        {
            var payment = new SavePaymentViewModel()
            {
                SourceAccountNumber = confirmVm.SourceAccountNumber,
                DestinationAccountNumber = confirmVm.DestinationAccountNumber,
                Amount = confirmVm.Amount,
            };

            var paymentVm = await _paymentService.BeneficiaryPayment(payment);
            if (paymentVm.HasError == true)
            {
                payment.HasError = paymentVm.HasError;
                payment.Error = paymentVm.Error;

                paymentVm.Products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                paymentVm.Beneficiaries = await _beneficiaryService.GetAllBeneficiaryViewModel();
                return View("BeneficiaryPayment", paymentVm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> AccountTransfer()
        {
            var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var model = new SavePaymentViewModel()
            {
                Products = products.Where(c => c.ProductType == ProductType.SavingAccount).ToList(),
            };
            return View("AccountTransfer", model);
        }

        [HttpPost]
        public async Task<IActionResult> AccountTransfer(SavePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);

                vm.Products = products.Where(c => c.ProductType == ProductType.SavingAccount).ToList();
                return View("AccountTransfer", vm);
            }

            var paymentVm = await _paymentService.AccountTrasfer(vm);
            if (paymentVm.HasError == true)
            {
                vm.HasError = paymentVm.HasError;
                vm.Error = paymentVm.Error;

                var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.Products = products.Where(c => c.ProductType == ProductType.SavingAccount).ToList();
                return View("AccountTransfer", vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public async Task<IActionResult> CashAdvances()
        {
            var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            var model = new SavePaymentViewModel()
            {
                CreditCardsProducts = products.Where(p => p.ProductType == ProductType.CreditCard).ToList(),
                Products = products.Where(p => p.ProductType == ProductType.SavingAccount).ToList()
            };
            return View("CashAdvances", model);
        }

        [HttpPost]
        public async Task<IActionResult> CashAdvances(SavePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.CreditCardsProducts = products.Where(p => p.ProductType == ProductType.CreditCard).ToList();
                vm.Products = products.Where(p => p.ProductType == ProductType.SavingAccount).ToList();

                return View("CashAdvances", vm);
            }

            var paymentVm = await _paymentService.CashAdvances(vm);
            if (paymentVm.HasError == true)
            {
                vm.HasError = paymentVm.HasError;
                vm.Error = paymentVm.Error;

                var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
                vm.CreditCardsProducts = products.Where(p => p.ProductType == ProductType.CreditCard).ToList();
                vm.Products = products.Where(p => p.ProductType == ProductType.SavingAccount).ToList();
                return View("CashAdvances", vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
