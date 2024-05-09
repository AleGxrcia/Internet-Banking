using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {
        public int? Id { get; set; }
        public string? UserOwnerId { get; set; }

        [Display(Name = "Numero de cuenta")]
        public string AccountNumberBeneficiary { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
