using InternetBanking.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace InternetBanking.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Nombre")]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Apellido")]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Correo electronico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Cedula")]
        [DataType(DataType.Text)]
        public string? IdNumber { get; set; }

        [Display(Name = "Nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Tipo de usuario")]
        public Roles UserType { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Monto inicial")]
        public double? InitialAmount { get; set; }
        public bool? IsActive { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
