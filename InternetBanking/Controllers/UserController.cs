using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View("SaveUser", new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveUser", vm);
            }

            var origin = Request.Headers["origin"];
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveUser", vm);
            }

            if (vm.UserType == Roles.Client && response.Id != null) 
            {
                SaveProductViewModel productVm = new()
                {
                    UserId = response.Id,
                    Amount = vm.InitialAmount.Value,
                    ProductType = ProductType.SavingAccount,
                    IsPrincipal = true
                };

                await _productService.Add(productVm);
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            SaveUserViewModel userVm = await _userService.GetByIdAsync(id); 
            return View("SaveUser", userVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveUser", vm);
            }

            SaveUserViewModel userVm = await _userService.GetByIdAsync(vm.Id);

            if (userVm.UserType == Roles.Client && vm.InitialAmount > 0)
            {
                await _productService.TransferAmountToPrincipal(vm.Id, vm.InitialAmount.Value);
            }

            await _userService.UpdateAsync(vm, vm.Id);

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public async Task<IActionResult> ConfirmAction(string id)
        {
            SaveUserViewModel userVm = await _userService.GetByIdAsync(id);

            return View("ConfirmAction", userVm);
        }

        [HttpPost]
        public async Task<IActionResult> ActiveUser(SaveUserViewModel vm)
        {
            await _userService.ActiveUser(vm.Id);
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> InactiveUser(SaveUserViewModel vm)
        {
            await _userService.InactiveUser(vm.Id);
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
