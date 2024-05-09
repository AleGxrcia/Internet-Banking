using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.ViewModels.Dashboard;

namespace InternetBanking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;

        public HomeController(IProductService productService, IPaymentService paymentService, IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _paymentService = paymentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        [Authorize]
        public IActionResult Index()
        {
            var isAdmin = userViewModel.Roles.Contains(Roles.Admin.ToString());
            if (isAdmin)
            {
                return RedirectToAction("Admin");
            }

            return RedirectToAction("Client");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var users = await _userService.GetAllAsync();

            var dashboardVm = new AdminDashboardViewModel()
            {
                TotalProductsCreated = await _productService.GetProductsCreatedAllTheTime(),
                TotalActiveUsers = users.Where(u => u.IsActive == true).Count(),
                TotalInactiveUsers = users.Where(u => u.IsActive == false).Count(),
                TotalPaymentsMade = await _paymentService.GetPaymentsMadeAllTheTime(),
                PaymentsMadeLast24Hours = await _paymentService.GetPaymentsMadeLast24Hours(),
                TotalTransactionsMade = await _paymentService.GetTransactionsMadeAllTheTime(),
                TransactionsMadeLast24Hours = await _paymentService.GetTransactionsMadeLast24Hours()
            };

            return View("Admin", dashboardVm);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Client()
        {
            var products = await _productService.GetProductsByUserViewModel(userViewModel.Id);
            return View("Client", products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
