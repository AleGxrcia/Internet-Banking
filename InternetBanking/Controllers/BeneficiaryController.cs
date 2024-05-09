using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.WebApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Beneficiaries = await _beneficiaryService.GetAllBeneficiaryViewModel();
            return View(new SaveBeneficiaryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiary(SaveBeneficiaryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            SaveBeneficiaryViewModel beneficiaryVm = await _beneficiaryService.Add(vm);
            if (beneficiaryVm.HasError == true)
            {
                vm.HasError = beneficiaryVm.HasError;
                vm.Error = beneficiaryVm.Error;
                return View("Index", vm);
            }

            return RedirectToRoute(new { controller = "Beneficiary", action = "Index" });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var beneficiary = await _beneficiaryService.GetByIdSaveViewModel(id);
            return View("Delete", beneficiary);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBeneficiary(int id)
        {
            await _beneficiaryService.Delete(id);
            return RedirectToRoute(new { controller = "Beneficiary", action = "Index" });
        }
    }
}
